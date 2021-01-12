using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbiltyStatsUI : MonoBehaviour
{
    public Text name, cost, power, info, action, action_number;
    public GameObject actionMenu,statMenu;
    public Image sprite, background, actionBackground;
    public Sprite[] colorStyles;
    public Sprite[] colorStylesAction;
    Vector3 lScale;

    private void Start()
    {
        lScale = transform.localScale;
        SetVisible(false);
    }

    public void SetVisible(bool _bool)
    {
        if (_bool)
        {
            transform.localScale = lScale;
        }
        else
        {
            transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public void SetUI(Ability _ability)
    {
        SetBackground(_ability);
        statMenu.SetActive(true);
        name.text = _ability._name;
        sprite.sprite = _ability.menuIcon;
        sprite.SetNativeSize();
        sprite.transform.localScale = new Vector3(_ability.icon_size, _ability.icon_size, _ability.icon_size);
        if (_ability.spriteRotatet)
        {
            sprite.transform.rotation = Quaternion.Euler(0, 0, 45);
        }
        else
        {
            sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        switch(_ability._cost_typ)
        {
            case Cost.mp:
                if (FindObjectOfType<PlayerStats>().CheckForPassiv(passiveSkill.big_brain) && _ability._typ == ability_typ.magical)
                {
                    cost.text = "Cost: " + ((int)(_ability._cost * 1.50f)).ToString() + " MP";
                }
                else
                {
                    cost.text = "Cost: " + _ability._cost.ToString() + " MP";
                }
                break;
            case Cost.hp:
                
                cost.text = "Cost: " + ((int)(_ability._cost * FindObjectOfType<PlayerStats>().hp_cost_percent)).ToString() + " HP ";
                break;
            case Cost.nothing:
            case Cost.consumable:
                cost.text = "";
                break;
        }

        if(_ability.power !=0 || _ability._typ == ability_typ.magical || _ability._typ == ability_typ.physical || _ability._typ == ability_typ.heal)
        {
            power.text = SetPower(_ability, FindObjectOfType<PlayerStats>());
        }
        else
        {
            power.text = "";
        }
        

        info.text = _ability._info;

    }

    public string SetPower(Ability _ability, PlayerStats player)
    {
        bool AP_null = false;
        string _string = "";
        _string += "Power: ";
        _string += CalculateStrenght(_ability, player);

        _string += " (";
        if (_ability.power !=0)
        {
            _string += _ability.power.ToString();
            AP_null = true;
        }

        
        
        if(_ability.stat_strenght == 0)
        {

        }
        else if(_ability.stat_strenght != 1)
        {
            if (AP_null == true)
            {
                _string += " + ";
            }
            _string += _ability.stat_strenght.ToString();
            _string +=AddStatMutiplier(_ability._typ);
        }
        else
        {
            if (AP_null == true)
            {
                _string += " + ";
            }
            _string +=AddStatMutiplier(_ability._typ);
        }
        _string += ")";



        return _string;
    }

    public string CalculateStrenght(Ability ability, PlayerStats player)
    {
        int power =0;

        power += ability.power;
        if(ability._typ == ability_typ.physical)
        {
            if (FindObjectOfType<PlayerStats>().CheckForPassiv(passiveSkill.rage) && (float)(player.hp/player.hp_max) <=0.5f)
            {
                power += (int)(ability.stat_strenght * player.strenght*2);
            }
            else
            {
                power += (int)(ability.stat_strenght * player.strenght);
            }
        }
        else if (ability._typ == ability_typ.magical)
        {
            power += (int)(ability.stat_strenght * player.intellect);
            if (FindObjectOfType<PlayerStats>().CheckForPassiv(passiveSkill.big_brain))
            {
                power = (int)(power *1.3f);
            }
           
        }
        else if (ability._typ == ability_typ.heal)
        {
            power += (int)(ability.stat_strenght * player.intellect);
        }

        return power.ToString();
    }

    public string AddStatMutiplier(ability_typ typ)
    {
        switch (typ)
        {
            case ability_typ.heal:
            case ability_typ.magical:
                return "INT";
            case ability_typ.physical:
                return "STR";
            default:
                return "";
        }
    }

    public void SetActions(string _string, string _number)
    {
        actionMenu.SetActive(true);
        action.text = _string;
        action_number.text = _number;
    }

    public void EmptyActions()
    {
        action.text = "";
        actionMenu.SetActive(false);
        
    }

    public void EmptyAbility()
    {
        statMenu.SetActive(false);
    }

    public void SetBackground(Ability _ability)
    {
        switch(_ability._element)
        {
            case Element.Normal:
                background.sprite = colorStyles[0];
                actionBackground.sprite = colorStylesAction[0];
                break;
            case Element.Fire:
                background.sprite = colorStyles[1];
                actionBackground.sprite = colorStylesAction[1];
                break;
            case Element.Ice:
                background.sprite = colorStyles[2];
                actionBackground.sprite = colorStylesAction[2];
                break;
            case Element.Thunder:
                background.sprite = colorStyles[3];
                actionBackground.sprite = colorStylesAction[3];
                break;
            case Element.Heal:
                background.sprite = colorStyles[4];
                actionBackground.sprite = colorStylesAction[4];
                break;

        }
    }

}
