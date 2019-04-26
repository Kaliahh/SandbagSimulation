using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SandbagSimulation
{
    // Styrer simulatinoen
    public class SimulationController : MonoBehaviour
    {
        // GameObject referencer
        public GameObject Drone;
        public GameObject SandBag;

        private List<GameObject> Drones;

        // Variable brugeren kan ændre på
        public float DroneSpeed;
        public float DroneViewDistance; 
        public int NumberOfDrones;

        private int NumOfFinishedDrones;

        // Genereringpunkter for droner of sandsække
        public Vector3 SandbagSpawnPoint;
        public Vector3 DroneSpawnPoint;

        // Variable for diget (Lige nu kun 2 nodes)
        public Vector3 Node2; // Enden af diget
        public Vector3 Node1; // Starten af diget
        public int DikeHeight;
        public Blueprint Blueprint;
        void Start()
        {
            NumOfFinishedDrones = 0;
            SetupSimulation();
        }

        void Update()
        {
            if (NumOfFinishedDrones == NumberOfDrones)
            {
                foreach (GameObject drone in Drones)
                {
                    if (drone.GetComponent<DroneController>().MySandbag != null)
                    {
                        //drone.GetComponent<DroneController>().MySandbag.SetActive(false);

                        Destroy(drone.GetComponent<DroneController>().MySandbag);
                    }

                    // drone.SetActive(false);
                    Destroy(drone);
                }

                NumOfFinishedDrones = 0;
            }
        }

        // Sætter simulationen op
        public void SetupSimulation()
        {
            // Genererer objekter
            InitializeDrones();
            SetDroneSpeed();
            SetDroneViewDistance();
            InitializeBlueprints();
            GiveDroneList();

            this.GetComponent<SandbagSpawner>().SpawnPoint = SandbagSpawnPoint;
            SetDroneSandbagPickUpLocation();
        }

        public void GiveDroneList()
        {
            foreach (GameObject drone in Drones)
            {
                List<GameObject> list = Drones
                    .Where(p => p != drone)
                    .ToList();

                drone.GetComponent<DroneController>().SetOtherDrones(list);
            }
        }

        // Instansierer droner i et gitter 
        public void InitializeDrones() 
        {
            // float j = 0;
            Drones = new List<GameObject>();

            bool isRightDrone = true;

            for (int i = 0; i < NumberOfDrones; i++)
            {
                // TODO: lav gitteret i varierende størrelse
                //if (i % 3 == 0)
                //    j++;

                Vector3 NewDroneSpawnPoint = new Vector3(DroneSpawnPoint.x/* + j * 3*/, DroneSpawnPoint.y, DroneSpawnPoint.z + /* (i % 3) */ i * 5);

                Drones.Add(Instantiate(Drone, NewDroneSpawnPoint, Quaternion.identity));
                Drones[i].GetComponent<DroneController>().IsRightDrone = isRightDrone;
                Drones[i].GetComponent<DroneController>().FinishedBuilding += FinishedDronesCounter;

                isRightDrone = !isRightDrone;

            }
        }

        private void FinishedDronesCounter(object sender, EventArgs e)
        {
            NumOfFinishedDrones++;
            Debug.Log(NumOfFinishedDrones);
        }

        // Genererer blueprints og giver dem til dronerne
        public void InitializeBlueprints() 
        {
            // Blueprint generering
            List<Vector3> ConstructionNodes = new List<Vector3> { Node1, Node2 };
            Blueprint = new Blueprint(ConstructionNodes, DikeHeight);

            // Giver en kopi af blueprintet til hver drone
            foreach (GameObject drone in Drones) 
            {
                drone.GetComponent<DroneController>().SetBlueprint(Blueprint);
            }
        }

        // Definerer punktet for alle droner hvor de kan samle sandsække op
        public void SetDroneSandbagPickUpLocation()
        {
            Vector3 location = new Vector3(SandbagSpawnPoint.x, SandbagSpawnPoint.y + 5, SandbagSpawnPoint.z);
            foreach (GameObject drone in Drones)
            {
                drone.GetComponent<DroneController>().SetSandbagPickUpLocation(location);
            }
        }

        // Sætter dronernes fart
        public void SetDroneSpeed() 
        {
            foreach (GameObject drone in Drones)
            {
                drone.GetComponent<DroneController>().SetSpeed(DroneSpeed);
            }
        }

        public void SetDroneViewDistance()
        {
            foreach (GameObject drone in Drones)
            {
                drone.GetComponent<DroneController>().SetViewDistance(DroneViewDistance);
            }
        }
    }
}



