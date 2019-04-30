using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SandbagSimulation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class AdjecentTests
    {
        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        [UnityTest]
        public IEnumerator Adjecent_PointInMiddleOfStraightLine_ReturnCorrectPointTowardsFirstNode()
        {
            Point point = new Point(new Vector3(0f, 0f, 10f));

            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);

            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.AddComponent(typeof(SandbagController));
            float sandbagLength = cube1.GetComponent<SandbagController>().Length;

            yield return null;
            Point[] adjecent = point.Adjecent(blueprint, cube1.GetComponent<SandbagController>());
            Assert.AreEqual(new Point(new Vector3(0f, 0f, 10f - sandbagLength)).Position, adjecent[0].Position);
        }

        [UnityTest]
        public IEnumerator Adjecent_PointInMiddleOfStraightLine_ReturnCorrectPointTowardsLastNode()
        {
            Point point = new Point(new Vector3(0f, 0f, 10f));

            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);

            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.AddComponent(typeof(SandbagController));
            float sandbagLength = cube1.GetComponent<SandbagController>().Length;

            yield return null;
            Point[] adjecent = point.Adjecent(blueprint, cube1.GetComponent<SandbagController>());
            Assert.AreEqual(new Point(new Vector3(0f, 0f, 10f + sandbagLength)).Position, adjecent[1].Position);
        }

        [UnityTest]
        public IEnumerator Adjecent_PointInMiddleOfDiagonalLine_ReturnCorrectPointTowardsFirstNode()
        {
            Point point = new Point(new Vector3(0f, 10f, 10f));

            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 20f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);

            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.AddComponent(typeof(SandbagController));
            float sandbagLength = cube1.GetComponent<SandbagController>().Length;

            yield return null;
            Point[] adjecent = point.Adjecent(blueprint, cube1.GetComponent<SandbagController>());
            Assert.AreEqual(new Point(new Vector3(0f, 10f - (0.5f * sandbagLength), 10f - (0.5f * sandbagLength))).Position, adjecent[0].Position);
        }

        [UnityTest]
        public IEnumerator Adjecent_PointInMiddleOfDiagonalLine_ReturnCorrectPointTowardsLastNode()
        {
            Point point = new Point(new Vector3(0f, 10f, 10f));

            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 20f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);

            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.AddComponent(typeof(SandbagController));
            float sandbagLength = cube1.GetComponent<SandbagController>().Length;

            yield return null;
            Point[] adjecent = point.Adjecent(blueprint, cube1.GetComponent<SandbagController>());
            Assert.AreEqual(new Point(new Vector3(0f, 10f + (0.5f * sandbagLength), 10f + (0.5f * sandbagLength))).Position, adjecent[1].Position);
        }
    }
}
