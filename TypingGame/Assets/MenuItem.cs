using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour
{
    public Image image;
    public Text text;

    public void SetEmpty()
    {
        SetText("");
        SetImage(null);
    }

    public void SetUI(Ability ability)
    {
        SetText(ability._name);
        SetImage(ability.menuIcon);
    }

    public void SetText(string text)
    {
        this.text.text = text; 
    }

    public void SetImage(Sprite sprite)
    {
        image.enabled = true;
        image.sprite = sprite;
        if (sprite == null)
        {
            image.enabled = false;
            return;
        }
        
    }
}
