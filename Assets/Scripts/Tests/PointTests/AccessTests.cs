using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class AccessTests
    {
        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        [UnityTest]
        public IEnumerator Access_OneDroneInViewBlockingAccess_ReturnFalse()
        {
            Point point = new Point(new Vector3(0f, 0f, 10f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 1f, 10f);
            cube1.tag = "Drone";
            float viewDistance = 10f;
            float minDistance = 2f;
            Vector3 position = new Vector3(0f, 2f, 10f);

            yield return null;
            Assert.IsFalse(point.Access(position, viewDistance, minDistance));
        }

        [UnityTest]
        public IEnumerator Access_NoDrones_ReturnTrue()
        {
            Point point = new Point(new Vector3(0f, 0f, 10f));
            float viewDistance = 10f;
            float minDistance = 2f;
            Vector3 position = new Vector3(0f, 2f, 10f);

            yield return null;
            Assert.IsTrue(point.Access(position, viewDistance, minDistance));
        }

        [UnityTest]
        public IEnumerator Access_OneDroneInViewNotBlockingAccess_ReturnTrue()
        {
            Point point = new Point(new Vector3(0f, 0f, 10f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 1f, 15f);
            cube1.tag = "Drone";
            float viewDistance = 20f;
            float minDistance = 2f;
            Vector3 position = new Vector3(0f, 2f, 10f);

            yield return null;
            Assert.IsTrue(point.Access(position, viewDistance, minDistance));
        }

        [UnityTest]
        public IEnumerator Access_OneDroneNotInViewNotBlockingAccess_ReturnTrue()
        {
            Point point = new Point(new Vector3(0f, 0f, 10f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 1f, 35f);
            cube1.tag = "Drone";
            float viewDistance = 20f;
            float minDistance = 2f;
            Vector3 position = new Vector3(0f, 2f, 10f);

            yield return null;
            Assert.IsTrue(point.Access(position, viewDistance, minDistance));
        }
        [UnityTest]
        public IEnumerator Access_OneDroneNotInViewBlockingAccess_ReturnTrue()
        {
            Point point = new Point(new Vector3(0f, 0f, 10f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 1f, 10f);
            cube1.tag = "Drone";
            float viewDistance = 20f;
            float minDistance = 2f;
            Vector3 position = new Vector3(0f, 2f, 35f);

            yield return null;
            Assert.IsTrue(point.Access(position, viewDistance, minDistance));
        }

        [UnityTest]
        public IEnumerator Access_TwoDronesInViewBlockingAccess_ReturnFalse()
        {
            Point point = new Point(new Vector3(0f, 0f, 10f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 1f, 10f);
            cube1.tag = "Drone";
            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.transform.position = new Vector3(0f, 1f, 9f);
            cube2.tag = "Drone";
            float viewDistance = 20f;
            float minDistance = 2f;
            Vector3 position = new Vector3(0f, 2f, 20f);

            yield return null;
            Assert.IsFalse(point.Access(position, viewDistance, minDistance));
        }
        [UnityTest]
        public IEnumerator Access_TwoDronesInViewNotBlockingAccess_ReturnTrue()
        {
            Point point = new Point(new Vector3(0f, 0f, 10f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 1f, 15f);
            cube1.tag = "Drone";
            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.transform.position = new Vector3(0f, 1f, 5f);
            cube2.tag = "Drone";
            float viewDistance = 20f;
            float minDistance = 2f;
            Vector3 position = new Vector3(0f, 2f, 20f);

            yield return null;
            Assert.IsTrue(point.Access(position, viewDistance, minDistance));
        }

        [UnityTest]
        public IEnumerator Access_TwoDronesNotInViewNotBlockingAccess_ReturnTrue()
        {
            Point point = new Point(new Vector3(0f, 0f, 10f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 1f, 13f);
            cube1.tag = "Drone";
            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.transform.position = new Vector3(0f, 1f, 7f);
            cube2.tag = "Drone";
            float viewDistance = 20f;
            float minDistance = 2f;
            Vector3 position = new Vector3(0f, 2f, 35f);

            yield return null;
            Assert.IsTrue(point.Access(position, viewDistance, minDistance));
        }
        [UnityTest]
        public IEnumerator Access_TwoDronesNotInViewBlockingAccess_ReturnTrue()
        {
            Point point = new Point(new Vector3(0f, 0f, 10f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 1f, 10f);
            cube1.tag = "Drone";
            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.transform.position = new Vector3(0f, 1f, 9f);
            cube2.tag = "Drone";
            float viewDistance = 20f;
            float minDistance = 2f;
            Vector3 position = new Vector3(0f, 2f, 35f);

            yield return null;
            Assert.IsTrue(point.Access(position, viewDistance, minDistance));
        }

        [UnityTest]
        public IEnumerator Access_TwoDronesInViewOneBlockingAccess_ReturnFalse()
        {
            Point point = new Point(new Vector3(0f, 0f, 10f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 1f, 10f);
            cube1.tag = "Drone";
            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.transform.position = new Vector3(0f, 1f, 7f);
            cube2.tag = "Drone";
            float viewDistance = 20f;
            float minDistance = 2f;
            Vector3 position = new Vector3(0f, 2f, 20f);

            yield return null;
            Assert.IsFalse(point.Access(position, viewDistance, minDistance));
        }
        [UnityTest]
        public IEnumerator Access_OneDroneInViewBlockingAccessOneNotInViewBlockingAccess_ReturnFalse()
        {
            Point point = new Point(new Vector3(0f, 0f, 10f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 1f, 10f);
            cube1.tag = "Drone";
            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.transform.position = new Vector3(0f, 1f, 9f);
            cube2.tag = "Drone";
            float viewDistance = 20f;
            float minDistance = 2f;
            Vector3 position = new Vector3(0f, 2f, 29f);

            yield return null;
            Assert.IsFalse(point.Access(position, viewDistance, minDistance));
        }
        [UnityTest]
        public IEnumerator Access_OneDroneInViewNotBlockingAccessOneNotInViewBlockingAccess_ReturnTrue()
        {
            Point point = new Point(new Vector3(0f, 0f, 10f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 1f, 15f);
            cube1.tag = "Drone";
            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.transform.position = new Vector3(0f, 1f, 9f);
            cube2.tag = "Drone";
            float viewDistance = 20f;
            float minDistance = 2f;
            Vector3 position = new Vector3(0f, 2f, 29f);

            yield return null;
            Assert.IsTrue(point.Access(position, viewDistance, minDistance));
        }

        [UnityTest]
        public IEnumerator Access_OneDroneInViewNotBlockingAccessOneNotInViewNotBlockingAccess_ReturnTrue()
        {
            Point point = new Point(new Vector3(0f, 0f, 10f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 1f, 15f);
            cube1.tag = "Drone";
            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.transform.position = new Vector3(0f, 1f, 7f);
            cube2.tag = "Drone";
            float viewDistance = 20f;
            float minDistance = 2f;
            Vector3 position = new Vector3(0f, 2f, 29f);

            yield return null;
            Assert.IsTrue(point.Access(position, viewDistance, minDistance));
        }
        [UnityTest]
        public IEnumerator Access_OneDroneInViewBlockingAccessOneNotInViewNotBlockingAccess_ReturnFalse()
        {
            Point point = new Point(new Vector3(0f, 0f, 10f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 1f, 10f);
            cube1.tag = "Drone";
            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.transform.position = new Vector3(0f, 1f, 7f);
            cube2.tag = "Drone";
            float viewDistance = 20f;
            float minDistance = 2f;
            Vector3 position = new Vector3(0f, 2f, 29f);

            yield return null;
            Assert.IsFalse(point.Access(position, viewDistance, minDistance));
        }
    }
}
