using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBaseHandle : MonoBehaviour
{
    AIPath                      aiPath;
    SpriteRenderer              spriteRenderer;
    Rigidbody2D                 rigidbody2D;

    public Transform            target;
    public float                speed = 3f;
    public float                nextWaypointDistance = 3f;
    Path                        path;
    int                         currentWaypoint = 0;
    bool                        reachedEndOfPath = false;
    Seeker                      seeker;

    private void Start ()
    {
        aiPath = GetComponent<AIPath>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        seeker = GetComponent<Seeker>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        InvokeRepeating ("UpdatePath", 0f, .5f);  
    }

    void UpdatePath ()
    {
        if (seeker.IsDone())
            seeker.StartPath(rigidbody2D.position, target.position, OnPathComplete);
    }

    void OnPathComplete (Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rigidbody2D.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rigidbody2D.AddForce(force);

        float distance = Vector2.Distance (rigidbody2D.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
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
}
