using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public EnemyAI[] enemies;
    List<GameObject> battle_enemies = new List<GameObject>();
    
    void Start()
    {
        
        foreach(EnemyAI enemy in enemies)
        {
            battle_enemies.Add(enemy.BattleEnemy);
        }
    }

    
    public void Transition_to_battle()
    {
        foreach(EnemyAI enemy in enemies)
        {
            if (enemy.gameObject.GetComponent<SaveableObject>())
            {
                Debug.Log("was set iditot");
                enemy.GetComponent<SaveableObject>().inGame = false;
            }
            
        }
        FindObjectOfType<SceneTransitioner>().TransitionBattle(battle_enemies);
    }
}
