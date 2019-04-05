using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;
using SandbagSimulation;

namespace Tests
{
    public class PlaceSandbagTests
    {
        [Test]
        public void PlaceSandbagTest_Position()
        {
            // Arrange
            GameObject sandbag = CreateSandbag();
            GameObject drone = CreateDrone();
            Vector3 position = new Vector3(0, 10, 0);

            sandbag.transform.position = Vector3.zero;
            drone.GetComponent<DroneController>().LocateNearestSandbag();
            drone.GetComponent<DroneController>().PickUpSandbag();

            // Act
            drone.GetComponent<DroneController>().PlaceSandbag(position);

            // Assert
            Assert.AreEqual(sandbag.transform.position, position);
        }

        [Test]
        public void PlaceSandbagTest_MySandbag_Is_Null()
        {
            // Arrange
            GameObject drone = CreateDrone();
            GameObject sandbag = CreateSandbag();

            drone.GetComponent<DroneController>().LocateNearestSandbag();
            drone.GetComponent<DroneController>().PickUpSandbag();

            // Act
            drone.GetComponent<DroneController>().PlaceSandbag(Vector3.zero);

            // Assert
            Assert.AreEqual(drone.GetComponent<DroneController>().MySandbag, null);
        }

        [Test]
        public void PlaceSandbagTest_MySandbag_Tag_Is_PlacedSandbag()
        {
            // Arrange
            GameObject drone = CreateDrone();
            GameObject sandbag = CreateSandbag();

            drone.GetComponent<DroneController>().LocateNearestSandbag();
            drone.GetComponent<DroneController>().PickUpSandbag();

            // Act
            drone.GetComponent<DroneController>().PlaceSandbag(Vector3.zero);

            // Assert
            Assert.AreEqual(sandbag.tag, "PlacedSandbag");
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
