using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "playerBody")]
public class PlayerBody : ScriptableObject
{
    public string _name;
    public Sprite sprite;
    public AnimatorOverrideController animaton;
    public int strenght, intellect, defence, resistance;
    public Ability[] abilities = new Ability[6];
    public Effectiveness fire, ice, thunder;
    public passiveSkill[] passiveSkills;
    public string info;
}
