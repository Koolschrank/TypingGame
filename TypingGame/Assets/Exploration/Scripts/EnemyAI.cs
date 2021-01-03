using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    public GameObject BattleEnemy;
    public Transform target;
    public float speed = 200f, lookDistance=3f;
    public float nextWaypointDistance = 3f;
    public bool dead, noMovement;

    Path path;
    int currentWaypoint;
    bool reachedEndPfPath, foundPlayer;

    Seeker seeker;
    Rigidbody2D rb;

    void Start()
    {
        target = FindObjectOfType<PlayerMovement>().transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        
        
    }

    public void Remove_from_current_scene()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().gameObject.active = false;
        dead = true;
    }

    public bool Walking()
    {
        return !reachedEndPfPath;
    }

    void FixedUpdate()
    {
        if (noMovement)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (!dead && !GetComponent<SaveableObject>().inGame)
        {
            Remove_from_current_scene();
        }


        if (dead) return;
        CheckForPlayer();
        if (path == null || !foundPlayer)
        {
            rb.velocity = Vector2.zero;
            reachedEndPfPath = true;
            return;
        } 

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndPfPath = true;
            return;
        }
        else
        {
            reachedEndPfPath = false;
        }

        Vector2 direction =  ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        
        rb.AddForce(force);


        float distace = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distace < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }

    public void CheckForPlayer()
    {
        float distace = Vector2.Distance(rb.position, target.position);
        if (distace< lookDistance)
        {
            foreach(EnemyAI enemy in GetComponentInParent<EnemyGroup>().enemies)
            {
                enemy.PlayerSpoted();
            }
        }
    }

    public void PlayerSpoted()
    {
        if (!foundPlayer)
        {
            Debug.Log("player found");
            foundPlayer = true;
        }

    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint =0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            GetComponentInParent<EnemyGroup>().Transition_to_battle();
        }
    }

}
