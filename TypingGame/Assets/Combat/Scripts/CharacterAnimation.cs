using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    Animator a;
    public Color damageColor;
    public float damageTime = 0.2f;
    public bool dead;
    SpriteRenderer sprite;
    void Start()
    {
        a = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Idle()
    {
        if (dead) return;
        a.Play("idle");
    }

    public void Move()
    {
        if (dead) return;
        a.Play("run");
    }

    public void Attack()
    {
        if (dead) return;
        a.Play("run");
    }

    public void TakeDamage()
    {
        if (dead) return;
        a.Play("run");
    }

    public void KO()
    {
        a.Play("destroy");
        dead = true;
    }

    public void Turn(bool left)
    {
        if (left)
        gameObject.transform.localScale= new Vector3(-1, 1, 1);
        else
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void Take_Damage()
    {
        StartCoroutine(ChangeSpriteColor(damageColor));
    }

    IEnumerator ChangeSpriteColor(Color newColor)
    {
        var old_color = sprite.color;
        sprite.color = newColor;
        yield return new WaitForSeconds(damageTime);
        sprite.color = old_color;
    }

    
}
