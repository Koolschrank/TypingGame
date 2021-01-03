using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stateLevelUI : MonoBehaviour
{
    public Text _name, currentLevel;


    public void SetLevel(int level)
    {
        currentLevel.text = "LV " + level.ToString();
    }
}

