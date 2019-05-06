using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;
using SandbagSimulation;
using System.Linq;
using System;

namespace Tests
{
    public class DikeFinderTests
    {
        #region FindDikeTests
        public class FindDikeTests
        {

            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void FindDike_WhenSandbagsExist_AddsToDikeList()
            {
                // Arrange. 
                GameObject obj = new GameObject();
                obj.tag = "PlacedSandbag";

                var MyDikeFinder = new DikeFinder();

                // Act.
                MyDikeFinder.FindDike();

                // Assert.
                Assert.IsTrue(MyDikeFinder.Dike.Count > 0);
            }


            [Test]
            public void FindDike_WhenNoSandbagExists_AddsNoneToDikeList()
            {
                // Arrange. 
                GameObject obj = new GameObject();
                obj.tag = "Drone";

                var MyDikeFinder = new DikeFinder();

                // Act.
                MyDikeFinder.FindDike();

                // Assert.
                Assert.AreEqual(MyDikeFinder.Dike.Count, 0);
            }


            [Test]
            public void FindDike_WhenMixedObjectsExist_OnlyAddsSandbagsToDikeList()
            {
                // Arrange. 
                var gameobjects = Enumerable.Range(1, 100)
                                  .Select(x => new GameObject() { tag = (x % 2 == 0) ? "PlacedSandbag" : "Drone" })
                                  .ToList();

                var MyDikeFinder = new DikeFinder();

                // Act.
                MyDikeFinder.FindDike();

                // Assert.
                Assert.AreEqual(MyDikeFinder.Dike.Count, 50);
            }

        }
        #endregion
    }
}
