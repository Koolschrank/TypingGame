using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public GameObject textPopUP;
    bool onCheckpoint, checked_bool;

    private void Start()
    {
        textPopUP.SetActive(false);
    }

    private void Update()
    {
        if(onCheckpoint && Input.GetKeyDown("space")&& !checked_bool)
        {
            checked_bool = true;
            Debug.Log("whaat");
            FindObjectOfType<PlayerStats>().RefillStats();
            var ST = FindObjectOfType<SceneTransitioner>();
            //ST.onCheckPoint = true;
            ST.Save_Game(FindObjectOfType<Settings>().currentSave);
            ST.LoadSaveFile(FindObjectOfType<Settings>().currentSave);
            ST.LoadGameFile();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerStats>())
        {
            textPopUP.SetActive(true);
            onCheckpoint = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerStats>())
        {
            textPopUP.SetActive(false);
            onCheckpoint = false;
        }
    }
}
