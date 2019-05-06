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
    public class EvaluatorTests
    {

        #region EvaluateDikeTests
        public class EvaluateDikeTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void EvaluateDike_WhenSampleSizeIsSufficient_TTestsResultsAreValid()
            {
                //Arrange. 
                var MyEvaluator = new Evaluator();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(10 * x, 10 * x, 10 * x)).ToList();
                var blueprint = new Blueprint(constructionNodes, 10);
                var pickupPoint = new Vector3(0, 0, 0);


                List<GameObject> gameobjects = Enumerable.Range(1, 200)
                                .Select(x => new GameObject() { tag = (x % 2 == 0) ? "PlacedSandbag" : "Drone" })
                                .ToList();

                float a = 1;

                gameobjects = gameobjects.Select((obj, i) =>
                {
                    a += (float) (i * (8 / 5));
                    obj.transform.position = new Vector3(1 * a, 1 * a, 3 * a);
                    obj.transform.right = new Vector3(1 * a, 2 * a, 3 * a);
                    obj.transform.up = new Vector3(1 * a, 2 * a, 3 * a);
                    obj.transform.forward = new Vector3(1 * a, 2 * a, 3 * a);
                    return obj;
                })
                .ToList();


                //Act.
                MyEvaluator.EvaluateDike(blueprint, pickupPoint);


                //Assert.
                Assert.IsTrue(MyEvaluator.RotationalTTest.TTestResultsAreValid && MyEvaluator.PositionalTTest.TTestResultsAreValid);

            }


            [Test]
            public void EvaluateDike_SampleSizeIsTooSmall_TTestsResultsForRotationAreSetToInvalid()
            {
                //Arrange. 
                var MyEvaluator = new Evaluator();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(1, 1, 1)).ToList();
                var blueprint = new Blueprint(constructionNodes, 1);
                var pickupPoint = new Vector3(1, 1, 1);

                //Act.
                MyEvaluator.EvaluateDike(blueprint, pickupPoint);

                //Assert.
                Assert.IsFalse(MyEvaluator.RotationalTTest.TTestResultsAreValid);
            }

        }

        #endregion

    }
}
