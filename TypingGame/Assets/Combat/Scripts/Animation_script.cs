using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_script : MonoBehaviour
{
    public void Initiate_Abilities()
    {
        FindObjectOfType<BattleSystem>().Initiate_Abilities();
    }
}
