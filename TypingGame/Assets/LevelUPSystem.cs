using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUPSystem : MonoBehaviour
{
    public int nextLevelUpCost;
    public Level_up_system LUS;
    public Text currentLevel, currentSouls, nextLevelCost;
    public stateLevelUI[] stats;
    int selectedStat, startingLevel_h, startingLevel_m, startingLevel_c;
    PlayerStats player;
    Vector3 lScale;
    bool active;

    private void Start()
    {
        lScale = transform.localScale;
        SetVisible(false);
        player = FindObjectOfType<PlayerStats>();
    }

    public void SetActive(bool _bool)
    {
        player = FindObjectOfType<PlayerStats>();
        active = _bool;
        GetNextLevelUP();
        SetSouls();
        if(active)
        {
            SetStatLevelUI();
            SetStartingLevel();
        }
    }

    public void SetStartingLevel()
    {
        startingLevel_h = player.level_health;
        startingLevel_m = player.level_magic;
        startingLevel_c = player.level_capability;
    }

    public void SetStatLevelUI()
    {
        stats[0].SetLevel(player.level_health);
        stats[1].SetLevel(player.level_magic);
        stats[2].SetLevel(player.level_capability);
    }

    public void MenuActive()
    {
        if (!active) return;

        SelectStat();
        ChangeStat();
    }

    public void SelectStat()
    {
        if (Input.GetKeyDown("1"))
        {
            selectedStat = 0;
        }
        if (Input.GetKeyDown("2"))
        {
            selectedStat = 1;
        }
        if (Input.GetKeyDown("3"))
        {
            selectedStat = 2;
        }
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }

    public void ChangeStat()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            switch (selectedStat)
            {
                case 0:
                    if(startingLevel_h< player.level_health)
                        LowerLevel(level_stats.health);
                    break;
                case 1:
                    if (startingLevel_m < player.level_magic)
                        LowerLevel(level_stats.magic);
                    break;
                case 2:
                    if (startingLevel_c < player.level_capability)
                        LowerLevel(level_stats.capability);
                    break;
            }
        }
        else if (nextLevelUpCost <= player.souls&& Input.GetKeyDown(KeyCode.RightArrow))
        {
            switch(selectedStat)
            {
                case 0:
                    RaiseLevel(level_stats.health);
                    break;
                case 1:
                    RaiseLevel(level_stats.magic);
                    break;
                case 2:
                    RaiseLevel(level_stats.capability);
                    break;
            }
        }
    }

    public void GetNextLevelUP()
    {
        nextLevelUpCost = player.levelUP_startCost + (player.levelUP_levelAdditionCost * player.level);
        nextLevelCost.text = nextLevelUpCost.ToString(); 
    }

    public void SetSouls()
    {
        Debug.Log(player.souls);
        currentLevel.text = player.level.ToString();
        currentSouls.text = player.souls.ToString();
    }

    public void SetVisible(bool _bool)
    {
        if (_bool)
        {
            transform.localScale = lScale;
        }
        else
        {
            transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public void RaiseLevel(level_stats stat)
    {
        switch(stat)
        {
            case level_stats.health:
                player.LevelUp(LUS.stats[0]);
                break;
            case level_stats.magic:
                player.LevelUp(LUS.stats[1]);
                break;
            case level_stats.capability:
                player.LevelUp(LUS.stats[2]);
                break;
        }
        player.souls -= nextLevelUpCost;
        GetNextLevelUP();
        SetSouls();
        SetStatLevelUI();
    }

    public void LowerLevel(level_stats stat)
    {
        switch (stat)
        {
            case level_stats.health:
                player.LevelDown(LUS.stats[0]);
                break;
            case level_stats.magic:
                player.LevelDown(LUS.stats[1]);
                break;
            case level_stats.capability:
                player.LevelDown(LUS.stats[2]);
                break;
        }
        player.souls += player.levelUP_startCost + (player.levelUP_levelAdditionCost * player.level);
        GetNextLevelUP();
        SetSouls();
        SetStatLevelUI();
    }

    public void GoBack()
    {
        
        FindObjectOfType<BackPackControle>().BackToMainMenu(state.inLevelUp);
        SetActive(false);
    }

}
