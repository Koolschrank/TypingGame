using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keys : MonoBehaviour
{
    public int key;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerStats>())
        {
            var player = collision.gameObject.GetComponent<PlayerStats>();
            player.keys[key] = true;
            Debug.Log("pick up");
        }
    }
}
