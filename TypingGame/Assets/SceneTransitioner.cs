using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

public class SceneTransitioner : MonoBehaviour
{
    Animator TransitionAnmimator;
    string currentMap;
    public List<GameObject> Enemies = new List<GameObject>();
    public SaveableObject[] savableObjects;
    public bool saveTest; // delete later
    public int currentSave=1;
    public bool onCheckPoint;
    public AudioClip BattleTransitionSound;

    private void Awake()
    {
        Debug.Log("awake");
        currentMap = SceneManager.GetActiveScene().name;
        var SaveDatas = FindObjectsOfType<SceneTransitioner>();
        if (SaveDatas.Length > 1)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentMap = SceneManager.GetActiveScene().name;
        TransitionAnmimator = GetComponentInChildren<Animator>();
        Debug.Log("start");
        FindObjectOfType<SceneTransitioner>().loadPositions("/playerSavePosition.test");
        if(FindObjectOfType<AudioBox>())
        FindObjectOfType<AudioBox>().PlayBackgroundMusic(0);
    }

    public void Save_Positions(string saveName)
    {

        if (SceneManager.GetActiveScene().name != currentMap) return;
        savableObjects = FindObjectsOfType<SaveableObject>();
        SaveSystem.SavePosition(savableObjects,false, "/playerSavePosition.test");
    }

    public void Save_Backpack(string saveName)
    {
        if(FindObjectOfType<Backpack>())
        {
            SaveSystem.SaveBackpack(FindObjectOfType<Backpack>(), saveName);
        }
    }

    public void loadPositions(string saveName)
    {
        if (!saveTest)
        {
            Save_Positions("/playerSavePosition.test");
            
            return;
        }
       
        savableObjects = FindObjectsOfType<SaveableObject>();
        var _save = SaveSystem.loadPostion(saveName);
        var index = _save.Get_Index();
        List<string> listList = new List<string>(index);
        var x = _save.Get_x();
        var y = _save.Get_y();
        var inGame = _save.Get_In_game();
        for (int i =0; i < savableObjects.Length;i++)
        {
            //if this is funky wunky than replace it with this  "int rightIndex = ArrayUtility.IndexOf(index, savableObjects[i].uniqueIdentifier);"  
            // only works in editor though
            int rightIndex = listList.IndexOf(savableObjects[i].uniqueIdentifier);
            if (rightIndex == -1) continue;
            savableObjects[i].transform.position = new Vector2(x[rightIndex], y[rightIndex]);
            savableObjects[i].inGame = inGame[rightIndex];

        }
    }

    public void TransitionBattle(List<GameObject> _Enemies, int battleTheme)
    {
        FindObjectOfType<AudioBox>().PlaySound(BattleTransitionSound, 1);
        FindObjectOfType<AudioBox>().PlayBattleMusic(battleTheme);
        currentMap = SceneManager.GetActiveScene().name;
        saveTest = true;
        Enemies = _Enemies;
        Save_Positions("/playerSavePosition.test");
        Save_Backpack("/BackpackSave.test");
        FindObjectOfType<PlayerStats>().SavePlayer("/playerSave.test");
        TransitionScene("Battle", "IntoBackpack");
    }

    public void TransitionOutOfBattle()
    {
        FindObjectOfType<AudioBox>().PlayBackgroundMusic(0);
        saveTest = true;
        FindObjectOfType<PlayerStats>().SavePlayer("/playerSave.test");
        TransitionScene(currentMap, "IntoBackpack");
        
        loadPositions("/playerSavePosition.test");


    }

    public void TransitionBackpack()
    {
        currentMap = SceneManager.GetActiveScene().name;
        saveTest = true;
        Save_Positions("/playerSavePosition.test");
        Save_Backpack("/BackpackSave.test");
        FindObjectOfType<PlayerStats>().SavePlayer("/playerSave.test");
        TransitionScene("Backpack", "IntoBackpack");
    }

    public void TransitionOutOFBackpack()
    {
        saveTest = true;
        Save_Backpack("/BackpackSave.test");
        FindObjectOfType<PlayerStats>().SavePlayer("/playerSave.test");
        TransitionScene(currentMap, "IntoBackpack");
        
    }

    private void Update()
    {
        if (Input.GetKeyDown("9"))
        {
            Save_Game(currentSave);
        }
        if (Input.GetKeyDown("0"))
        {
            LoadGameFile();
        }
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (SceneManager.GetActiveScene().name == currentMap)
        //    {
        //        TransitionBackpack();
        //    }
        //    else if(SceneManager.GetActiveScene().name == "Backpack")
        //    {
        //        TransitionOutOFBackpack();
        //    }

            
        //}
    }

    public void LoadGameFile()
    {
        saveTest = true;
        onCheckPoint = true;
        TransitionScene(currentMap, "IntoBackpack"); //replaceCurrent scene with save scene which you have to create first
    }

    public void TransitionScene(string scenen, string transition)
    {
        StartCoroutine(Transition(scenen, transition));
    }

    IEnumerator Transition(string scene, string transition)
    {
        SetMovement(false);

        PlayAnimation(transition);
        var animation = TransitionAnmimator.GetCurrentAnimatorClipInfo(0);
        yield return new WaitForSeconds(animation[0].clip.length);
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        PlayAnimation(transition +"2");
    }

    public void SetMovement(bool move)
    {
        foreach (EnemyAI enemy in FindObjectsOfType<EnemyAI>())
        {
            enemy.noMovement = !move;

        }

        if (FindObjectOfType<PlayerMovement>())
        {
            FindObjectOfType<PlayerMovement>().noMovement = !move;
        }
    }

    public void PlayAnimation(string transitionName)
    {
        TransitionAnmimator.Play(transitionName);
    }

    public void Save_Game(int save)
    {
        Debug.Log("save game");
        SaveSystem.Save_Game(FindObjectsOfType<SaveableObject>(), FindObjectOfType<PlayerStats>(), FindObjectOfType<Backpack>(), currentSave);
        Debug.Log("save complete");
    }

    public void LoadSaveFile(int save)
    {
        if (save ==0)
        {
            save = currentSave;
        }

        SaveFile current_savefile = SaveSystem.GetSaveFile(currentSave);
        loadPositions("/checkpoint_SavePosition.test" + save.ToString());
        FindObjectOfType<PlayerStats>().LoadPlayerSave("/checkpoint_SavePlayer.test" + save.ToString());
        FindObjectOfType<Backpack>().LoadBackpackSave("/checkpoint_BackpackSave.test" + save.ToString());


    }

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!saveTest) return;

        if(onCheckPoint)
        {
            loadPositions("/checkpoint_SavePosition.test" + currentSave.ToString());
            FindObjectOfType<PlayerStats>().LoadPlayerSave("/checkpoint_SavePlayer.test" + currentSave.ToString());
            FindObjectOfType<Backpack>().LoadBackpackSave("/checkpoint_BackpackSave.test" + currentSave.ToString());
            onCheckPoint = false;
            return;
        }


        if (FindObjectOfType<PlayerStats>() != null)
        {
            FindObjectOfType<PlayerStats>().LoadPlayerSave("/playerSave.test");
        }

        if (FindObjectOfType<Backpack>() != null)
        {
            FindObjectOfType<Backpack>().LoadBackpackSave("/BackpackSave.test");
        }
            
         loadPositions("/playerSavePosition.test");
        
        
        
    }
}


