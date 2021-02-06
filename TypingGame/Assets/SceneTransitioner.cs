using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

public class SceneTransitioner : MonoBehaviour
{
    Animator TransitionAnmimator;
    Settings settings;
    public List<GameObject> Enemies = new List<GameObject>();
    public SaveableObject[] savableObjects;
    public bool saveTest; // delete later
    public bool onCheckPoint;
    public AudioClip BattleTransitionSound;
    bool levelTransition;

    private void Awake()
    {
        Debug.Log("awake");


        settings = GetComponent<Settings>();
        settings._scene = SceneManager.GetActiveScene().name;
        var SaveDatas = FindObjectsOfType<SceneTransitioner>();
        if (SaveDatas.Length > 1)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        TransitionAnmimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        settings._scene = SceneManager.GetActiveScene().name;
        
        Debug.Log("start");
        if(saveTest )
        {
            LoadSaveFile(settings.currentSave);
        }
        else
        {
            Debug.Log("bruhhhh");
            FindObjectOfType<SceneTransitioner>().loadPositions("/playerSavePosition.test");
        }
        
        if(FindObjectOfType<AudioBox>())
        FindObjectOfType<AudioBox>().PlayBackgroundMusic(0);
    }

    public void Save_Positions(string saveName)
    {

        if (SceneManager.GetActiveScene().name != settings._scene) return;
        savableObjects = FindObjectsOfType<SaveableObject>();
        SaveSystem.SavePosition(savableObjects,false, "/playerSavePosition.test", SaveSystem.loadPostion("/checkpoint_SavePosition.test" + settings.currentSave.ToString()));
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
            Debug.Log("pls do nothing");
            return;
        }
        
        savableObjects = FindObjectsOfType<SaveableObject>();
        var _save = SaveSystem.loadPostion(saveName);
        if(_save == null)
        {
            Debug.Log("do nothing");
            return;
        }
        Debug.Log(_save.Get_Index().Length);
        var index = _save.Get_Index();
        List<string> listList = new List<string>(index);
        var x = _save.Get_x();
        var y = _save.Get_y();
        var inGame = _save.Get_In_game();
        for (int i =0; i < savableObjects.Length;i++)
        {
            if(onCheckPoint && !savableObjects[i].stay_after_reloard)
            {

            }
            else
            {
                int rightIndex = listList.IndexOf(savableObjects[i].uniqueIdentifier);
                if (rightIndex == -1)
                {

                }
                else
                {
                    savableObjects[i].transform.position = new Vector2(x[rightIndex], y[rightIndex]);
                    savableObjects[i].inGame = inGame[rightIndex];
                }
            }
            

        }
        //if (FindObjectOfType<LevelTransition>() && levelTransition)
        //{
        //    FindObjectOfType<LevelTransition>().placePlayer();
        //    levelTransition = false;
        //}
    }

    public void TransitionBattle(List<GameObject> _Enemies, int battleTheme)
    {
        FindObjectOfType<AudioBox>().PlaySound(BattleTransitionSound, 1);
        FindObjectOfType<AudioBox>().PlayBattleMusic(battleTheme);
        settings._scene = SceneManager.GetActiveScene().name;
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
        TransitionScene(settings._scene, "IntoBackpack");
        Debug.Log("why would you transition out of battle");
        loadPositions("/playerSavePosition.test");


    }

    public void TransitionBackpack()
    {
        settings._scene = SceneManager.GetActiveScene().name;
        saveTest = true;
        Debug.Log("why would you do backpack");
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
        TransitionScene(settings._scene, "IntoBackpack");
        
    }

    private void Update()
    {
        if (Input.GetKeyDown("9"))
        {
            Save_Game(settings.currentSave);
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
        FindObjectOfType<AudioBox>().PlayBackgroundMusic(0);
        saveTest = true;
        onCheckPoint = true;
        TransitionScene(settings._scene, "IntoBackpack"); //replaceCurrent scene with save scene which you have to create first
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

    IEnumerator Transition(int scene, string transition)
    {
        
        SetMovement(false);

        PlayAnimation(transition);
        var animation = TransitionAnmimator.GetCurrentAnimatorClipInfo(0);
        yield return new WaitForSeconds(animation[0].clip.length);
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        PlayAnimation(transition + "2");
        onCheckPoint=(true);
        Save_Game(settings.currentSave);
        LoadSaveFile(settings.currentSave);

        SetMovement(true);
        FindObjectOfType<LevelTransition>().placePlayer();
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
        Debug.Log(transitionName);
        TransitionAnmimator.Play(transitionName);
    }

    public void Save_Game(int save)
    {
        Debug.Log("save game");
        SaveSystem.Save_Game(FindObjectsOfType<SaveableObject>(), FindObjectOfType<PlayerStats>(), FindObjectOfType<Backpack>(), settings, save);
        Debug.Log("save complete");
    }

    public void LoadSaveFile(int save)
    {


        SaveFile current_savefile = SaveSystem.GetSaveFile(save);
        loadPositions("/checkpoint_SavePosition.test" + save.ToString());
        FindObjectOfType<PlayerStats>().LoadPlayerSave("/checkpoint_SavePlayer.test" + save.ToString());
        FindObjectOfType<Backpack>().LoadBackpackSave("/checkpoint_BackpackSave.test" + save.ToString());
        settings.OverRideSetting("/checkpoint_SettingsSave.test" + save.ToString());


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
            loadPositions("/checkpoint_SavePosition.test" + settings.currentSave.ToString());
            FindObjectOfType<PlayerStats>().LoadPlayerSave("/checkpoint_SavePlayer.test" + settings.currentSave.ToString());
            FindObjectOfType<Backpack>().LoadBackpackSave("/checkpoint_BackpackSave.test" + settings.currentSave.ToString());
            settings.OverRideSetting("/checkpoint_SettingsSave.test" + settings.currentSave.ToString());
            onCheckPoint = false;
            return;
        }
        else
        {
            if (FindObjectOfType<PlayerStats>() != null)
            {
                FindObjectOfType<PlayerStats>().LoadPlayerSave("/playerSave.test");
            }

            if (FindObjectOfType<Backpack>() != null)
            {
                FindObjectOfType<Backpack>().LoadBackpackSave("/BackpackSave.test");
            }
            Debug.Log("why would you do that");
            loadPositions("/playerSavePosition.test");
        }


        
        
        
        
    }

    public void TransitionToLevel(int i)
    {
        saveTest = true;
        onCheckPoint = true;
        Save_Game(settings.currentSave);
        LoadSaveFile(settings.currentSave);
        string pathToScene = SceneUtility.GetScenePathByBuildIndex(i);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(pathToScene);
        Debug.Log(sceneName);
        settings._scene = sceneName;
        Save_Backpack("/BackpackSave.test");
        FindObjectOfType<PlayerStats>().SavePlayer("/playerSave.test");
        StartCoroutine(Transition(i, "IntoBackpack"));
        levelTransition = true;
    }

    public void LoadGameNew(int i)
    {
        LoadSaveFile(i);
        LoadGameFile();

    }

    IEnumerator LoadGameNewTransition(string transition, int newSave)
    {
        SetMovement(false);

        PlayAnimation(transition);
        var animation = TransitionAnmimator.GetCurrentAnimatorClipInfo(0);
        
        yield return new WaitForSeconds(animation[0].clip.length);
        LoadSaveFile(newSave);
        SceneManager.LoadScene(settings._scene, LoadSceneMode.Single);
        PlayAnimation(transition + "2");
    }
}




