using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public Mode mode = Mode.ability_select;
    public PlayerStats player;
    public List<EnemyStats> enemies = new List<EnemyStats>();
    public List<GameObject> startingEnemies = new List<GameObject>();
    public List<turns> turn_order;
    public GameObject[] enemy_positions;
    public enemyUI[] enemyUIs;
    public EnemyStats selected_enemy;
    public GameObject damage_text, HealText, XPText;
    public bool ko_combo=true;
    public float followTurnSpeedUp = 0.75f;
    public int currentExtraTurns;
    float currentSpeed =1;
    List<Action> current_actions = new List<Action>();
    AbilitySelect ability_select;
    Theatre theatre;
    Ability _current_ability;
    SceneTransitioner sceneTransition;
    int enemy_Counter, currentTurn =-1;
    bool extraTurnBool; // checks if multible foes activated extraTurn


    private void Start()
    {
        sceneTransition = FindObjectOfType<SceneTransitioner>();
        theatre = FindObjectOfType<Theatre>();
        player = FindObjectOfType<PlayerStats>();
        ability_select = FindObjectOfType<AbilitySelect>();
        load_enemies();
        Set_turn_order();
        Next_turn();
    }

    public void load_enemies()
    {
        if (sceneTransition)
        {
            startingEnemies = sceneTransition.Enemies;
        }

        for(int i =0; i< startingEnemies.Count; i ++)
        {
            var loarded_enemy = Instantiate(startingEnemies[i],new Vector3(0,0,0), Quaternion.identity) as GameObject;
            enemies.Add(loarded_enemy.GetComponent<EnemyStats>());
            loarded_enemy.GetComponent<CharacterAnimation>().Turn(true);
        }
        theatre.SetEnemies(enemies);
        Set_enemies_positions();
    }

    public void Next_turn()
    {
        currentTurn++;
        if (currentTurn >= turn_order.Count) currentTurn = 0;
        PlayTurn(turn_order[currentTurn]);
    }

    public void PlayTurn(turns _turn)
    {
        switch (_turn)
        {
            case turns.player_step_forward:
                player_step_forward();
                break;
            case turns.player_ability_select:
                player_ability_select();
                break;
            case turns.player_attack_typing:
                player_attack_typing();
                break;
            case turns.player_attack_animation:
                player_attack_animation();
                break;
            case turns.player_step_back:
                player_step_back();
                break;
            case turns.enemy_step_forward:
                enemy_step_forward();
                break;
            case turns.enemy_attack_typing:
                enemy_attack_typing();
                break;
            case turns.enemy_attack_animation:
                enemy_attack_animation();
                break;
            case turns.enemy_step_back:
                enemy_step_back();
                break;
        }
    }

    public void player_step_forward()
    {
        player.time_effects();
        theatre.Step(player.gameObject, true, true);
        enemy_Counter = 0;
        player.SetStartingBody();
    }

    public void player_ability_select()
    {
        ability_select.Set_Active();
    }

    public void player_attack_typing()
    {

    }

    public void player_attack_animation()
    {
        foreach (Action _action in current_actions)
        {
            if (_action._ability.Impact_object)
            {
                foreach(GameObject target in _action._target)
                {
                    GameObject _impact = Instantiate(_action._ability.Impact_object, target.transform.position, transform.rotation) as GameObject;
                    _impact.transform.localScale = new Vector3(_action._ability.impact_size, _action._ability.impact_size, _action._ability.impact_size);
                }
                
            }
        }
        theatre.Ability_Animation(player.gameObject, _current_ability);
        
    }

    public void player_step_back()
    {
        currentSpeed = 1f;
        currentExtraTurns = 0;
        theatre.Step(player.gameObject, false, true);
    }

    public void enemy_step_forward()
    {
        selected_enemy = enemies[enemy_Counter];
        theatre.Step(enemies[enemy_Counter].gameObject, true, false);
        enemies[enemy_Counter].time_effects();
    }

    public void enemy_attack_typing()
    {
        StartEnemyTurn();
    }

    public void enemy_attack_animation()
    {
        foreach (Action _action in current_actions)
        {

            if (_action._ability.Impact_object)
            {
                foreach (GameObject target in _action._target)
                {
                    GameObject _impact = Instantiate(_action._ability.Impact_object, target.transform.position, transform.rotation) as GameObject;
                    _impact.transform.localScale = new Vector3(_action._ability.impact_size, _action._ability.impact_size, _action._ability.impact_size);
                }
            }
        }
        theatre.Ability_Animation(selected_enemy.gameObject, _current_ability);
        
        
    }

    public void enemy_step_back()
    {
        theatre.Step(enemies[enemy_Counter].gameObject, false, false);
        NextEnemy();
    }

    public void NextEnemy()
    {
        
        enemy_Counter++;
        //if (enemies.Count <= enemy_Counter) return;
        //if (!enemies[enemy_Counter].IsDead())
        //{
        //}
    }

    public void RemoveEnemy(EnemyStats _dead_enemie, Ability ability_stats, bool onGround)
    {
        theatre.KO_object(_dead_enemie.gameObject);
        enemies.Remove(_dead_enemie);
        if (currentTurn<4 && ability_stats._typ == ability_typ.physical)
        {
            GoToPlayerTurn();
        }
        else if(currentTurn >= 4)
        {
            currentTurn--;
            Set_turn_order();
            Next_turn();
        }
        else 
        {
            // staies empty if everything work out 
            Set_turn_order();
        }
        
        
        if(enemies.Count <=0)
        {
            WinState();
        }
    }

    public void Player_move(float score, bool typos_made, Ability ability)
    {
        
        int power = 0;
        int playerStrenght = player.strenght; 
        if(player.CheckForPassiv(passiveSkill.rage) && player.hp*2 <= player.hp_max)
        {
            playerStrenght += playerStrenght;
        }

        if (player.CheckForPassiv(passiveSkill.itemBoost) && !typos_made &&(ability._typ == ability_typ.item_heal || ability._typ == ability_typ.mp_heal))
        {
            Debug.Log("itemBoost");
            Debug.Log(score);
            score += 0.5f;
            Debug.Log(score);
        }


        switch (ability._typ)
        {
            case ability_typ.physical:
                power = PlayerPowerCalculation(ability.power, (int)(playerStrenght * ability.stat_strenght), score);
                break;
            case ability_typ.magical:
                power = PlayerPowerCalculation(ability.power, (int)(player.intellect * ability.stat_strenght), score);
                if (player.CheckForPassiv(passiveSkill.big_brain) && ability._typ == ability_typ.magical)
                {
                    power = (int)(power* 1.30f);
                }
                break;
            case ability_typ.heal:
                power = PlayerPowerCalculation(ability.power, (int)(player.intellect * ability.stat_strenght), score);
                break;
            case ability_typ.item_heal:
            case ability_typ.mp_heal:
                if (player.CheckForPassiv(passiveSkill.itemBoost) && !typos_made && (ability._typ == ability_typ.item_heal || ability._typ == ability_typ.mp_heal))
                    power = ItemPowerCalculation(ability.power, score);
                else
                    power = PlayerPowerCalculation(ability.power, 0, score);
                break;
            default:
                power = (int)((ability.power) * (1 + score));
                break;
        }

        // low hp power boost
        if (Effects.Check_for_effect(ability.effect_array, effects.low_hp_power_boost) != null)
        {
            var effect_stats = Effects.Check_for_effect(ability.effect_array, effects.low_hp_power_boost);
            if ((float)player.Get_HP_before_Attack() / (float)player.hp_max <= (float)effect_stats.value1)
            {
                power = (int)(power * effect_stats.value2);
            }

        }

        if (Effects.Check_for_effect(ability.effect_array, effects.full_hp_boost) != null)
        {
            var effect_stats = Effects.Check_for_effect(ability.effect_array, effects.full_hp_boost);
            if ((float)player.Get_HP_before_Attack() / (float)player.hp_max >= 1f)
            {
                power = (int)(power * effect_stats.value1);
            }

        }


        if (enemies.Count == 1)
        {
            selected_enemy = enemies[0];
        }

        switch(ability._target)
        {
            case Target.one:
                Save_Action(power, typos_made, ability, selected_enemy.gameObject, score);
                break;
            case Target.multible:
                foreach(EnemyStats enemy in enemies)
                {
                    Save_Action(power, typos_made, ability, enemy.gameObject, score);
                }
                break;
            case Target.self:
                Save_Action(power, typos_made, ability, player.gameObject, score);
                break;
            case Target.all:
                {
                    Save_Action(power, typos_made, ability, player.gameObject, score);
                    foreach (EnemyStats enemy in enemies)
                    {
                        Save_Action(power, typos_made, ability, enemy.gameObject, score);
                    }
                }
                break;
        }
        _current_ability = ability;

    }

    public int PlayerPowerCalculation(int ability_power, int player_power, float score)
    {
        if (score ==0)
        {
            return ((ability_power + player_power) / 2);
        }
        else if (score>=0.5)
        {
            return (int)((ability_power + player_power) * (1.5f));
        }

        return (int)((ability_power + player_power) * (1 + score));
    }

    public int ItemPowerCalculation(int ability_power, float score)
    {
        if (score >= 0.5)
        {
            return (int)((ability_power) * (2f));
        }

        return (int)((ability_power) * (1.5 + score));
    }

    public void Enemy_Acts(Ability _ability)
    {
        bool toPlayer = false;
        foreach(GameObject target in _ability.Get_enemy_target())
        {
            if (target.GetComponent<PlayerStats>())
            {
                FindObjectOfType<BattleTyper>().Start_typing(_ability._words, _ability._word_count, _ability.time_per_character, false, _ability.gimmick);
                toPlayer = true;
            }
            else
            {

                Enemy_move(1, true, _ability); // 1 and true don't matter
                Next_turn();
                return;
            }
        }

        
    }

    public void Enemy_move(float score, bool typos_made, Ability ability)
    {
        if (ability.castSound != null)
        {
            FindObjectOfType<AudioBox>().PlaySound(ability.castSound, ability.soundVolume);
        }
        int power = 0;
        switch (ability._typ)
        {
            case ability_typ.physical:
                power = (int)(ability.power + ability.GetUser().strenght) ;
                break;
            case ability_typ.magical:
                power = (int)(ability.power + (ability.GetUser().intellect)) ;
                break;
            case ability_typ.heal:
                power = (int)(ability.power + (ability.GetUser().intellect )) ;
                break;
            default:
                power = (int)(ability.power) ;
                break;
        }

        // low hp power boost
        if (Effects.Check_for_effect(ability.effect_array, effects.low_hp_power_boost) != null)
        {
            var effect_stats = Effects.Check_for_effect(ability.effect_array, effects.low_hp_power_boost);
            Debug.Log(player.hp / player.hp_max);
            Debug.Log(effect_stats.value1);
            if (player.hp / player.hp_max <= effect_stats.value1)
            {
                power = (int)(power * effect_stats.value2);
            }

        }

        if (Effects.Check_for_effect(ability.effect_array, effects.full_hp_boost) != null)
        {
            var effect_stats = Effects.Check_for_effect(ability.effect_array, effects.full_hp_boost);
            Debug.Log(player.hp / player.hp_max);
            if (player.hp / player.hp_max >= 1)
            {
                power = (int)(power * effect_stats.value1);
            }

        }


        Save_Action(power, typos_made, ability, score);
        //switch (ability._target)
        //{
        //    case Target.one:
        //        Save_Action(power, typos_made, ability, player.gameObject, score);
        //        break;
        //    case Target.multible:
        //        foreach (EnemyStats enemy in enemies)
        //        {
        //            Save_Action(power, typos_made, ability, enemy.gameObject, score);
        //        }
        //        break;
        //    case Target.self:
        //        Save_Action(power, typos_made, ability, selected_enemy.gameObject, score);
        //        break;
        //    case Target.all:
        //        {
        //            Save_Action(power, typos_made, ability, player.gameObject, score);
        //            foreach (EnemyStats enemy in enemies)
        //            {
        //                Save_Action(power, typos_made, ability, enemy.gameObject, score);
        //            }
        //        }
        //        break;
        //}

        _current_ability = ability;
    }
        
    public class Action
    {
        public int _power;
        public bool _typos_made;
        public Ability _ability;
        public List<GameObject> _target = new List<GameObject>();
        public float _score;
    }

    public void Save_Action(int power,bool typos_made,Ability ability, float score)
    {
        Action new_action = new Action();
        new_action._power = power;
        new_action._typos_made = typos_made;
        new_action._ability = ability;
        new_action._target = ability.Get_enemy_target();
        new_action._score = score;
        current_actions.Add(new_action);
    }

    public void Save_Action(int power, bool typos_made, Ability ability, GameObject target, float score)
    {
        Action new_action = new Action();
        new_action._power = power;
        new_action._typos_made = typos_made;
        new_action._ability = ability;
        new_action._target.Add(target);
        new_action._score = score;
        current_actions.Add(new_action);
    }

    public void Initiate_Abilities()
    {
        foreach (Action _action in current_actions)
        {
            foreach(GameObject target in _action._target)
            if (target.GetComponent<PlayerStats>())
            {
                target.GetComponent<PlayerStats>().Apply_ability(_action._power, _action._typos_made, _action._ability,_action._score);
            }
            else
            {
                target.GetComponent<EnemyStats>().Apply_ability(_action._power, _action._typos_made, _action._ability);
            }
        }
        current_actions.Clear();
    }

    public void StartEnemyTurn()
    {
        enemies[enemy_Counter].UseAbility();
        
        
    }

    public EnemyStats GetEnemiy()
    {
        return selected_enemy;
    }

    public void Set_enemies_positions()
    {
        Change_enemy_names();
        switch (enemies.Count)
        {
            case 1:
                Set_enemy_position(enemies[0], enemy_positions[2], enemyUIs[2]);
                enemyUIs[0].gameObject.SetActive(false); 
                enemyUIs[1].gameObject.SetActive(false); 
                enemyUIs[3].gameObject.SetActive(false); 
                break;
            case 2:
                Set_enemy_position(enemies[0], enemy_positions[1], enemyUIs[1]);
                Set_enemy_position(enemies[1], enemy_positions[3], enemyUIs[3]);
                enemyUIs[0].gameObject.SetActive(false); 
                enemyUIs[2].gameObject.SetActive(false);
                break;
            case 3:
                Set_enemy_position(enemies[0], enemy_positions[1], enemyUIs[1]);
                Set_enemy_position(enemies[1], enemy_positions[2], enemyUIs[2]);
                Set_enemy_position(enemies[2], enemy_positions[3], enemyUIs[3]);
                enemyUIs[0].gameObject.SetActive(false);
                break;
            case 4:
                Set_enemy_position(enemies[0], enemy_positions[0], enemyUIs[0]);
                Set_enemy_position(enemies[1], enemy_positions[1], enemyUIs[1]);
                Set_enemy_position(enemies[2], enemy_positions[2], enemyUIs[2]);
                Set_enemy_position(enemies[3], enemy_positions[3], enemyUIs[3]);
                break;
        }

        foreach(EnemyStats enemy in enemies)
        {
            enemy.Set_UI();
        }

    }

    public void Set_enemy_position(EnemyStats enemy, GameObject position, enemyUI UI)
    {
        enemy.gameObject.transform.parent = theatre.gameObject.transform;
        enemy.transform.position = position.transform.position;
        
        enemy.UI = UI;
    }

    public void Change_enemy_names()
    {
        List<NameSaver> names = new List<NameSaver>();
        foreach (EnemyStats enemy in enemies)
        {
            NameSaver name = new NameSaver();
            name.name = enemy._name;
            foreach(NameSaver _name in names)
            {
                if (name.name == _name.name)
                {
                    _name.multiName = true;
                }
            }
            names.Add(name);
        }
        foreach (EnemyStats enemy in enemies)
        {
            foreach (NameSaver name in names)
            {
                
                if (enemy._name == name.name)
                {
                    if (!name.multiName) break;
                    enemy._name = name.name + (name.nameCount + 1).ToString();
                    name.nameCount += 1;
                    break;
                }
            }
        }

        // ahhh yes names
    }
    private class NameSaver
    {
        public string name;
        public int nameCount;
        public bool multiName;
    }

    /*IEnumerator Enemy_Attacks()
    {
        foreach (EnemyStats enemy in enemies)
        {
            enemy.UseAbility();
            yield return re
        }
    }*/

    public void Set_turn_order()
    {
        turn_order.Clear();
        enemy_Counter = 0;
        AddPlayerTurn();
        foreach(EnemyStats enemy in enemies)
        {
            AddEnemyTurn();
        }
    }

    public void GoToPlayerTurn()
    {
        if(extraTurnBool)
        {
            return;
        }
        StartCoroutine(ExtraTurnDelay());

        currentExtraTurns++;
        Debug.Log("extra turn baby");
        if(player.CheckForPassiv(passiveSkill.comboBoost))
        {
            currentSpeed = (1+Mathf.Pow(followTurnSpeedUp,currentExtraTurns))/2;
        }
        else
        {
            currentSpeed = Mathf.Pow(followTurnSpeedUp, currentExtraTurns);
        }
        Debug.Log(currentSpeed);
        Set_turn_order();
        currentTurn =0;
    }

    IEnumerator ExtraTurnDelay()
    {
        extraTurnBool = true;
        yield return new WaitForSeconds(0.5f);
        extraTurnBool = false;
    }

    public float Get_current_speed()
    {
        return currentSpeed;
    }

    public void AddPlayerTurn()
    {
        turn_order.Add(turns.player_step_forward);
        turn_order.Add(turns.player_ability_select);
        turn_order.Add(turns.player_attack_typing);
        turn_order.Add(turns.player_attack_animation);
        turn_order.Add(turns.player_step_back);
    }

    public void AddEnemyTurn()
    {
        turn_order.Add(turns.enemy_step_forward);
        turn_order.Add(turns.enemy_attack_typing);
        turn_order.Add(turns.enemy_attack_animation);
        turn_order.Add(turns.enemy_step_back);
    }

    public void WinState()
    {
        player.RemoveAllBuffs();
        StartCoroutine(GoBackToMap());
    }

    public void ShowEnemyStats()
    {
        for(int i =0; i < enemies.Count; i++)
        {
            enemies[i].UI.GetComponentInChildren<EnemyInfo>().SetInfo(enemies[i]);
        }
    }

    public void RemoveInfo()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].UI.GetComponentInChildren<EnemyInfo>().GoBack();
        }
    }

    IEnumerator GoBackToMap()
    {
        yield return new WaitForSeconds(2f);
        sceneTransition.TransitionOutOfBattle();
    }

}
