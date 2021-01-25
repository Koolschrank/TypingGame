using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTextEdit : MonoBehaviour
{
    public Text FileNameText, deleteInfoText, WPM, autoMode,gameStartText, optionesText, deleteText;


    public void SetFileName(string fileName)
    {
        FileNameText.text = fileName;
    }

    public void SetDeleteText(string fileName)
    {
        deleteInfoText.text = "Delete " +fileName+ "?";
    }

    public void SetWPM(float i)
    {
        WPM.text = "<- " + i.ToString() + " ->";
    }

    public void SetAutoMode(bool b)
    {
        if(b)
        {
            autoMode.text = "Auto-mode on";
        }
        else
        {
            autoMode.text = "Auto-mode off";
        }
        
    }

    public void SetActionUI(bool b)
    {
        if(b)
        {
            gameStartText.text = "continue";
            optionesText.color = Color.white;
            deleteText.color = Color.white;
        }
        else
        {
            gameStartText.text = "new Game";
            optionesText.color = Color.gray;
            deleteText.color = Color.grey;
        }
    }

}
