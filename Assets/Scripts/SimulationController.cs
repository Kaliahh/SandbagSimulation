using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public int NumberOfDrones;
        // TODO: Tilføj DroneViewDistance og en method der kan sætte den hos dronerne

        // Genereringpunkter for droner of sandsække
        public Vector3 SandbagSpawnPoint;
        public Vector3 DroneSpawnPoint;

        // Variable for diget (Lige nu kun 2 nodes)
        public Vector3 Node2; // Enden af diget
        public Vector3 Node1; // Starten af diget
        public int Height;

        void Start()
        {
            SetupSimulation();
        }

        // Sætter simulationen op
        public void SetupSimulation()
        {
            // Genererer objekter
            InitializeDrones();
            SetDroneSpeed();
            InitializeBlueprints();

            this.GetComponent<SandbagSpawner>().SpawnPoint = SandbagSpawnPoint;
            SetDroneSandbagPickUpLocation();
        }

        // Definerer punktet for alle droner hvor de kan samle sandsække op
        private void SetDroneSandbagPickUpLocation()
        {
            Vector3 location = new Vector3(SandbagSpawnPoint.x, SandbagSpawnPoint.y + 5, SandbagSpawnPoint.z);
            foreach (GameObject drone in Drones)
            {
                drone.GetComponent<DroneController>().SetSandbagPickUpLocation(location);
            }
        }

        // Instansierer droner i et gitter 
        private void InitializeDrones() 
        {
            float j = 0;
            Drones = new List<GameObject>();

            for (int i = 0; i < NumberOfDrones; i++)
            {
                // TODO: lav gitteret i varierende størrelse
                if (i % 3 == 0)
                    j++;

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
                drone.GetComponent<DroneController>().SetSpeed(DroneSpeed);
            }
        }
    }
}



