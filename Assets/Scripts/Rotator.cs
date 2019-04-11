using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    float Speed;

    private void Start()
    {
        Speed = Random.Range(-60f, 60f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, Speed) * Time.deltaTime);
    }
}
