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
        [Test]
        public void FindPlace_PassValid_ReturnValid()
        {
            Vector3 position = new Vector3(10f, 10f, 0f);
            Section section = new Section(position);
            float viewDistance = 5f;
            List<Vector3> constructionNodes = new List<Vector3>();
            // Left
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            // Right
            constructionNodes.Add(new Vector3(20f, 0f, 0f));

            // Lav sandsæk lige under dronen
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "Sandbag";
            cube1.transform.position = new Vector3(10f, 7f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            Vector3[] result = section.FindPlace(viewDistance, position, constructionNodes);
            //Assert.AreEqual(result, new Vector3(15f, 10f, 0f));
            Assert.IsEmpty(result);
        }

        [Test]
        public void FindPlace_PassSingleSandbag_ReturnTwoPlaces()
        {
            Vector3 position = new Vector3(10f, 3f, 0f);
            Section section = new Section(position);
            float viewDistance = 10f;
            List<Vector3> constructionNodes = new List<Vector3>();
            // Left
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            // Right
            constructionNodes.Add(new Vector3(20f, 0f, 0f));

            // Lav sandsæk lige under dronen
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "Sandbag";
            cube1.transform.position = new Vector3(10f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            Vector3[] result = section.FindPlace(viewDistance, position, constructionNodes);
            //Assert.AreEqual(result, new Vector3(15f, 10f, 0f));
            Assert.AreEqual(result.Length, 2);
        }
    }
}
