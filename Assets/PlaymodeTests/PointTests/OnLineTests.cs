using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SandbagSimulation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class OnLineTests
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
        public IEnumerator OnLine_PointIsDirectlyOnLine_ReturnTrue()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;
            //Act
            yield return null;
            bool result = point.OnLine(blueprint, maxDistance);
            //Assert
            Assert.IsTrue(result);
        }

        [UnityTest]
        public IEnumerator OnLine_PointIsNotOnLine_ReturnFalse()
        {
            //Arrange
            Point point = new Point(new Vector3(1f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;
            //Act
            yield return null;
            bool result = point.OnLine(blueprint, maxDistance);
            //Assert
            Assert.IsFalse(result);
        }
        [UnityTest]
        public IEnumerator OnLine_PointIsMaxDistanceAwayFromLine_ReturnTrue()
        {
            //Arrange
            Point point = new Point(new Vector3(0.5f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;
            //Act
            yield return null;
            bool result = point.OnLine(blueprint, maxDistance);
            //Assert
            Assert.IsTrue(result);
        }

        [UnityTest]
        public IEnumerator OnLine_PointIsNegativeMaxDistanceAwayFromLine_ReturnTrue()
        {
            //Arrange
            Point point = new Point(new Vector3(-0.5f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;
            //Act
            yield return null;
            bool result = point.OnLine(blueprint, maxDistance);
            //Assert
            Assert.IsTrue(result);
        }

        [UnityTest]
        public IEnumerator OnLine_PointIsHalfMaxDistanceAwayFromLine_ReturnTrue()
        {
            //Arrange
            Point point = new Point(new Vector3(0.25f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;
            //Act
            yield return null;
            bool result = point.OnLine(blueprint, maxDistance);
            //Assert
            Assert.IsTrue(result);
        }

        [UnityTest]
        public IEnumerator OnLine_PointIsDirectlyOnDiagonalLine_ReturnTrue()
        {
            //Arrange
            Point point = new Point(new Vector3(10f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(20f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;
            //Act
            yield return null;
            bool result = point.OnLine(blueprint, maxDistance);
            //Assert
            Assert.IsTrue(result);
        }


        [UnityTest]
        public IEnumerator OnLine_PointIsNotOnDiagonalLine_ReturnFalse()
        {
            //Arrange
            Point point = new Point(new Vector3(15f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(20f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;
            //Act
            yield return null;
            bool result = point.OnLine(blueprint, maxDistance);
            //Assert
            Assert.IsFalse(result);
        }

        [UnityTest]
        public IEnumerator OnLine_PointIsMaxDistanceFromDiagonalLine_ReturnTrue()
        {
            //Arrange
            Point point = new Point(new Vector3(10.5f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(20f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;
            //Act
            yield return null;
            bool result = point.OnLine(blueprint, maxDistance);
            //Assert
            Assert.IsTrue(result);
        }

        [UnityTest]
        public IEnumerator OnLine_PointIsHalfMaxDistanceFromDiagonalLine_ReturnTrue()
        {
            //Arrange
            Point point = new Point(new Vector3(10.25f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(20f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;
            //Act
            yield return null;
            bool result = point.OnLine(blueprint, maxDistance);
            //Assert
            Assert.IsTrue(result);
        }
    }
}
