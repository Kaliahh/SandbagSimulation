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

        public int NumOfFinishedDrones;

        // Genereringpunkter for droner of sandsække
        public Vector3 SandbagSpawnPoint;
        public Vector3 DroneSpawnPoint;

        // Variable for diget (Lige nu kun 2 nodes)
        public Vector3 Node2; // Enden af diget
        public Vector3 Node1; // Starten af diget
        public int DikeHeight;
        public Blueprint Blueprint;

        //Tilstand
        public bool IsSimulationRunning;

        //Tid
        public float TotalTime;

        void Start()
        {
            NumOfFinishedDrones = 0;
            NumberOfDrones = ((int)Vector3.Distance(Node1, Node2) + 8) / 4;
        }

        void Update()
        {
            if (IsSimulationRunning) 
            {
                // Tegner en rød streg hvor diget skal ligge, kun i scene view
                Debug.DrawLine(Blueprint.ConstructionNodes.First(), Blueprint.ConstructionNodes.Last(), Color.red);
                Debug.DrawLine(Blueprint.ConstructionNodes.First(), Blueprint.ConstructionNodes.First() + new Vector3(0, 10, 0));
                Debug.DrawLine(Blueprint.ConstructionNodes.Last(), Blueprint.ConstructionNodes.Last() + new Vector3(0, 10, 0));

                if (NumOfFinishedDrones == NumberOfDrones) {
                    foreach (GameObject drone in Drones) {
                        if (drone.GetComponent<DroneController>().MySandbag != null) {
                            Destroy(drone.GetComponent<DroneController>().MySandbag);
                        }

                        Destroy(drone);
                    }
                }
                TotalTime += Time.deltaTime;
            }
        }

        // Sætter simulationen op
        public void BeginSimulation()
        {
            // Genererer objekter
            InitializeDrones();
            SetDroneSpeed();
            SetDroneViewDistance();
            InitializeBlueprints();
            GiveDroneList();

            this.GetComponent<SandbagSpawner>().SpawnPoint = SandbagSpawnPoint;
            SetDroneSandbagPickUpLocation();
            IsSimulationRunning = true;
        }

        // Giver hver drone en liste der indeholder alle droner, undtaget den selv
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

        // Instansierer droner i en række 
        public void InitializeDrones() 
        {
            // float j = 0;
            Drones = new List<GameObject>();

            bool isRightDrone = true;

            for (int i = 0; i < NumberOfDrones; i++)
            {
                // TODO: Lav gitteret (i varierende størrelse)
                //if (i % 3 == 0)
                //    j++;

                Vector3 NewDroneSpawnPoint = new Vector3(DroneSpawnPoint.x/* + j * 3*/, DroneSpawnPoint.y, DroneSpawnPoint.z + /* (i % 3) */ i * 5);

                Drones.Add(Instantiate(Drone, NewDroneSpawnPoint, Quaternion.identity));
                Drones[i].GetComponent<DroneController>().IsRightDrone = isRightDrone;
                Drones[i].GetComponent<DroneController>().FinishedBuilding += FinishedDronesCounter;

                isRightDrone = !isRightDrone;

            }
        }

        // Tæller op hvor mange droner der er færdige med at bygge, ud fra dronens event, FinishedBuilding
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
            float metersAboveGround = 1.5f;
            Vector3 location = new Vector3(SandbagSpawnPoint.x, SandbagSpawnPoint.y + metersAboveGround, SandbagSpawnPoint.z);

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

        // Sætter hvor langt dronen kan se
        public void SetDroneViewDistance()
        {
            foreach (GameObject drone in Drones)
            {
                drone.GetComponent<DroneController>().SetViewDistance(DroneViewDistance);
            }
        }
    }
}



