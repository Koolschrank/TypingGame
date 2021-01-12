using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    Animator a;
    Rigidbody2D rb;
    public BoxCollider2D GroundCheck;
    public float speed, jumpSpeed, jumpHold, jumpResistance, walkResistance, jumpResistanceZero, trueHMove, moveCursor, moveCursorMax;
    float hMove, vMove, shiftValue = 0;
    public int lookDirection;
    bool lastOnGround, inJump;
    public bool onGround;





    void Start()
    {
        a = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        Move();

        onGround = GroundCheck.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (onGround && !lastOnGround)
        {
            lastOnGround = onGround;
            inJump = false;
        }
        if (!onGround)
        {
            lastOnGround = false;
        }



        if (Input.GetKeyDown("space") && onGround && !inJump)
        {
            Jump();
        }

    }

    private void FixedUpdate()
    {


        if (rb.velocity.x < jumpResistanceZero && rb.velocity.x > -jumpResistanceZero )
        {
            trueHMove = 0;
        }
        if (!inJump)
        {

            if (trueHMove < hMove)
                trueHMove += walkResistance;
            else if (trueHMove > hMove)
                trueHMove -= walkResistance;
            
            
            if (trueHMove < walkResistance && trueHMove > -walkResistance)
            {
                if (!a.GetCurrentAnimatorStateInfo(0).IsName("idle"))
                {
                    a.Play("idle");
                }
                
                trueHMove = 0;
            }
            else
            {
                if (!a.GetCurrentAnimatorStateInfo(0).IsName("run"))
                {
                    a.Play("run");
                }
            }
        }
        else
        {

            if (trueHMove < hMove && hMove == 0)
                trueHMove += jumpResistanceZero;
            else if (trueHMove > hMove && hMove == 0)
                trueHMove -= jumpResistanceZero;
            else if (trueHMove < hMove)
                trueHMove += jumpResistance;
            else if (trueHMove > hMove)
                trueHMove -= jumpResistance;


            if (trueHMove < jumpResistanceZero && trueHMove > -jumpResistanceZero)
            {
                trueHMove = 0;
            }

        }

        /*if (_input.Player.Jump.ReadValue<float>() >0 && rb.velocity.y > 0)
        {

            rb.velocity = new Vector3(trueHMove * speed * Time.deltaTime, rb.velocity.y + jumpHold * Time.deltaTime);
        }
        else
        {
            rb.velocity = new Vector3(trueHMove * speed * Time.deltaTime, rb.velocity.y);
        }*/

        if (Input.GetKeyDown("space") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(trueHMove * speed * Time.deltaTime, rb.velocity.y + jumpHold * Time.deltaTime);
            a.Play("jump");
        }
        else
        {
            rb.velocity = new Vector2(trueHMove * speed * Time.deltaTime, rb.velocity.y );
        }


        if(trueHMove<0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (trueHMove > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }


    }

    public void Jump()
    {
        inJump = true;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.velocity += new Vector2(0, jumpSpeed);
        a.Play("jump");
    }

    public void Move()
    {
        hMove =Input.GetAxis("Horizontal");
    }



}

