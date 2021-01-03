using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerBodyUI : MonoBehaviour
{
    public Text str, intellect, def, res, fire, ice ,thunder, _name, abilityText;
    public AbilityDisplay[] abilities;
    public Vector3 lScale;


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

    public void ShowBody(PlayerBody body)
    {
        _name.text = body._name;
        abilityText.text = body.info;
        str.text = body.strenght.ToString();
        intellect.text = body.intellect.ToString();
        def.text = body.defence.ToString();
        res.text = body.resistance.ToString();

        SetElemetalText(body.fire, fire);
        SetElemetalText(body.ice, ice);
        SetElemetalText(body.thunder, thunder);

        int i = 0;

        while(i < body.abilities.Length)
        {
            ShowAbility(abilities[i], true);
            abilities[i].UpdateDisplay(body.abilities[i]);
            i++;
        }

        //i = 0;
        //while(i < body.passiveSkills.Length)
        //{
        //    abilities[i + body.abilities.Length].SetPassiv(body.passiveSkills[i]);
        //    i++;
        //}

        //i = body.passiveSkills.Length + body.abilities.Length ;
        while(i < abilities.Length)
        {
            ShowAbility(abilities[i], false);
            abilities[i ].ShowEmpty();
            i++;
        }




    }

    public void ShowAbility(AbilityDisplay _display, bool _bool)
    {
        _display.GetComponent<Image>().enabled = _bool;
        var _texte = _display.GetComponentsInChildren<Text>();
        foreach (Text _text in _texte)
        {
            _text.enabled = _bool;
        }
    }

    public void SetElemetalText(Effectiveness effectiveness, Text _text)
    {
        string _string = "";
        switch(effectiveness)
        {
            case Effectiveness.normal:
                _string = "";
                break;
            case Effectiveness.resistent:
                _string = "Resistent";
                _text.color = Color.black;
                break;
            case Effectiveness.immun:
                _string = "Immun";
                _text.color = Color.black;
                break;
            case Effectiveness.absorb:
                _string = "Absorb";
                _text.color = Color.green;
                break;
            case Effectiveness.weak:
                _string = "Weak";
                _text.color = Color.red;
                break;
        }


        _text.text = _string;
    }

    
}
