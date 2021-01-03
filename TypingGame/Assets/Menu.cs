using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    //Do the sizing ok
    public Sprite[] sprites;
    Image image;
    public Text _text, numbers;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void SetText(string _new_text)
    {
        _text.text = _new_text;
        
    }

    public void SetNumbers(string _new_text)
    {
         numbers.text = _new_text;

    }
}
