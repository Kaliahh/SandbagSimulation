using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SandbagSimulation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class InViewTests
    {
        [SetUp]
        public void ResetScene()
        {
            var list = GameObject.FindObjectsOfType<GameObject>();

            foreach (var item in list)
            {
                GameObject.Destroy(item);
            }
        }

        [UnityTest]
        public IEnumerator InView_BagDirectlyBelowInView_ReturnTrue()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            float sandbagHeight = 0.5f;
            float viewDistance = 20f;
            Vector3 position = new Vector3(0f, 10f, 0f);
            //Act
            yield return null;
            bool result = point.InView(position, viewDistance, sandbagHeight);
            //Assert
            Assert.IsTrue(result);
        }

        [UnityTest]
        public IEnumerator InView_BagDirectlyBelowViewBlocked_ReturnFalse()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 1f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 1f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.transform.position = new Vector3(0f, 5f, 0f);
            cube2.AddComponent(typeof(SphereCollider));
            cube2.AddComponent(typeof(SandbagController));

            float sandbagHeight = 0.5f;
            float viewDistance = 20f;
            Vector3 position = new Vector3(0f, 10f, 0f);
            //Act 
            yield return null;
            bool result = point.InView(position, viewDistance, sandbagHeight);
            //Assert
            Assert.IsFalse(result);
        }

        [UnityTest]
        public IEnumerator InView_BagDiagonallyBelowInView_ReturnTrue()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            float sandbagHeight = 0.5f;
            float viewDistance = 20f;
            Vector3 position = new Vector3(0f, 10f, 5f);
            //Act
            yield return null;
            bool result = point.InView(position, viewDistance, sandbagHeight);
            //Assert
            Assert.IsTrue(result);
        }

        [UnityTest]
        public IEnumerator InView_BagDiagonallyBelowViewBlocked_ReturnFalse()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.transform.position = new Vector3(0f, 2f, 0f);
            cube2.AddComponent(typeof(SphereCollider));
            cube2.AddComponent(typeof(SandbagController));

            float sandbagHeight = 0.5f;
            float viewDistance = 20f;
            Vector3 position = new Vector3(0f, 10f, 2f);
            //Act
            yield return null;
            bool result = point.InView(position, viewDistance, sandbagHeight);
            //Assert
            Assert.IsFalse(result);
        }

        [UnityTest]
        public IEnumerator InView_BagAndDroneSameHeightBagInView_ReturnTrue()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            float sandbagHeight = 0.5f;
            float viewDistance = 20f;
            Vector3 position = new Vector3(0f, 0f, 5f);
            //Act
            yield return null;
            bool result = point.InView(position, viewDistance, sandbagHeight);
            //Assert
            Assert.IsTrue(result);
        }

        [UnityTest]
        public IEnumerator InView_BagAndDroneSameHeightViewBlocked_ReturnFalse()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.transform.position = new Vector3(0f, 0f, 2.5f);
            cube2.AddComponent(typeof(SphereCollider));
            cube2.AddComponent(typeof(SandbagController));

            float sandbagHeight = 0.5f;
            float viewDistance = 20f;
            Vector3 position = new Vector3(0f, 0f, 5f);
            //Act
            yield return null;
            bool result = point.InView(position, viewDistance, sandbagHeight);
            //Assert
            Assert.IsFalse(result);
        }

        [UnityTest]
        public IEnumerator InView_BagDiagonallyBelowWithAdjecentBagsInView_ReturnTrue()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.transform.position = new Vector3(0f, 0f, 1f);
            cube2.AddComponent(typeof(SphereCollider));
            cube2.AddComponent(typeof(SandbagController));

            GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube3.transform.position = new Vector3(0f, 0f, -1f);
            cube3.AddComponent(typeof(SphereCollider));
            cube3.AddComponent(typeof(SandbagController));

            float sandbagHeight = 0.5f;
            float viewDistance = 20f;
            Vector3 position = new Vector3(0f, 1f, 1f);
            //Act
            yield return null;
            bool result = point.InView(position, viewDistance, sandbagHeight);
            //Assert
            Assert.IsTrue(result);
        }

        [UnityTest]
        public IEnumerator InView_BagDirectlyAboveDrone_ReturnFalse()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            float sandbagHeight = 0.5f;
            float viewDistance = 20f;
            Vector3 position = new Vector3(0f, -5f, 0f);
            //Act
            yield return null;
            bool result = point.InView(position, viewDistance, sandbagHeight);
            //Assert
            Assert.IsFalse(result);
        }

        [UnityTest]
        public IEnumerator InView_BagDiagonallyAboveDrone_ReturnFalse()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            float sandbagHeight = 0.5f;
            float viewDistance = 20f;
            Vector3 position = new Vector3(0f, -5f, 2f);
            //Act
            yield return null;
            bool result = point.InView(position, viewDistance, sandbagHeight);
            //Assert
            Assert.IsFalse(result);
        }

    }
}
