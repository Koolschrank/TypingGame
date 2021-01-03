using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PlayerMovement : MonoBehaviour
{
    
    public Transform target;
    public float speed = 200f;
    public float pathSpeed = 200f;
    public float nextWaypointDistance = 3f;
    public bool noMovement;
    //public bool SaveTesting;

    Path path;
    int currentWaypoint;
    bool reachedEndPfPath;

    Seeker seeker;
    Rigidbody2D rb;
    Vector2 currentForce;
    PlayerStats playerStats;

    private void Awake()
    {
        
    }

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        //if (SaveTesting) LoadPlayerSave();
        if (!GetComponent<SaveableObject>().inGame)
        {
            Remove_from_current_scene();
        }
    }

    public void Remove_from_current_scene()
    {
        GetComponent<Collider2D>().enabled = false;
        var children = GetComponentsInChildren<Transform>();
        foreach(Transform child in children)
        {
            child.gameObject.active = false;
        }
        this.enabled = false;
    }

    void Update()
    {
        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("Vertical");
        if(!noMovement)
        Move_Manual(moveX, moveY);

        if (path != null) WalkPath();
    }

    public void WalkPath()
    {
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndPfPath = true;
            path = null;
            return;
        }
        else
        {
            reachedEndPfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * pathSpeed * Time.deltaTime;
        rb.AddForce(force);

        float distace = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distace < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    public void UpdatePath(Transform _target)
    {
        target = _target;
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public void Move_Manual(float x, float y)
    {
        SwapBody();
        if (x!=0 || y!= 0)
        {
            reachedEndPfPath = true;
            path = null;
        }
        var force = new Vector2(x, y);
        transform.position = new Vector2(transform.position.x + force.x * speed * Time.deltaTime, transform.position.y+ force.y * speed * Time.deltaTime) ;
        //rb.velocity = new Vector2( force.x * speed * Time.deltaTime,  force.y * speed * Time.deltaTime);
    }

    public void SwapBody()
    {
        if (Input.GetKeyDown("1") && playerStats.playerBodies.Count >=1)
        {
            playerStats.currentBody = 0;
            playerStats.SetBody(playerStats.playerBodies[0]);
        }
        else if (Input.GetKeyDown("2") && playerStats.playerBodies.Count >= 2)
        {
            playerStats.currentBody = 1;
            playerStats.SetBody(playerStats.playerBodies[1]);
        }
        else if (Input.GetKeyDown("3") && playerStats.playerBodies.Count >= 3)
        {
            playerStats.currentBody = 2;
            playerStats.SetBody(playerStats.playerBodies[2]);
        }
        else if (Input.GetKeyDown("4") && playerStats.playerBodies.Count >= 4)
        {
            playerStats.currentBody = 3;
            playerStats.SetBody(playerStats.playerBodies[3]);
        }
        else if (Input.GetKeyDown("5") && playerStats.playerBodies.Count >= 5)
        {
            playerStats.currentBody = 4;
            playerStats.SetBody(playerStats.playerBodies[4]);
        }
        else if (Input.GetKeyDown("6") && playerStats.playerBodies.Count >= 6)
        {
            playerStats.currentBody = 5;
            playerStats.SetBody(playerStats.playerBodies[5]);
        }
    }

}
