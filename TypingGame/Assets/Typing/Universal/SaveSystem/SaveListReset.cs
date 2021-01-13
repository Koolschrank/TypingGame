using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
[CustomEditor(typeof(ObjectIndexGiver))]
public class SaveListReset : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectIndexGiver myScript = (ObjectIndexGiver)target;
        if (GUILayout.Button("Shuffle Index"))
        {
            myScript.ShuffleNumbers();
        }
    }
}
#endif
