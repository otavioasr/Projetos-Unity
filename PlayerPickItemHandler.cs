using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickItemHandler : MonoBehaviour
{
    private  Animator           animator;

    [SerializeField]
    private float               range;
    public UI_Controller        uI_Controller;

    private void Awake ()
    {
        animator = GetComponent<Animator>();
    }

    public void PickItem ()
    {
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D collider2D in colliderArray)
        {
            if (collider2D.TryGetComponent<Item>(out Item item))
            {
                animator.SetTrigger("grab");
                item.PickUp();

                if (item.GetType() == Item.ItemType.Heal)
                    GetComponent<Player>().Heal(4);

                uI_Controller.LockController(.5f);
                return;
            }
        }
    }
}
