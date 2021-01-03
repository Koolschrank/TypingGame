using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityFolder")]
public class AbilityFolder : ScriptableObject
{
    public List<Ability> Abilities;
    public List<PlayerBody> playerBodies;
    public List<Effects.Buff> BuffList;
}
