using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySwitcher : MonoBehaviour
{
    PlayerStats playerStats;
    Backpack backpack;
    public GameObject UI;
    public Player_universal.Ability_Typ SelectedFolder_p, SelectedFolder_b;
    public Ability ability_selected;
    bool SIB;
    public bool Seach_in_backpack { set { SIB = value; SetNumbers(value); } get { return SIB; } }
    int rowIntAdd;
    currentSelection CS = currentSelection.selectTyp;

    enum currentSelection
    {
        selectTyp,
        selectAbility,
        selectAction,
        selectSwap,
        selectAdd,
    }

    public void Start()
    {
       
        playerStats = GetComponent<PlayerStats>();
        backpack = GetComponent<Backpack>();
        SetStartUI();
    }

    public void SetStartUI()
    {
        List<Player_universal.Ability_Typ> typs = new List<Player_universal.Ability_Typ>();
        typs.Add(playerStats.Ability_typs[0]);
        typs.Add(playerStats.Ability_typs[1]);
        typs.Add(playerStats.Ability_typs[3]);
        UI.GetComponent<TestUI>().setFolderList(typs);

    }

    public void PlayerAbilityUI()
    {
        UI.GetComponent<TestUI>().setPlayerAbilities(SelectedFolder_p);
    }

    public void BackPackAbilityUI()
    {
        Debug.Log(SelectedFolder_b._name);
        UI.GetComponent<TestUI>().setBackpackAbilities(SelectedFolder_b);
    }

    public string ActionUI(Ability ability)
    {
        string _string = "";
        _string += "1 Swap  \n";
        switch (ability._typ)
        {
            case ability_typ.mp_heal:
            case ability_typ.item_heal:
            case ability_typ.heal:
                _string += "2 Use";
                break;
        }

        
        return _string;
    }

    public void Update()
    {
        switch(CS)
        {
            case currentSelection.selectTyp:
                SelectTyp();
                break;
            case currentSelection.selectAbility:
                SelectAbility(Seach_in_backpack);
                break;
            case currentSelection.selectAction:
                SelectAction();
                break;
            case currentSelection.selectSwap:
                SelectAbility(Seach_in_backpack);
                break;
            case currentSelection.selectAdd:
                SelectAbility(Seach_in_backpack);
                break;
        }

        
    }

    public void GoToAbilitySelector(int typ)
    {
        SelectedFolder_p = playerStats.Ability_typs[typ];
        SelectedFolder_b = backpack.abilities[typ];
        BackPackAbilityUI();
        PlayerAbilityUI();
        ability_selected = null;
        CS = currentSelection.selectAbility;
        Seach_in_backpack = false;

    }

    public void GoToAbilitySelector(Player_universal.Ability_Typ SelectedFolder_1, Player_universal.Ability_Typ SelectedFolder_2)
    {
        SelectedFolder_p = SelectedFolder_1;
        SelectedFolder_b = SelectedFolder_2;
        BackPackAbilityUI();
        PlayerAbilityUI();
        
        ability_selected = null;
        CS = currentSelection.selectAbility;
        UI.GetComponent<TestUI>().EmptyAction();
        Debug.Log(SelectedFolder_p._name);
        SetNumbers(Seach_in_backpack);
    }

    public void GoToSelectTyp()
    {
        SelectedFolder_p = null;
        SelectedFolder_p = null;
        ability_selected = null;
        CS = currentSelection.selectTyp;
        UI.GetComponent<TestUI>().EmptyBackpack();
        UI.GetComponent<TestUI>().EmptyPlayer();
    }
    
    public void GoToSwapScreen()
    {
        Seach_in_backpack = !Seach_in_backpack;
        CS =currentSelection.selectSwap;
        UI.GetComponent<TestUI>().EmptyActionUI();
    }

    public void GoToAdd()
    {
        Seach_in_backpack = true;
        CS = currentSelection.selectAdd;
        UI.GetComponent<TestUI>().EmptyAction();
    }

    public void GoToSelectAction(int ability_int, bool fromBackPack)
    {
        if (fromBackPack)
        {
            ability_selected = SelectedFolder_b.abilities[ability_int];
        }
        else
        {
            ability_selected = SelectedFolder_p.abilities[ability_int];
        }
        UI.GetComponent<TestUI>().SetStatUI(ability_selected, ActionUI(ability_selected));
        CS = currentSelection.selectAction;
        UI.GetComponent<TestUI>().SetPNumberEmpty();
        UI.GetComponent<TestUI>().SetBNumberEmpty();
        Debug.Log(ability_selected._name);
        
    }

    public void GoToSelectAction(Ability ability)
    {
        Seach_in_backpack = !Seach_in_backpack;
        ability_selected = ability;
        UI.GetComponent<TestUI>().SetStatUI(ability_selected, ActionUI(ability_selected));
        CS = currentSelection.selectAction;
        UI.GetComponent<TestUI>().SetPNumberEmpty();
        UI.GetComponent<TestUI>().SetBNumberEmpty();
        Debug.Log(ability_selected._name);

    }

    public void AbilitySelected(int ability_int, bool fromBackPack)
    {
        var _newAbility = new Ability();
        if (fromBackPack)
        {
            if (SelectedFolder_b.abilities.Count <= ability_int)
            {
                Debug.Log("not posible");
                return;
            }
            _newAbility = SelectedFolder_b.abilities[ability_int];
        }
        else
        {
            if (SelectedFolder_p.abilities.Count <= ability_int && SelectedFolder_p.slots>= ability_int)
            {
                GoToAdd();
                return;
            }
            else if (SelectedFolder_p.abilities.Count <= ability_int)
            {
                Debug.Log("not posible");
                return;
            }
            _newAbility = SelectedFolder_p.abilities[ability_int];
            if (_newAbility ==null)
            {
                GoToSwapScreen();
            }
        }

        switch (CS)
        {
            case currentSelection.selectSwap:
                Action_swap(_newAbility);
                break;
            case currentSelection.selectAbility:
                GoToSelectAction(ability_int, fromBackPack);
                break;
            case currentSelection.selectAdd:
                Action_Add(_newAbility);
                break;
        }
    }


    public void Action_use(Ability ability)
    {
        if (ability._cost_typ == Cost.mp )
        {
            if(playerStats.mp < ability._cost)
            {
                Debug.Log("not enoght mana");
                return;
            }
            playerStats.mp -= ability._cost;
        }
        switch (ability._typ)
        {
            case ability_typ.heal:
                playerStats.hp = Mathf.Min(playerStats.hp_max, playerStats.hp + ability.power + playerStats.intellect / 2);
                break;
            case ability_typ.item_heal:
                playerStats.hp = Mathf.Min(playerStats.hp_max, playerStats.hp + ability.power);
                break;
            case ability_typ.mp_heal:
                playerStats.mp = Mathf.Min(playerStats.mp_max, playerStats.mp + ability.power);
                break;
        }
        if(ability._cost_typ == Cost.consumable)
        {

            SelectedFolder_p.abilities.Remove(ability);
            UI.GetComponent<TestUI>().EmptyActionUI();
            UI.GetComponent<TestUI>().EmptyStatUI();
            GoToAbilitySelector(SelectedFolder_p, SelectedFolder_b);
        }

        
    }

    public void Action_swap(Ability ability)
    {
        Debug.Log("swap");
        


        int swap1, swap2;
        if (Seach_in_backpack)
        {
             swap1 = SelectedFolder_p.abilities.IndexOf(ability_selected);
             swap2 = SelectedFolder_b.abilities.IndexOf(ability);
        }
        else
        {
             swap1 = SelectedFolder_p.abilities.IndexOf(ability);
             swap2 = SelectedFolder_b.abilities.IndexOf(ability_selected);
        }
        Debug.Log(SelectedFolder_p.abilities[swap1]._name);
        Debug.Log(SelectedFolder_b.abilities[swap2]._name);
        var newAbility1 = SelectedFolder_p.abilities[swap1];
        var newAbility2 = SelectedFolder_b.abilities[swap2];
        SelectedFolder_p.abilities[swap1] = newAbility2;
        SelectedFolder_b.abilities[swap2] = newAbility1;

        Seach_in_backpack = !Seach_in_backpack;
        GoToAbilitySelector(SelectedFolder_p, SelectedFolder_b);
        UI.GetComponent<TestUI>().EmptyStatUI();

    }

    public void Action_Add(Ability ability)
    {
        Debug.Log("add");

        SelectedFolder_p.abilities.Add(ability);
        SelectedFolder_b.abilities.Remove(ability);
        

        Seach_in_backpack = !Seach_in_backpack;
        GoToAbilitySelector(SelectedFolder_p, SelectedFolder_b);

    }

    public void Action_remove()
    {

    }

    public void Action_info()
    {

    }

    public void SelectTyp()
    {
        if (Input.GetKeyDown("1"))
        {
            GoToAbilitySelector(0);
            Debug.Log("Weapons");
        }
        else if (Input.GetKeyDown("2"))
        {
            GoToAbilitySelector(1);
            Debug.Log("Magic");
        }
        else if(Input.GetKeyDown("3"))
        {
            GoToAbilitySelector(3);
            Debug.Log("Items");
        }
        else if(Input.GetKeyDown("4"))
        {

        }
        else if (Input.GetKeyDown("5"))
        {

        }
        else if (Input.GetKeyDown("6"))
        {

        }
        else if (Input.GetKeyDown("7"))
        {

        }
        else if (Input.GetKeyDown("8"))                                         
        {

        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            
        }
    }

    public void SelectAbility(bool in_backpack)
    {
        if (Input.GetKeyDown("1"))
        {
            AbilitySelected(0 + rowIntAdd, in_backpack);
        }
        else if (Input.GetKeyDown("2"))
        {
            AbilitySelected(1 + rowIntAdd, in_backpack);
        }
        else if (Input.GetKeyDown("3"))
        {
            AbilitySelected(2 + rowIntAdd, in_backpack);
        }
        else if (Input.GetKeyDown("4"))
        {
            AbilitySelected(3 + rowIntAdd, in_backpack);
        }
        else if (Input.GetKeyDown("5"))
        {
            AbilitySelected(4 + rowIntAdd, in_backpack);
        }
        else if (Input.GetKeyDown("6"))
        {
            AbilitySelected(5 + rowIntAdd, in_backpack);
        }
        else if (Input.GetKeyDown("7"))
        {
            AbilitySelected(6 + rowIntAdd, in_backpack);
        }
        else if (Input.GetKeyDown("8"))
        {
            AbilitySelected(7 + rowIntAdd, in_backpack);
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            switch(CS)
            {
                case currentSelection.selectSwap:
                    GoToSelectAction(ability_selected);
                    
                    break;
                case currentSelection.selectAbility:
                    GoToSelectTyp();
                    break;
            }
        }
    }

    public void SelectAction()
    {
        if (Input.GetKeyDown("1"))
        {
            GoToSwapScreen();
        }
        else if (Input.GetKeyDown("2"))
        {
            switch (ability_selected._typ)
            {
                case ability_typ.mp_heal:
                case ability_typ.item_heal:
                case ability_typ.heal:
                    Action_use(ability_selected);
                    break;
            }
        }
        else if (Input.GetKeyDown("3"))
        {

        }
        else if (Input.GetKeyDown("4"))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            UI.GetComponent<TestUI>().EmptyActionUI();
            UI.GetComponent<TestUI>().EmptyStatUI();
            GoToAbilitySelector(SelectedFolder_p, SelectedFolder_b);
        }
    }

    public void SetNumbers(bool backpack)
    {
        
        if (!backpack)
        {
            UI.GetComponent<TestUI>().SetPlayerNumber();
            UI.GetComponent<TestUI>().SetBNumberEmpty();
        }
        else
        {
            UI.GetComponent<TestUI>().SetBackpackNumber();
            UI.GetComponent<TestUI>().SetPNumberEmpty();
        }
    }
}
