using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Func<Vector3> getCameraFollowPosition;

    public void Setup (Func<Vector3> getCameraFollowPosition)
    {
        this.getCameraFollowPosition = getCameraFollowPosition; 
    }
    void Update()
    { 
        Vector3 cameraFollowPosition = getCameraFollowPosition();
        cameraFollowPosition.z = transform.position.z;

        Vector3 cameraMoveDirection = (cameraFollowPosition - transform.position).normalized;
        float distance = Vector3.Distance(cameraFollowPosition, transform.position);
        float cameraMoveSpeed = 2f;

        if (distance > 0)
        {
            Vector3 newCameraPosition = transform.position + cameraMoveDirection * distance * cameraMoveSpeed * Time.deltaTime;
            float distanceAfterMoving = Vector3.Distance(newCameraPosition, cameraFollowPosition);

            if (distanceAfterMoving > distance)
            {
                newCameraPosition = cameraFollowPosition;
            }
            
            transform.position = newCameraPosition;
        }
    
    }
}
