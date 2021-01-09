using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability")]
public class Ability : ScriptableObject
{

    public string _name; 
    [TextArea]
    public string _words;
    public int _word_count = 5;
    public float time_per_character = 1;
    public textGimmick gimmick = textGimmick.none;
    public Cost _cost_typ = Cost.nothing;
    public int _cost = 0, power;
    public float stat_strenght=1;
    public ability_typ _typ = ability_typ.physical;
    public Element _element = Element.Normal;
    public Target _target = Target.one;
    public battle_animation _animation = battle_animation.attack;
    public GameObject _sprite_object;
    public GameObject Impact_object;
    public float impact_size = 1;
    public AudioClip castSound;
    public float soundVolume=1f;
    public string _info = "text to read, yay";
    public Effects._effect[] effect_array;
    bool fromPlayer;
    EnemyStats enemy;
    List<GameObject> enemy_target = new List<GameObject>(); // only for BattleAI
    public Sprite menuIcon;
    public float icon_size = 2;
    public bool spriteRotatet;


    public void SetUser(GameObject _object)
    {
        if (_object.GetComponent<PlayerStats>())
        {
            fromPlayer = true;
        }
        else
        {
            enemy = _object.GetComponent<EnemyStats>();
        }
    }

    public EnemyStats GetUser()
    {
        return enemy;
    }

    public void Set_enemy_target(List<GameObject> _target)
    {
        enemy_target = _target;
    }

    public List<GameObject> Get_enemy_target()
    {
        return enemy_target;
    }
}
