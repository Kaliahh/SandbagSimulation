using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SandbagSimulation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class AboveTests
    {
        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        [UnityTest]
        public IEnumerator Above_GivenValidValues_ReturnsCorrectNumberOfElements()
        {
            //Arrange 
            Point point = new Point(new Vector3(0f, 0f, 10f));
            SandbagMeasurements sandbag = new SandbagMeasurements();
            Point[] adjecent = new Point[]
            {
                new Point(new Vector3(point.Position.x, point.Position.y, point.Position.z - sandbag.Length)),
                new Point(new Vector3(point.Position.x, point.Position.y, point.Position.z + sandbag.Length)),
            };

            //Act
            yield return null;
            Point[] above = point.Above(adjecent, sandbag);
            //Assert
            Assert.AreEqual(2, above.Length);
        }

        [UnityTest]
        public IEnumerator Above_GivenCorrectValues_ReturnsCorrectLeftPoint()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 10f));
            SandbagMeasurements sandbag = new SandbagMeasurements();
            Point[] adjecent = new Point[]
            {
                new Point(new Vector3(point.Position.x, point.Position.y, point.Position.z - sandbag.Length)),
                new Point(new Vector3(point.Position.x, point.Position.y, point.Position.z + sandbag.Length)),
            };
            //Act
            yield return null;
            Vector3 leftMiddle = Vector3.Lerp(point.Position, adjecent[0].Position, 0.5f);
            Point[] above = point.Above(adjecent, sandbag);
            Vector3 correctPosition = new Vector3(leftMiddle.x, leftMiddle.y + sandbag.Height, leftMiddle.z);
            //Assert
            Assert.AreEqual(correctPosition, above[0].Position);
        }

        [UnityTest]
        public IEnumerator Above_GivenCorrectValues_ReturnsCorrectRightPoint()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 10f));
            SandbagMeasurements sandbag = new SandbagMeasurements();
            Point[] adjecent = new Point[]
            {
                new Point(new Vector3(point.Position.x, point.Position.y, point.Position.z - sandbag.Length)),
                new Point(new Vector3(point.Position.x, point.Position.y, point.Position.z + sandbag.Length)),
            };
            
            //Act 
            yield return null;
            Vector3 rightMiddle = Vector3.Lerp(point.Position, adjecent[1].Position, 0.5f);
            Point[] above = point.Above(adjecent, sandbag);
            Vector3 correctPosition = new Vector3(rightMiddle.x, rightMiddle.y + sandbag.Height, rightMiddle.z);
            //Assert
            Assert.AreEqual(correctPosition, above[1].Position);
        }

        [UnityTest]
        public IEnumerator Above_GivenInCorrectValue_ReturnsInCorrectLeftPoint()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 10f));
            SandbagMeasurements sandbag = new SandbagMeasurements();
            Point[] adjecent = new Point[]
            {
                new Point(new Vector3(point.Position.x, point.Position.y, point.Position.z - 2 * sandbag.Length)),
                new Point(new Vector3(point.Position.x, point.Position.y, point.Position.z + 2 * sandbag.Length)),
            };

            //Act
            yield return null;
            Vector3 correctLeft = new Vector3(point.Position.x, point.Position.y, point.Position.z - sandbag.Length);
            Vector3 leftMiddle = Vector3.Lerp(point.Position, correctLeft, 0.5f);
            Point[] above = point.Above(adjecent, sandbag);
            Vector3 correctPosition = new Vector3(leftMiddle.x, leftMiddle.y + sandbag.Height, leftMiddle.z);
            //Assert
            Assert.AreNotEqual(correctPosition, above[0].Position);
        }

        [UnityTest]
        public IEnumerator Above_GivenInCorrectValue_ReturnsInCorrectRightPoint()
        {
            //Arrange
            Point point = new Point(new Vector3(0f, 0f, 10f));
            SandbagMeasurements sandbag = new SandbagMeasurements();
            Point[] adjecent = new Point[]
            {
                new Point(new Vector3(point.Position.x, point.Position.y, point.Position.z - 2 * sandbag.Length)),
                new Point(new Vector3(point.Position.x, point.Position.y, point.Position.z + 2 * sandbag.Length)),
            };

            //Act
            yield return null;
            Vector3 correctRight = new Vector3(point.Position.x, point.Position.y, point.Position.z + sandbag.Length);
            Vector3 rightMiddle = Vector3.Lerp(point.Position, correctRight, 0.5f);
            Point[] above = point.Above(adjecent, sandbag);
            Vector3 correctPosition = new Vector3(rightMiddle.x, rightMiddle.y + sandbag.Height, rightMiddle.z);
            //Assert
            Assert.AreNotEqual(correctPosition, above[1].Position);
        }
    }
}
