using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{


    public float WPM,time_per_character, playerTypeTime =2f, enemyTypeTime = 1.5f;

    public void Start()
    {
        time_per_character = 60 / 5 / WPM;
    }
}
