using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsInLevel : MonoBehaviour
{
    PlayerStats player_stats;
    public Slider hp_slider, mp_slider;
    public Text hp_text, mp_text, current_XP;

    private void Start()
    {
        player_stats = FindObjectOfType<PlayerStats>();
        UpdateUI();
    }

    public void UpdateUI()
    {
        hp_text.text = player_stats.hp.ToString() + "/" + player_stats.hp_max.ToString()+" HP";
        mp_text.text = player_stats.mp.ToString() + "/" + player_stats.mp_max.ToString() + " MP";
        //if(player_stats.levelUP_startCost + (player_stats.levelUP_levelAdditionCost * player_stats.level)<= player_stats.souls)
        //{
        //    current_XP.text = player_stats.souls.ToString() + " XP +";
        //}
        current_XP.text = player_stats.souls.ToString() + "/"+ (player_stats.levelUP_startCost + (player_stats.levelUP_levelAdditionCost * player_stats.level))+" XP";

        hp_slider.value = (float)player_stats.hp / (float)player_stats.hp_max;
        mp_slider.value = (float)player_stats.mp / (float)player_stats.mp_max;
    }
}
