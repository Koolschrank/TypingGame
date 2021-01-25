using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    public Transform playerSpwan;
    public int sceneInt;
    public float timeToTrigger = 1.5f;
    public bool playerPlaced;

    private void Awake()
    {
        GetComponent<Collider2D>().enabled = (false);
        StartCoroutine(EnableHitbox());
    }

    IEnumerator EnableHitbox()
    {
        yield return new WaitForSeconds(timeToTrigger);
        GetComponent<Collider2D>().enabled = (true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerStats>())
        {
            FindObjectOfType<SceneTransitioner>().TransitionToLevel(sceneInt);
        }
    }

    public void placePlayer()
    {
        FindObjectOfType<PlayerMovement>().gameObject.transform.position = playerSpwan.position;
        playerPlaced = true;
    }
}
