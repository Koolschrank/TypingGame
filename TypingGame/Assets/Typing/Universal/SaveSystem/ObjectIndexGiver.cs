using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[ExecuteAlways]
public class ObjectIndexGiver : MonoBehaviour
{
    
    public SaveableObject[] savableObjects;
    List<int> saveInts = new List<int>();
    public int nextIndex;

    
    private void Update()
    {
        savableObjects = FindObjectsOfType<SaveableObject>();

        foreach(SaveableObject _object in savableObjects)
        {

            if (_object.uniqueIdentifier == "" || _object.uniqueIdentifier.ToCharArray().Length <5)
            {
                _object.uniqueIdentifier = nextIndex.ToString() + Random.Range(1, 1000000).ToString();
                nextIndex++;
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(_object);
                AssetDatabase.SaveAssets();
            }
        }
    }


}
