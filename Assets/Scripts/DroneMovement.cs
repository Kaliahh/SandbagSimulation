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

    // Moves the GameObject a Step towards a Vector3 target during a single frame.
    public void FlyTo(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Step);
    }
}
