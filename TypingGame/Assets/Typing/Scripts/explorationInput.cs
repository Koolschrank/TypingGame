using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class explorationInput : MonoBehaviour
{
    PlayerMovement player;
    InputField input;
    bool is_active = true;
    List<TextPopUp> _currentTextList = new List<TextPopUp>();

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        input = GetComponent<InputField>();
        input.onValueChange.AddListener(delegate { ValueChangeCheck(input.text); });
    }

   
    void Update()
    {
        if (input.isFocused == false && is_active)
        {
            input.ActivateInputField();
        }
    }

    public void ValueChangeCheck(string _text)
    {
        if (_text.Contains(" "))
        {
            if (_currentTextList.Count <= 0) return;
            foreach(TextPopUp _object_name in _currentTextList)
            {
                if (_text == _object_name.GetString() +" ")
                {
                    player.UpdatePath(_object_name.GetObject().transform);
                }
            }
        }
        
    }

    public void AddToTextList(TextPopUp _new_text)
    {
        _currentTextList.Add(_new_text);
        Change_names();
    }

    public void Change_names()
    {
        List<NameSaver> names = new List<NameSaver>();
        foreach (TextPopUp _text in _currentTextList)
        {
            NameSaver name = new NameSaver();
            name.name = _text.GetString();
            names.Add(name);
        }
        foreach (TextPopUp _text in _currentTextList)
        {
            foreach (NameSaver name in names)
            {
                if (_text.GetString() == name.name)
                {
                    if (name.nameCount ==0)
                    {
                        _text.SetString(name.name);
                    }
                    else
                    {
                        _text.SetString(name.name + (name.nameCount + 1).ToString());
                    }
                    
                    name.nameCount += 1;
                    break;
                }
            }
        }
    }
    private class NameSaver
    {
        public string name;
        public int nameCount;
    }
}
