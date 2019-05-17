using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SandbagSimulation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class WithinHeightTests
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
        public IEnumerator WithinHeight_PointIsWithinHeight_ReturnTrue()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float sandbagHeight = 0.5f;
            //Act
            yield return null;
            bool result = point.WithinHeight(blueprint, sandbagHeight);
            //Assert
            Assert.IsTrue(result);
        }

        [UnityTest]
        public IEnumerator WithinHeight_PointIsNotWithinHeight_ReturnFalse()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 2f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float sandbagHeight = 0.5f;
            //Act
            yield return null;
            bool result = point.WithinHeight(blueprint, sandbagHeight);
            //Assert
            Assert.IsFalse(result);
        }

        [UnityTest]
        public IEnumerator WithinHeight_PointIsInTopLayer_ReturnTrue()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 1f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float sandbagHeight = 0.5f;
            //Act
            yield return null;
            bool result = point.WithinHeight(blueprint, sandbagHeight);
            //Assert
            Assert.IsTrue(result);
        }
    }
}
