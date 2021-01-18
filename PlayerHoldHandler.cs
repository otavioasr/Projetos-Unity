using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoldHandler : MonoBehaviour
{
    private  Animator           animator;
    private bool                isHolding;
    [SerializeField]
    private float               range;
    public UI_Controller        uI_Controller;

    private void Awake ()
    {
        animator = GetComponent<Animator>();
    }

    private void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleHold ();
        }    
    }

    public void HandleHold ()
    {
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D collider2D in colliderArray)
        {
            if (collider2D.TryGetComponent<PickableItem>(out PickableItem pickableItem))
            {
                isHolding = !isHolding;
                animator.SetTrigger("grab");
                animator.SetBool("holding", isHolding);
                pickableItem.PickUp (transform);

                uI_Controller.LockController(.5f);
                return;
            }
        }
    }
}
