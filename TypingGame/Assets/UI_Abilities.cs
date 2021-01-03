using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Abilities : MonoBehaviour
{
    public Menu _menu,_enemyMenu;
    public List<AbilityDisplay> _abilitieUIs;
    

    private void Start()
    {
        Set_abilities_off();
        Set_Enemy_Menu_off();
    }

    public void Set_Menu(string _text,string _numbers)
    {
        _menu.SetText(_text);
        _menu.SetNumbers(_numbers);
    }

    public void Set_Enemy_Menu(string _text)
    {
        _enemyMenu.GetComponent<Image>().enabled = true;
        _enemyMenu.GetComponentInChildren<Text>().enabled = true;
        _enemyMenu.SetText(_text);
    }

    public void Set_Enemy_Menu_off()
    {
        _enemyMenu.GetComponent<Image>().enabled = false;
        _enemyMenu.GetComponentInChildren<Text>().enabled = false;
    }

    public void Set_Abilities(Player_universal.Ability_Typ _abilities)
    {
        int _numbers = 1;
        for(int i =0; i < _abilitieUIs.Count; i++)
        {
            if (i >= _abilities.slots)
            {
                ShowAbility(_abilitieUIs[i], false);
                continue;
            }
            ShowAbility(_abilitieUIs[i], true);
            if(_abilities.abilities.Count<=i)
            {
                _abilitieUIs[i].ShowEmpty();
                _abilitieUIs[i].SetNumber(_numbers);
                _numbers++;
            }
            else
            {
                _abilitieUIs[i].UpdateDisplay(_abilities.abilities[i]);
                _abilitieUIs[i].SetNumber(_numbers);
                _numbers++;
            }
            
        }
    }

    public void Set_body_list(List<PlayerBody> _bodies)
    {
        int _numbers = 1;
        for (int i = 0; i < _abilitieUIs.Count; i++)
        {
            if (i >= _bodies.Count)
            {
                ShowAbility(_abilitieUIs[i], false);
                continue;
            }
            ShowAbility(_abilitieUIs[i], true);
            if (_bodies.Count <= i)
            {
                _abilitieUIs[i].ShowEmpty();

            }
            else
            {
                _abilitieUIs[i].UpdateWithBuddy(_bodies[i]);
                _abilitieUIs[i].SetNumber(_numbers);
                _numbers++;
            }
            _abilitieUIs[i]._cost.text = "";
        }
    }

    public void Set_abilities_off()
    {

        foreach(AbilityDisplay display in _abilitieUIs)
        {
            ShowAbility(display, false);
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
}
