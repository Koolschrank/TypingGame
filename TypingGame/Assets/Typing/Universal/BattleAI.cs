using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class BattleAI
{
    public static bool CheckCondition(behaviour_AI condition, EnemyStats enemy, List<EnemyStats> all_enemy)
    {
        if (condition.currentCoolDown>0)
        {
            return false;
        }

        switch (condition.condition)
        {
            case AI_condition.nothing:
                return condition_nothing(condition.probability);
            case AI_condition.low_health:
                return condition_low_health(condition.C_Value1,enemy,condition.probability);
            case AI_condition.low_allie_health:
                return condition_low_allie_health(condition.C_Value1, all_enemy, condition.probability);
        }
        return true;
    }

    public static bool condition_nothing(float probability)
    {
        return Random.RandomRange(0f, 1f) <= probability;
    }

    public static bool condition_low_health(float i1, EnemyStats enemy, float probability)
    {
        if (((float)enemy.hp / (float)enemy.hp_max) <= i1)
        {
            return Random.RandomRange(0f, 1f) <= probability;
        }
        return false;
    }

    public static bool condition_low_allie_health(float i1, List<EnemyStats> enemies, float probability)
    {
        
        foreach(EnemyStats enemy in enemies)
        {
            if (((float)enemy.hp / (float)enemy.hp_max) <= i1)
            {
                return Random.RandomRange(0f, 1f) <= probability;
            }
        }
        return false;
    }

    public static List<GameObject> FindTarget(behaviour_AI behaviour, EnemyStats enemy, List<EnemyStats> all_enemy, PlayerStats player)
    {
        behaviour.currentCoolDown = behaviour.cooldown;
        List<GameObject> targets= new List<GameObject>(); 
        switch (behaviour.behaviour)
        {
            case AI_behaviour.Attack:
                targets.Add(behabiour_Attack(player));
                break;
            case AI_behaviour.on_self:
                targets.Add(behabiour_on_self(enemy));
                break;
            case AI_behaviour.on_low_health_allie:
                targets.Add(behabiour_on_low_health_allie(all_enemy));
                break;
            case AI_behaviour.on_random_allie:
                targets.Add(behabiour_on_random_allie(all_enemy));
                break;
            case AI_behaviour.on_all_allies:
                behaviour_on_all_allies(targets,all_enemy);
                break;
        }
        return targets;
    }

    public static GameObject behabiour_Attack(PlayerStats player)
    {
        return player.gameObject;
    }

    public static GameObject behabiour_on_self(EnemyStats enemy)
    {
        return enemy.gameObject;
    }

    public static void behaviour_on_all_allies(List<GameObject> targets , List<EnemyStats> allEnemies)
    {
        foreach(EnemyStats enemy in allEnemies)
        {
            targets.Add(enemy.gameObject);
        }
    }

    public static GameObject behabiour_on_low_health_allie(List<EnemyStats> enemies)
    {
        EnemyStats lowestEnemy = null;
        foreach (EnemyStats enemy in enemies)
        {
            if (lowestEnemy == null)
            {
                lowestEnemy = enemy;
            }
            else if (((float)lowestEnemy.hp/ (float)lowestEnemy.hp_max) > ((float)enemy.hp/ (float)enemy.hp_max))
            {
                lowestEnemy = enemy;
            }
        }
        return lowestEnemy.gameObject;
    }

    public static GameObject behabiour_on_random_allie(List<EnemyStats> enemies)
    {
        EnemyStats lowestEnemy = null;
        foreach (EnemyStats enemy in enemies)
        {
            if (lowestEnemy == null)
            {
                lowestEnemy = enemy;
            }
            else if (((float)lowestEnemy.hp / (float)lowestEnemy.hp_max) > ((float)enemy.hp / (float)enemy.hp_max))
            {
                lowestEnemy = enemy;
            }
        }
        return enemies[Random.Range(0, enemies.Count-1)].gameObject;
    }

    //public static GameObject behabiour_on_random_allie(List<EnemyStats> enemies)
    //{
    //    EnemyStats lowestEnemy = null;
    //    foreach (EnemyStats enemy in enemies)
    //    {
    //        if (lowestEnemy == null)
    //        {
    //            lowestEnemy = enemy;
    //        }
    //        else if (((float)lowestEnemy.hp / (float)lowestEnemy.hp_max) > ((float)enemy.hp / (float)enemy.hp_max))
    //        {
    //            lowestEnemy = enemy;
    //        }
    //    }
    //    return enemies[Random.Range(0.)];
    //}

}

[System.Serializable]
public class behaviour_AI
{
    public AI_behaviour behaviour;
    public float B_Value1, B_Value2;

    public AI_condition condition;
    public float C_Value1, C_Value2;

    public Ability[] abilities;
    public int cooldown;
    public int currentCoolDown;
    public float probability=1f;
    public string test_string;
}


