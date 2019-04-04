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

    public void FlyTo(Vector3 Target)
    {
        transform.position = Vector3.MoveTowards(transform.position, Target, Step);
    }
}
