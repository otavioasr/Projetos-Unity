using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [SerializeField]
    private Vector3                 PlayerHandsPosition;
    private new Rigidbody2D         rigidbody2D;
    Transform                       parent;

    private void Awake ()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start ()
    {
        PlayerHandsPosition =  new Vector3 (-0.007f, -0.052f, -1f);
    }

    public void PickUp (Transform parent)
    {
        if (transform.parent != null)
        {
            transform.SetParent (null);
            parent.GetComponent<PlayerMovementHandler>().ResetSpeed();
        }
        else
        {
            this.parent = parent;
            StartCoroutine(FollowParent());
        }
    }

    IEnumerator FollowParent ()
    {
        yield return new WaitForSeconds(.4f);
        transform.SetParent (parent);
        parent.GetComponent<PlayerMovementHandler>().speed = .4f;
        transform.localPosition = PlayerHandsPosition;
    }

}
