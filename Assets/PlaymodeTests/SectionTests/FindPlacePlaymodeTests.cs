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
using NSubstitute;

namespace Tests
{
    public class FindPlacePlaymodeTests
    {
        // Tester om der kommer et gyldigt output, givent et gyldigt input

        [UnityTest]
        public IEnumerator FindPlace_PassValid_ReturnValid()
        {
            //Arrange
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

            //Act
            yield return new WaitForFixedUpdate();
            Vector3[] result = section.FindPlace(viewDistance, position, blueprint);

            //Assert
            Assert.IsNotNull(result);
        }

        [UnityTest]
        public IEnumerator FindPlace_PassSingleSandbag_ReturnTwoPlaces()
        {
            //Arrange
            Vector3 position = new Vector3(0f, 3f, 10f);
            Section section = new Section();
            float viewDistance = 10f;
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 10);

            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "PlacedSandbag";
            cube1.transform.position = new Vector3(0f, 0f, 10f);
            cube1.AddComponent(typeof(SandbagController));

            //Act
            yield return new WaitForFixedUpdate();
            Vector3[] result = section.FindPlace(viewDistance, position, blueprint);

            //Assert
            Assert.AreEqual(2, result.Length);
        }

        [UnityTest]
        public IEnumerator FindPlace_NoSandbagHit_ReturnNull()
        {
            //Arrange
            Vector3 position = new Vector3(0f, 3f, 10f);
            Section section = new Section();
            float viewDistance = 10f;
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 5);

            //Act
            yield return new WaitForFixedUpdate();
            Vector3[] result = section.FindPlace(viewDistance, position, blueprint);

            //Assert
            Assert.IsNull(result);
        }

        [UnityTest]
        public IEnumerator FindPlace_PassOneBagInViewOneOutOfView_ReturnTwoPlaces()
        {
            //Arrange
            Vector3 position = new Vector3(0f, 3f, 10f);
            Section section = new Section();
            float viewDistance = 10f;
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 50f));
            Blueprint blueprint = new Blueprint(constructionNodes, 10);

            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "PlacedSandbag";
            cube1.transform.position = new Vector3(0f, 0f, 10f);
            cube1.AddComponent(typeof(SandbagController));
            // Lav sandsæk uden for viewdistance
            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.tag = "PlacedSandbag";
            cube2.transform.position = new Vector3(0f, 0f, 40f);
            cube2.AddComponent(typeof(SandbagController));

            //Act
            yield return new WaitForFixedUpdate();
            Vector3[] result = section.FindPlace(viewDistance, position, blueprint);

            //Assert
            Assert.AreEqual(2, result.Length);
        }

        [UnityTest]
        public IEnumerator FindPlace_PassTwoBagsInView_ReturnThreePlaces()
        {
            //Arrange
            Vector3 position = new Vector3(0f, 3f, 10f);
            Section section = new Section();
            float viewDistance = 20f;
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 50f));
            Blueprint blueprint = new Blueprint(constructionNodes, 10);

            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "PlacedSandbag";
            cube1.transform.position = new Vector3(0f, 0f, 10f);
            cube1.AddComponent(typeof(SandbagController));

            //Act
            yield return new WaitForFixedUpdate();

            float length = cube1.GetComponent<SandbagController>().Length;

            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.tag = "PlacedSandbag";
            cube2.transform.position = new Vector3(0f, 0f, 10f + length);
            cube2.AddComponent(typeof(SandbagController));

            Debug.Log(cube1.transform.position.ToString() + " " + cube2.transform.position.ToString());

            yield return new WaitForFixedUpdate();

            Vector3[] result = section.FindPlace(viewDistance, position, blueprint);

            //Assert
            Assert.AreEqual(3, result.Length);
        }

        [UnityTest]
        public IEnumerator FindPlace_PassStartingSandbagNotDirectlyUnder_ReturnTwoPlaces()
        {
            //Arrange
            Vector3 position = new Vector3(0f, 3f, 10f);
            Section section = new Section();
            float viewDistance = 10f;
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 10);

            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "PlacedSandbag";
            cube1.transform.position = new Vector3(0f, 0f, 15f);
            cube1.AddComponent(typeof(SandbagController));

            //Act
            yield return new WaitForFixedUpdate();
            Vector3[] result = section.FindPlace(viewDistance, position, blueprint);

            //Assert
            Assert.AreEqual(2, result.Length);
        }
        
        [UnityTest]
        public IEnumerator FindPlace_PassCompleteDike_ReturnTwoPositions()
        {
            //Arrange
            Vector3 position = new Vector3(0f, 3f, 11f);
            Section section = new Section();
            float viewDistance = 20f;
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 10.5f));
            constructionNodes.Add(new Vector3(0f, 0f, 11.5f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);

            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "PlacedSandbag";
            cube1.transform.position = new Vector3(0f, 0f, 11f);
            cube1.AddComponent(typeof(SandbagController));

            //Act
            yield return new WaitForFixedUpdate();
            Vector3[] result = section.FindPlace(viewDistance, position, blueprint);
            
            //Assert
            Assert.AreEqual(2, result.Length);
        }
    }
}
