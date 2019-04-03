using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    //GameObject References
    public GameObject drone; //Reference to drone object

    private List<GameObject> drones; //List of all instantiated drones

    //Variables user can edit
    public int speed; //Speed of the drones
    public int numberOfDrones; //Amount of drones

    void Start () //initialization of simulation
    {
        InitializeDrones(); //Spawn the drones
        SetSpeed(); //Set speed of the drones equal speed variable

        //Blueprint example
        List<Vector3> nodes = new List<Vector3> { new Vector3(10f, 0f, 0f), new Vector3(-10f, 0f, 0f)};
        Blueprint blueprint = new Blueprint(nodes, 10);
	}
    private void InitializeDrones() //Droner spawner
    {
        drones = new List<GameObject>();
        for (int i = 0; i < numberOfDrones; i++)
        {
            drones.Add(Instantiate(drone));
        }
    }
    private void InitializeBlueprints() //Initiale Blueprint
    {
        foreach (GameObject drone in drones)
        {
            //NEED TO IMPLEMENT BLUEPRINT OF DRONECONTROLLER
        }
    }
    private void SetSpeed()
    {
        foreach (GameObject drone in drones)
        {
            //NEED SET THE SPEED OF DRONECONTROLLER 
        }
    }
}

