using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillFountain : MonoBehaviour
{
    public GameObject textPopUP;
    bool onFountain, checked_bool;
    public Sprite full, empty;
    public bool filled= true;
    SaveableObject inGameChecker;

    private void Start()
    {
        textPopUP.SetActive(false);
         inGameChecker = GetComponent<SaveableObject>();
        if(inGameChecker)
        {
            if(!inGameChecker.inGame)
            {
                GetComponent<SpriteRenderer>().sprite = empty;
                filled = false;
            }
        }
    }

    private void Update()
    {
        if (onFountain && Input.GetKeyDown("space") && !checked_bool&& filled)
        {
            checked_bool = true;
            Debug.Log("whaat");
            FindObjectOfType<PlayerStats>().RefillHalfStats();
            GetComponent<SpriteRenderer>().sprite = empty;
            filled = false;
            inGameChecker.inGame = false;
            FindObjectOfType<StatsInLevel>().UpdateUI();
            textPopUP.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerStats>()&&filled)
        {
            textPopUP.SetActive(true);
            onFountain = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerStats>())
        {
            textPopUP.SetActive(false);
            onFountain = false;
        }
    }
}
