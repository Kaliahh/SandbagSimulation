using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SandbagSimulation;

namespace Tests
{
    public class FindPlaceTests
    {
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator FindPlace_PassSingleSandbag_ReturnTwoPlaces()
        {
            Vector3 position = new Vector3(10f, 3f, 0f);
            Section section = new Section(position);
            float viewDistance = 10f;
            List<Vector3> constructionNodes = new List<Vector3>();
            // Left
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            // Right
            constructionNodes.Add(new Vector3(20f, 0f, 0f));

            // Lav sandsæk lige under dronen
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.tag = "Sandbag";
            cube1.transform.position = new Vector3(10f, 0f, 0f);
            cube1.AddComponent(typeof(SphereCollider));
            cube1.AddComponent(typeof(SandbagController));

            yield return null;

            Vector3[] result = section.FindPlace(viewDistance, position, constructionNodes);
            Assert.AreEqual(result.Length, 2);
        }
    }
}
