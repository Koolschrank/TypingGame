using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public int keyNunber;
    public Sprite open, closed;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        sprite.sprite = closed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerStats>())
        {
            CheckDoor(collision.gameObject.GetComponent<PlayerStats>().keys);
        }
    }

    public void CheckDoor(bool[] playerKeys)
    {
        if(playerKeys[keyNunber] == true)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        sprite.sprite = open;
        Debug.Log("open");
        GetComponent<Collider2D>().enabled = false;
    }
}
