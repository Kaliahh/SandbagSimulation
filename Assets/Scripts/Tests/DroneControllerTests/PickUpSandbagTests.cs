using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;
using SandbagSimulation;

namespace Tests
{
    public class PickUpSandbagTests
    {
        [Test]
        public void PickUpSandbagTest()
        {
            // Arrange
            GameObject sandbag = CreateSandbag();
            GameObject drone = CreateDrone();

            drone.GetComponent<DroneController>().LocateNearestSandbag();

            // Act
            drone.GetComponent<DroneController>().PickUpSandbag();

            // Assert
            Assert.AreEqual(drone.GetComponent<DroneController>().MySandbag, sandbag);
        }


        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

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
    }
}
