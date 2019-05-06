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
    public class TTesterTests
    {
        public class RunTTestTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void RunTTest_WhenSampleSizeIsSufficientAndStandardDeviationIsOverZero_AssignsCorrectTStatistic()
            {
                // Arrange. 
                var MyTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var misses = Enumerable.Range(1, 5)
                    .Select(x => 50.0)
                    .ToList();

                listOfErrors.AddRange(misses);

                var expectedT = 2.3805;

                // Act.
                MyTTester.RunTTest(listOfErrors);


                // Assert.
                Assert.AreEqual(expectedT, MyTTester.TStatistic, 0.1);

            }


            [Test]
            public void RunTTest_WhenSampleSizeIsSufficientAndStandardDeviationIsOverZero_AssignsCorrectPNullHypothesis()
            {
                // Arrange. 
                var MyTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var misses = Enumerable.Range(1, 5)
                    .Select(x => 50.0)
                    .ToList();

                listOfErrors.AddRange(misses);

                var expectedP = 0.0115185;

                // Act.
                MyTTester.RunTTest(listOfErrors);

                // Assert.
                Assert.AreEqual(expectedP, MyTTester.PNullHypothesis, 0.1);

            }


            [Test]
            public void RunTTest_WhenSampleSizeIsSufficientAndStandardDeviationIsOverZero_AssignsCorrectCohensD()
            {
                // Arrange. 
                var MyTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var misses = Enumerable.Range(1, 5)
                    .Select(x => 50.0)
                    .ToList();

                listOfErrors.AddRange(misses);

                // Gennemsnit / Standardafvigelse beregnet "manuelt".
                var expectedD = 7.1429 / 17.7518;

                // Act.
                MyTTester.RunTTest(listOfErrors);

                // Assert.
                Assert.AreEqual(expectedD, MyTTester.CohensD, 0.1);

            }


            [Test]
            public void RunTTest_WhenSampleSizeIsSufficientOneAndStandardDeviationIsZero_AssignNearZeroToStandardDeviation()
            {
                // Arrange. 
                var MyTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.1)
                    .ToList();

                // Act.
                MyTTester.RunTTest(listOfErrors);

                // Assert.
                Assert.AreEqual(0.001, MyTTester.StandardDeviation);

            }


            [Test]
            public void RunTTest_WhenSampleSizeIsBelowThirty_SetsTTestResultsAreValidToFalse()
            {
                // Arrange. 
                var MyTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 1)
                    .Select(x => 0.1)
                    .ToList();

                // Act.
                MyTTester.RunTTest(listOfErrors);

                // Assert.
                Assert.IsFalse(MyTTester.TTestResultsAreValid);

            }


            [Test]
            public void RunTTest_WhenSampleIsAnEmptyList_SetsTTestResultsAreValidToFalse()
            {
                // Arrange. 
                var MyTTester = new TTester();

                var listOfErrors = new List<double>();

                // Act.
                MyTTester.RunTTest(listOfErrors);

                // Assert.
                Assert.IsFalse(MyTTester.TTestResultsAreValid);

            }

        }

        public class GetMeanTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void GetMean_WhenCalled_ReturnsCorrectMeanValue()
            {
                // Arrange. 
                var MyTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 100)
                    .Select(x => (double)x)
                    .ToList();

                var expected = 50.5;

                // Act.
                var result = MyTTester.GetMean(listOfErrors);
                

                // Assert.
                Assert.AreEqual(result, expected);

            }
        }


        public class GetStandardDeviationTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void GetStandardDeviation_WhenCalled_ReturnsCorrectValue()
            {
                // Arrange. 
                var MyTTester = new TTester();

                double sumOfSquares = 82.5;
                int sampleSize = 10;

                // Act.
                var result = MyTTester.GetStandardDeviation(sumOfSquares, sampleSize);
                var expected = 3.03;

                // Assert.
                Assert.AreEqual(expected, result, 0.1);

            }
        }


        public class GetSumOfSquaresTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void GetSumOfSquares_WhenCalled_ReturnsCorrectValue()
            {
                // Arrange. 
                var MyTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 10)
                    .Select(x => (double)x)
                    .ToList();

                double meanError = 5.5;

                // Act.
                var result = MyTTester.GetSumOfSquares(listOfErrors, meanError);
                var expected = 82.5;

                // Assert.
                Assert.AreEqual(result, expected);

            }
        }


        public class GetTTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void GetT_WhenCalled_ReturnsCorrectValue()
            {
                // Arrange. 
                var MyTTester = new TTester();

                double standardDeviation = 1.3,
                       mean = 0.12;

                int sampleSize = 180;

                // Act.
                var result = MyTTester.GetT(0, mean, standardDeviation, sampleSize);
                var expected = 1.23843765;

                // Assert.
                Assert.AreEqual(result, expected, 0.1);

            }
        }


        public class GetPTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void GetP_WhenCalled_ReturnsCorrectValue()
            {
                // Arrange. 
                var MyTTester = new TTester();

                double t = 1.23843765;
                int sampleSize = 180;

                // Act.
                var result = MyTTester.GetP(t, sampleSize);
                var expected = 0.10858775;

                //Assert.
                Assert.AreEqual(expected, result, 0.1);

            }
        }


        public class GetCohensDTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void GetCohensD_WhenCalled_ReturnsFractionBetweenMeanAndStandardDeviation()
            {
                // Arrange. 
                var MyTTester = new TTester();
                var mean = 0.3;
                var standardDeviation = 0.15;

                var expectedReturnValue = mean / standardDeviation;

                // Act.
                var result = MyTTester.GetCohensD(mean, 0, standardDeviation);

                // Assert.
                Assert.AreEqual(expectedReturnValue, result);

            }
        }
    }
}
