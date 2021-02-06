using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackPackControle : MonoBehaviour
{
    public transitionTiles[] transitionTiles, transitionAbility;
    public Image backgroundLeft, backgroundRight;
    public Color W1, W2, M1, M2, I1, I2, B1, B2, lv1,lv2;
    public Text inventoryText, BackpackText;
    public float changeCoolDown;
    BackpackMenu BM;
    LevelUPSystem LUS;
    statsPage SP;
    state currentState = state.closed;
    bool inMenu, inDelay;
    public AudioClip openMenu, closeMenu, selectOption, useItemSound;
    AudioBox AB;

    private void Start()
    {
        BM = GetComponent<BackpackMenu>();
        LUS = GetComponentInChildren<LevelUPSystem>();
        SP = FindObjectOfType<statsPage>();
        AB = FindObjectOfType<AudioBox>();
    }

    private void Update()
    {


        PlayerInput();
        if (inMenu  && (currentState == state.opend || currentState == state.open_backpack))
            SelectMenu();
    }

    public void PlayerInput()
    {
        if (currentState == state.closed)
        {
            if (!inDelay && SceneManager.GetActiveScene().name != "Battle" && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown((KeyCode.Backspace))))
            {
                AB.PlaySound(openMenu, 1);
                StartCoroutine(StartDelay());
                OpenTransition();
                FindObjectOfType<SceneTransitioner>().SetMovement(false);
                inMenu = true;
            }

        }
        else if (currentState == state.opend)
        {
            
            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown((KeyCode.Backspace))))
            {
                FindObjectOfType<StatsInLevel>().UpdateUI();
                AB.PlaySound(closeMenu, 1);
                StartCoroutine(StartDelay());
                CloseTransition();
                FindObjectOfType<SceneTransitioner>().SetMovement(true);
                inMenu = false;
            }
        }
        else if (currentState == state.close_backpack)
        {
            if (transitionTiles[0].tile.GetState() == state.closed)
            {
                currentState = state.closed;
            }
        }
        else if (currentState == state.open_backpack)
        {
            if (transitionTiles[transitionTiles.Length - 1].tile.GetState() == state.opend)
            {
                currentState = state.opend;
            }
        }
        else if (currentState == state.inWeapon )
        {
            InWeapons();
        }
        else if (currentState == state.inMagic )
        {
            InMagic();
        }
        else if (currentState == state.inItem )
        {
            InItems();
        }
        else if (currentState == state.inBody )
        {
            InBody();
        }
        else if(currentState == state.inLevelUp)
        {
            InLevelUP();
        }
    }

    public void CloseTransition()
    {
        currentState = state.close_backpack;
        StartCoroutine(Transition(transitionTiles, state.close_backpack,true));
    }

    public void OpenTransition()
    {
        currentState = state.open_backpack;
        StartCoroutine( Transition(transitionTiles, state.open_backpack,false));
    }

    IEnumerator Transition(transitionTiles[] tiles, state _state, bool reverse)
    {
        int i = 0;
        if (reverse)
        {
            i = tiles.Length - 1;
        }
        

        while (i < tiles.Length && i>=0)
        {
            //Debug.Log(tiles[i].delay);
            yield return new WaitForSeconds(tiles[i].delay);
            Debug.Log(_state);
            tiles[i].tile.StartTimer(_state);

            if(reverse)
            {
                i--;
            }
            else
            {
                i++;
            }
        }
    }

    IEnumerator StartDelay()
    {
        inDelay = true;
        yield return new WaitForSeconds(changeCoolDown);
        inDelay = false;
    }

    public bool GetDelay()
    {
        return inDelay;
    }

    public void SelectMenu()
    {
        if(inDelay)
        {
            return;
        }

        if(Input.GetKeyDown("1"))
        {
            AB.PlaySound(openMenu, 1);
            inventoryText.text = "Weapons";
            BackpackText.text = "Backpack";
            backgroundLeft.color = W1;
            backgroundRight.color = W2;
            OpenSubMenu(transitionAbility, state.inWeapon);
            SP.SetVisible(false);
            LUS.SetVisible(false);
            BM.SetVisible(true);
            BM.CreateLists(FindObjectOfType<PlayerStats>().Ability_typs[0], FindObjectOfType<Backpack>().abilities[0]);
            StartCoroutine(StartDelay());
        }
        if (Input.GetKeyDown("2"))
        {
            AB.PlaySound(openMenu, 1);
            inventoryText.text = "Magic";
            BackpackText.text = "Backpack";
            backgroundLeft.color = M1;
            backgroundRight.color = M2;
            OpenSubMenu(transitionAbility, state.inMagic);
            SP.SetVisible(false);
            LUS.SetVisible(false);
            BM.SetVisible(true);
            BM.CreateLists(FindObjectOfType<PlayerStats>().Ability_typs[1], FindObjectOfType<Backpack>().abilities[1]);
            StartCoroutine(StartDelay());
        }
        if (Input.GetKeyDown("3"))
        {
            AB.PlaySound(openMenu, 1);
            inventoryText.text = "Items";
            BackpackText.text = "Backpack";
            backgroundLeft.color = I1;
            backgroundRight.color = I2;
            OpenSubMenu(transitionAbility, state.inItem);
            SP.SetVisible(false);
            LUS.SetVisible(false);
            BM.SetVisible(true);
            BM.CreateLists(FindObjectOfType<PlayerStats>().Ability_typs[3], FindObjectOfType<Backpack>().abilities[3]);
            StartCoroutine(StartDelay());
        }
        if (Input.GetKeyDown("4"))
        {
            AB.PlaySound(openMenu, 1);
            inventoryText.text = "Bodies";
            BackpackText.text = "Backpack";
            backgroundLeft.color = B1;
            backgroundRight.color = B2;
            OpenSubMenu(transitionAbility, state.inBody);
            SP.SetVisible(false);
            LUS.SetVisible(false);
            BM.SetVisible(true);
            BM.CreateBodyLists(FindObjectOfType<PlayerStats>().playerBodies, FindObjectOfType<Backpack>().playerBodies);
            StartCoroutine(StartDelay());
        }
        if (Input.GetKeyDown("5"))
        {
            AB.PlaySound(openMenu, 1);
            inventoryText.text = "Level up";
            BackpackText.text = "Stats";
            backgroundLeft.color = lv1;
            backgroundRight.color = lv2;
            OpenSubMenu(transitionAbility, state.inLevelUp);
            SP.SetVisible(true);
            LUS.SetVisible(true);
            LUS.SetActive(true);
            BM.SetVisible(false);
            StartCoroutine(StartDelay());

        }
    }

    public void OpenSubMenu(transitionTiles[] tiles, state newState)
    {
        StartCoroutine(Transition(tiles, state.open_backpack, false));
        currentState = newState;
        BM.currenAbilityTyp = currentState;
    }

    public void ReturnToMenu(transitionTiles[] tiles)
    {
        currentState = state.opend;
        Debug.Log(currentState);
        StartCoroutine(Transition(tiles, state.close_backpack, true));
    }

    public void InWeapons()
    {
        

        BM.MenuActive();

        
    }



    public void InMagic()
    {
        BM.MenuActive();

    }

    public void InItems()
    {

        BM.MenuActive();
    }

    public void InBody()
    {
        BM.MenuActive();

    }

    public void InLevelUP()
    {
        LUS.MenuActive();

    }

    public void BackToMainMenu(state last_state)
    {
        StartCoroutine(StartDelay());
        Debug.Log("some body once told me the world ");
        switch(last_state)
        {
            case state.inWeapon:
                ReturnToMenu(transitionAbility);
                break;
            case state.inMagic:
                ReturnToMenu(transitionAbility);
                break;
            case state.inItem:
                ReturnToMenu(transitionAbility);
                break;
            case state.inBody:
                ReturnToMenu(transitionAbility);
                break;
            case state.inLevelUp:
                SP.SetVisible(false);
                LUS.SetActive(false);
                ReturnToMenu(transitionAbility);
                break;
        }
        
    }

}

[System.Serializable]
public class transitionTiles
{
    public BackpackTransition tile;
    public float delay;
}
