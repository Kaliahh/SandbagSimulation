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
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        [UnityTest]
        public IEnumerator OnLine_PointIsDirectlyOnLine_ReturnTrue()
        {
            Point point = new Point(new Vector3(0f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;

            yield return null;

            Assert.IsTrue(point.OnLine(blueprint, maxDistance));
        }

        [UnityTest]
        public IEnumerator OnLine_PointIsNotOnLine_ReturnFalse()
        {
            Point point = new Point(new Vector3(1f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;

            yield return null;

            Assert.IsFalse(point.OnLine(blueprint, maxDistance));
        }
        [UnityTest]
        public IEnumerator OnLine_PointIsMaxDistanceAwayFromLine_ReturnTrue()
        {
            Point point = new Point(new Vector3(0.5f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;

            yield return null;

            Assert.IsTrue(point.OnLine(blueprint, maxDistance));
        }

        [UnityTest]
        public IEnumerator OnLine_PointIsNegativeMaxDistanceAwayFromLine_ReturnTrue()
        {
            Point point = new Point(new Vector3(-0.5f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;

            yield return null;

            Assert.IsTrue(point.OnLine(blueprint, maxDistance));
        }

        [UnityTest]
        public IEnumerator OnLine_PointIsHalfMaxDistanceAwayFromLine_ReturnTrue()
        {
            Point point = new Point(new Vector3(0.25f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;

            yield return null;

            Assert.IsTrue(point.OnLine(blueprint, maxDistance));
        }

        [UnityTest]
        public IEnumerator OnLine_PointIsDirectlyOnDiagonalLine_ReturnTrue()
        {
            Point point = new Point(new Vector3(10f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(20f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;

            yield return null;

            Assert.IsTrue(point.OnLine(blueprint, maxDistance));
        }


        [UnityTest]
        public IEnumerator OnLine_PointIsNotOnDiagonalLine_ReturnFalse()
        {
            Point point = new Point(new Vector3(15f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(20f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;

            yield return null;

            Assert.IsFalse(point.OnLine(blueprint, maxDistance));
        }

        [UnityTest]
        public IEnumerator OnLine_PointIsMaxDistanceFromDiagonalLine_ReturnTrue()
        {
            Point point = new Point(new Vector3(10.5f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(20f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;

            yield return null;

            Assert.IsTrue(point.OnLine(blueprint, maxDistance));
        }

        [UnityTest]
        public IEnumerator OnLine_PointIsHalfMaxDistanceFromDiagonalLine_ReturnTrue()
        {
            Point point = new Point(new Vector3(10.25f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(20f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            float maxDistance = 0.5f;

            yield return null;

            Assert.IsTrue(point.OnLine(blueprint, maxDistance));
        }
    }
}
