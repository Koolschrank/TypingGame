using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public GameObject ItimIcon;
    public Ability loot;
    public LootTyp typ = LootTyp.Weapon;
    public bool transfer_to_backpack = true;
    bool open = false;
    public PublicVaribles PV;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!open && !GetComponent<SaveableObject>().inGame)
        {
            GetComponent<Animator>().Play("open");
            open = true;
        }
    }

    public void Add_Ability_to_player(PlayerStats player)
    {
        if(PV !=null)
        {
            FindObjectOfType<AudioBox>().PlaySound(PV.pickUpSound, 1);
        }
        
        switch(typ)
        {
            case LootTyp.Weapon:
                Add_Ability(player, 0);
                break;
            case LootTyp.spell:
                Add_Ability(player, 1);
                break;
            case LootTyp.skill:
                Add_Ability(player, 2);
                break;
            case LootTyp.item:
                Add_Ability(player, 3);
                break;
        }
        open = true;
        GetComponent<SaveableObject>().inGame = false;
        GetComponent<Animator>().Play("open");
        playLootAnimation();
    }

    public void Add_Ability(PlayerStats player, int typ)
    {
        if(transfer_to_backpack)
        {
            player.gameObject.GetComponent<Backpack>().AddAbility(typ, loot);
            Debug.Log(loot._name);
            return;
        }
        
        player.Ability_typs[typ].abilities.Add(loot);
        Debug.Log(loot._name);
    }

    public void playLootAnimation()
    {
        GameObject icon = Instantiate(ItimIcon, new Vector3(transform.position.x, transform.position.y, 3) , transform.rotation) as GameObject;
        icon.transform.parent = transform;
        icon.GetComponent<abilityIcon>().SetIcon(loot._name, loot.menuIcon);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(!open &&collision.GetComponent<PlayerStats>() && Time.timeSinceLevelLoad>1f)
        {
            Add_Ability_to_player(collision.GetComponent<PlayerStats>());
        }
    }

}
