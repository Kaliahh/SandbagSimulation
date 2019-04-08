using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SandbagSimulation
{
    public class SimulationController : MonoBehaviour
    {
        // GameObject referencer
        public GameObject Drone;
        public GameObject SandBag;
        private List<GameObject> Drones;

        // Variable brugeren kan ændre på
        public int DroneSpeed;
        public int NumberOfDrones;
        public int NumberOfSandbags;

        public Vector3 SandbagSpawnPoint; //Sandsæk genereringspunkt
        public Vector3 DroneSpawnPoint; //Drone genereringspunkt

        // Variable for diget (Lige nu kun 2 nodes)
        public Vector3 Node1; //Starten af diget
        public Vector3 Node2; //Enden af diget
        public int Height;

        void Start()
        {
            // Object Generering
            InitializeDrones();
            SetDroneSpeed();
            InitializeBlueprints();
            InitializeSandBags();
        }
        private void InitializeDrones() //Genererer droner
        {
            Drones = new List<GameObject>();
            for (int i = 0; i < NumberOfDrones; i++)
            {
                Drones.Add(Instantiate(Drone, DroneSpawnPoint, Quaternion.identity));
            }
        }
        private void InitializeBlueprints() //Giver dronerne blueprintet
        {
            // Blueprint generering
            List<Vector3> nodes = new List<Vector3> { Node1, Node2 };
            Blueprint blueprint = new Blueprint(nodes, 10);

            foreach (GameObject drone in Drones) //Giver en kopi til hver drone
            {
                //Kan ikke få adgang til felterne for objekter


                //drone.GetComponent<DroneController>().MyBlueprint = blueprint;
            }
        }
        private void SetDroneSpeed() //Sætter dronernes fart
        {
            foreach (GameObject drone in Drones)
            {
                //Kan ikke få adgang til felterne for objekter

                //DroneMovement droneMovement = new DroneMovement();
                //drone.AddComponent(DroneMovement(typeof(script)));
                //drone.GetComponent<DroneController>().ViewDistance = 15f;
                //drone.GetComponent<DroneController>().MyMovement = droneMovement;
                //
                //DroneController.MyMovement.Speed = DroneSpeed;
            }
        }
        private void InitializeSandBags() //Generer sandsække
        {
            Vector3 NewSandbagSpawnPoint; //Varible for a new spawnposition
            for (int i = 0; i < NumberOfSandbags; i++)
            {
                Vector3 NewPos = SandbagSpawnPoint + new Vector3(2f * i, 0f, 0f); //Logik for genereringspunkt for enkelte sandsække
                Instantiate(SandBag, NewPos, Quaternion.identity);
            }
        }
    }
}



