using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public GameObject textPopUP;
    bool onCheckpoint;

    private void Start()
    {
        textPopUP.SetActive(false);
    }

    private void Update()
    {
        if(onCheckpoint && Input.GetKeyDown("space"))
        {
            Debug.Log("whaat");
            FindObjectOfType<PlayerStats>().RefillStats();
            var ST = FindObjectOfType<SceneTransitioner>();
            //ST.onCheckPoint = true;
            ST.Save_Game(0);
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
