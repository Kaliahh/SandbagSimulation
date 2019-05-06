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
    public class DikeErrorFinderTests
    {
        #region GetListsOfErrorsTests

        public class GetListsOfErrorsTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }



            [Test]
            public void GetListsOfErrors_WhenCalled_RotationalErrorListIsCreated()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                var gameobjects = Enumerable.Range(1, 100)
                  .Select(x => new GameObject() { tag = (x % 2 == 0) ? "PlacedSandbag" : "Drone" })
                  .ToList();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(10 * x, 10 * x, 10 * x)).ToList();
                var blueprint = new Blueprint(constructionNodes, 10);

                // Act.
                MyDikeErrorFinder.GetListsOfErrors(blueprint, Vector3.zero);

                // Assert.
                Assert.AreEqual(50, MyDikeErrorFinder.RotationalErrorsList.Count);
            }


            [Test]
            public void GetListsOfErrors_WhenCalled_PositionalErrorListIsCreated()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                var gameobjects = Enumerable.Range(1, 100)
                  .Select(x => new GameObject() { tag = (x % 2 == 0) ? "PlacedSandbag" : "Drone" })
                  .ToList();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(10 * x, 10 * x, 10 * x)).ToList();
                var blueprint = new Blueprint(constructionNodes, 10);

                // Act.
                MyDikeErrorFinder.GetListsOfErrors(blueprint, Vector3.zero);

                // Assert.
                Assert.AreEqual(285, MyDikeErrorFinder.PositionalErrorsList.Count);
            }
        }



        public class GetAdjustedErrorListTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }



            [Test]
            public void GetAdjustedErrorList_WhenCalled_ResultListHasSameNumberOfElements()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                List<double> listOfErrors = Enumerable.Range(1, 100)
                    .Select(x => (double)x)
                    .ToList();

                var errorToleranceDelta = 0.05;

                // Act.
                var result = MyDikeErrorFinder.GetAdjustedErrorList(listOfErrors, errorToleranceDelta);


                // Assert.
                Assert.AreEqual(result.Count, 100);
            }


            [Test]
            public void GetAdjustedErrorList_WhenElementIsWithinDelta_ElementEqualsZero()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                List<double> listOfErrors = Enumerable.Range(1, 100)
                    .Select(x => 0.01)
                    .ToList();

                var errorToleranceDelta = 0.05;

                // Act.
                var result = MyDikeErrorFinder.GetAdjustedErrorList(listOfErrors, errorToleranceDelta);

                // Assert.
                Assert.AreEqual(0, result.First());
            }


            [Test]
            public void GetAdjustedErrorList_WhenElementIsAboveDelta_ElementEqualsErrorBeyondDelta()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                List<double> listOfErrors = Enumerable.Range(1, 100)
                    .Select(x => (double)x)
                    .ToList();

                var errorToleranceDelta = 0.05;

                // Act.
                var result = MyDikeErrorFinder.GetAdjustedErrorList(listOfErrors, errorToleranceDelta);


                // Assert.
                Assert.AreEqual(0.95, result.First());
            }
        }

        #endregion

        #region GetRotationErrorsListTests
        public class GetRotationalErrorsListTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void GetRotationalErrorsList_WhenCalled_EveryElementInDikeHasCorrespondingElementInListOfErrors()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                MyDikeErrorFinder.BuiltDikeFinder.Dike = Enumerable.Range(0, 100)
                                  .Select(x => new SandbagData(new Vector3(x, 0, 0), new Vector3(0, x, 0), new Vector3(0, 0, x)))
                                  .ToList();

                MyDikeErrorFinder.OptimalDikeFinder.OptimalRotation = new SandbagData(Vector3.right, Vector3.up, Vector3.forward);
                var sampleExpectedElement = MyDikeErrorFinder.RotationalErrorHandler(MyDikeErrorFinder.BuiltDikeFinder.Dike[42]);

                // Act.
                var resultList = MyDikeErrorFinder.GetRotationalErrorsList();


                // Assert.
                Assert.AreEqual(100, resultList.Count);
            }


            [Test]
            public void GetRotationalErrorsList_WhenCalled_ResultListHasCorrectElements()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                MyDikeErrorFinder.BuiltDikeFinder.Dike = Enumerable.Range(0, 100)
                                  .Select(x => new SandbagData(new Vector3(x, 0, 0), new Vector3(0, x, 0), new Vector3(0, 0, x)))
                                  .ToList();

                MyDikeErrorFinder.OptimalDikeFinder.OptimalRotation = new SandbagData(Vector3.right, Vector3.up, Vector3.forward);
                var sampleExpectedElement = MyDikeErrorFinder.RotationalErrorHandler(MyDikeErrorFinder.BuiltDikeFinder.Dike[42]);

                // Act.
                var resultList = MyDikeErrorFinder.GetRotationalErrorsList();

                // Assert.
                Assert.AreEqual(sampleExpectedElement, resultList[42]);
            }
        }


        public class RotationalErrorHandlerTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void RotationalErrorHandler_WhenCalled_ReturnsSumOfAxisErrors()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                MyDikeErrorFinder.OptimalDikeFinder.OptimalRotation = new SandbagData(new Vector3(3, 4, 5), new Vector3(3, 4, 5), new Vector3(3, 4, 5));

                var dataPoint = new SandbagData(new Vector3(-5, -4, -3), new Vector3(-3, -4, -5), new Vector3(5, 4, 3));
                var expected = (180.0 - 156.92) + 0 + 23.07;

                // Act.
                var result = MyDikeErrorFinder.RotationalErrorHandler(dataPoint);


                // Assert.
                Assert.AreEqual(result, expected, 0.1);
            }
        }


        public class GetAxisErrorTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void GetAxisError_WhenAnglesAreStraight_ReturnsZero()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                var vectorA = new Vector3(3, 4, 5);
                var vectorB = new Vector3(-3, -4, -5);


                // Act.
                var result = MyDikeErrorFinder.GetAxisError(vectorA, vectorB);
                var expected = 0;

                // Assert.
                Assert.AreEqual(result, expected);
            }


            [Test]
            public void GetAxisError_WhenAngleIsOrthogonal_Returns90()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                var vectorA = new Vector3(3, 0, 5);
                var vectorB = new Vector3(0, 4, 0);


                // Act.
                var result = MyDikeErrorFinder.GetAxisError(vectorA, vectorB);
                var expected = 90.0;

                // Assert.
                Assert.AreEqual(result, expected);
            }


            [Test]
            public void GetAxisError_WhenAngleIsAreIdentical_ReturnsZero()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                var vectorA = new Vector3(3, 4, 5);
                var vectorB = vectorA;


                // Act.
                var result = MyDikeErrorFinder.GetAxisError(vectorA, vectorB);
                var expected = 0;

                // Assert.
                Assert.AreEqual(result, expected);
            }


            [Test]
            public void GetAxisError_WhenAngleIsAcute_ReturnsAcuteAngle()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                var vectorA = new Vector3(3, 4, 5);
                var vectorB = new Vector3(5, 4, 3);


                // Act.
                var result = MyDikeErrorFinder.GetAxisError(vectorA, vectorB);
                var expected = 23.07;

                // Assert.
                Assert.AreEqual(result, expected, 0.1);
            }


            [Test]
            public void GetAxisError_WhenAngleIsObtuse_ReturnsSupplementaryAngle()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                var vectorA = new Vector3(3, 4, 5);
                var vectorB = new Vector3(-5, -4, -3);


                // Act.
                var result = MyDikeErrorFinder.GetAxisError(vectorA, vectorB);
                var expected = 180.0 - 156.92;

                // Assert.
                Assert.AreEqual(result, expected, 0.1);
            }
        }
        #endregion

        #region GetPositionalErrorsListTests

        public class GetPositionalErrorsListTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }



            [Test]
            public void GetPositionalErrorsList_WhenCalled_ResultListIsTheAppendedResultOfItsComponentMethods()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                MyDikeErrorFinder.BuiltDikeFinder.Dike = Enumerable.Range(0, 90)
                                  .Select(x => new SandbagData(new Vector3(x, 0, 0)))
                                  .ToList();

                MyDikeErrorFinder.OptimalDikeFinder.OptimalPositions = Enumerable.Range(0, 100)
                                  .Select(x => new Vector3(x, 0, 0))
                                  .ToList();

                MyDikeErrorFinder.PickupPoint = Vector3.zero;


                var MyOtherDikeErrorFinder = new DikeErrorFinder();

                MyOtherDikeErrorFinder.BuiltDikeFinder.Dike = Enumerable.Range(0, 90)
                                  .Select(x => new SandbagData(new Vector3(x, 0, 0)))
                                  .ToList();

                MyOtherDikeErrorFinder.OptimalDikeFinder.OptimalPositions = Enumerable.Range(0, 100)
                                  .Select(x => new Vector3(x, 0, 0))
                                  .ToList();

                MyOtherDikeErrorFinder.PickupPoint = Vector3.zero;

                List<double> expectedresultList = MyOtherDikeErrorFinder.GetErrorsForSandbagsCorrespondingToOptimalPositions();
                MyOtherDikeErrorFinder.HandleSuperfluousSandbags(expectedresultList);


                // Act.
                var actualResultList = MyDikeErrorFinder.GetPositionalErrorsList();


                // Assert.
                Assert.IsTrue(actualResultList.SequenceEqual(expectedresultList));
            }
        }


        public class GetErrorsForSandbagsCorrespondingToOptimalPositionsTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }



            [Test]
            public void GetErrorsForSandbagsCorrespondingToOptimalPositions_WhenCalled_EveryElementInOptimalPositionsHasCorrespondingElementInReturnedList()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                MyDikeErrorFinder.BuiltDikeFinder.Dike = Enumerable.Range(0, 90)
                                  .Select(x => new SandbagData(new Vector3(x, 0, 0)))
                                  .ToList();

                MyDikeErrorFinder.OptimalDikeFinder.OptimalPositions = Enumerable.Range(0, 100)
                                  .Select(x => new Vector3(x, 0, 0))
                                  .ToList();

                MyDikeErrorFinder.PickupPoint = Vector3.zero;

                var sampleExpectedElement = Vector3.Distance(MyDikeErrorFinder.BuiltDikeFinder.Dike[42].Position, 
                                                             MyDikeErrorFinder.OptimalDikeFinder.OptimalPositions[42]);

                // Act.
                var resultList = MyDikeErrorFinder.GetErrorsForSandbagsCorrespondingToOptimalPositions();


                // Assert.
                Assert.AreEqual(100, resultList.Count);
            }



            [Test]
            public void GetErrorsForSandbagsCorrespondingToOptimalPositions_WhenCalled_ResultListHasCorrectElements()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                MyDikeErrorFinder.BuiltDikeFinder.Dike = Enumerable.Range(0, 90)
                                  .Select(x => new SandbagData(new Vector3(x, 0, 0)))
                                  .ToList();

                MyDikeErrorFinder.OptimalDikeFinder.OptimalPositions = Enumerable.Range(0, 100)
                                  .Select(x => new Vector3(x, 0, 0))
                                  .ToList();

                MyDikeErrorFinder.PickupPoint = Vector3.zero;

                var sampleExpectedElement = Vector3.Distance(MyDikeErrorFinder.BuiltDikeFinder.Dike[42].Position,
                                                             MyDikeErrorFinder.OptimalDikeFinder.OptimalPositions[42]);

                // Act.
                var resultList = MyDikeErrorFinder.GetErrorsForSandbagsCorrespondingToOptimalPositions();


                // Assert.
                Assert.AreEqual(sampleExpectedElement, resultList[42]);
            }


            [Test]
            public void GetErrorsForSandbagsCorrespondingToOptimalPositions_WhenDikeSmallerThanOptimal_EmptiesDikeList()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                MyDikeErrorFinder.BuiltDikeFinder.Dike = Enumerable.Range(0, 90)
                                  .Select(x => new SandbagData(new Vector3(x, 0, 0)))
                                  .ToList();

                MyDikeErrorFinder.OptimalDikeFinder.OptimalPositions = Enumerable.Range(0, 100)
                                  .Select(x => new Vector3(x, 0, 0))
                                  .ToList();

                MyDikeErrorFinder.PickupPoint = Vector3.zero;

                // Act.
                var resultList = MyDikeErrorFinder.GetErrorsForSandbagsCorrespondingToOptimalPositions();


                // Assert.
                Assert.IsFalse(MyDikeErrorFinder.BuiltDikeFinder.Dike.Any());
            }


            [Test]
            public void GetErrorsForSandbagsCorrespondingToOptimalPositions_WhenDikeEquallySizedToOptimal_EmptiesDikeList()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                MyDikeErrorFinder.BuiltDikeFinder.Dike = Enumerable.Range(0, 100)
                                  .Select(x => new SandbagData(new Vector3(x, 0, 0)))
                                  .ToList();

                MyDikeErrorFinder.OptimalDikeFinder.OptimalPositions = Enumerable.Range(0, 100)
                                  .Select(x => new Vector3(x, 0, 0))
                                  .ToList();

                MyDikeErrorFinder.PickupPoint = Vector3.zero;

                // Act.
                var resultList = MyDikeErrorFinder.GetErrorsForSandbagsCorrespondingToOptimalPositions();


                // Assert.
                Assert.IsFalse(MyDikeErrorFinder.BuiltDikeFinder.Dike.Any());
            }


            [Test]
            public void GetErrorsForSandbagsCorrespondingToOptimalPositions_WhenDikeLargerThanOptimal_LeavesRemainder()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                MyDikeErrorFinder.BuiltDikeFinder.Dike = Enumerable.Range(0, 100)
                                  .Select(x => new SandbagData(new Vector3(x, 0, 0)))
                                  .ToList();

                MyDikeErrorFinder.OptimalDikeFinder.OptimalPositions = Enumerable.Range(0, 90)
                                  .Select(x => new Vector3(x, 0, 0))
                                  .ToList();

                MyDikeErrorFinder.PickupPoint = Vector3.zero;

                // Act.
                var resultList = MyDikeErrorFinder.GetErrorsForSandbagsCorrespondingToOptimalPositions();


                // Assert.
                Assert.AreEqual(10, MyDikeErrorFinder.BuiltDikeFinder.Dike.Count);
            }
        }


        public class HandleSuperfluousSandbagsTests
                {
                    [SetUp]
                    public void ResetScene()
                    {
                        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
                    }


                    [Test]
                    public void HandleSuperfluousSandbags_WhenDikeListIsEmpty_AddsNoneToPositionalErrorsList()
                    {
                        // Arrange. 
                        var MyDikeErrorFinder = new DikeErrorFinder();

                        MyDikeErrorFinder.BuiltDikeFinder.Dike = new List<SandbagData>();
                        MyDikeErrorFinder.PickupPoint = Vector3.zero;
                        var positionalErrorsList = new List<double>();


                        // Act.
                        MyDikeErrorFinder.HandleSuperfluousSandbags(positionalErrorsList);


                        // Assert.
                        Assert.IsFalse(positionalErrorsList.Any());
                    }

                    [Test]
                    public void HandleSuperfluousSandbags_WhenDikeListIsNonEmpty_AddsElementsToPositionalErrorsList()
                    {
                        // Arrange. 
                        var MyDikeErrorFinder = new DikeErrorFinder();

                        MyDikeErrorFinder.BuiltDikeFinder.Dike = new List<SandbagData>();
                        var dikeElement = new SandbagData(Vector3.up);
                        MyDikeErrorFinder.BuiltDikeFinder.Dike.Add(dikeElement);

                        MyDikeErrorFinder.PickupPoint = Vector3.zero;

                        var positionalErrorsList = new List<double>();


                        // Act.
                        MyDikeErrorFinder.HandleSuperfluousSandbags(positionalErrorsList);


                        // Assert.
                        Assert.AreEqual(positionalErrorsList.Count, 1);
                    }


                    [Test]
                    public void HandleSuperfluousSandbags_WhenDikeListIsNonEmpty_AddsCorrectErrorValueToPositionalErrorsList()
                    {
                        // Arrange. 
                        var MyDikeErrorFinder = new DikeErrorFinder();

                        MyDikeErrorFinder.BuiltDikeFinder.Dike = new List<SandbagData>();
                        var dikeElement = new SandbagData(Vector3.up);
                        MyDikeErrorFinder.BuiltDikeFinder.Dike.Add(dikeElement);

                        MyDikeErrorFinder.PickupPoint = Vector3.zero;

                        var expectedErrorElement = (double)Vector3.Distance(dikeElement.Position, MyDikeErrorFinder.PickupPoint);

                        var positionalErrorsList = new List<double>();


                        // Act.
                        MyDikeErrorFinder.HandleSuperfluousSandbags(positionalErrorsList);


                        // Assert.
                        Assert.AreEqual(expectedErrorElement, positionalErrorsList.First());
                    }

                }


        public class PositionalErrorHandlerTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void PositionalErrorHandler_WhenDikeIsEmpty_ReturnsDistanceFromPickupPoint()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                MyDikeErrorFinder.BuiltDikeFinder.Dike = new List<SandbagData>();
                MyDikeErrorFinder.PickupPoint = Vector3.zero;

                var optimalPositionInput = new Vector3(42, 0, 0);
                var expected = Vector3.Distance(optimalPositionInput, MyDikeErrorFinder.PickupPoint);

                // Act.
                var result = MyDikeErrorFinder.PositionalErrorHandler(optimalPositionInput);


                // Assert.
                Assert.AreEqual(expected, result);

            }


            [Test]
            public void PositionalErrorHandler_WhenDikeIsNotEmpty_ReturnsDistanceFromClosestMatch()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                MyDikeErrorFinder.BuiltDikeFinder.Dike = Enumerable.Range(0, 100)
                                  .Select(x => new SandbagData(new Vector3(x, 0, 0)))
                                  .ToList();

                MyDikeErrorFinder.PickupPoint = Vector3.zero;

                var optimalPositionInput = new Vector3(42.1f, 0, 0);
                var expected = Vector3.Distance(optimalPositionInput, MyDikeErrorFinder.BuiltDikeFinder.Dike[42].Position);

                // Act.
                var result = MyDikeErrorFinder.PositionalErrorHandler(optimalPositionInput);


                // Assert.
                Assert.AreEqual(expected, result);

            }


            [Test]
            public void PositionalErrorHandler_WhenDikeIsNotEmpty_EliminatesClosestMatchFromDike()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                MyDikeErrorFinder.BuiltDikeFinder.Dike = Enumerable.Range(0, 100)
                                  .Select(x => new SandbagData(new Vector3(x, 0, 0)))
                                  .ToList();

                MyDikeErrorFinder.PickupPoint = Vector3.zero;

                var optimalPositionInput = new Vector3(42.1f, 0, 0);
                var closestMatch = MyDikeErrorFinder.BuiltDikeFinder.Dike[42];

                // Act.
                var result = MyDikeErrorFinder.PositionalErrorHandler(optimalPositionInput);


                // Assert.
                Assert.IsFalse(MyDikeErrorFinder.BuiltDikeFinder.Dike.Contains(closestMatch));

            }
        }


        public class GetDistanceFromClosestMatchTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void GetDistanceFromClosestMatch_WhenCalled_AssignsCorrectIndexToOutParameter()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                MyDikeErrorFinder.BuiltDikeFinder.Dike = Enumerable.Range(0, 100)
                                  .Select(x => new SandbagData(new Vector3(x, 0, 0)))
                                  .ToList();


                // Act.
                var resultDistance = MyDikeErrorFinder.GetDistanceFromClosestMatch(new Vector3(42, 0, 0), out int resultIndex);


                // Assert.
                Assert.AreEqual(42, resultIndex);

            }

            [Test]
            public void GetDistanceFromClosestMatch_WhenCalled_ReturnsCorrectDistance()
            {
                // Arrange. 
                var MyDikeErrorFinder = new DikeErrorFinder();

                MyDikeErrorFinder.BuiltDikeFinder.Dike = Enumerable.Range(0, 100)
                                  .Select(x => new SandbagData(new Vector3(x, 0, 0)))
                                  .ToList();


                // Act.
                var resultDistance = MyDikeErrorFinder.GetDistanceFromClosestMatch(new Vector3(42, 0, 0), out int resultIndex);


                // Assert.
                Assert.AreEqual(0, resultDistance);

            }
        }

        #endregion

    }
}
