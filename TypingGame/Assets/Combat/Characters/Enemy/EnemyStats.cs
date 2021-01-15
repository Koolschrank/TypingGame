using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    public string _name = "Enemy";
    public int hp_max, hp, strenght, intellect, defence, resistance;
    //public Ability[] abilities;
    Ability currentAbility;
    BattleSystem _system;
    ScreenShake shaker;
    SpriteRenderer sprite;
    public enemyUI UI;
    public EnemyResistens resistens = EnemyResistens.normal;
    public List<Effects.over_time_ability> _overTimeAbilities = new List<Effects.over_time_ability>();
    Effects.Buff[] _buffs = new Effects.Buff[4];
    StatusText[] stat_texts = new StatusText[4];
    public behaviour_AI[] behaviours;
    public ElementalEffectiveness[] elements;
    public AudioClip defeatSound;

    bool defeated, onGround;
    public AbilityFolder AllAbilities;
    public PublicVaribles p_varibles;
    public int souls_drop;



    private void Start()
    {
        hp = hp_max;
        _system = FindObjectOfType<BattleSystem>();
        shaker = FindObjectOfType<ScreenShake>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Set_UI()
    {
        UI._name.text = _name;
        UI.hp_text.text = hp.ToString() + "/" + hp_max.ToString();
        if (!UI.showStats) return;
        UI.defence.text = defence.ToString();
        UI.resistance.text = resistance.ToString();
    }

    private void Update()
    {
        Set_UI();
    }

    public void Take_damage(int damage, Ability attack_stats)
    {
        switch (attack_stats._typ)
        {
            case ability_typ.physical:
                damage -= defence;
                break;
            case ability_typ.magical:
                damage -= resistance;
                break;
            default:

                break;
        }

        damage = CheckForElementEffects(damage, attack_stats);

        switch (resistens)
        {
            case EnemyResistens.Resistend:
                if (attack_stats._typ == ability_typ.magical)
                {
                    damage = damage / 2;
                }
                    break;
            case EnemyResistens.Defesive:
                if (attack_stats._typ == ability_typ.physical)
                {
                    damage = damage / 2;
                }
                break;
            default:

                break;
        }

        if (Effects.Check_for_effect(attack_stats.effect_array, effects.absorb) != null)
        {
            var effect_stats = Effects.Check_for_effect(attack_stats.effect_array, effects.absorb);
            FindObjectOfType<PlayerStats>().Heal(Effects.Absorb(Mathf.Min(hp, damage), effect_stats.value1));
        }
        if(Effects.Check_for_effect(attack_stats.effect_array, effects.oneShotBoost) != null)
        {
            if(Effects.maxHealthBoost(hp_max,hp))
            {
                var effect_stats = Effects.Check_for_effect(attack_stats.effect_array, effects.oneShotBoost);
                damage = (int)(damage * effect_stats.value1);
            }
        }
        if (damage>0)
        {
            GetComponent<CharacterAnimation>().Take_Damage();
        }
        hp -= Mathf.Max( 0,damage);
        showDamageNumber(damage);
        UI.slider.maxValue = hp_max;
        UI.slider.value = hp;
        UI.hp_text.text = hp.ToString() + "/" + hp_max.ToString();
        if (hp <= 0)
        {
            Defeated(attack_stats);
            shaker.ShakeCam(shake.big);
        }
        else if (damage > 0)
        {
            shaker.ShakeCam(shake.small);
        }

    }

    public int CheckForElementEffects(int _damage, Ability _ability)
    {
        foreach(ElementalEffectiveness _element_check in elements)
        {
            if(_element_check.element == _ability._element)
            {
                switch(_element_check.effect)
                {
                    case Effectiveness.weak:
                        if(!onGround)
                        {
                            FindObjectOfType<BattleSystem>().GoToPlayerTurn();
                            sprite.color = p_varibles.downColor;
                            onGround = true;
                            
                        }
                        Create_weakness_text("WEAK");
                        return (int)(_damage * 1.25);
                    case Effectiveness.resistent:
                        Create_weakness_text("RESISTEND");
                        return _damage / 2;
                    case Effectiveness.immun:
                        Create_weakness_text("NULL");
                        return 0;
                    case Effectiveness.absorb:
                        Create_weakness_text("ABSORB");
                        Heal(_damage);
                        return 0;
                }
            }
        }

        return _damage;
    }

    public void Create_weakness_text(string _string)
    {
        var Weak_Text = Instantiate(p_varibles.weakText, transform.position, Quaternion.identity);
        Weak_Text.GetComponent<WeaknessText>().Set_String(_string);
    }

    public void Defeated(Ability ability_stats)
    {
        if(defeatSound != null)
        {
            FindObjectOfType<AudioBox>().PlaySound(defeatSound,1);
        }
        FindObjectOfType<PlayerStats>().GainSouls(souls_drop);
        UI.gameObject.active = false;
        defeated = true;
        FindObjectOfType<BattleSystem>().RemoveEnemy(this, ability_stats, onGround);
    }

    public bool IsDead()
    {
        return defeated;
    }

    public void showDamageNumber(int _number)
    {
        

        GameObject _damage_text = Instantiate(_system.damage_text, transform.position, transform.rotation) as GameObject;
        _damage_text.GetComponentInChildren<TextMesh>().text = Mathf.Max(0,_number).ToString();
    }

    public void showHealNumber(int _number)
    {
        GameObject _heal_text = Instantiate(_system.HealText, transform.position, transform.rotation) as GameObject;
        _heal_text.GetComponentInChildren<TextMesh>().text = Mathf.Max(0, _number).ToString();
    }

    public void UseAbility()
    {
        if (onGround)
        {
            onGround = false;
            sprite.color = new Color(1, 1, 1, 1);
        }
        
        foreach (behaviour_AI behaviour in behaviours)
        {
            behaviour.currentCoolDown--;
        }
        foreach(behaviour_AI behaviour in behaviours)
        {
            if ( BattleAI.CheckCondition(behaviour, this, _system.enemies))
            {
                currentAbility = behaviour.abilities[Random.Range(0, behaviour.abilities.Length)];
                currentAbility.Set_enemy_target(BattleAI.FindTarget(behaviour, this, _system.enemies, FindObjectOfType<PlayerStats>()));
                currentAbility.SetUser(gameObject);

                _system.Enemy_Acts(currentAbility);
                return;
            }
        }
    }

    public void Apply_ability(int power, bool typos_made, Ability ability)
    {
        foreach (Effects._effect _effect in ability.effect_array)
        {
            if (_effect.effect == effects.effect_over_time)
            {
                _overTimeAbilities.Add(Effects.Create_over_time_ability(power, ability));
            }
            else if(_effect.effect == effects.buff)
            {
                AddBuff(AllAbilities.BuffList[(int)_effect.value1]);
            }
        }


        //if (Effects.Check_for_effect(ability.effect_array, effects.effect_over_time) != null)
        //{
        //    var effect_stats = Effects.Check_for_effect(ability.effect_array, effects.effect_over_time);
        //    _overTimeAbilities.Add(Effects.Create_over_time_ability(power, ability));
        //}
        //if (Effects.Check_for_effect(ability.effect_array, effects.buff) != null)
        //{
            

        //    var effect_stats = Effects.Check_for_effect(ability.effect_array, effects.buff);
        //    apply_stat_buff(effect_stats, 1);
        //}


        switch (ability._typ)
        {
            case ability_typ.physical:
                Take_damage(power, ability);
                break;
            case ability_typ.magical:
                Take_damage(power, ability);
                break;
            case ability_typ.heal:
                Heal(power);
                break;
        }
    }

    public void Apply_OTA(int power, bool typos_made, Ability ability)
    {
        switch (ability._typ)
        {
            case ability_typ.physical:
                Take_damage(power, ability);
                break;
            case ability_typ.magical:
                Take_damage(power, ability);
                break;
            case ability_typ.heal:
                Heal(power);
                break;
        }
    }

    public void Play_Ability(float score, bool typos_made)
    {

        FindObjectOfType<BattleSystem>().Enemy_move(score, typos_made, currentAbility);

    }

    public void Heal(int heal)
    {
        hp = Mathf.Min(hp + heal, hp_max);

        UI.slider.maxValue = hp_max;
        UI.slider.value = hp;
        showHealNumber(heal);
    }

    public int GetHP()
    {
        return hp;
    }

    public void time_effects()
    {
        for(int i = 0; i < _buffs.Length; i++)
        {
            if (_buffs[i] == null || _buffs[i].Name == "")
            {
                continue;
            }
            var _buff = _buffs[i];
            _buff.turnsLeft--;
            if (_buff.turnsLeft <0)
            {
                ApplyBuff(_buff, -1);
                SetBuffToNull(i);
                stat_texts[i].Set_Off();
            }
        }


        for (int i = 0; i < _overTimeAbilities.Count; i++)
        {
            var OTA = _overTimeAbilities[i]; // OTA overtime ability
            Apply_OTA(OTA.power, false, OTA._ability);
            OTA.turns_left -= 1;
            if (OTA.turns_left <= 0 && OTA.uses_left <= 0)
            {
                _overTimeAbilities.Remove(OTA);
            }
        }
    }

    public void SetBuffToNull(int i)
    {
        _buffs[i].Name = "";

    }

    public void AddBuff(Effects.Buff _buff)
    {
        foreach (Effects.Buff _buff_name in _buffs)
        {
            if(_buff_name == null)
            {
                continue;
            }
            if (_buff_name.Name == _buff.Name)
            {
                _buff_name.turnsLeft = _buff.turnsLeft;
                return;
            }
        }

        var _new_buff = new Effects.Buff();
        _new_buff.Name = _buff.Name;
        foreach(Effects.BuffEffect stat in _buff.stats)
        {
            var _new_stat = new Effects.BuffEffect();
            _new_stat.stat = stat.stat;
            _new_stat.power = stat.power;
            _new_buff.stats.Add(_new_stat);
            
        }
        _new_buff.turnsLeft = _buff.turnsLeft;
        for (int i = 0; i < _buffs.Length; i++)
        {
            if (_buffs[i] == null || _buffs[i].Name == "")
            {
                _buffs[i] = (_new_buff);
                ApplyBuff(_new_buff, 1);
                AddStatText(_new_buff.Name, i);
                break;
            }
        }

    }

    public void AddStatText(string name, int position)
    {
        var _stat_text = Instantiate(p_varibles.StatText, new Vector2(transform.position.x + p_varibles.buff_text_positions[position].x, transform.position.y + p_varibles.buff_text_positions[position].y), Quaternion.identity);
        _stat_text.GetComponent<StatusText>()._text = name;
        _stat_text.transform.parent = transform;
        stat_texts[position] =(_stat_text.GetComponent<StatusText>());

    }

    public void ApplyBuff(Effects.Buff buff, int add)
    {
        if(buff.Name =="")
        {
            return;
        }
        foreach(Effects.BuffEffect effect in buff.stats )
        {
            switch(effect.stat)
            {
                case BuffEffects.strenght:
                    strenght += effect.power * add;
                    break;
                case BuffEffects.intellect:
                    intellect += effect.power * add;
                    break;
                case BuffEffects.defence:
                    defence += effect.power * add;
                    break;
                case BuffEffects.resistance:
                    resistance += effect.power * add;
                    break;
            }
        }
        Debug.Log(buff.Name);
    }

    public void RemoveEnemy()
    {
        Destroy(this.gameObject);
    }

}
