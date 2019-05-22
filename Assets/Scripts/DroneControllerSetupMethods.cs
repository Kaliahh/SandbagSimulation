using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SandbagSimulation
{
    public static class SetupMethods 
    {
        public static GameObject CreateDrone(Vector3 position, Vector3 node1, Vector3 node2, int dikeHeight)
        {
            GameObject drone = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            drone.AddComponent<DroneController>();

            drone.AddComponent<DroneMover>();
            drone.AddComponent<BoxCollider>();

            CapsuleCollider collider = drone.GetComponent<CapsuleCollider>();
            GameObject.Destroy(collider);

            drone.GetComponent<DroneController>().SetBlueprint(CreateBlueprint(node1, node2, dikeHeight));
            drone.GetComponent<DroneController>().SetSpeed(8);
            drone.GetComponent<DroneController>().SetViewDistance(4);
            drone.GetComponent<DroneController>().SetSandbagPickUpLocation(new Vector3(1, 0, 0));

            //drone.AddComponent<Scaler>();
            //drone.AddComponent<Rotator>();

            drone.tag = "Drone";
            drone.transform.position = position;

            return drone;
        }

        public static GameObject CreateDrone()
        {
            Vector3 position = new Vector3(0, 0, 0);
            Vector3 node1 = new Vector3(10, 0, 0);
            Vector3 node2 = new Vector3(-10, 0, 0);
            int dikeHeight = 4;

            GameObject drone = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            drone.AddComponent<DroneController>();

            drone.AddComponent<DroneMover>();
            drone.AddComponent<BoxCollider>();

            CapsuleCollider collider = drone.GetComponent<CapsuleCollider>();
            GameObject.Destroy(collider);

            drone.GetComponent<DroneController>().SetBlueprint(CreateBlueprint(node1, node2, dikeHeight));
            drone.GetComponent<DroneController>().SetSpeed(8);
            drone.GetComponent<DroneController>().SetViewDistance(4);
            drone.GetComponent<DroneController>().SetSandbagPickUpLocation(new Vector3(-1, 0, 0));

            //drone.AddComponent<Scaler>();
            //drone.AddComponent<Rotator>();

            drone.tag = "Drone";
            drone.transform.position = position;

            return drone;
        }

        public static GameObject CreateSandbag()
        {
            GameObject sandbag = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sandbag.AddComponent<Rigidbody>();
            sandbag.GetComponent<Rigidbody>().isKinematic = true;
            sandbag.AddComponent<SandbagController>();
            sandbag.AddComponent<BoxCollider>();

            sandbag.tag = "Sandbag";

            return sandbag;
        }

        public static Blueprint CreateBlueprint(Vector3 node1, Vector3 node2, int height)
        {
            List<Vector3> ConstructionNodes = new List<Vector3> { node1, node2 };
            return new Blueprint(ConstructionNodes, height);
        }
    }
}


