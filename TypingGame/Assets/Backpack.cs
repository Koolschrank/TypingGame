using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Backpack : MonoBehaviour
{
    public Player_universal.Ability_Typ[] abilities;
    public List<PlayerBody> playerBodies = new List<PlayerBody>();
    public AbilityFolder AllAbilities;


    public void Start()
    {

    }

    public void AddAbility(int typ, Ability ability)
    {
        abilities[typ].abilities.Add(ability);
        Debug.Log("worked");
    }

    public Ability SwapAbilitiy(int typ, int place, Ability ability)
    {
        var wanted_ability = abilities[typ].abilities[place];
        abilities[typ].abilities[place] = ability;


        return wanted_ability;
    }

    public void LoadBackpackSave(string saveName)
    {
        foreach (Player_universal.Ability_Typ _at in abilities)
        {
            _at.abilities.Clear();
        }

        Backpack_Save _save = SaveSystem.loadBackpackFile(saveName);

        //AbilityFolder folder = Player_universal.Get_Ability_Folder("AllAbilities");
        LoadAbilityFolder(AllAbilities, _save.Weapons, 0);
        LoadAbilityFolder(AllAbilities, _save.Magic, 1);
        LoadAbilityFolder(AllAbilities, _save.Skills, 2);
        LoadAbilityFolder(AllAbilities, _save.Items, 3);
        LoadBodies(AllAbilities,_save.Bodies);


    }

    public void LoadAbilityFolder(AbilityFolder folder, List<string> list, int folderInt)
    {
        foreach (string _string in list)
        {
            foreach (Ability _ability in folder.Abilities)
            {
                if (_ability._name == _string)
                {
                    abilities[folderInt].abilities.Add(_ability);
                    break;
                }
            }
        }
    }

    public void LoadBodies(AbilityFolder folder, List<string> list)
    {
        playerBodies.Clear();
        foreach (string _string in list)
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


}
