using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotRow : MonoBehaviour
{
    [SerializeField] MenuItem[] slots;
    public bool DontShowEmptySpace = false;


    public void SetSlots(Player_universal.Ability_Typ abilities)
    {
        Debug.Log(abilities.abilities.Count);
        for(int i = 0; i < 8;i++)
        {
            if (i < abilities.slots)
            {
                if (i>= abilities.slots || (DontShowEmptySpace && abilities.abilities.Count <= i))
                {
                    Debug.Log("other here");
                    slots[i].gameObject.SetActive(false);
                }
                else if (abilities.abilities.Count <= i)
                {
                    Debug.Log("Bruuuuuuuu");
                    slots[i].gameObject.SetActive(true);
                    slots[i].SetEmpty();
                }
                else 
                {
                    slots[i].gameObject.SetActive(true);
                    slots[i].SetUI(abilities.abilities[i]);
                }
                
            }


            
        }
    }

    public void SetEmpty()
    {
        foreach(MenuItem item in slots)
        {
            item.gameObject.SetActive(false);
        }
    }
}
