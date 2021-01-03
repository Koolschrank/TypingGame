using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPickUp : MonoBehaviour
{
    public PlayerBody body;
    bool colected;
    // Start is called before the first frame update
    void Start()
    {
        if(!GetComponent<SaveableObject>().inGame)
        {
            Colected();
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!colected && collision.GetComponent<PlayerStats>())
        {
            
            Colected();
        }
    }

    void Colected()
    {
        GetComponent<Collider2D>().enabled = false;
        colected = true;
        FindObjectOfType<Backpack>().playerBodies.Add(body);
        GetComponent<SaveableObject>().inGame = false;
        transform.localScale = new Vector3(0, 0, 0);
        var children = GetComponentInChildren<Transform>();
        if (children == null) return;
        foreach(Transform child in children)
        {
            child.transform.localScale = new Vector3(0, 0, 0);
        }
    }
}
