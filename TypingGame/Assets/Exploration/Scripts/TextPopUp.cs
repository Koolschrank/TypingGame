using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopUp : MonoBehaviour
{
    string word;
    TextMeshPro text;
    GameObject _parent_gameobject;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetText(string _text, GameObject _gameobject)
    {
        text = GetComponentInChildren<TextMeshPro>();
        text.text = _text;
        word = _text;

        var _input = FindObjectOfType<explorationInput>();
        _parent_gameobject = _gameobject;
        _input.AddToTextList(this);
    }

    public string GetString()
    {
        return word;
    }

    public void SetString(string _new_string)
    {
        word = _new_string;
        text.text = word;
    }

    public GameObject GetObject()
    {
        return _parent_gameobject;
    }
}
