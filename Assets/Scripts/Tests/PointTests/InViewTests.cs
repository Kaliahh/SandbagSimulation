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
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        [UnityTest]
        public IEnumerator InView_BagDirectlyBelowInView_ReturnTrue()
        {
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            float sandbagHeight = 0.5f;
            float viewDistance = 20f;
            Vector3 position = new Vector3(0f, 10f, 0f);
            yield return null;
            Assert.IsTrue(point.InView(position, viewDistance, sandbagHeight));
        }

        [UnityTest]
        public IEnumerator InView_BagDirectlyBelowViewBlocked_ReturnFalse()
        {
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
            yield return null;
            Assert.IsFalse(point.InView(position, viewDistance, sandbagHeight));
        }

        [UnityTest]
        public IEnumerator InView_BagDiagonallyBelowInView_ReturnTrue()
        {
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            float sandbagHeight = 0.5f;
            float viewDistance = 20f;
            Vector3 position = new Vector3(0f, 10f, 5f);
            yield return null;
            Assert.IsTrue(point.InView(position, viewDistance, sandbagHeight));
        }

        [UnityTest]
        public IEnumerator InView_BagDiagonallyBelowViewBlocked_ReturnFalse()
        {
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
            Vector3 position = new Vector3(0f, 10f, 5f);
            yield return null;
            Assert.IsFalse(point.InView(position, viewDistance, sandbagHeight));
        }

        [UnityTest]
        public IEnumerator InView_BagAndDroneSameHeightBagInView_ReturnTrue()
        {
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            float sandbagHeight = 0.5f;
            float viewDistance = 20f;
            Vector3 position = new Vector3(0f, 0f, 5f);
            yield return null;
            Assert.IsTrue(point.InView(position, viewDistance, sandbagHeight));
        }

        [UnityTest]
        public IEnumerator InView_BagAndDroneSameHeightViewBlocked_ReturnFalse()
        {
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
            yield return null;
            Assert.IsFalse(point.InView(position, viewDistance, sandbagHeight));
        }

        [UnityTest]
        public IEnumerator InView_BagDiagonallyBelowWithAdjecentBagsInView_ReturnTrue()
        {
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
            yield return null;
            Assert.IsTrue(point.InView(position, viewDistance, sandbagHeight));
        }

        [UnityTest]
        public IEnumerator InView_BagDirectlyAboveDrone_ReturnFalse()
        {
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            float sandbagHeight = 0.5f;
            float viewDistance = 20f;
            Vector3 position = new Vector3(0f, -5f, 0f);
            yield return null;
            Assert.IsFalse(point.InView(position, viewDistance, sandbagHeight));
        }

        [UnityTest]
        public IEnumerator InView_BagDiagonallyAboveDrone_ReturnFalse()
        {
            Point point = new Point(new Vector3(0f, 0f, 0f));
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(0f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            float sandbagHeight = 0.5f;
            float viewDistance = 20f;
            Vector3 position = new Vector3(0f, -5f, 5f);
            yield return null;
            Assert.IsFalse(point.InView(position, viewDistance, sandbagHeight));
        }

    }
}
