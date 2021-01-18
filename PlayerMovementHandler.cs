using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    public float                speed = .7f, initialSpeed = .7f;
    private new Rigidbody2D     rigidbody2D;
    private  Animator           animator;
    private SpriteRenderer      spriteRenderer;
    private Vector2             movement;
    public UI_Controller uI_Controller;

    private void Awake ()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate ()
    {
        HandleMovement();
    }

    private void HandleMovement ()
    {
        movement.x = uI_Controller.GetHorizontal();
        movement.y = uI_Controller.GetVertical();

        if (movement.x != 0 && movement.y != 0)
            movement *= 0.5f;

        rigidbody2D.MovePosition (rigidbody2D.position + movement.normalized * speed * Time.fixedDeltaTime);

        if (movement.x == 0 && movement.y == 0)
        {
            animator.SetBool("walking", false);
        }
        else
        {
            animator.SetBool("walking", true);
        }

        if (movement.x < 0)
            spriteRenderer.flipX = true;    
        else if (movement.x > 0)
            spriteRenderer.flipX = false;
    }

    public void ResetSpeed ()
    {
        speed = initialSpeed;
    }
}
