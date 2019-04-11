using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SandbagSimulation;
using UnityEditor.SceneManagement;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Tests
{
    public class FindPlaceTests
    {
        // Tester om der kommer et gyldigt output, givent et gyldigt input
        [Test]
        public void FindPlace_PassValid_ReturnValid()
        {
            Vector3 position = new Vector3(10f, 10f, 0f);
            Section section = new Section();
            float viewDistance = 5f;
            List<Vector3> constructionNodes = new List<Vector3>();
            // Left
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            // Right
            constructionNodes.Add(new Vector3(20f, 0f, 0f));
            Blueprint blueprint = new Blueprint(constructionNodes, 10);

            // Lav sandsæk lige under dronen
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "PlacedSandbag";
            cube1.transform.position = new Vector3(10f, 7f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            Vector3[] result = section.FindPlace(viewDistance, position, blueprint);
            //Assert.AreEqual(result, new Vector3(15f, 10f, 0f));
            Assert.IsEmpty(result);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator FindPlace_PassSingleSandbag_ReturnTwoPlaces()
        {
            Vector3 position = new Vector3(10f, 3f, 0f);
            Section section = new Section();
            float viewDistance = 10f;
            List<Vector3> constructionNodes = new List<Vector3>();
            // Left
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            // Right
            constructionNodes.Add(new Vector3(20f, 0f, 0f));
            Blueprint blueprint = new Blueprint(constructionNodes, 10);

            // Lav sandsæk lige under dronen
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "PlacedSandbag";
            cube1.transform.position = new Vector3(10f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            yield return null;

            Vector3[] result = section.FindPlace(viewDistance, position, blueprint);
            Assert.AreEqual(result.Length, 2);
        }

        [UnityTest]
        public IEnumerator FindPlace_NoSandbagHit_ReturnNull()
        {
            Vector3 position = new Vector3(10f, 3f, 0f);
            Section section = new Section();
            float viewDistance = 10f;
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(20f, 0f, 0f));
            Blueprint blueprint = new Blueprint(constructionNodes, 10);

            yield return null;

            Vector3[] result = section.FindPlace(viewDistance, position, blueprint);
            Assert.IsNull(result);
        }

        [UnityTest]
        public IEnumerator FindPlace_PassOneBagInViewOneOutOfView_ReturnTwoPlaces()
        {
            Vector3 position = new Vector3(10f, 3f, 0f);
            Section section = new Section();
            float viewDistance = 10f;
            List<Vector3> constructionNodes = new List<Vector3>();
            // Left
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            // Right
            constructionNodes.Add(new Vector3(50f, 0f, 0f));
            Blueprint blueprint = new Blueprint(constructionNodes, 10);

            // Lav sandsæk lige under dronen
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "PlacedSandbag";
            cube1.transform.position = new Vector3(10f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));
            // Lav sandsæk uden for viewdistance
            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.tag = "PlacedSandbag";
            cube2.transform.position = new Vector3(40f, 0f, 0f);
            cube2.AddComponent(typeof(SphereCollider));
            cube2.AddComponent(typeof(SandbagController));

            yield return null;

            Vector3[] result = section.FindPlace(viewDistance, position, blueprint);
            Assert.AreEqual(result.Length, 2);
        }

        [UnityTest]
        public IEnumerator FindPlace_PassTwoBagsInView_ReturnThreePlaces()
        {
            Vector3 position = new Vector3(10f, 3f, 0f);
            Section section = new Section();
            float viewDistance = 20f;
            List<Vector3> constructionNodes = new List<Vector3>();
            // Left
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            // Right
            constructionNodes.Add(new Vector3(50f, 0f, 0f));
            Blueprint blueprint = new Blueprint(constructionNodes, 10);

            // Lav sandsæk lige under dronen
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "PlacedSandbag";
            cube1.transform.position = new Vector3(10f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            yield return null;

            float lenght = cube1.GetComponent<SandbagController>().Length;

            // Lav sandsæk uden for viewdistance
            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.tag = "PlacedSandbag";
            cube2.transform.position = new Vector3(10f + lenght, 0f, 0f);
            cube2.AddComponent(typeof(SphereCollider));
            cube2.AddComponent(typeof(SandbagController));

            Debug.Log(cube1.transform.position.ToString() + " " + cube2.transform.position.ToString());

            yield return null;

            Vector3[] result = section.FindPlace(viewDistance, position, blueprint);
            Assert.AreEqual(3, result.Length);
        }

        [UnityTest]
        public IEnumerator FindPlace_PassStartingSandbagNotDirectlyUnder_ReturnTwoPlaces()
        {
            Vector3 position = new Vector3(10f, 3f, 0f);
            Section section = new Section();
            float viewDistance = 10f;
            List<Vector3> constructionNodes = new List<Vector3>();
            // Left
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            // Right
            constructionNodes.Add(new Vector3(20f, 0f, 0f));
            Blueprint blueprint = new Blueprint(constructionNodes, 10);

            // Lav sandsæk lige under dronen
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "PlacedSandbag";
            cube1.transform.position = new Vector3(15f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            yield return null;

            Vector3[] result = section.FindPlace(viewDistance, position, blueprint);
            Assert.AreEqual(result.Length, 2);
        }

        [UnityTest]
        public IEnumerator FindPlace_PassTwoBagsInView_TestPerformance()
        {
            Vector3 position = new Vector3(10f, 3f, 0f);
            Section section = new Section();
            float viewDistance = 20f;
            List<Vector3> constructionNodes = new List<Vector3>();
            // Left
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            // Right
            constructionNodes.Add(new Vector3(50f, 0f, 0f));
            Blueprint blueprint = new Blueprint(constructionNodes, 10);

            // Lav sandsæk lige under dronen
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "PlacedSandbag";
            cube1.transform.position = new Vector3(10f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            yield return null;

            float lenght = cube1.GetComponent<SandbagController>().Length;

            // Lav sandsæk uden for viewdistance
            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.tag = "PlacedSandbag";
            cube2.transform.position = new Vector3(10f + lenght, 0f, 0f);
            cube2.AddComponent(typeof(SphereCollider));
            cube2.AddComponent(typeof(SandbagController));

            Debug.Log(cube1.transform.position.ToString() + " " + cube2.transform.position.ToString());

            yield return null;

            Vector3[] result = { };

            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            for (int i = 0; i < 10; i++)
            {
                // Begin timing.
                stopwatch.Start();

                result = section.FindPlace(viewDistance, position, blueprint);

                // Stop timing.
                stopwatch.Stop();

                // Write result.
                Debug.LogFormat("Time elapsed: {0}", stopwatch.Elapsed);
            }


            Assert.AreEqual(3, result.Length);
        }
    }
}
