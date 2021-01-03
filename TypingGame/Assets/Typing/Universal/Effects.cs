using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Effects
{
    [System.Serializable]
    public class _effect
    {
        public effects effect = effects.low_hp_power_boost;
        public float value1 = 0;
        public float value2 = 0;
    }

    public static _effect Check_for_effect(_effect[] list, effects seach_for)
    {
        foreach(_effect item in list)
        {
            if (seach_for == item.effect)
            {
                return item;
            }
        }
        
        return null;
    }

    // value1 = threshold; value2 = muliplier
    public static int Low_hp_power_boost(int power, int hp, int hp_max, float threshold, float multiplier)
    {
        if ((hp/hp_max) <= threshold)
        {
            return (int)(power * multiplier);
        }
        
        return power;
    }

    // value1 = percentage
    public static int Absorb(int damage,float percentage)
    {
        return (int)(damage * percentage);
    }

    public static bool maxHealthBoost(int maxHP, float HP)
    {
        return  (maxHP == HP);
    }


    // value1 = time
    public static over_time_ability Create_over_time_ability(int power, Ability _ability)
    {
        over_time_ability _overTimeAbility = new over_time_ability();
        _overTimeAbility._ability = _ability;
        _overTimeAbility.power = power;
        foreach(_effect _effect in _ability.effect_array)
        {
            if (_effect.effect == effects.effect_over_time)
            {
                _overTimeAbility.turns_left = (int)_effect.value1;
                _overTimeAbility.uses_left = (int)_effect.value2;
            }
        }
        return _overTimeAbility;
    }

    [System.Serializable]
    public class over_time_ability
    {
        public Ability _ability;
        public int power;
        public int turns_left;
        public int uses_left;
    }

    [System.Serializable]
    public class Buff
    {
        public string Name;
        public List<BuffEffect> stats = new List<BuffEffect>();
        public int turnsLeft;
        
    }

    [System.Serializable]
    public class BuffEffect
    {
        public BuffEffects stat;
        public int power; //0 weak , 1  resistent,  2  immun,3    absorb
    }


}
