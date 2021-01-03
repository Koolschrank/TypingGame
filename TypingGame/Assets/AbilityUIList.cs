using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUIList : MonoBehaviour
{
    public AbilityDisplay[] displays;
    public List<Ability> abilities = new List<Ability>();
    int currentSlots;
    Vector3 lScale;
    
    void Start()
    {
        lScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVisible(bool _bool)
    {
        if(_bool)
        {
            transform.localScale = lScale;
        }
        else
        {
            transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public void SetAbilities(List<Ability> _abilities, int slots)
    {
        if(slots != -1)
        {
            currentSlots = slots;
            abilities.Clear();
            if (_abilities != null)
            {
                foreach (Ability ability in _abilities)
                {
                    abilities.Add(ability);
                }
            }
        }
        
        
        

        for (int i = 0; i <8; i++)
        {
            if((abilities.Count <=i || abilities[i] == null)&& currentSlots > i)
            {
                displays[i].SetVisible(true);
                displays[i].ShowEmpty();
                displays[i].SetNumber(i + 1);
            }
            else if(abilities.Count <= i || abilities[i] == null)
            {
                displays[i].SetVisible(false);
                displays[i].ShowEmpty(); // change later to not showing at all
            }
            else
            {
                displays[i].SetVisible(true);
                displays[i].UpdateDisplay(abilities[i]);
                displays[i].SetNumber(i + 1);
            }

            
            
        }
    }
}
