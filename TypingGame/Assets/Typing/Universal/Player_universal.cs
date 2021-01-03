using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_universal
{

    [System.Serializable]
    public class Ability_Typ
    {
        public string _name;
        public int slots = 4;
        public List<Ability> abilities;

    }

    public static AbilityFolder Get_Ability_Folder(string name)
    {
        AbilityFolder[] instances = Resources.FindObjectsOfTypeAll<AbilityFolder>();
        foreach (AbilityFolder folder in instances)
        {
            if (folder.name == name)
            {
                return folder;
            }
        }
        Debug.Log("can't find folder, bruh   " + instances.Length);
        return null;
    }
}
