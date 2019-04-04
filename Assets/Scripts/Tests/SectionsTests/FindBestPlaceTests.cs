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
        public void Constructor_should_assign_CurrentSection()
        {
            Vector3 testVector = new Vector3(10, 10, 10);
            // Create new section
            Section section = new Section(testVector);
            
            Assert.AreEqual(testVector, section.CurrentSection);
        }

        [Test]
        public void IsAccess_should_return_negative_100_vector_when_no_access()
        {
            // Arrange 

            // Heavy and tedious setup (Please improve!)
            // Create new section
            Section section = new Section(new Vector3(10f, 10f, 10f));

            // Set the minimumSeperation
            section.MinimumSeperation = 5f;

            // Create a number of drones/obstacles
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "Drone";
            cube1.transform.position = new Vector3(1f, 1f, 1f);
            

            // Set ViewDistance so that drone can see obstacle
            float viewDistance = 20f;

            // Create array of places, with one possible location
            Vector3[] places = { new Vector3(0f, 0f, 0f) };
            Vector3 position = new Vector3(10f, 10f, 10f);

            // Act
            Vector3 result = section.FindBestPlace(places, position, viewDistance);

            // Assert
            Assert.AreEqual(result, new Vector3(-100f, -100f, -100f));
        }

        // A Test behaves as an ordinary method
        [Test]
        public void FindBestPlaceTestsSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator FindBestPlaceTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
