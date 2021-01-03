using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElementalEffectiveness
{
    public Element element= Element.Fire;
    public Effectiveness effect = Effectiveness.weak;
}

[System.Serializable]
public class Level_stat
{
    public string Name;
    public level_stats _enum;
    public List<stat_evolve> stats = new List<stat_evolve>();

}
[System.Serializable]
public class stat_evolve
{
    public stat_lv stat;
    public List<int> value = new List<int>();


}

