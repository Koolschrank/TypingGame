using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityDisplay : MonoBehaviour
{
    public Sprite[] sprites;
    Image _currentSpeite;
    public Text _name, _power, _cost, _number;
    Vector2 normalScale;


    void Start()
    {
        _currentSpeite = GetComponent<Image>();
        normalScale = transform.localScale;
    }

    public void SetNumber(int number)
    {
        _number.text = number.ToString();
    }

    public void SetVisible(bool _bool)
    {
        if (_bool)
        {
            transform.localScale = normalScale;
        }
        else
        {
            transform.localScale = new Vector2(0,0);
        }
    }

    public void ShowEmpty()
    {
        Set_Background_color(null);
        _cost.text = "";
        _name.text = "";
        if (_number != null)
        _number.text = "";
    }

    public void UpdateDisplay(Ability _ability)
    {
        SetVisible(true);
        Set_Background_color(_ability);
        _name.text = _ability._name;

    }

    public void UpdateWithBuddy(PlayerBody _body)
    {
        _name.text = _body._name;
        _currentSpeite.sprite = sprites[0];
    }

    public void Set_Background_color(Ability _ability)
    {
        if(_ability==null)
        {
            _currentSpeite.sprite = sprites[0];
            return;
        }

        switch (_ability._element)
        {
            case Element.Normal:
                if (_ability._cost == 0)
                {
                    _currentSpeite.sprite = sprites[0];
                    Set_Cost(_ability);
                }
                else
                {
                    _currentSpeite.sprite = sprites[1];
                    Set_Cost(_ability);
                }
                break;
            case Element.Fire:
                if (_ability._cost == 0)
                {
                    _currentSpeite.sprite = sprites[2];
                    Set_Cost(_ability);
                }
                else
                {
                    _currentSpeite.sprite = sprites[3];
                    Set_Cost(_ability);
                }
                break;
            case Element.Ice:
                if (_ability._cost == 0)
                {
                    _currentSpeite.sprite = sprites[4];
                    Set_Cost(_ability);
                }
                else
                {
                    _currentSpeite.sprite = sprites[5];
                    Set_Cost(_ability);
                }
                break;
            case Element.Thunder:
                if (_ability._cost == 0)
                {
                    _currentSpeite.sprite = sprites[6];
                    Set_Cost(_ability);
                }
                else
                {
                    _currentSpeite.sprite = sprites[7];
                    Set_Cost(_ability);
                }
                break;
            case Element.Heal:
                if (_ability._cost == 0)
                {
                    _currentSpeite.sprite = sprites[8];
                    Set_Cost(_ability);
                }
                else
                {
                    _currentSpeite.sprite = sprites[9];
                    Set_Cost(_ability);
                }
                break;
        }
    }

    public void SetPassiv(passiveSkill skill)
    {
        Set_Background_color(null);
        _cost.text = "";
        
        switch(skill)
        {
            case passiveSkill.comboBoost:
                _name.text = "combo boost";
                break;
            case passiveSkill.Phenix:
                _name.text = "Phenix";
                break;
            case passiveSkill.rage:
                _name.text = "rage";
                break;
            case passiveSkill.magicUser:
                _name.text = "magic user";
                break;
            case passiveSkill.noTypoBoost:
                _name.text = "type boost";
                break;
        }

        _name.text += " (passiv)";
    }

    public void Set_Cost(Ability _ability)
    {
        switch (_ability._cost_typ)
        {
            case Cost.hp:
                _cost.text = ((int)(_ability._cost * FindObjectOfType<PlayerStats>().hp_cost_percent)).ToString() + "HP";
                break;
            case Cost.mp:
                if(FindObjectOfType<PlayerStats>().CheckForPassiv(passiveSkill.big_brain) && _ability._typ == ability_typ.magical)
                {
                    _cost.text = ((int)(_ability._cost * 1.50f)).ToString() + "MP";
                }
                else
                {
                    _cost.text = _ability._cost.ToString() + "MP";
                }
               
                break;
            case Cost.nothing:
            case Cost.consumable:
                _cost.text = "";
                break;
        }
    }

}
