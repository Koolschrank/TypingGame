using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    public AbiltyStatsUI AbilityStatsUI;
    public UI_Abilities playerUI, backpackUI;
    public Text folderlist, abilities_p, abilities_b, actions, info;
    public SlotRow player, backpack;
    public bool testUI=true;
    public Text[] count_p, count_b;
    int slots_p, slots_b;

    private void Start()
    {
        //SetSlotRowEmpty(playerUI);
        //SetSlotRowEmpty(backpackUI);
        AbilityStatsUI.EmptyActions();
        AbilityStatsUI.EmptyAbility();
        SetPNumberEmpty();
        SetBNumberEmpty();
    }


    public void Update()
    {
        
    }

    public void SetSlotRowEmpty(UI_Abilities row)
    {
        row.Set_abilities_off();
    }

    public void setFolderList(List<Player_universal.Ability_Typ> folders)
    {


        Debug.Log("worked");
        string _string = "";
        foreach (Player_universal.Ability_Typ folder in folders)
        {
            _string += folder._name + "\n";
        }
        folderlist.text = _string;
    }

    public void setPlayerAbilities(Player_universal.Ability_Typ folder)
    {



        if (testUI)
        {
            slots_p = folder.slots;
            playerUI.Set_Abilities(folder);
            
            return;
        }
        string _string = "";
        for (int i = 0; i < folder.slots;i++)
        {
            if (folder.abilities.Count <= i || folder.abilities[i] == null)
            {
                _string += "Empty   \n";
                
            }
            else
            {
                _string += folder.abilities[i]._name + "\n";
            }

            
            
        }
        abilities_p.text = _string;
    }

    public void setBackpackAbilities(Player_universal.Ability_Typ folder)
    {
        if (testUI)
        {
            slots_b = folder.slots;
            folder.slots = folder.abilities.Count;
            backpackUI.Set_Abilities(folder);
            
            return;
        }

        string _string = "";
        for (int i = 0; i < 64; i++)
        {
            
            if (folder.abilities.Count <= i)
            {
                abilities_b.text = _string;
                return;
            }
            else if(folder.abilities[i] == null)
            {
                _string += "Empty   \n";

            }
            else
            {
                _string += folder.abilities[i]._name + "\n";
            }
        }
        abilities_b.text = _string;
    }

    public void setActions(string _text)
    {
        actions.text = _text;
    }

    public void EmptyAction()
    {
        actions.text = "";
        EmptyInfo();
    }

    public void EmptyBackpack()
    {
        if (testUI)
        {

            SetSlotRowEmpty(backpackUI);
            SetBNumberEmpty();
            return;
        }
        abilities_b.text = "";
    }

    public void EmptyPlayer()
    {
        if (testUI)
        {

            SetSlotRowEmpty(playerUI);
            SetPNumberEmpty();
            return;
        }
        Debug.Log("emptyPlayer");
        abilities_p.text = "";
    }

    public void Set_info(string _string)
    {
        info.text = _string;
    }

    public void EmptyInfo()
    {
        info.text = "";
    }

    public void SetStatUI(Ability ability, string actions)
    {
        AbilityStatsUI.SetUI(ability);
        AbilityStatsUI.SetActions(actions, "");
    }

    public void EmptyStatUI()
    {
        AbilityStatsUI.EmptyAbility();
        
    }

    public void EmptyActionUI()
    {
        AbilityStatsUI.EmptyActions();
    }

    public void SetPlayerNumber()
    {
        for (int i = 0; i < slots_p; i++)
        {
            count_p[i].text = (i+1).ToString();
        }
    }

    public void SetBackpackNumber()
    {
        for (int i = 0; i < slots_b; i++)
        {
            count_b[i].text = (i+1).ToString();
        }
    }

    public void SetPNumberEmpty()
    {
        foreach (Text _text in count_p)
        {
            _text.text = "";
        }
    }

    public void SetBNumberEmpty()
    {
        foreach (Text _text in count_b)
        {
            _text.text = "";
        }
    }
}
