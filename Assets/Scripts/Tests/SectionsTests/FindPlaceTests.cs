using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SandbagSimulation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class FindPlaceTests
    {
        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        // Tester om der kommer et gyldigt output, givent et gyldigt input
        [UnityTest]
        public IEnumerator FindPlace_PassValid_ReturnValid()
        {
            Vector3 position = new Vector3(0f, 5f, 10f);
            Section section = new Section();
            float viewDistance = 10f;
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);

            // Lav sandsæk lige under dronen
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "PlacedSandbag";
            cube1.transform.position = new Vector3(0f, 0f, 10f);
            cube1.AddComponent(typeof(SandbagController));

            yield return null;

            Vector3[] result = section.FindPlace(viewDistance, position, blueprint);
            //Assert.AreEqual(result, new Vector3(15f, 10f, 0f));
            Assert.IsNotNull(result);
        }
        
        [UnityTest]
        public IEnumerator FindPlace_PassSingleSandbag_ReturnTwoPlaces()
        {
            Vector3 position = new Vector3(10f, 3f, 0f);
            Section section = new Section();
            float viewDistance = 10f;
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(20f, 0f, 0f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);

            // Lav sandsæk lige under dronen
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "Sandbag";
            cube1.transform.position = new Vector3(10f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            yield return null;

            Vector3[] result = section.FindPlace(viewDistance, position, blueprint);
            Assert.AreEqual(result.Length, 2);
        }
        

    }
}
