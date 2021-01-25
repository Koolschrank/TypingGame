using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSave : MonoBehaviour
{

    public int saveFile;
    public string startString;
    private void Awake()
    {
        var SaveDatas = FindObjectsOfType<SettingsSave>();
        if (SaveDatas.Length > 1)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

}
