using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBase : MonoBehaviour
{

    public enum State
    {
        Idle, 
        Walking,
        Atack, 
        Hurt,
    }

    float                       range = .1f;
    AIPath                      aiPath;
    SpriteRenderer              spriteRenderer;
    State                       state;
    Animator                    animator;

    public int                  health = 5;
    bool                        ishurting;

    private void Start ()
    {
        aiPath = GetComponent<AIPath>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        state = State.Idle;
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, aiPath.destination) < 1 && !ishurting)
            state = State.Walking;
        else if (!ishurting)
            state = State.Idle;

        //search for closest player 
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D collider2D in colliderArray)
        {
            if (collider2D.TryGetComponent<Player>(out Player player) && !ishurting)
            {
                state = State.Atack;
                player.Damage(1);
            }
        }

        switch (state)
        {
            default:
            case State.Idle:
                aiPath.isStopped = true;
                animator.SetBool("walking", false);
                break;

            case State.Walking:
                aiPath.isStopped = false;
                animator.SetBool("walking", true);
                break;

            case State.Atack:
                aiPath.isStopped = true;
                animator.SetTrigger("atack");
                state = State.Idle;
                break;

            case State.Hurt:
                aiPath.isStopped = true;
                animator.SetTrigger("hurt");
                state = State.Idle;
                break;
        }

        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            spriteRenderer.flipX = true;
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            spriteRenderer.flipX = false;        
        }
    }

    public void Damage (int damageAmount)
    {
        if (health - damageAmount > 0)
            health -= damageAmount;
        else
            health = 0;

        state = State.Hurt;
        Popup.Create(new Vector3(transform.position.x, transform.position.y+.12f), "-" + damageAmount.ToString(), false, Color.red);
    
        if (!ishurting)
        {
            ishurting = true;
            StartCoroutine (SetIsHurting (false, 3f));
        }
    }

    IEnumerator SetIsHurting (bool value, float time)
    {
        yield return new WaitForSeconds(time);
        ishurting = value;
    }
}
