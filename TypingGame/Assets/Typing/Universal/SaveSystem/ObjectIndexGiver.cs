using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[ExecuteInEditMode]
public class ObjectIndexGiver : MonoBehaviour
{
    
    public SaveableObject[] savableObjects;
    List<int> saveInts = new List<int>();
    public int nextIndex;

#if UNITY_EDITOR
    public void ShuffleNumbers()
    {
        foreach(SaveableObject _object in savableObjects)
        {
            _object.uniqueIdentifier = nextIndex.ToString() + Random.Range(1, 1000000).ToString();
            nextIndex++;
            AssetDatabase.Refresh();

            EditorUtility.SetDirty(_object);
            AssetDatabase.SaveAssets();
            Debug.Log("shuffle");
        }
    }
#endif
    private void Update()
    {

        savableObjects = FindObjectsOfType<SaveableObject>();
#if UNITY_EDITOR
        {
            foreach (SaveableObject _object in savableObjects)
            {

                if (_object.uniqueIdentifier == "" || _object.uniqueIdentifier.ToCharArray().Length < 5)
                {
                    _object.uniqueIdentifier = nextIndex.ToString() + Random.Range(1, 1000000).ToString();
                    nextIndex++;
                    AssetDatabase.Refresh();

                    EditorUtility.SetDirty(_object);
                    AssetDatabase.SaveAssets();
                }
            }
        }
#endif
    }


}
