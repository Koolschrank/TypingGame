public enum Mode
{
    ability_select,
    attack_typing,
    attack_animation,
    defense_typing,
    enemy_attack_animation
}
public enum Cost
{
    nothing,
    hp,
    mp,
    consumable
}

public enum Target
{
    one,
    multible,
    self,
    all
}

public enum ability_typ
{
    physical,
    magical,
    heal,
    buff,
    debuff,
    item_heal,
    mp_heal
}

public enum effects
{
    low_hp_power_boost,
    effect_over_time,
    buff,
    absorb,
    oneShotBoost,
}

public enum turns
{
    player_step_forward,
    player_ability_select,
    player_attack_typing,
    player_attack_animation,
    player_step_back,
    enemy_step_forward,
    enemy_attack_typing,
    enemy_attack_animation,
    enemy_step_back,
}

public enum battle_animation
{
    attack,
    cast,
    use
}

public enum shake
{
    very_small,
    small,
    medium,
    big, 
    very_big,
    extrem
}

public enum AI_behaviour
{
    Attack,
    on_self,
    on_low_health_allie,
    on_allie,
    on_random_allie
}

public enum AI_condition
{
    nothing,
    low_health,
    low_allie_health,
}

public enum Element
{
    Normal,
    Fire,
    Ice,
    Thunder,
    Poisen,
    Heal
}

public enum Effectiveness
{
    normal,
    weak,
    resistent,
    immun,
    absorb
}

public enum BuffEffects
{
    strenght,
    intellect,
    defence,
    resistance,
    Fire,
    Ice,
    Thunder,
    Poisen
}

public enum LootTyp
{
    Weapon,
    spell,
    skill,
    item,
}

public enum EnemyResistens
{
    normal,
    Defesive,
    Resistend,
}

public enum passiveSkill
{
    magicUser, // can use magic
    noTypoBoost, // less cost if no typo (25%)
    comboBoost, // type time reduce by half
    rage, // if below 50% HP, strenght doubles
    Phenix, // revives once per battle(50%hp), cost magic (10)
    itemBoost, // items 50%better when no typo
}

public enum state
{
    open_backpack,
    close_backpack,
    opend,
    closed,
    inWeapon,
    inMagic,
    inItem,
    inBody,
    inLevelUp
}

public enum currentSelection
{
    selectAbility,
    selectAction,
    selectSwap,
    selectAdd,
}

public enum textGimmick
{
    none,
    firstLetterUppercase,
    swap_uppercase_lowercase
}

public enum level_stats
{
    health,
    magic,
    capability,
}

public enum stat_lv
{
    hp,
    mp,
    weapon_slots,
    hp_cost,
    magic_slots,
    item_slots,
}
