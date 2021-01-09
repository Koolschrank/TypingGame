using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class statsPage : MonoBehaviour
{
    public Text hp, mp, weaponSlots, magicSlots, hp_cost;
    Vector3 normalScale;

    private void Start()
    {
        normalScale = transform.localScale;
    }

    public void SetVisible(bool _bool)
    {
        if (_bool)
        {
            transform.localScale = normalScale;
        }
        else
        {
            transform.localScale = new Vector2(0, 0);
        }
    }

    public void SetStats(PlayerStats player)
    {
        hp.text = player.hp_max.ToString();
        mp.text = player.mp_max.ToString();
        weaponSlots.text = player.Ability_typs[0].slots.ToString();
        magicSlots.text = player.Ability_typs[1].slots.ToString();
        float _hp_cost = (float)((int)(player.hp_cost_percent * 100))/100;
        hp_cost.text = _hp_cost.ToString();
    }
}
