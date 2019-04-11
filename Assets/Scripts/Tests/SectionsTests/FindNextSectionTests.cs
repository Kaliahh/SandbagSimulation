using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;
using SandbagSimulation;


namespace Tests
{
    public class FindNextSectionTests
    {
        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        [Test]
        public void FindNextSection_PassIsRightDrone_ReturnPositionRightOfOrigin()
        {
            Vector3 position = new Vector3(10f, 10f, 0f);
            Section section = new Section();
            float viewDistance = 5f;
            List<Vector3> constructionNodes = new List<Vector3>();
            // Left
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            // Right
            constructionNodes.Add(new Vector3(20f, 0f, 0f));
            Blueprint blueprint = new Blueprint(constructionNodes, 10);

            Vector3 result = section.FindNextSection(viewDistance, position, true, blueprint);
            //Assert.AreEqual(result, new Vector3(15f, 10f, 0f));
            Assert.Greater(result.x, position.x);
        }

        [Test]
        public void FindNextSection_PassIsNotRightDrone_ReturnPositionLeftOfOrigin()
        {
            Vector3 position = new Vector3(10f, 10f, 0f);
            Section section = new Section();
            float viewDistance = 5f;
            List<Vector3> constructionNodes = new List<Vector3>();
            // Left
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            // Right
            constructionNodes.Add(new Vector3(20f, 0f, 0f));
            Blueprint blueprint = new Blueprint(constructionNodes, 10);

            Vector3 result = section.FindNextSection(viewDistance, position, false, blueprint);
            //Assert.AreEqual(result, new Vector3(15f, 10f, 0f));
            Assert.Greater(position.x, result.x);
        }

        [Test]
        public void FindNextSection_PassGoalEqualsPosition_ReturnSamePosition()
        {
            Vector3 position = new Vector3(20f, 10f, 0f);
            Section section = new Section();
            float viewDistance = 5f;
            List<Vector3> constructionNodes = new List<Vector3>();
            // Left
            constructionNodes.Add(new Vector3(0f, 0f, 0f));
            // Right
            constructionNodes.Add(new Vector3(20f, 0f, 0f));
            Blueprint blueprint = new Blueprint(constructionNodes, 10);

            Vector3 result = section.FindNextSection(viewDistance, position, true, blueprint);
            //Assert.AreEqual(result, new Vector3(15f, 10f, 0f));
            Assert.AreEqual(position.x, result.x);
        }

    }
}
