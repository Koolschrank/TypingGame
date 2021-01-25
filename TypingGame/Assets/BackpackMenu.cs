using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackMenu : MonoBehaviour
{
    public List<AbilityUIList> abilitieLists = new List<AbilityUIList>(), abilitieLists_u = new List<AbilityUIList>();
    public AbiltyStatsUI ASUI;
    public playerBodyUI PBUI;
    public int currentList = 0;
    public Text pageNumberText;
    public SelectedItem currentItem;
    public currentSelection selectTyp = currentSelection.selectAbility;
    public state currenAbilityTyp = state.inWeapon;
    public GameObject cursor;
    public AbilityFolder AllAbilities;
    public AudioClip openMenu, closeMenu, selectOption, useItemSound;
    AudioBox AB;


    void Start()
    {
        ShowPage(1);
        AB = FindObjectOfType<AudioBox>();
    }

    public void SetVisible(bool visible)
    {
        foreach(AbilityUIList abilityList  in abilitieLists)
        {
            abilityList.SetVisible(visible);
        }
    }

    public void MenuActive()
    {
        UpdaterCursorPosition();

        switch (selectTyp)
        {
            case currentSelection.selectAbility:
                SelectAbility();
                break;
            case currentSelection.selectAction:
                SelectAction();
                break;
            case currentSelection.selectSwap:
                SelectSwap();
                break;
            case currentSelection.selectAdd:
                SelectAdd();
                break;
        }

    }

    public void UpdaterCursorPosition()
    {
        if(selectTyp == currentSelection.selectAction)
        {
            cursor.SetActive(false);
            return;
        }
        cursor.SetActive(true);

        cursor.transform.position = abilitieLists_u[currentList].transform.position;
    }

    public void SelectAbility()
    {
        MoveCursor();
        SelectItem();
    }

    public void UseItem(SelectedItem item)
    {
        AB.PlaySound(useItemSound, 1);
        var ability = item.headList.abilities[item.index];
        var player = FindObjectOfType<PlayerStats>();

        if (ability._cost_typ == Cost.mp)
        {
            if (player.mp < ability._cost)
            {
                Debug.Log("not enoght mana");
                return;
            }
            player.mp -= ability._cost;
        }
        switch (ability._typ)
        {
            case ability_typ.heal:
                player.hp = Mathf.Min(player.hp_max, player.hp + ability.power + player.intellect / 2);
                break;
            case ability_typ.item_heal:
                player.hp = Mathf.Min(player.hp_max, player.hp + ability.power);
                break;
            case ability_typ.mp_heal:
                player.mp = Mathf.Min(player.mp_max, player.mp + ability.power);
                break;
        }
        if (ability._cost_typ == Cost.consumable)
        {
            item.headList.displays[item.index].Select(false);
            item.headList.abilities[item.index]= null;
            UpdateUI();
            //SelectedFolder_p.abilities.Remove(ability);
            //UI.GetComponent<TestUI>().EmptyActionUI();
            //UI.GetComponent<TestUI>().EmptyStatUI();
            //GoToAbilitySelector(SelectedFolder_p, SelectedFolder_b);
            selectTyp = currentSelection.selectAbility;
            ASUI.SetVisible(false);
            PBUI.SetVisible(false);
        }


    }

    public void SelectAction()
    {
        
        if (Input.GetKeyDown("1"))
        {
            GoToSwap();
        }
        else if (Input.GetKeyDown("2") && currentItem.headList.abilities[currentItem.index]._target == Target.self && (currentItem.headList.abilities[currentItem.index]._typ == ability_typ.heal || currentItem.headList.abilities[currentItem.index]._typ == ability_typ.mp_heal || currentItem.headList.abilities[currentItem.index]._typ == ability_typ.item_heal))
        {
            UseItem(currentItem);   
        }


        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            GoBack();
        }
    }

    public void SelectSwap()
    {
        MoveCursor();
        SelectItem();
    }

    public void SelectAdd()
    {
        MoveCursor();
        SelectItem();
    }

    public void Add(SelectedItem newItem, SelectedItem emptySlot)
    {
        newItem.headList.displays[newItem.index].Select(false);
        emptySlot.headList.displays[emptySlot.index].Select(false);
        Ability ability = newItem.headList.abilities[newItem.index];
        emptySlot.headList.abilities.Add(ability);
        newItem.headList.abilities[newItem.index] = null;
        selectTyp = currentSelection.selectAbility;
        UpdateUI();
        if (currentList == 0)
        {
            currentList = 1;
            ShowPage(currentList);
        }
        else
        {
            currentList = 0;
        }
    }

    public void GoToAdd(SelectedItem cItem)
    {
        currentItem = cItem;
        if (currentList == 0)
        {
            currentList = 1;
            ShowPage(currentList);
        }
        else
        {
            currentList = 0;
        }
        selectTyp = currentSelection.selectAdd;
    }

    public void AddAbility()
    {

        MoveCursor();
        SelectItem();
    }

    public void GoToSwap()
    {
        if(currentList ==0)
        {
            currentList = 1;
            ShowPage(currentList);
        }
        else
        {
            currentList = 0;
        }
        ASUI.SetVisible(false);
        PBUI.SetVisible(false);
        selectTyp = currentSelection.selectSwap;
    }

    public void SwapAbility(SelectedItem item1, SelectedItem item2)
    {
        item1.headList.displays[item1.index].Select(false);
        item2.headList.displays[item2.index].Select(false);
        if (item1.headList.abilities[item1.index] ==null)
        {
            SwapWithEmpty(item1, item2);
            return;
        }
        else if(item2.headList.abilities[item2.index] == null)
        {
            SwapWithEmpty(item2, item1);
            return;
        }


        Ability ability1 = item1.headList.abilities[item1.index];
        Ability ability2 = item2.headList.abilities[item2.index];

        item1.headList.abilities[item1.index] = ability2;
        item2.headList.abilities[item2.index] = ability1;

        Debug.Log(item1.headList.abilities[item1.index]._name);
        Debug.Log(item2.headList.abilities[item2.index]._name);
        selectTyp = currentSelection.selectAbility;
        if(currentList != 0)
        {
            currentList = 0;
        }

        UpdateUI();
    }

    public void SwapWithEmpty(SelectedItem empty, SelectedItem item)
    {
        Ability ability = item.headList.abilities[item.index];

        empty.headList.abilities[empty.index] = ability;
        item.headList.abilities[item.index] = null;

        selectTyp = currentSelection.selectAbility;
        if (currentList != 0)
        {
            currentList = 0;
        }

        UpdateUI();
    }

    public void ItemSelected(SelectedItem SI)
    {
        currentItem = SI;
        if(currenAbilityTyp == state.inBody)
        {
            PBUI.SetVisible(true);
            PBUI.ShowBody(SetBodyUI(SI));
        }
        else
        {
            ASUI.SetUI(currentItem.headList.abilities[SI.index]);
            ASUI.SetVisible(true);
        }
        
        selectTyp = currentSelection.selectAction;
        SetActionText(SI.headList.abilities[SI.index]);
    }

    public PlayerBody SetBodyUI(SelectedItem SI)
    {
        string _string = SI.headList.abilities[SI.index]._name;
        List<PlayerBody> bodies = new List<PlayerBody>();


        foreach (PlayerBody body in AllAbilities.playerBodies)
        {
            if (_string != null && body._name == _string)
            {
                return body;
            }

        }
        return null;
    }

    public void MoveCursor()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentList = Mathf.Max(0, currentList - 1);
            if (currentList != 0)
            {
                ShowPage(currentList);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentList = Mathf.Min(abilitieLists_u.Count - 1, currentList + 1);
            if (currentList != 0)
            {
                ShowPage(currentList);
            }
        }
    }

    public void ShowPage(int pageNumber)
    {
        for(int i =1; i < abilitieLists.Count; i++)
        {
            if (pageNumber == i)
            {
                abilitieLists[i].SetVisible(true);
            }
            else
            {
                abilitieLists[i].SetVisible(false);
            }
        }
        if (abilitieLists_u.Count > 2)
            pageNumberText.text = (pageNumber).ToString() + " / " + (abilitieLists_u.Count - 1).ToString();
        else
            pageNumberText.text = "";
        
    }

    public void SelectItem()
    {
        if (Input.GetKeyDown("1"))
        {
            ItemSelected(CreateItemSave(abilitieLists_u[currentList], 0), selectTyp);
        }
        else if (Input.GetKeyDown("2"))
        {
            ItemSelected(CreateItemSave(abilitieLists_u[currentList], 1), selectTyp);
        }
        else if (Input.GetKeyDown("3"))
        {
            ItemSelected(CreateItemSave(abilitieLists_u[currentList], 2), selectTyp);
        }
        else if (Input.GetKeyDown("4"))
        {
            ItemSelected(CreateItemSave(abilitieLists_u[currentList], 3), selectTyp);
        }
        else if (Input.GetKeyDown("5"))
        {
            ItemSelected(CreateItemSave(abilitieLists_u[currentList], 4), selectTyp);
        }
        else if (Input.GetKeyDown("6"))
        {
            ItemSelected(CreateItemSave(abilitieLists_u[currentList], 5), selectTyp);
        }
        else if (Input.GetKeyDown("7"))
        {
            ItemSelected(CreateItemSave(abilitieLists_u[currentList], 6), selectTyp);
        }
        else if (Input.GetKeyDown("8"))
        {
            ItemSelected(CreateItemSave(abilitieLists_u[currentList], 7), selectTyp);
        }
        if (Input.GetKeyDown(KeyCode.Backspace)|| Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }

    public void GoBack()
    {

        cursor.SetActive(false);
        switch (selectTyp)
        {
            case currentSelection.selectAbility:
                if (!FindObjectOfType<BackPackControle>().GetDelay())
                { CloseMenu(); }
                break;
            case currentSelection.selectAction:
                PBUI.SetVisible(false);
                ASUI.SetVisible(false);
                currentItem.headList.displays[currentItem.index].Select(false);
                currentItem = null;
                selectTyp = currentSelection.selectAbility;
                break;
            case currentSelection.selectSwap:
                if(currenAbilityTyp == state.inBody)
                {
                    PBUI.SetVisible(true);
                }
                else
                {
                    ASUI.SetVisible(true);
                }
                
                selectTyp = currentSelection.selectAction;
                break;
            case currentSelection.selectAdd:
                PBUI.SetVisible(false);
                ASUI.SetVisible(false);
                currentItem = null;
                selectTyp = currentSelection.selectAbility;
                break;
        }
    }

    public void CloseMenu()
    {


        AB.PlaySound(closeMenu, 1);
        switch (currenAbilityTyp)
        {
            case state.inWeapon:
                ReturnToPlayer(abilitieLists_u[0], 0);
                abilitieLists_u.RemoveAt(0);
                ReturnToBackpack(abilitieLists_u, 0);
                break;
            case state.inMagic:
                ReturnToPlayer(abilitieLists_u[0], 1);
                abilitieLists_u.RemoveAt(0);
                ReturnToBackpack(abilitieLists_u, 1);
                break;
            case state.inItem:
                ReturnToPlayer(abilitieLists_u[0], 3);
                abilitieLists_u.RemoveAt(0);
                ReturnToBackpack(abilitieLists_u, 3);
                break;
            case state.inBody:
                ReturnToPlayerBody(TurnToBodyList(abilitieLists_u[0].abilities));
                
                ReturnToBackpackBody(TurnToBodyList(abilitieLists_u[1].abilities)); // fix that please later maeby
                var player = FindObjectOfType<PlayerStats>();
                player.SetBody(player.playerBodies[ player.currentBody]);
                break;
        }
        FindObjectOfType<BackPackControle>().BackToMainMenu(currenAbilityTyp);
    }

    public void ReturnToPlayer(AbilityUIList list, int playerList)
    {
        var player = FindObjectOfType<PlayerStats>();

        var PL = player.Ability_typs[playerList];
        PL.abilities.Clear();
        foreach (Ability ability in list.abilities)
        {
            if(ability != null)
            PL.abilities.Add(ability);
        }
    }

    public void ReturnToPlayerBody(List<PlayerBody> list)
    {
        var player = FindObjectOfType<PlayerStats>();
        player.playerBodies = list;
    }

    public void ReturnToBackpackBody(List<PlayerBody> list)
    {
        var backpack = FindObjectOfType<Backpack>();
        backpack.playerBodies = list;
    }

    public void ReturnToBackpack(List<AbilityUIList> lists, int playerList)
    {
        var backpack = FindObjectOfType<Backpack>();
        var BL = backpack.abilities[playerList];
        BL.abilities.Clear();
        foreach (AbilityUIList list in lists)
        {
            foreach (Ability ability in list.abilities)
            {
                if (ability != null)
                    BL.abilities.Add(ability);
            }
        }

    }

    public void ItemSelected(SelectedItem cItem,currentSelection CS)
    {
        AB.PlaySound(selectOption, 1);
        Debug.Log(abilitieLists_u[currentList].displays.Length);
        if(abilitieLists_u[currentList].displays[cItem.index]._number.text =="")
        {
            Debug.Log("emptySpaceBro");
            return;
        }
        else if(abilitieLists_u[currentList].abilities.Count<= cItem.index)
        {
            if(selectTyp == currentSelection.selectAbility)
            {
                abilitieLists_u[currentList].displays[cItem.index].Select(true);
                GoToAdd(cItem);
                return;
            }
            else if (selectTyp == currentSelection.selectSwap)
            {
                Add(currentItem, cItem);
                return;
            }
            else
            {
                Debug.Log("emptySpaceBro");
                return;
            }
            
        }
        else if(abilitieLists_u[currentList].abilities[cItem.index]== null)
        {
            if (selectTyp == currentSelection.selectAbility)
            {
                abilitieLists_u[currentList].displays[cItem.index].Select(true);
                currentItem = cItem;
                GoToSwap();
                return;
            }
            else if (selectTyp == currentSelection.selectSwap)
            {
                SwapAbility(currentItem, cItem);
                return;
            }
            else
            {
                Debug.Log("emptySpaceBro");
                return;
            }
        }


        switch (selectTyp)
        {
            case currentSelection.selectAbility:
                abilitieLists_u[currentList].displays[cItem.index].Select(true);
                ItemSelected(cItem);
                break;
            case currentSelection.selectAction:
                
                break;
            case currentSelection.selectSwap:
                SwapAbility(cItem, currentItem);
                break;
            case currentSelection.selectAdd:
                Add(cItem, currentItem);
                break;
        }
    }

    public void SetActionText(Ability _ability)
    {
        Debug.Log(_ability._name);
        string _text = "";
        string _number = "";

        _text += "Switch \n";
        _number += "1 \n";


        if (_ability._target == Target.self && (_ability._typ == ability_typ.heal|| _ability._typ == ability_typ.mp_heal || _ability._typ == ability_typ.item_heal))
        {
            _text += "Use \n";
            _number += "2 \n";
        }

        ASUI.action.text = _text;
        ASUI.action_number.text = _number;
    }

    public void CreateLists(Player_universal.Ability_Typ playerSlots, Player_universal.Ability_Typ backpackSlots)
    {
        currentList = 0;
        abilitieLists_u.Clear();
        abilitieLists[0].SetAbilities(playerSlots.abilities, playerSlots.slots);
        abilitieLists_u.Add(abilitieLists[0]);

        int fillInt = 0;
        List<Ability> abilitiesToAdd = new List<Ability>();
        for(int i =0; i < backpackSlots.abilities.Count; i++ )
        {
            if(i % 8 ==0&& i != 0)
            {
                
                fillInt++;
                Debug.Log(abilitieLists[fillInt].name);
                abilitieLists[fillInt].SetAbilities(abilitiesToAdd, abilitiesToAdd.Count);
                abilitieLists_u.Add(abilitieLists[fillInt]);
                abilitiesToAdd.Clear();
            }
            abilitiesToAdd.Add(backpackSlots.abilities[i]);
        }
        fillInt++;
        Debug.Log(abilitieLists[fillInt].name);
        abilitieLists[fillInt].SetAbilities(abilitiesToAdd, abilitiesToAdd.Count);
        abilitieLists_u.Add(abilitieLists[fillInt]);
        abilitiesToAdd.Clear();

        fillInt++;
        while (fillInt < abilitieLists.Count)
        {
            abilitieLists[fillInt].SetAbilities(null, 0);
            fillInt++;
        }
        ShowPage(Mathf.Max(1, currentList));
    }

    public void CreateBodyLists(List<PlayerBody> playerSlots, List<PlayerBody> backpackSlots)
    {
        currentList = 0;
        abilitieLists_u.Clear();
        var playerBodyList = CreateList(playerSlots, 4);
        var backpackBodyList = CreateList(backpackSlots, backpackSlots.Count);
        abilitieLists[0].SetAbilities(playerBodyList.abilities, playerBodyList.slots);
        abilitieLists_u.Add(abilitieLists[0]);


        int fillInt = 0;
        List<Ability> abilitiesToAdd = new List<Ability>();
        for (int i = 0; i < backpackBodyList.abilities.Count; i++)
        {
            if (i % 8 == 0 && i != 0)
            {

                fillInt++;
                Debug.Log(abilitieLists[fillInt].name);
                abilitieLists[fillInt].SetAbilities(abilitiesToAdd, abilitiesToAdd.Count);
                abilitieLists_u.Add(abilitieLists[fillInt]);
                abilitiesToAdd.Clear();
            }
            abilitiesToAdd.Add(backpackBodyList.abilities[i]);
        }
        fillInt++;
        abilitieLists[fillInt].SetAbilities(abilitiesToAdd, abilitiesToAdd.Count);
        abilitieLists_u.Add(abilitieLists[fillInt]);
        abilitiesToAdd.Clear();

        fillInt++;
        while (fillInt < abilitieLists.Count)
        {
            abilitieLists[fillInt].SetAbilities(null, 0);
            fillInt++;
        }
        ShowPage(Mathf.Max(1, currentList));
    }

    public Player_universal.Ability_Typ CreateList(List<PlayerBody> bodies, int slots)
    {
        currentList = 0;
        Player_universal.Ability_Typ newList = new Player_universal.Ability_Typ();
        newList.slots = slots;
        List<Ability> abilityList = new List<Ability>();
        foreach (PlayerBody body in bodies)
        {
            if (body == null) continue;
            Ability newAbility = TurnBodyToAbility(body);

            abilityList.Add(newAbility);
        }
        newList.abilities = abilityList;
        return newList;
    }

    public Ability TurnBodyToAbility(PlayerBody body)
    {
        
        Ability newAbillity = new Ability();
        newAbillity._name = body._name;
        return newAbillity;
    }

    public List<PlayerBody> TurnToBodyList(List<Ability> abilities)
    {
        List<PlayerBody> bodies = new List<PlayerBody>();
        

        foreach (Ability _string in abilities)
        {
            foreach (PlayerBody _body in AllAbilities.playerBodies)
            {

                if (_string != null && _body._name == _string._name)
                {
                    bodies.Add(_body);
                    break;
                }
            }
        }
        return bodies;
    }

    public void UpdateUI()
    {
        foreach(AbilityUIList list in abilitieLists_u)
        {
            list.SetAbilities(list.abilities, -1);
        }
    }

    public SelectedItem CreateItemSave(AbilityUIList headList, int index)
    {
        SelectedItem item = new SelectedItem();
        item.headList = headList;
        item.index = index;
        return item;
    }

}

public class SelectedItem
{
    public AbilityUIList headList;
    public int index;
}


