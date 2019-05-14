using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SandbagSimulation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class WithinBorderTests
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
        public IEnumerator WithinBorder_PointIsWithinBorder_ReturnTrue()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;
            float sandbagHeigth = 0.5f;
            //Act
            yield return null;
            bool result = point.WithinBorder(blueprint, sandbagHeigth, maxDistance);
            //Assert
            Assert.IsTrue(result);
        }

        [UnityTest]
        public IEnumerator WithinBorder_PointIsNotWithinHeight_ReturnFalse()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 1.5f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;
            float sandbagHeigth = 0.5f;
            //Act
            yield return null;
            bool result = point.WithinBorder(blueprint, sandbagHeigth, maxDistance);
            //Assert
            Assert.IsFalse(result);
        }

        [UnityTest]
        public IEnumerator WithinBorder_PointIsNotOnLine_ReturnFalse()
        {
            //Arrange
            Point point = new Point(new Vector3(1f, 1f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;
            float sandbagHeigth = 0.5f;
            //Act
            yield return null;
            bool result = point.WithinBorder(blueprint, sandbagHeigth, maxDistance);
            //Assert
            Assert.IsFalse(result);
        }

        [UnityTest]
        public IEnumerator WithinBorder_PointIsNotOnLineOrWithinHeight_ReturnFalse()
        {
            //Arrange
            Point point = new Point(new Vector3(1f, 1.5f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;
            float sandbagHeigth = 0.5f;
            //Act
            yield return null;
            bool result = point.WithinBorder(blueprint, sandbagHeigth, maxDistance);
            //Assert
            Assert.IsFalse(result);
        }

    }
}
