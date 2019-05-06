using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            SandbagMeasurements sandbag = new SandbagMeasurements();

            //Act
            yield return null;
            Point[] adjecent = point.Adjecent(blueprint, sandbag);
            Vector3 correctPoint = new Vector3(0f, 0f, 10f - sandbag.Length);
            //Assert
            Assert.AreEqual(correctPoint, adjecent[0].Position);
        }

        [UnityTest]
        public IEnumerator Adjecent_PointInMiddleOfStraightLine_ReturnCorrectPointTowardsLastNode()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(0f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            SandbagMeasurements sandbag = new SandbagMeasurements();

            //Act
            yield return null;
            Point[] adjecent = point.Adjecent(blueprint, sandbag);
            Vector3 correctPoint = new Vector3(0f, 0f, 10f + sandbag.Length);
            //Assert
            Assert.AreEqual(correctPoint, adjecent[1].Position);
        }

        [UnityTest]
        public IEnumerator Adjecent_PointInMiddleOfDiagonalLine_ReturnCorrectPointTowardsFirstNode()
        {
            //Arrange
            Point point = new Point(new Vector3(10f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(20f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            SandbagMeasurements sandbag = new SandbagMeasurements();

            //Act
            yield return null;
            Point[] adjecent = point.Adjecent(blueprint, sandbag);
            Vector3 expected = point.Position + (blueprint.ConstructionNodes.First() - blueprint.ConstructionNodes.Last()).normalized * sandbag.Length;
            //Assert
            Assert.AreEqual(expected, adjecent[0].Position);
        }

        [UnityTest]
        public IEnumerator Adjecent_PointInMiddleOfDiagonalLine_ReturnCorrectPointTowardsLastNode()
        {
            //Arrange
            Point point = new Point(new Vector3(10f, 0f, 10f));
            List<Vector3> constructionNodes = new List<Vector3>();
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            constructionNodes.Add(new Vector3(20f, 0f, 20f));
            Blueprint blueprint = new Blueprint(constructionNodes, 2);
            SandbagMeasurements sandbag = new SandbagMeasurements();

            //Act
            yield return null;
            Point[] adjecent = point.Adjecent(blueprint, sandbag);
            Vector3 expected = point.Position + (blueprint.ConstructionNodes.Last() - blueprint.ConstructionNodes.First()).normalized * sandbag.Length;
            //Assert
            Assert.AreEqual(expected, adjecent[1].Position);
        }
    }
}
