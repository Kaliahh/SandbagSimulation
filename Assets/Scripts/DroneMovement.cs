using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    public float Speed;
    public float Step { get; private set; }

    private void Awake()
    {
        Step = Speed * Time.deltaTime;
    }

    // Flytter GameObject et Step mod Vector3 target i løbet af en enkelt frame.
    public void FlyTo(Vector3 target)
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, Step);
    }
}
