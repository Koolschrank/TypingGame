using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR

[CustomEditor(typeof(Settings))]
[CanEditMultipleObjects]
public class SaveSlots : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        Settings myScript = (Settings)target;
        if (GUILayout.Button("1. SaveFile"))
        {
            myScript.SwitchSave(0);
        }

        if (GUILayout.Button("2. SaveFile"))
        {
            myScript.SwitchSave(1);
        }
        if (GUILayout.Button("3. SaveFile"))
        {
            myScript.SwitchSave(2);
        }
        if (GUILayout.Button("Delete Save"))
        {
            myScript.DeleteAllSaveFiles();
        }
    }
}
#endif
