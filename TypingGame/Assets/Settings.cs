using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Settings : MonoBehaviour
{
    SettingsSave saveInfo;
    public int currentSave;
    public string _scene;
    public float WPM,time_per_character, playerTypeTime =2f, enemyTypeTime = 1.5f;
    public bool autoMode, doublWordCount;

    public void Start()
    {
        saveInfo = FindObjectOfType<SettingsSave>();
        if(saveInfo)
        {
            SwitchSave(saveInfo.saveFile);
        }
        time_per_character = 60 / 5 / WPM;
    }

    public void OverRideSetting(string _saveString)
    {
        Settings_save _save = SaveSystem.LordSettings(_saveString);
        if(_save == null)
        {
            return;
        }
        _scene = _save.scene;
        WPM = _save.WPM;
        autoMode = _save.autoMode;
        
    }

    public void SwitchSave(int i)
    {
        currentSave = i;
        FindObjectOfType<SceneTransitioner>().LoadGameNew(currentSave);
    }
#if UNITY_EDITOR
    public void DeleteAllSaveFiles()
    {
        SaveSystem.DeleteAllSaveFills();
    }
#endif
}


