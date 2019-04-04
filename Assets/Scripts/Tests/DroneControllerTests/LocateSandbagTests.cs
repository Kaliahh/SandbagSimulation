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
        [Test]
        public void LocateSandbagTestsSimplePasses()
        {
            //if (EditorSceneManager.GetActiveScene() != EditorSceneManager.GetSceneByName("LocateSandbagTestScene1"))
            //{
            //    EditorSceneManager.LoadScene("LocateSandbagTestScene1");
            //}

            EditorSceneManager.LoadScene("LocateSandbagTestScene1");

            GameObject drone = GameObject.FindGameObjectWithTag("Drone");

            DroneController controller = drone.GetComponent<DroneController>();

            if (controller == null)
            {
                Assert.Fail();
            }

            controller.LocateNearestSandbag();

         

        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator LocateSandbagTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }

        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }
    }
}
