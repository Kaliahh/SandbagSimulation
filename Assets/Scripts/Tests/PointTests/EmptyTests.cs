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
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void EmptyTestsSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator EmptyTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }

        [UnityTest]
        public IEnumerator Empty_NoBags_ReturnTrue()
        {
            Point point = new Point(new Vector3(0f, 0f, 0f));
            yield return null;
            Assert.IsTrue(point.Empty(new Vector3(0f, 10f, 0f)));
        }

        [UnityTest]
        public IEnumerator Empty_NoBagsAndDroneOnPoint_ReturnTrue()
        {
            Point point = new Point(new Vector3(0f, 10f, 0f));
            yield return null;
            Assert.IsTrue(point.Empty(new Vector3(0f, 10f, 0f)));
        }

        [UnityTest]
        public IEnumerator Empty_BagOnPointAndDroneAbove_ReturnFalse()
        {
            Point point = new Point(new Vector3(0f, 0f, 10f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 10f);

            yield return null;

            Assert.IsFalse(point.Empty(new Vector3(0f, 10f, 0f)));
        }

        [UnityTest]
        public IEnumerator Empty_BagOnPointAndDroneBelow_ReturnFalse()
        {
            Point point = new Point(new Vector3(0f, 10f, 10f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 10f, 10f);

            yield return null;

            Assert.IsFalse(point.Empty(new Vector3(0f, 0f, 0f)));
        }

        [UnityTest]
        public IEnumerator Empty_BagBlockingPoint_ReturnFalse()
        {
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 5f, 10f);

            yield return null;

            Assert.IsTrue(point.Empty(new Vector3(0f, 10f, 0f)));
        }

        [UnityTest]
        public IEnumerator Empty_BagNextToPointAndDroneRightAbove_ReturnTrue()
        {
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 1f);

            yield return null;

            Assert.IsTrue(point.Empty(new Vector3(0f, 10f, 0f)));
        }

        [UnityTest]
        public IEnumerator Empty_BagNextToBlockingPointAndDroneDiagonallyAbove_ReturnFalse()
        {
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 1f);

            yield return null;

            Assert.IsFalse(point.Empty(new Vector3(0f, 1f, 5f)));
        }

        [UnityTest]
        public IEnumerator Empty_BagNextToPointAndDroneDiagonallyAbove_ReturnFalse()
        {
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 1f);

            yield return null;

            Assert.IsTrue(point.Empty(new Vector3(10f, 1f, 0f)));
        }




    }
}
