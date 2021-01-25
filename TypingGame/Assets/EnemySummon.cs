using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySummon : MonoBehaviour
{
    public float[] HP_thresholds;
    public GameObject[] the_big_boys,bigEnemies, slightyBigEnemies , enemies, smallEnemies;
    EnemyStats enemy;

    private void Start()
    {
        enemy = GetComponent<EnemyStats>();
    }

    public GameObject[] GetEnemyArray(int i)
    {
        foreach(float threshold in HP_thresholds)
        {
            if(((float)enemy.hp/ (float)enemy.hp_max) <= threshold)
            {
                i++;
            }
        }

        switch(i)
        {
            case 0:
                return smallEnemies;
            case 1:
                return enemies;
            case 2:
                return slightyBigEnemies;
            case 3:
                return bigEnemies;
            case 4:
                return the_big_boys;

        }
        return the_big_boys;
    }
}
