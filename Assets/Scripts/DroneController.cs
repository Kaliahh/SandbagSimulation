using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public DroneMovement gtfo;

    // Start is called before the first frame update
    void Start()
    {
        gtfo = gameObject.AddComponent<DroneMovement>() as DroneMovement;
    }

    // Update is called once per frame
    void Update()
    {

        gtfo.FlyTo(new Vector3(0.0f, 0.0f, 0.0f));
    }
}
