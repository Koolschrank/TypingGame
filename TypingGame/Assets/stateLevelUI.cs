using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stateLevelUI : MonoBehaviour
{
    public Text _name, currentLevel;
    public Image left, right;


    public void SetLevel(int level)
    {
        currentLevel.text = level.ToString();
    }

    public void SetOff()
    {
        left.enabled = true;
        right.enabled = true;
    }

    public void ShowLeft(bool _bool)
    {
        left.enabled = _bool;
    }

    public void ShowRight(bool _bool)
    {
        right.enabled = _bool;
    }
}

