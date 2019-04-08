using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class FindBestPlaceTests
    {
        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        [Test]
        public void FindBestPlace_PassPositionToConstructor_SetProperyCorrectly()
        {
            Vector3 testVector = new Vector3(10, 10, 10);
            Section section = new Section(testVector);
            Assert.AreEqual(testVector, section.CurrentSection);
        }

        [Test]
        public void FindBestPlace_PassSinglePositionWithoutAccess_ReturnErrorVector()
        {
            // Arrange 

            // Heavy and tedious setup (Please improve!)
            // Create new section
            Section section = new Section(new Vector3(10f, 10f, 10f));

            // Set ViewDistance so that drone can see obstacle
            float viewDistance = 20f;

            // Create array of places, with one possible location
            Vector3[] places = { new Vector3(0f, 0f, 0f) };
            Vector3 position = new Vector3(10f, 10f, 10f);

            // Set the minimumSeperation
            section.MinimumSeperation = 5f;

            // Create a number of drones/obstacles
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "Drone";
            cube1.transform.position = new Vector3(1f, 1f, 1f);

            // Act
            Vector3 result = section.FindBestPlace(places, position, viewDistance);

            // Assert
            Assert.AreEqual(result, new Vector3(-100f, -100f, -100f));
        }

        [Test]
        public void FindBestPlace_PassMultiplePositionWithoutAccess_ReturnErrorVector()
        {
            Section section = new Section(new Vector3(10f, 10f, 10f));
            float viewDistance = 20f;

            // Tre locations på linje
            Vector3 Location1 = new Vector3(0f, 0f, 0f);
            Vector3 Location2 = new Vector3(5f, 0f, 0f);
            Vector3 Location3 = new Vector3(10f, 0f, 0f);

            // Create array of places, with multiple possible locations
            Vector3[] places = { Location1, Location2, Location3 };
            Vector3 position = new Vector3(5f, 5f, 5f);

            // Sæt minimumSeperation
            section.MinimumSeperation = 5f;

            // Lav et antal forhindringer/andre droner
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "Drone";
            // For tæt på Location1
            cube1.transform.position = new Vector3(0f, 1f, 0f);

            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.tag = "Drone";
            // For tæt på Location2
            cube1.transform.position = new Vector3(5f, 1f, 0f);

            GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube3.tag = "Drone";
            // For tæt på Location3
            cube1.transform.position = new Vector3(10f, 1f, 0f);

            Vector3 result = section.FindBestPlace(places, position, viewDistance);

            // Burde returner fejlværdien, da ingen locationer er tilgængelige
            Assert.AreEqual(result, new Vector3(-100f, -100f, -100f));
        }

        [Test]
        public void FindBestPlace_PassAccessiblePositionAndSingleObstacle_ReturnCorrectPosition()
        {
            Section section = new Section(new Vector3(10f, 10f, 10f));
            section.MinimumSeperation = 5f;

            // Set ViewDistance so that drone can see obstacle
            float viewDistance = 20f;

            Vector3 Location = new Vector3(0f, 0f, 0f);

            // Create array of places, with one possible location
            Vector3[] places = { Location };
            Vector3 position = new Vector3(10f, 10f, 10f);

            // Create a number of drones/obstacles
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "Drone";
            cube1.transform.position = new Vector3(15f, 15f, 15f);

            // Act
            Vector3 result = section.FindBestPlace(places, position, viewDistance);

            // Assert
            Assert.AreEqual(result, Location);
        }

        [Test]
        public void FindBestPlace_PassOneAccessiblePositionAndOneInAccessiblePosition_ReturnAccessiblePosition()
        {
            Section section = new Section(new Vector3(10f, 10f, 10f));
            section.MinimumSeperation = 5f;

            // Set ViewDistance so that drone can see obstacle
            float viewDistance = 20f;

            Vector3 Location1 = new Vector3(5f, 5f, 5f);
            Vector3 Location2 = new Vector3(0f, 0f, 0f);

            // Create array of places, with one possible location
            Vector3[] places = { Location1, Location2 };
            // Place drone
            Vector3 position = new Vector3(10f, 10f, 10f);

            // Create a number of drones/obstacles
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "Drone";

            // Placer forhindring mindre end minimumSeperation fra location2
            cube1.transform.position = new Vector3(6f, 6f, 6f);

            Vector3 result = section.FindBestPlace(places, position, viewDistance);

            Assert.AreEqual(result, Location2);
        }

        [Test]
        public void FindBestPlace_PassMultipleAccessiblePositions_ReturnFirstAccessible()
        {
            Section section = new Section(new Vector3(10f, 10f, 10f));

            // Set ViewDistance so drone can see obstacle
            float viewDistance = 20f;

            Vector3 Location1 = new Vector3(0f, 0f, 0f);
            Vector3 Location2 = new Vector3(5f, 5f, 5f);

            // Create array of places, with multiple possible locations
            Vector3[] places = { Location1, Location2 };
            // Place drone
            Vector3 position = new Vector3(10f, 10f, 10f);

            Vector3 result = section.FindBestPlace(places, position, viewDistance);

            // Burde returnerer den første af de mulige placeringer
            Assert.AreEqual(result, Location1);
        }

        [Test]
        public void FindBestPlace_PassEmptyArray_ReturnErrorVector()
        {
            Section section = new Section(new Vector3());
            float viewDistance = 20f;
            // Create array of places, with multiple possible locations
            Vector3[] places = { };
            // Place drone
            Vector3 position = new Vector3(10f, 10f, 10f);

            Vector3 result = section.FindBestPlace(places, position, viewDistance);

            // Burde returnerer den første af de mulige placeringer
            Assert.AreEqual(result, new Vector3(-100f, -100f, -100f));
        }

    }
}
