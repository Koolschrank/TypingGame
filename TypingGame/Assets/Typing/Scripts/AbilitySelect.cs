using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySelect : MonoBehaviour
{
    public List<Player_universal.Ability_Typ> Ability_typs = new List<Player_universal.Ability_Typ>();
    public InputField input;
    public UI_Abilities AUI;
    Vector3 UIScale;
    public playerBodyUI PBUI;
    public AbiltyStatsUI ASUI;
    BattleSystem battleSystem;
    PlayerStats player;
    Player_universal.Ability_Typ current_folder;
    Ability _ability;
    public Slider hp, mp;
    public bool fastMode;
    public Text hp_text, mp_text;
    bool activated = false; // later to false
    bool selectAbility = false;
    bool inBodySelect;


    private void Start()
    {
        battleSystem = FindObjectOfType<BattleSystem>();
        player = FindObjectOfType<PlayerStats>();
        Set_UI();
        input.onValueChange.AddListener(delegate { ValueChangeCheck(input.text); });
        UpdateSlider();
        UIScale = AUI.transform.localScale;
    }

    private void Update()
    {

        if (fastMode && activated)
        {
            FastMode();
        }

        if (activated)
        {
            input.ActivateInputField();
            if (Input.GetKeyDown(KeyCode.Return))
            {
                var _text = input.text;
                _text = _text + " ";
                ValueChangeCheck(_text);
            }
        }
        else
        {
            input.DeactivateInputField();
        }

    }

    public void ValueChangeCheck(string _text)
    {
        if (fastMode) return;

        if (_text.Contains(" "))
        {
            foreach (Player_universal.Ability_Typ folder in player.Ability_typs)
            {
                if (_text.Length <= folder._name.Length) continue;
                if(TextChanger.Return_sub_string(0,folder._name.Length + 1, _text ) == folder._name +" ")
                {
                    if (current_folder == null)
                    {
                        Show_Abilities(folder);
                        current_folder = folder;
                        //SetVisible(side_right,true);
                        //SetVisible(side_cost, true);
                    }

                    foreach (Ability current_ability in current_folder.abilities)
                    {
                        if (_text.Length <= folder._name.Length + current_ability._name.Length+1) continue;
                        if (TextChanger.Return_sub_string(0, folder._name.Length + current_ability._name.Length + 2, _text) == folder._name+" " + current_ability._name+ " ")
                        {
                            if(_ability == null)
                            {
                                _ability = current_ability;
                                create_target_UI(battleSystem.enemies, _ability);
                                //SetVisible(side_targets, true);
                            }

                            if (_text == folder._name + " " + current_ability._name + " " + "info" + " ")
                            {
                                SetInfo();

                                //SetVisible(side_info, true);
                                //side_info.text = _ability._info;
                            }
                            else /*SetVisible(side_info, false)*/;

                            switch (_ability._target)
                            {
                                case Target.one:
                                    foreach(EnemyStats enemy in battleSystem.enemies)
                                    {
                                        if (_text == folder._name + " " + current_ability._name + " " + enemy._name + " ")
                                        {
                                            battleSystem.selected_enemy = enemy;
                                            ActivateBattleTyper(current_ability);
                                            break;
                                        }
                                    }
                                    break;
                                case Target.multible:
                                    if (_text == folder._name + " " + current_ability._name + " " + "enemies" + " ")
                                    {
                                        ActivateBattleTyper(current_ability);
                                        break;
                                    }
                                    break;
                                case Target.self:
                                    if (_text == folder._name + " " + current_ability._name + " " + "self" + " ")
                                    {
                                        ActivateBattleTyper(current_ability);
                                        break;
                                    }
                                    break;

                            }


                            return;
                        }
                        else
                        {
                            //Empty_field(side_targets);
                            _ability = null;
                            //SetVisible(side_targets, false);
                        }

                    }
                    _ability = null;
                    //SetVisible(side_targets, false);
                    //Empty_field(side_targets);
                    return;
                }
            }
        }
        ResetUI();
    }

    public void SetInfo()
    {
        Debug.Log("info");
    }

    public void ResetUI()
    {
        AUI.Set_abilities_off();
        AUI.Set_Enemy_Menu_off();
        //Empty_field(side_right);
        //Empty_field(side_cost);
        //Empty_field(side_targets);
        selectAbility = false;
        current_folder = null;
        _ability = null;

        //SetVisible(side_right, false);
        //SetVisible(side_cost, false);
        //SetVisible(side_targets, false);
        //SetVisible(side_info, false);
    }

    public void SetVisible(InputField side, bool vivible)
    {
        side.gameObject.SetActive(vivible);
    }

    public void ActivateBattleTyper(Ability ability)
    {
        if (ability._cost_typ == Cost.consumable)
        {
            current_folder.abilities.Remove(ability);
        }


        ASUI.SetVisible(false);
        input.text = "";
        FindObjectOfType<BattleSystem>().Next_turn();
        FindObjectOfType<BattleTyper>().Start_typing(ability._words, ability._word_count, ability.time_per_character, true, ability.gimmick);
        player.Set_Ability(ability);
        activated = false;
    }

    public void Set_UI()
    {
        Ability_typs.Clear();
        string _string = "", _numbers ="";
        int numbers = 1; 
        for(int i =0; i < player.Ability_typs.Length-1; i++)
        {
            if (!player.CheckForPassiv(passiveSkill.magicUser) && i ==1)
            {
                continue;
            }

            //if (fastMode)
            //{
            //    _string += i.ToString() + ": ";
            //}
            if(player.Ability_typs[i].abilities.Count >0)
            {
                _string += player.Ability_typs[i]._name + "\n";
                _numbers += numbers.ToString() + "\n";
                Ability_typs.Add(player.Ability_typs[i]);
                numbers++;
            }
            
        }
        if (player.can_swich_body || player.playerBodies.Count >1)
        {
            _string += "Shift";
            _numbers += numbers.ToString() + "\n";
        }

        AUI.Set_Menu(_string, _numbers);
    }

    public void Show_Abilities(Player_universal.Ability_Typ folder)
    {
        AUI.Set_Abilities(folder);
        selectAbility = true;
        return;
    }

    public void create_target_UI(List<EnemyStats> enemies,Ability ability)
    {
        string _text = "", _number = "";
        if(ability._cost_typ == Cost.mp &&player.mp < ability._cost)
        {
            _text = "not enougt MP";
            ASUI.SetActions(_text, _number);
            return;
        }

        int _number_int = 1;
        if (enemies != null && ability._target == Target.one)
        {
            foreach (EnemyStats enemy in battleSystem.enemies)
            {
                _number += _number_int.ToString()+ "\n";
                _text += enemy._name + "\n";
                _number_int++;
            }
        }
        if (ability._target == Target.multible)
        {
            _number = "1";
            _text += "enemies" + "\n";
        }
        if (ability._target == Target.self)
        {
            _number = "1";
            _text += "self" + "\n";
        }

        ASUI.SetActions(_text,_number);

    }

    public void Empty_field(InputField field)
    {
        field.text = "";
    }

    public void OpenShift()
    {
        AUI.Set_body_list(player.playerBodies);
        inBodySelect = true;
        return;
    }

    public void UpdateSlider()
    {
        Vector2 HPV = player.Get_hp();
        Vector2 MPV = player.Get_mp();
        hp.maxValue = HPV.y;
        hp.value = HPV.x;
        mp.maxValue = MPV.y;
        mp.value = MPV.x;
        hp_text.text = HPV.x + "/" + HPV.y;
        mp_text.text = MPV.x + "/" + MPV.y;
    }

    public void Set_Active()
    {
        if (!activated )
        {
            AUI.transform.localScale = UIScale;
            ASUI.SetVisible(false);
            activated = true;
            input.text = "";
        }
        
    }


    public void FastMode()
    {
        if(inBodySelect)
        {
            Shift_body();
        }
        else if (current_folder == null)
        {
            FM_folder();
        }
        else if (_ability == null)
        {
            FM_Ability();
        }
        else
        {
            FM_Enemy();
        }
    }


    public void FM_folder()
    {
        if (Input.GetKeyDown("1") && Ability_typs[0] != null)
        {
            Show_Abilities(Ability_typs[0]);
            current_folder = Ability_typs[0];
        }
        if (Input.GetKeyDown("2") )
        {
            if (1 == Ability_typs.Count && player.can_swich_body && player.playerBodies.Count>1)
            {
                OpenShift();
            }
            else if (Ability_typs[1] != null)
            {
                Show_Abilities(Ability_typs[1]);
                current_folder = Ability_typs[1];
            }
        }
        if (Input.GetKeyDown("3") )
        {
            if (2 == Ability_typs.Count && player.can_swich_body && player.playerBodies.Count > 1)
            {
                OpenShift();
            }
            else if (Ability_typs[2] != null)
            {
                Show_Abilities(Ability_typs[2]);
                current_folder = Ability_typs[2];
            }
        }
        if (Input.GetKeyDown("4") )
        {
            if (3 == Ability_typs.Count && player.can_swich_body && player.playerBodies.Count > 1)
            {
                OpenShift();
            }
            else if (player.Ability_typs[3] != null)
            {
                Show_Abilities(Ability_typs[3]);
                current_folder = Ability_typs[3];
            }
        }
        if (Input.GetKeyDown("5") )
        {
            if (4 == Ability_typs.Count && player.can_swich_body && player.playerBodies.Count > 1)
            {
                OpenShift();
            }
            else if (Ability_typs[4] != null)
            {
                Show_Abilities(Ability_typs[4]);
                current_folder = Ability_typs[4];
            }
            
        }
        if (Input.GetKeyDown("6") )
        {
            if (5 == Ability_typs.Count && player.can_swich_body && player.playerBodies.Count > 1)
            {
                OpenShift();
            }
            else if (Ability_typs[5] != null)
            {
                Show_Abilities(Ability_typs[5]);
                current_folder = Ability_typs[5];
            }
        }
    }

    public void FM_Ability()
    {
        if (Input.GetKeyDown("1") && current_folder.abilities.Count >0)
        {
            _ability = current_folder.abilities[0];
            SetAbilityStats();
            create_target_UI(battleSystem.enemies, _ability);
        }
        if (Input.GetKeyDown("2") && current_folder.abilities.Count > 1)
        {
            _ability = current_folder.abilities[1];
            SetAbilityStats();
            create_target_UI(battleSystem.enemies, _ability);
        }
        if (Input.GetKeyDown("3") && current_folder.abilities.Count > 2)
        {
            _ability = current_folder.abilities[2];
            SetAbilityStats();
            create_target_UI(battleSystem.enemies, _ability);
        }
        if (Input.GetKeyDown("4") && current_folder.abilities.Count > 3)
        {
            _ability = current_folder.abilities[3];
            SetAbilityStats();
            create_target_UI(battleSystem.enemies, _ability);
        }
        if (Input.GetKeyDown("5") && current_folder.abilities.Count > 4)
        {
            _ability = current_folder.abilities[4];
            SetAbilityStats();
            create_target_UI(battleSystem.enemies, _ability);
        }
        if (Input.GetKeyDown("6") && current_folder.abilities.Count > 5)
        {
            _ability = current_folder.abilities[5];
            SetAbilityStats();
            create_target_UI(battleSystem.enemies, _ability);
        }
        if (Input.GetKeyDown("7") && current_folder.abilities.Count > 6)
        {
            _ability = current_folder.abilities[6];
            SetAbilityStats();
            create_target_UI(battleSystem.enemies, _ability);
        }
        if (Input.GetKeyDown("8") && current_folder.abilities.Count > 7)
        {
            _ability = current_folder.abilities[7];
            SetAbilityStats();
            create_target_UI(battleSystem.enemies, _ability);
        }
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
        {
            AUI.Set_abilities_off();
            current_folder = null;
        }
    }

    public void SetAbilityStats()
    {
        AUI.transform.localScale = new Vector3(0,0,0);
        ASUI.SetVisible(true);
        ASUI.SetUI(_ability);
    }

    public void Shift_body()
    {
        if (Input.GetKeyDown("1") && player.playerBodies.Count>0)
        {
            SelectBody(0);
        }
        if (Input.GetKeyDown("2") && player.playerBodies.Count > 1)
        {
            SelectBody(1);
        }
        if (Input.GetKeyDown("3") && player.playerBodies.Count > 2)
        {
            SelectBody(2);
        }
        if (Input.GetKeyDown("4") && player.playerBodies.Count > 3)
        {
            SelectBody(3);
        }
        if (Input.GetKeyDown("5") && player.playerBodies.Count > 4)
        {
            SelectBody(4);
        }
        if (Input.GetKeyDown("6") && player.playerBodies.Count > 5)
        {
            SelectBody(5);
        }
        if (Input.GetKeyDown("7") && player.playerBodies.Count > 6)
        {
            SelectBody(6);
        }
        if (Input.GetKeyDown("8") && player.playerBodies.Count > 7)
        {
            SelectBody(7);
        }
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
        {
            AUI.Set_abilities_off();
            current_folder = null;
            inBodySelect = false;
            PBUI.SetVisible(false);

        }
    }

    public void SelectBody(int i)
    {
        
        player.currentBody = i;
        player.SetBody(player.playerBodies[i]);
        PBUI.SetVisible(true);
        PBUI.ShowBody(player.playerBodies[i]);
    }

    public void FM_Enemy()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
        {
            AUI.transform.localScale = UIScale;
            ASUI.SetVisible(false);
            AUI.Set_Enemy_Menu_off();
            _ability = null;
        }
        if (_ability== null ||_ability._cost_typ == Cost.mp && player.mp< _ability._cost)
        {
            return;
        }

        switch(_ability._target)
        {
            case Target.multible:
                if (Input.GetKeyDown("1") )
                {

                    ActivateBattleTyper(_ability);
                    ResetUI();
                }
                if (Input.GetKeyDown("2"))
                {
                    SetInfo();
                }
                break;
            case Target.one:
                if (Input.GetKeyDown("1"))
                {
                    if(battleSystem.enemies[0] != null)
                    {
                        
                        battleSystem.selected_enemy = battleSystem.enemies[0];
                        ActivateBattleTyper(_ability);
                        ResetUI();
                    }
                }
                if (Input.GetKeyDown("2"))
                {
                    if (battleSystem.enemies[1] != null)
                    {
                        
                        battleSystem.selected_enemy = battleSystem.enemies[1];
                        ActivateBattleTyper(_ability);
                        ResetUI();
                    }
                }
                if (Input.GetKeyDown("3"))
                {
                    if (battleSystem.enemies[2] != null)
                    {
                        
                        battleSystem.selected_enemy = battleSystem.enemies[2];
                        ActivateBattleTyper(_ability);
                        ResetUI();
                    }
                }
                if (Input.GetKeyDown("4"))
                {
                    if (battleSystem.enemies[3] != null)
                    {
                        
                        battleSystem.selected_enemy = battleSystem.enemies[3];
                        ActivateBattleTyper(_ability);
                        ResetUI();
                    }
                }
                break;
            case Target.self:
                if (Input.GetKeyDown("1"))
                {
                    
                    ActivateBattleTyper(_ability);
                    ResetUI();
                }
                break;


        }

        
    }
}
