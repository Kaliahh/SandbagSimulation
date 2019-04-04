using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    // GameObject referencer
    public GameObject Drone;
    private List<GameObject> Drones;

    // Variable brugeren kan ændre på
    public int DroneSpeed;
    public int NumberOfDrones;

    void Start () 
    {
        InitializeDrones();
        SetDroneSpeed();

        // Blueprint eksempel
        List<Vector3> nodes = new List<Vector3> { new Vector3(10f, 0f, 0f), new Vector3(-10f, 0f, 0f)};
        Blueprint blueprint = new Blueprint(nodes, 10);
	}
    private void InitializeDrones()
    {
        Drones = new List<GameObject>();
        for (int i = 0; i < NumberOfDrones; i++)
        {
            Drones.Add(Instantiate(Drone));
        }
    }
    private void InitializeBlueprints()
    {
        foreach (GameObject drone in Drones)
        {
            //NEED TO IMPLEMENT BLUEPRINT OF DRONECONTROLLER
        }
    }
    private void SetDroneSpeed()
    {
        foreach (GameObject drone in Drones)
        {
            //NEED SET THE SPEED OF DRONECONTROLLER 
        }
    }
}

