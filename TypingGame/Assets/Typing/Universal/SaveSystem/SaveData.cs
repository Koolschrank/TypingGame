using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player_save
{
    public int hp_max, hp, mp_max, mp, sp_max, sp, strenght, intellect, defence, resistance, currentBody, level,souls,level_h,level_m,level_c;
    public float hp_cost;
    public int[]slots = new int[5];
    public List<string> Weapons = new List<string>(), Magic = new List<string>(), Skills = new List<string>(), Items = new List<string>(), Items_perma = new List<string>(), Bodies = new List<string>();
    public bool[] keys;


    public Player_save(PlayerStats player)
    {
        hp_max = player.hp_max;
        hp = player.GetHP();
        mp_max = player.mp_max;
        mp = player.GetMP();
        sp_max = player.sp_max;
        sp = player.GetSP();
        strenght = player.strenght;
        intellect = player.intellect;
        defence = player.defence;
        resistance = player.resistance;
        hp_cost = player.hp_cost_percent;
        level = player.level;
        souls = player.souls;
        level_h = player.level_health;
        level_m = player.level_magic;
        level_c = player.level_capability;
        keys = player.keys;

        if (player.Ability_typs[0].abilities.Count !=0)
        {
            slots[0] = player.Ability_typs[0].slots;
            foreach (Ability ability in player.Ability_typs[0].abilities)
            {
                if(ability ==null)
                {
                    Weapons.Add("null");
                    continue;
                }
                Weapons.Add(ability._name);
            }
        }
        if (player.Ability_typs[1].abilities.Count !=0)
        {
            slots[1] = player.Ability_typs[1].slots;
            foreach (Ability ability in player.Ability_typs[1].abilities)
            {
                if (ability == null)
                {
                    Magic.Add("null");
                    continue;
                }
                Magic.Add(ability._name);
            }
        }
        if (player.Ability_typs[2].abilities.Count != 0)
        {
            slots[2] = player.Ability_typs[2].slots;
            foreach (Ability ability in player.Ability_typs[2].abilities)
            {
                if (ability == null)
                {
                    Skills.Add("null");
                    continue;
                }
                Skills.Add(ability._name);
            }
        }
        if (player.Ability_typs[3].abilities.Count != 0)
        {
            slots[3] = player.Ability_typs[3].slots;
            foreach (Ability ability in player.Ability_typs[3].abilities)
            {
                if (ability == null)
                {
                    Items.Add("null");
                    continue;
                }
                Items.Add(ability._name);
            }
        }
        if (player.Ability_typs[4].abilities.Count != 0)
        {
            slots[4] = player.Ability_typs[4].slots;
            foreach (Ability ability in player.Ability_typs[4].abilities)
            {
                if (ability == null)
                {
                    Items_perma.Add("null");
                    continue;
                }
                Items_perma.Add(ability._name);
            }
        }
        if (player.playerBodies.Count !=0)
        {
            currentBody = player.currentBody;
            foreach (PlayerBody body in player.playerBodies)
            {
                if (body == null)
                {
                    Bodies.Add("null");
                    continue;
                }
                Bodies.Add(body._name);

            }
            
        }
    }
}

[System.Serializable]
public class Backpack_Save
{
    public List<string> Weapons = new List<string>(), Magic = new List<string>(), Skills = new List<string>(), Items = new List<string>(), Bodies = new List<string>();
    public Backpack_Save(Backpack backpack)
    {
        


        if (backpack.abilities[0].abilities.Count != 0)
        {
            foreach (Ability ability in backpack.abilities[0].abilities)
            {
                if (ability == null)
                {
                    Weapons.Add("null");
                    continue;
                }
                Weapons.Add(ability._name);
            }
        }
        if (backpack.abilities[1].abilities.Count != 0)
        {
            foreach (Ability ability in backpack.abilities[1].abilities)
            {
                if (ability == null)
                {
                    Magic.Add("null");
                    continue;
                }
                Magic.Add(ability._name);
            }
        }
        if (backpack.abilities[2].abilities.Count != 0)
        {
            foreach (Ability ability in backpack.abilities[2].abilities)
            {
                if (ability == null)
                {
                    Skills.Add("null");
                    continue;
                }
                Skills.Add(ability._name);
            }
        }
        if (backpack.abilities[3].abilities.Count != 0)
        {
            foreach (Ability ability in backpack.abilities[3].abilities)
            {
                if (ability == null)
                {
                    Items.Add("null");
                    continue;
                }
                Items.Add(ability._name);
            }
        }

        if (backpack.playerBodies.Count != 0)
        {
            foreach (PlayerBody body in backpack.playerBodies)
            {
                if (body == null)
                {
                    Bodies.Add("null");
                    continue;
                }
                Bodies.Add(body._name);
                Debug.Log(body._name + " bodieSaved");
            }

        }
    }
}

[System.Serializable]
public class Position_Save
{


    private string[] _index;
    private float[] position_x, position_y;
    private bool[] inGame;

    public Position_Save(SaveableObject[] _objects, bool atCheckpoint, Position_Save oldSave)
    {
        Debug.Log(_objects.Length);
        if (oldSave != null)
            Debug.Log(oldSave._index.Length);
        List<float> list_x = new List<float>();
        List<float> list_y = new List<float>();
        List<bool> list_b = new List<bool>();
        List<string> list_i = new List<string>();
        for (int i = 0; i < _objects.Length; i++)
        {
            if (atCheckpoint && !_objects[i].stay_after_reloard)
            {
                list_i.Add(_objects[i].uniqueIdentifier);
                list_x.Add(_objects[i].transform.position.x);
                list_y.Add(_objects[i].transform.position.y);
                list_b.Add(true);
                continue;
            }
            list_i.Add(_objects[i].uniqueIdentifier);
            list_x.Add( _objects[i].transform.position.x);
            list_y.Add( _objects[i].transform.position.y);
            list_b.Add( _objects[i].inGame);
        }
        if (oldSave != null)
        {
            for (int i = 0; i < oldSave._index.Length; i++)
            {
                list_i.Add(oldSave._index[i]);
                list_x.Add(oldSave.position_x[i]);
                list_y.Add(oldSave.position_y[i]);
                list_b.Add(oldSave.inGame[i]);
                continue;
            }
        }



        position_x = new float[list_i.Count];
        position_y = new float[list_i.Count];
        inGame = new bool[list_i.Count];
        _index = new string[list_i.Count];
        for (int i = 0; i < list_i.Count; i++)
        {

            _index[i] = list_i[i];
            position_x[i] = list_x[i];
            position_y[i] = list_y[i];
            inGame[i] = list_b[i];
        }
    }

    public string[] Get_Index()
    {
        return _index;
    }

    public float[] Get_x()
    {
        return position_x;
    }

    public float[] Get_y()
    {
        return position_y;
    }

    public bool[] Get_In_game()
    {
        return inGame;
    }

}

[System.Serializable]
public class Settings_save
{
    public string scene;
    public float WPM;
    public bool autoMode, doubleWords;
    public Settings_save(Settings settings)
    {
        scene = settings._scene;
        WPM = settings.WPM;
        autoMode = settings.autoMode;
        doubleWords = settings.doublWordCount;
    }

    public Settings_save(Settings_save settings, float newWPM, bool newAutoMode, bool newWordCount)
    {
        scene = settings.scene;
        WPM = newWPM;
        autoMode = newAutoMode;
        doubleWords = newWordCount;
    }

    public Settings_save(string newScene, float newWPM, bool newAutoMode, bool newWordCount)
    {
        scene = newScene;
        WPM = newWPM;
        autoMode = newAutoMode;
        doubleWords = newWordCount;
    }

}

[System.Serializable]
public class SaveFile
{
    private Position_Save positions;
    private Player_save player;
    private Backpack_Save backpack;
    private Settings_save settings;

    public SaveFile(Position_Save positions, Player_save player, Backpack_Save backpack, Settings_save settings)
    {
        this.positions = positions;
        this.player = player;
        this.backpack = backpack;
        this.settings = settings;
    }

    public Position_Save GetPosition()
    {
        return positions;
    }

    public Player_save GetPlayer()
    {
        return player;
    }

    public Backpack_Save GetBackpack()
    {
        return backpack;
    }

}
