using System;
using UnityEngine;

public class MoveableSmoothDamp : MoveableBase
{
    Vector3 velocity; 
    public float smoothTime = 0.3F; 
    public float maxVelocity = 10f;
    Vector3 currentVelocity;

    void Start()
    {
        targetPosition = transform.position; 
    }

    protected override void PausableUpdate()
    {
        MoveXY();
    }

    void MoveXY()
    {
        if (IsCloseEnough() && !IsMoving()) return;
        
        var newPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime, maxVelocity, Time.deltaTime);
        velocity = (newPosition - transform.position) / Time.deltaTime;
        if (velocity.sqrMagnitude > maxVelocity * maxVelocity)
            velocity = velocity.normalized * maxVelocity;
        transform.position = newPosition + velocity * Time.deltaTime;

        if (IsCloseEnough() && !IsMoving())
        {
            transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
            velocity = Vector3.zero;    
        }
    }

    bool IsCloseEnough()
    {
        return Vector3.Distance(transform.position, targetPosition) < 0.01f;
    }

    bool IsMoving()
    {
        return velocity.magnitude > 0.01f;
    }
    
}