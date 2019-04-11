using System;
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
        public float DroneSpeed;
        public int NumberOfDrones;
        // public int NumberOfSandbags;
        // TODO: Tilføj DroneViewDistance og en method der kan sætte den hos dronerne

        public Vector3 SandbagSpawnPoint; // Sandsæk genereringspunkt
        public Vector3 DroneSpawnPoint;   // Drone genereringspunkt

        // Variable for diget (Lige nu kun 2 nodes)
        public Vector3 Node2; // Enden af diget
        public Vector3 Node1; // Starten af diget
        public int Height;

        void Start()
        {
            SetupSimulation();
        }

        public void SetupSimulation()
        {
            // Object Generering
            InitializeDrones();
            SetDroneSpeed();
            InitializeBlueprints();

            this.GetComponent<SandbagSpawner>().SpawnPoint = SandbagSpawnPoint;
            SetDroneSandbagPickUpLocation();

            //InitializeSandBags();
        }

        private void SetDroneSandbagPickUpLocation()
        {
            Vector3 location = new Vector3(SandbagSpawnPoint.x, SandbagSpawnPoint.y + 5, SandbagSpawnPoint.z);
            foreach (GameObject drone in Drones)
            {
                drone.GetComponent<DroneController>().SetSandbagPickUpLocation(location);
            }
        }

        // Instansierer droner
        private void InitializeDrones() 
        {
            float j = 0;
            Drones = new List<GameObject>();

            for (int i = 0; i < NumberOfDrones; i++)
            {
                if (i % 3 == 0)
                {
                    j++;
                }
                Vector3 NewDroneSpawnPoint = new Vector3(DroneSpawnPoint.x + j * 3, DroneSpawnPoint.y, DroneSpawnPoint.z + (i % 3) * 3);

                Drones.Add(Instantiate(Drone, NewDroneSpawnPoint, Quaternion.identity));
            }
        }

        // Genererer blueprints og giver dem til dronerne
        private void InitializeBlueprints() 
        {
            // Blueprint generering
            List<Vector3> ConstructionNodes = new List<Vector3> { Node1, Node2 };
            Blueprint blueprint = new Blueprint(ConstructionNodes, 10);

            // Giver en kopi af blueprintet til hver drone
            foreach (GameObject drone in Drones) 
            {
                drone.GetComponent<DroneController>().SetBlueprint(blueprint);
            }
        }

        // Sætter dronernes fart
        private void SetDroneSpeed() 
        {
            foreach (GameObject drone in Drones)
            {
                // TODO: Virker ikke
                drone.GetComponent<DroneController>().SetSpeed(DroneSpeed);
            }
        }

        // Generer sandsække
        //private void InitializeSandBags() 
        //{
        //    for (int i = 0; i < NumberOfSandbags; i++)
        //    {
        //        Vector3 NewPos = SandbagSpawnPoint + new Vector3(2f * i, 0f, 0f); //Logik for genereringspunkt for enkelte sandsække
        //        Instantiate(SandBag, NewPos, Quaternion.identity);
        //    }
        //}
    }
}



