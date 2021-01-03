using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SpriteScript : MonoBehaviour
{
    public AIPath aiPath;
    
    public float run_freshhold =0.01f, turn_freshhold = 0.01f;
    public bool enemy;
    Rigidbody2D rb;
    Vector2 last_position;
    Animator a;
    SpriteRenderer sprite;


    public void Start()
    {
        a = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    private void Update()
    {
        if(rb.position.x- last_position.x > turn_freshhold)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (rb.position.x - last_position.x < -turn_freshhold)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }



        var move_range = Vector2.Distance(last_position, rb.position);
        last_position = rb.position;
        if(enemy)
        {
            if (GetComponentInParent<EnemyAI>().Walking())
            {
                a.Play("run");
            }
            else
            {
                a.Play("idle");
            }
        }
        else if (move_range > run_freshhold)
        {
            a.Play("run");
        }
        else
        {
            a.Play("idle");
        }
        /*
        if (aiPath.desiredVelocity.x>= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }*/
    }


}
