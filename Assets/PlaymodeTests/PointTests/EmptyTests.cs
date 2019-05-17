using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EmptyTests
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
        public IEnumerator Empty_NoBags_ReturnTrue()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 0f));
            //Act
            yield return null;
            bool result = point.Empty(new Vector3(0f, 10f, 0f));
            //Assert
            Assert.IsTrue(result);
        }

        [UnityTest]
        public IEnumerator Empty_NoBagsAndDroneOnPoint_ReturnTrue()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 10f, 0f));
            //Act
            yield return null;
            bool result = point.Empty(new Vector3(0f, 10f, 0f));
            //Assert
            Assert.IsTrue(result);
        }

        [UnityTest]
        public IEnumerator Empty_BagOnPointAndDroneAbove_ReturnFalse()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 10f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 10f);
            //Act
            yield return null;
            bool result = point.Empty(new Vector3(0f, 10f, 0f));
            //Assert
            Assert.IsFalse(result);
        }

        [UnityTest]
        public IEnumerator Empty_BagOnPointAndDroneBelow_ReturnFalse()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 10f, 10f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 10f, 10f);
            //Act
            yield return null;
            //Assert
            Assert.IsFalse(point.Empty(new Vector3(0f, 0f, 0f)));
        }

        [UnityTest]
        public IEnumerator Empty_BagBlockingPoint_ReturnFalse()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 5f, 10f);
            //Act
            yield return null;
            bool result = point.Empty(new Vector3(0f, 10f, 0f));
            //Assert
            Assert.IsTrue(result);
        }

        [UnityTest]
        public IEnumerator Empty_BagNextToPointAndDroneRightAbove_ReturnTrue()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 1f);
            //Act 
            yield return null;
            bool result = point.Empty(new Vector3(0f, 10f, 0f));
            //Assert
            Assert.IsTrue(result);
        }

        [UnityTest]
        public IEnumerator Empty_BagNextToBlockingPointAndDroneDiagonallyAbove_ReturnFalse()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 1f);
            //Act
            yield return null;
            bool result = point.Empty(new Vector3(0f, 1f, 5f));
            //Assert
            Assert.IsFalse(result);
        }

        [UnityTest]
        public IEnumerator Empty_BagNextToPointAndDroneDiagonallyAbove_ReturnFalse()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 1f);
            //Act
            yield return null;
            bool result = point.Empty(new Vector3(10f, 1f, 0f));
            //Assert
            Assert.IsTrue(result);
        }
    }
}
