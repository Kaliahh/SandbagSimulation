using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    public float Speed = 30.0f;
    public float Step;

    private void Awake()
    {
        Step = Speed * Time.deltaTime;
    }

    // Flytter GameObject et Step mod Vector3 target i løbet af en enkelt frame.
    public void FlyTo(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Step);
    }
}
