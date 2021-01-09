using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    AbilitySelect UI;
    public int hp_max, mp_max, sp_max, hp, mp, sp, strenght, intellect, defence, resistance;
    public float hp_cost_percent = 1f;
    float no_typo_boost = 1.5f;
    bool phenix_used;
    public Player_universal.Ability_Typ[] Ability_typs = null;
    public Effectiveness fire, ice, thunder;
    public passiveSkill[] passiveSkills;
    public List<PlayerBody> playerBodies = new List<PlayerBody>();
    public int currentBody, startingBody;
    public bool can_swich_body= true;
    Ability currentAbility;
    BattleSystem _system;
    ScreenShake shaker;
    Animator animator;
    SpriteRenderer sprite;
    public List<Effects.over_time_ability> _overTimeAbilities = new List<Effects.over_time_ability>();
    public List<Effects.Buff> _buffs = new List<Effects.Buff>();
    List<StatusText> stat_texts = new List<StatusText>();
    public AbilityFolder AllAbilities;
    public PublicVaribles p_varibles;
    public int level, souls, levelUP_startCost, levelUP_levelAdditionCost, level_health, level_magic, level_capability;
    



    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        UI = FindObjectOfType<AbilitySelect>();
        _system = FindObjectOfType<BattleSystem>();
        shaker = FindObjectOfType<ScreenShake>();
        if (!FindObjectOfType<SceneTransitioner>() || !FindObjectOfType<SceneTransitioner>().saveTest)
        {
            RefillStats();
            
        }
        

    }



    public void LoadPlayerSave(string saveName)
    {
        foreach(Player_universal.Ability_Typ _at in Ability_typs)
        {
            _at.abilities.Clear();
        }

        var _save = SaveSystem.loadPlayerFile(saveName);
        hp_max = _save.hp_max;
        hp = _save.hp;
        mp_max = _save.mp_max;
        mp = _save.mp;
        sp_max = _save.sp_max;
        sp = _save.sp;
        hp_cost_percent = _save.hp_cost;
        level = _save.level;
        souls = _save.souls;
        level_health = _save.level_h;
        level_magic = _save.level_m;
        level_capability = _save.level_c;
        //strenght = _save.strenght;
        //intellect = _save.intellect;
        //defence = _save.defence;
        //resistance = _save.resistance;

        //AbilityFolder folder = Player_universal.Get_Ability_Folder("AllAbilities");
        LoadAbilityFolder(AllAbilities, _save.Weapons, 0, _save.slots);
        LoadAbilityFolder(AllAbilities, _save.Magic, 1, _save.slots);
        LoadAbilityFolder(AllAbilities, _save.Skills, 2, _save.slots);
        LoadAbilityFolder(AllAbilities, _save.Items, 3, _save.slots);
        LoadAbilityFolder(AllAbilities, _save.Items_perma, 4, _save.slots);
        LoadPlayerBodies(AllAbilities, _save);
        SetBody(playerBodies[currentBody]);


        //SceneManager.LoadScene("Battle", LoadSceneMode.Single);
        if (SceneManager.GetActiveScene().name == "Battle")
        {
            if(UI != null)
            UI.UpdateSlider();
        }
        
    }

    public void LoadAbilityFolder(AbilityFolder folder, List<string> list, int folderInt,int[] slots)
    {
        Ability_typs[folderInt].slots = slots[folderInt];
        foreach (string _string in list)
        {
            foreach (Ability _ability in folder.Abilities)
            {
                if (_ability._name == _string)
                {
                    Ability_typs[folderInt].abilities.Add(_ability);
                    break;
                }
            }
        }
    }

    public void LoadPlayerBodies(AbilityFolder folder, Player_save _save)
    {
        playerBodies.Clear();
        currentBody = _save.currentBody;
        foreach (string _string in _save.Bodies)
        {
            foreach (PlayerBody _body in folder.playerBodies)
            {

                if (_body._name == _string)
                {
                    playerBodies.Add(_body);
                    break;
                }
            }
        }
    }

    public void SavePlayer(string saveName)
    {
        SaveSystem.SavePlayer(this, saveName);
    }

    public void RefillStats()
    {
        hp = hp_max;
        mp = mp_max;
        sp = sp_max;
        RefillItems();
        if (FindObjectOfType<AbilitySelect>())
        FindObjectOfType<AbilitySelect>().UpdateSlider();
    }

    public void RefillItems()
    {
        Ability_typs[3].abilities.Clear();
        foreach(Ability ability in Ability_typs[4].abilities)
        {
            Ability_typs[3].abilities.Add(ability);
        }
        Ability_typs[3].slots = Ability_typs[3].abilities.Count;
    }

    private void Update()
    {
        // remove later
        //if (Input.GetKeyDown("k"))
        //{
        //    SavePlayer();
        //}

    }

    public void SetBody(PlayerBody newBody)
    {

        strenght = newBody.strenght;
        intellect = newBody.intellect;
        defence = newBody.defence;
        resistance = newBody.resistance;
        

        Ability_typs[2].abilities.Clear();
        for (int i =0; i < 8; i++)
        {
            if (newBody.abilities.Length <= i || newBody.abilities[i] == null) break;
            Ability_typs[2].abilities.Add(newBody.abilities[i]);
        }
        Ability_typs[2].slots = Ability_typs[2].abilities.Count;

        if (GetComponentInChildren<Animator>())
        {
            animator = GetComponentInChildren<Animator>();
            animator.runtimeAnimatorController = newBody.animaton;
        }
           
        if(GetComponentInChildren<SpriteRenderer>())
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            sprite.sprite = newBody.sprite;
        }
        fire = newBody.fire;
        ice = newBody.ice;
        thunder = newBody.thunder;
        passiveSkills = newBody.passiveSkills;
        foreach (Effects.Buff _buff in _buffs)
        {
            ApplyBuff(_buff, 1);
        }
        //BodySwapPartical();
        if (UI != null)
            UI.Set_UI();
    }

    public void BodySwapPartical()
    {
        GameObject partica = Instantiate(p_varibles.bodySwapPartical.gameObject, transform.position, Quaternion.Euler(90,0,0)) as GameObject;
        Destroy(partica, 2f);
    }


    public int GetMP()
    {
        return mp;
    }

    public int GetHP()
    {
        return hp;
    }

    public int GetSP()
    {
        return sp;
    }

    public Vector2 Get_hp()
    {
        return new Vector2(hp, hp_max);
    }

    public Vector2 Get_mp()
    {
        return new Vector2(mp, mp_max);
    }

    public void Use_Ability(float score, bool typos_made)
    {
        if(currentAbility.castSound != null)
        {
            FindObjectOfType<AudioBox>().PlaySound(currentAbility.castSound, currentAbility.soundVolume);
        }

        if (currentBody != startingBody)
        {
            can_swich_body = false;
        }
        switch (currentAbility._cost_typ)
        {
            case Cost.hp:
                hp = Mathf.Max(1, hp- (int)(Change_Stat(currentAbility._cost, !typos_made) * hp_cost_percent));
                UI.UpdateSlider();
                break;
            case Cost.mp:
                if (CheckForPassiv(passiveSkill.big_brain) && currentAbility._typ == ability_typ.magical)
                {
                    mp -= (int)(Change_Stat(currentAbility._cost, !typos_made)* 1.50f); //1.50 is hardcoded sadly
                }
                else
                {
                    mp -= Change_Stat(currentAbility._cost, !typos_made);
                }
                
                if (CheckForPassiv(passiveSkill.Mp_to_HP))
                {
                    Heal(currentAbility._cost);
                }
                
                UI.UpdateSlider();
                break;
            case Cost.consumable:
                
                break;
            case Cost.nothing:
                break;
        }

        currentAbility.SetUser(gameObject);
        FindObjectOfType<BattleSystem>().Player_move(score, typos_made, currentAbility);
        UI.Set_UI();

    }

    public void Set_Ability(Ability _ability)
    {
        currentAbility = _ability;
    }

    public int Change_Stat(int _stat, bool noTypo)
    {
        if (noTypo)
        {
            if (CheckForPassiv(passiveSkill.noTypoBoost))
            {
                return (int)(_stat / no_typo_boost);
            }
        }
        return _stat;
    }

    public bool CheckForPassiv(passiveSkill skill)
    {
        foreach (passiveSkill p_skill in passiveSkills)
        {
            if (p_skill == skill)
            {
                return true;
            }
        }
        return false;
    }

    public void Apply_ability(int power, bool typos_made, Ability ability, float score)
    {
        
        foreach (Effects._effect _effect in ability.effect_array)
        {
            if (_effect.effect == effects.effect_over_time)
            {
                _overTimeAbilities.Add(Effects.Create_over_time_ability(power, ability));
            }
            else if (_effect.effect == effects.buff && score>0)
            {
                AddBuff(AllAbilities.BuffList[(int)_effect.value1]);
            }
        }

        switch (ability._typ)
        {
            case ability_typ.physical:
                Take_damage(power, ability, score);
                break;
            case ability_typ.magical:
                Take_damage(power, ability, score);
                break;
            case ability_typ.heal:
                Heal(power);
                break;
            case ability_typ.item_heal:
                Heal(power);
                break;
            case ability_typ.mp_heal:
                MPHeal(power);
                break;
        }
    }

    public void AddBuff(Effects.Buff _buff)
    {
        foreach(Effects.Buff _buff_name in _buffs)
        {
            if (_buff_name.Name == _buff.Name)
            {
                _buff_name.turnsLeft = _buff.turnsLeft;
                return;
            }
        }

        var _new_buff = new Effects.Buff();
        _new_buff.Name = _buff.Name;
        foreach (Effects.BuffEffect stat in _buff.stats)
        {
            var _new_stat = new Effects.BuffEffect();
            _new_stat.stat = stat.stat;
            _new_stat.power = stat.power;
            _new_buff.stats.Add(_new_stat);

        }
        _new_buff.turnsLeft = _buff.turnsLeft;
        _buffs.Add(_new_buff);
        ApplyBuff(_new_buff, 1);
        AddStatText(_new_buff.Name, _buffs.Count - 1);
    }

    public void Apply_OTA(int power, bool typos_made, Ability ability,float score)
    {
        switch (ability._typ)
        {
            case ability_typ.physical:
                Take_damage(power, ability, score);
                break;
            case ability_typ.magical:
                Take_damage(power, ability, score);
                break;
            case ability_typ.heal:
                Heal(power);
                break;
        }
    }

    public void Take_damage(int damage, Ability attack_stats, float score)
    {

        damage = CheckElementalEffects(attack_stats, damage);
        switch (attack_stats._typ)
        {
            case ability_typ.physical:
                damage -= ApplyScore(defence, score);
                break;
            case ability_typ.magical:
                damage -= ApplyScore(resistance, score); 
                break;
            default:
                
                break;
        }

        


        if (Effects.Check_for_effect(attack_stats.effect_array, effects.absorb) != null)
        {
            var effect_stats = Effects.Check_for_effect(attack_stats.effect_array, effects.absorb);
            attack_stats.GetUser().Heal(Effects.Absorb(damage, effect_stats.value1));
        }

        if (damage > 0)
        {
            GetComponent<CharacterAnimation>().Take_Damage();
        }
        showDamageNumber(damage);
        hp -= Mathf.Max(0, damage);
        if (hp <= 0)
        {
            if(CheckForPassiv(passiveSkill.Phenix)&& !phenix_used)
            {
                phenix_used = true;
                mp = Mathf.Max(0, mp - 10);
                hp = 0;
                Heal(hp_max / 2);
            }
            else
            {
                Debug.Log("plea ded");
                FindObjectOfType<SceneTransitioner>().LoadGameFile();
                shaker.ShakeCam(shake.extrem);
            }


           
        }
        else if (damage>0)
        {
            shaker.ShakeCam(shake.medium);
        }

        UI.UpdateSlider();
    }

    public void Take_damage(int damage)
    {
        if (damage > 0)
        {
            GetComponent<CharacterAnimation>().Take_Damage();
        }
        showDamageNumber(damage);
        hp -= Mathf.Max(0, damage);
        if (hp <= 0)
        {
            if (CheckForPassiv(passiveSkill.Phenix) && !phenix_used)
            {
                phenix_used = true;
                mp = Mathf.Max(0, mp - 10);
                hp = 0;
                Heal(hp_max / 2);
            }
            else
            {
                Debug.Log("plea ded");
                FindObjectOfType<SceneTransitioner>().LoadGameFile();
                shaker.ShakeCam(shake.extrem);
            }



        }
        else if (damage > 0)
        {
            shaker.ShakeCam(shake.medium);
        }

        UI.UpdateSlider();
    }

    public int CheckElementalEffects(Ability ability, int damage)
    {
        switch(ability._element)
        {
            case Element.Fire:
                return ChangeElementalDamage(fire, damage);
            case Element.Ice:
                return ChangeElementalDamage(ice, damage);
            case Element.Thunder:
                return ChangeElementalDamage(thunder, damage);
            default:
                return damage;
        }
    }

    public int ChangeElementalDamage(Effectiveness effect,int damage)
    {
        switch (effect)
        {
            case Effectiveness.weak:
                Create_weakness_text("WEAK");
                return (int)(damage * 1.25);
            case Effectiveness.resistent:
                Create_weakness_text("RESISTEND");
                return damage / 2;
            case Effectiveness.immun:
                Create_weakness_text("NULL");
                return 0;
            case Effectiveness.absorb:
                Create_weakness_text("ABSORB");
                Heal(damage -resistance);
                return 0;
            default:
                return damage;

        }
       
    }

    public void Create_weakness_text(string _string)
    {
        var Weak_Text = Instantiate(p_varibles.weakText, transform.position, Quaternion.identity);
        Weak_Text.GetComponent<WeaknessText>().Set_String(_string);
    }

    public int ApplyScore(int _value, float score )
    {
        if (score==0)
        {
            return _value / 2;
        }
        else if (score >=0.5)
        {
            return (int)(_value * 1.5f);
        }
        else if (score == -1)
        {
            Debug.Log("buff");
            return (_value);
        }
        else
        {
            return (int)(_value * (score + 1));
        }


        
    }

    public void showDamageNumber(int _number)
    {
        GameObject _damage_text = Instantiate(_system.damage_text, transform.position, transform.rotation) as GameObject;
        _damage_text.GetComponentInChildren<TextMesh>().text = _number.ToString();
    }

    public void showHealNumber(int _number)
    {
        GameObject _heal_text = Instantiate(_system.HealText, transform.position, transform.rotation) as GameObject;
        _heal_text.GetComponentInChildren<TextMesh>().text = Mathf.Max(0, _number).ToString();
    }

    public void Heal(int heal)
    {
        if (CheckForPassiv(passiveSkill.Undead))
        {
            Take_damage(heal);
            return;
        }
        hp = Mathf.Min(hp+heal,hp_max);
        showHealNumber(heal);
        UI.UpdateSlider();
    }

    public void MPHeal(int heal)
    {
        mp = Mathf.Min(mp + heal, mp_max);

        UI.UpdateSlider();
    }

    public void AddStatText(string name, int position)
    {
        var _stat_text = Instantiate(p_varibles.StatText, new Vector2(transform.position.x + p_varibles.buff_text_positions[position].x, transform.position.y + p_varibles.buff_text_positions[position].y), Quaternion.identity);
        _stat_text.GetComponent<StatusText>()._text = name;
        _stat_text.transform.parent = transform;
        stat_texts.Add(_stat_text.GetComponent<StatusText>());

    }

    public void time_effects()
    {
        for (int i = 0; i < _buffs.Count; i++)
        {
            var _buff = _buffs[i];
            _buff.turnsLeft--;
            if (_buff.turnsLeft < 0)
            {
                _buffs.RemoveAt(i);
                SetBody(playerBodies[currentBody]);
                stat_texts[i].Set_Off();
            }
        }


        for (int i = 0; i < _overTimeAbilities.Count; i++)
        {
            var OTA = _overTimeAbilities[i]; // OTA overtime ability
            Apply_OTA(OTA.power, false, OTA._ability, -1); // fix -1 with real score someday
            OTA.turns_left -= 1;
            if (OTA.turns_left <= 0 && OTA.uses_left <= 0)
            {
                _overTimeAbilities.Remove(OTA);
            }
        }
    }

    public void ApplyBuff(Effects.Buff buff, int add)
    {
        foreach (Effects.BuffEffect effect in buff.stats)
        {
            switch (effect.stat)
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
                case BuffEffects.Fire:
                    if (fire == Effectiveness.absorb || (fire == Effectiveness.immun && effect.power < 2))
                    {
                        break;
                    }
                    else
                    {
                        SetElementalBuff(Element.Fire, effect.power, add);
                        break;
                    }
                case BuffEffects.Ice:
                    if (ice == Effectiveness.absorb || (ice == Effectiveness.immun && effect.power < 2))
                    {
                        break;
                    }
                    else
                    {
                        SetElementalBuff(Element.Ice, effect.power, add);
                        break;
                    }
                case BuffEffects.Thunder:
                    if (thunder == Effectiveness.absorb || (thunder == Effectiveness.immun && effect.power < 2))
                    {
                        break;
                    }
                    else
                    {
                        SetElementalBuff(Element.Thunder, effect.power, add);
                        break;
                    }
            }
        }
        Debug.Log(buff.Name);
    }

    public void SetElementalBuff(Element element,int effectPower, int goBack)
    {
        if(goBack== -1)
        {
            return ;
        }

        switch (effectPower)
        {
            case 0:
                ApplyEffectivness(element, Effectiveness.weak);
                break;
            case 1:
                ApplyEffectivness(element, Effectiveness.resistent);
                break;
            case 2:
                ApplyEffectivness(element, Effectiveness.immun);
                break;
            case 3:
                ApplyEffectivness(element, Effectiveness.absorb);
                break;
        }
    }

    public void ApplyEffectivness(Element element, Effectiveness effect)
    {
        switch(element)
        {
            case Element.Fire:
                fire = effect;
                break;
            case Element.Ice:
                ice = effect;
                break;
            case Element.Thunder:
                thunder = effect;
                break;
        }
    }

    public void RemoveAllBuffs()
    {
        for (int i = 0; i < _buffs.Count; i++)
        {
            ApplyBuff(_buffs[i], -1);
            stat_texts[i].Set_Off();

        }
    }

    public void SetStartingBody()
    {
        can_swich_body = true;
        startingBody = currentBody;
        if (UI != null)
            UI.Set_UI();
    }

    public void GainSouls(int enemy_souls)
    {
        souls += enemy_souls;
    }

    public void LevelUp(Level_stat stat)
    {
        level++;
        switch(stat._enum)
        {
            case level_stats.health:
                foreach(stat_evolve _stat in stat.stats)
                {
                    StatChange(_stat, level_health,1);
                }

                level_health++;
                break;
            case level_stats.magic:
                foreach (stat_evolve _stat in stat.stats)
                {
                    StatChange(_stat, level_magic,1);
                }

                level_magic++;
                break;
            case level_stats.capability:
                foreach (stat_evolve _stat in stat.stats)
                {
                    StatChange(_stat, level_capability,1);
                }

                level_capability++;
                break;
        }
    }

    public void LevelDown(Level_stat stat)
    {
        level--;
        switch (stat._enum)
        {
            case level_stats.health:
                level_health--;
                foreach (stat_evolve _stat in stat.stats)
                {
                    StatChange(_stat, level_health,-1);
                }

                
                break;
            case level_stats.magic:
                level_magic--;
                foreach (stat_evolve _stat in stat.stats)
                {
                    StatChange(_stat, level_magic,-1);
                }

                
                break;
            case level_stats.capability:
                level_capability--;
                foreach (stat_evolve _stat in stat.stats)
                {
                    StatChange(_stat, level_capability,-1);
                }

                
                break;
        }
    }

    public void StatChange(stat_evolve stat, int level, int value)
    {
        int power = stat.value[level];
        switch(stat.stat)
        {
            case stat_lv.hp:
                hp_max += power * value;
                hp += power * value;
                break;
            case stat_lv.mp:
                mp_max += power * value;
                mp += power * value;
                break;
            case stat_lv.weapon_slots:
                Ability_typs[0].slots += power * value;
                break;
            case stat_lv.magic_slots:
                Ability_typs[1].slots += power * value;
                break;
            case stat_lv.item_slots:
                Ability_typs[3].slots += power * value;
                Ability_typs[4].slots += power * value;
                break;
            case stat_lv.hp_cost:
                hp_cost_percent -= power * value / 100f;
                break;
        }
    }
}
