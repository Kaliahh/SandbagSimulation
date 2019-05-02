using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;
using SandbagSimulation;

namespace Tests
{
    public class LocateSandbagTests
    {
        // A Test behaves as an ordinary method
        //[Test]
        //public void LocateSandbag_1_Sandbag()
        //{
        //    // Arrange
        //    GameObject sandbag = CreateSandbag();
        //    GameObject drone = CreateDrone();

        //    // Act
        //    drone.GetComponent<DroneController>().LocateNearestSandbag();
            
        //    // Assert
        //    Assert.AreEqual(drone.GetComponent<DroneController>().LocatedSandbag, sandbag);

        //}

        public GameObject CreateSandbag()
        {
            GameObject sandbag = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sandbag.AddComponent<Rigidbody>();
            sandbag.GetComponent<Rigidbody>().isKinematic = true;
            sandbag.tag = "Sandbag";

            return sandbag;
        }

        public GameObject CreateDrone()
        {
            GameObject drone = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            drone.AddComponent<DroneController>();
            drone.tag = "Drone";

            return drone;
        }

        //[Test]
        //public void LocateSandbag_2_Sandbags()
        //{
        //    // Arrange
        //    GameObject sandbag1 = CreateSandbag();
        //    GameObject sandbag2 = CreateSandbag();

        //    sandbag1.transform.position = new Vector3(10, 2, 0);
        //    sandbag2.transform.position = new Vector3(20, 2, 0);

        //    GameObject drone = CreateDrone();

        //    drone.transform.position = new Vector3(0, 2, 0);

        //    // Act
        //    drone.GetComponent<DroneController>().LocateNearestSandbag();

        //    // Assert
        //    Assert.AreEqual(drone.GetComponent<DroneController>().LocatedSandbag, sandbag1);
        //}

        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }
    }
}
