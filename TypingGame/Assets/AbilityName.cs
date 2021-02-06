using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityName : MonoBehaviour
{
    Image background;
    Text text;
    public Sprite[] colors;

    private void Start()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        SetVisible(false);
    }

    public void SetVisible(bool visibility)
    {
        if(visibility)
        {
            background.enabled = true;
            text.enabled = true;
        }
        else
        {
            background.enabled = false;
            text.enabled = false;
        }
    }

    public void SetAbility(Ability ability)
    {
        if(ability==null)
        {
            Debug.Log("bruh  bruh");
            return;
        }
        SetVisible(true);
        text.text = ability._name;
        switch(ability._element)
        {
            case Element.Normal:
                background.sprite = colors[0];
                break;
            case Element.Fire:
                background.sprite = colors[1];
                break;
            case Element.Ice:
                background.sprite = colors[2];
                break;
            case Element.Thunder:
                background.sprite = colors[3];
                break;
            case Element.Heal:
                background.sprite = colors[4];
                break;
        }
    }
}
