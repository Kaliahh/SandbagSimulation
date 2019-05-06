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
    public class DikeOptimizerTests
    {

        #region FindOptimalDikeTests

        public class FindOptimalDikeTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void FindOptimalDike_WhenCalled_EvaluatedBlueprintIsSet()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(10 * x, 10 * x, 10 * x)).ToList();
                var blueprint = new Blueprint(constructionNodes, 10);

                // Act.
                MyDikeOptimizer.FindOptimalDike(blueprint);


                // Assert.
                Assert.IsTrue(MyDikeOptimizer.EvaluatedBlueprint != null);

            }


            [Test]
            public void FindOptimalDike_WhenCalled_OptimalRotationIsSet()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(10 * x, 10 * x, 10 * x)).ToList();
                var blueprint = new Blueprint(constructionNodes, 10);

                // Act.
                MyDikeOptimizer.FindOptimalDike(blueprint);


                // Assert.
                Assert.IsTrue(MyDikeOptimizer.OptimalRotation != null);

            }


            [Test]
            public void FindOptimalDike_WhenCalled_OptimalPositionsIsSet()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(10 * x, 10 * x, 10 * x)).ToList();
                var blueprint = new Blueprint(constructionNodes, 10);

                // Act.
                MyDikeOptimizer.FindOptimalDike(blueprint);

                // Assert.
                Assert.IsTrue(MyDikeOptimizer.OptimalPositions != null);

            }

        }



        #endregion

        #region FindOptimalRotationTests

        public class FindOptimalRotationTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void FindOptimalRotation_WhenCalled_OptimalRotationRightIsSetCorrectly()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(x, x, x)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 1);
                var expectedresultVector = new Vector3(1, 1, 1);

                // Act.
                MyDikeOptimizer.FindOptimalRotation();


                // Assert.
                Assert.IsTrue(MyDikeOptimizer.OptimalRotation.Right.Equals(expectedresultVector));

            }


            [Test]
            public void FindOptimalRotation_WhenCalled_OptimalRotationUpIsSetCorrectly()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(x, x, x)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 1);

                var expectedresultVector = new Vector3(0, 1, 0);

                // Act.
                MyDikeOptimizer.FindOptimalRotation();


                // Assert.
                Assert.IsTrue(MyDikeOptimizer.OptimalRotation.Up.Equals(expectedresultVector));

            }


            [Test]
            public void FindOptimalRotation_WhenCalled_OptimalRotationForwardIsSetCorrectly()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(x, x, x)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 1);

                // Act.
                MyDikeOptimizer.FindOptimalRotation();

                var expectedresultVector = Vector3.Cross(MyDikeOptimizer.OptimalRotation.Right, MyDikeOptimizer.OptimalRotation.Up);


                // Assert.
                Assert.IsTrue(MyDikeOptimizer.OptimalRotation.Forward.Equals(expectedresultVector));

            }

        }

        #endregion

        #region FindOptimalPositionsTests

        public class FindOptimalPositionsTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }



            [Test]
            public void FindOptimalPositions_WhenMoreThanOneLayerIsNeeded_AssignsAppendedListToField()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(x * 10, x * 10, x * 10)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 4);

                var firstLayer = MyDikeOptimizer.GetFirstOptimalLayer();
                var remainingLayers = MyDikeOptimizer.GetAllRemainingLayers(firstLayer);

                var expectedResultList = firstLayer.Concat(remainingLayers).ToList();

                // Act.
                MyDikeOptimizer.FindOptimalPositions();

                // Assert.
                Assert.IsTrue(expectedResultList.SequenceEqual(MyDikeOptimizer.OptimalPositions));
            }


            [Test]
            public void FindOptimalPositions_WhenOnlyOneLayerIsNeeded_ReturnsOnlyOneLayerList()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(x * 10, x * 10, x * 10)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 1);

                var expectedResultList = MyDikeOptimizer.GetFirstOptimalLayer();

                // Act.
                MyDikeOptimizer.FindOptimalPositions();

                // Assert.
                Assert.IsTrue(expectedResultList.SequenceEqual(MyDikeOptimizer.OptimalPositions));

            }


            [Test]
            public void FindOptimalPositions_WhenNoLayersAreNeeded_ReturnsEmptyList()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(x * 10, x * 10, x * 10)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 0);

                // Act.
                MyDikeOptimizer.FindOptimalPositions();

                // Assert.
                Assert.IsFalse(MyDikeOptimizer.OptimalPositions.Any());

            }

        }

        public class GetFirstOptimalLayerTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void GetFirstOptimalLayerTests_WhenCalled_ReturnsAppendedListOfResults()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(x * 10, x * 10, x * 10)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 1);

                var expectedResultList = new List<Vector3>();
                MyDikeOptimizer.GetFirstBagPosition(expectedResultList);
                MyDikeOptimizer.CompleteFirstLayerFromCenter(expectedResultList);

                // Act.
                var actualResultList = MyDikeOptimizer.GetFirstOptimalLayer();

                // Assert.
                Assert.IsTrue(actualResultList.SequenceEqual(expectedResultList));

            }
        }

        public class GetFirstBagPositionTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void GetFirstBagPosition_WhenCalled_ReturnsSomething()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(x, x, x)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 1);

                var firstLayerResult = new List<Vector3>();

                // Act.
                MyDikeOptimizer.GetFirstBagPosition(firstLayerResult);


                // Assert.
                Assert.IsTrue(firstLayerResult.Any());

            }


            [Test]
            public void GetFirstBagPosition_WhenCalled_ReturnVectorIsEquidistantToWaypointsOnTheGround()
            {
                //Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(x, x, x)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 1);


                var firstLayerResult = new List<Vector3>();

                var firstWayPointHeightAdjusted = MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.First();
                firstWayPointHeightAdjusted.y = MyDikeOptimizer.SandbagModel.Height / 2.0f;

                var lastWayPointHeightAdjusted = MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.Last();
                lastWayPointHeightAdjusted.y = MyDikeOptimizer.SandbagModel.Height / 2.0f;


                // Act.
                MyDikeOptimizer.GetFirstBagPosition(firstLayerResult);

                var distanceToFirstWaypoint = Vector3.Distance(firstLayerResult.First(), firstWayPointHeightAdjusted);
                var distanceToLastWaypoint = Vector3.Distance(firstLayerResult.First(), lastWayPointHeightAdjusted);

                // Assert.
                Assert.IsTrue(distanceToFirstWaypoint == distanceToLastWaypoint);

            }


            [Test]
            public void GetFirstBagPosition_WhenCalled_ReturnsVectorWithCorrectHeight()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(x, x, x)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 1);

                var firstLayerResult = new List<Vector3>();
                var expectedYCoordinate = MyDikeOptimizer.SandbagModel.Height / 2.0f;

                // Act.
                MyDikeOptimizer.GetFirstBagPosition(firstLayerResult);


                // Assert.
                Assert.AreEqual(firstLayerResult.First().y, expectedYCoordinate);

            }

        }

        public class CompleteFirstLayerFromCenterTests
        {

            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void CompleteFirstLayerFromCenter_WhenCalled_AddsBothWaypointAndStabilizerSandbagsToList()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(10 * x, 10 * x, 10 * x)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 3);

                var expectedFirstLayerResult = new List<Vector3>();
                MyDikeOptimizer.GetFirstBagPosition(expectedFirstLayerResult);

                Vector3 firstDirection = MyDikeOptimizer.GetBuildingDirection(expectedFirstLayerResult.First(), MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.First()),
                        secondDirection = MyDikeOptimizer.GetBuildingDirection(expectedFirstLayerResult.First(), MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.Last());

                MyDikeOptimizer.AddSandbagsUntilWaypointsAreCovered(expectedFirstLayerResult, firstDirection, secondDirection);

                MyDikeOptimizer.AddNeccesaryStabilizerBagsForUpperLayers(expectedFirstLayerResult, firstDirection, secondDirection);


                var actualFirstLayerResult = new List<Vector3>();
                MyDikeOptimizer.GetFirstBagPosition(actualFirstLayerResult);

                // Act.
                MyDikeOptimizer.CompleteFirstLayerFromCenter(actualFirstLayerResult);

                // Assert.
                Assert.IsTrue(expectedFirstLayerResult.SequenceEqual(actualFirstLayerResult));

            }
        }

        public class GetBuildingDirectionTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void GetBuildingDirection_WhenCalled_ResultDirectionVectorHasTheSameDirectionAsTheOriginalDirectionVector()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var adjustedHeight = MyDikeOptimizer.SandbagModel.Height / 2;

                var firstBagPosition = new Vector3(42, adjustedHeight, 21);
                var constructionNode = new Vector3(24, 43, 12);

                var adjustedConstructionNode = new Vector3(constructionNode.x, adjustedHeight, constructionNode.z);


                // Act.
                var result = MyDikeOptimizer.GetBuildingDirection(firstBagPosition, constructionNode);

                var angle = Vector3.Angle((adjustedConstructionNode - firstBagPosition), (result - firstBagPosition));


                // Assert.
                Assert.AreEqual(0, angle);

            }


            [Test]
            public void GetBuildingDirection_WhenCalled_ResultDirectionVectorFormsAStraightAngleWithTheDirectionFromWaypointToFirstSandbag()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var adjustedHeight = MyDikeOptimizer.SandbagModel.Height / 2;

                var firstBagPosition = new Vector3(42, adjustedHeight, 21);
                var constructionNode = new Vector3(24, 43, 12);

                var adjustedConstructionNode = new Vector3(constructionNode.x, adjustedHeight, constructionNode.z);


                // Act.
                var result = MyDikeOptimizer.GetBuildingDirection(firstBagPosition, constructionNode);

                var angle = Vector3.Angle((firstBagPosition - adjustedConstructionNode), (result - adjustedConstructionNode));

                // Assert.
                Assert.AreEqual(180, angle);
            }


            [Test]
            public void GetBuildingDirection_WhenCalled_ResultDirectionVectorIs1024TimesLongerThanOriginalDirectionVector()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var adjustedHeight = MyDikeOptimizer.SandbagModel.Height / 2;

                var firstBagPosition = new Vector3(42, adjustedHeight, 21);
                var constructionNode = new Vector3(24, 43, 12);

                var adjustedConstructionNode = new Vector3(constructionNode.x, adjustedHeight, constructionNode.z);
                var expectedLength = 1024 * Vector3.Distance(adjustedConstructionNode, firstBagPosition);


                // Act.
                var result = MyDikeOptimizer.GetBuildingDirection(firstBagPosition, constructionNode);

                var actualLength = Vector3.Distance(result, firstBagPosition);

                // Assert.
                Assert.AreEqual(expectedLength, actualLength);

            }


            [Test]
            public void GetBuildingDirection_WhenCalled_TargetPointHasSameHeightAsFirstSandbag()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var adjustedHeight = MyDikeOptimizer.SandbagModel.Height / 2;

                var firstBagPosition = new Vector3(42, adjustedHeight, 21);
                var constructionNode = new Vector3(24, 43, 12);


                // Act.
                var result = MyDikeOptimizer.GetBuildingDirection(firstBagPosition, constructionNode);


                // Assert.
                Assert.AreEqual(firstBagPosition.y, result.y);

            }
        }

        public class AddSandbagsUntilWaypointsAreCoveredTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void AddSandbagsUntilWaypointsAreCovered_WhenCalled_FinalAddedElementsCoverWaypoints()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(x * 10, x * 10, x * 10)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 1);

                var firstLayerResult = new List<Vector3>();
                MyDikeOptimizer.GetFirstBagPosition(firstLayerResult);

                Vector3 firstDirection = MyDikeOptimizer.GetBuildingDirection(firstLayerResult.First(), MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.First()),
                        secondDirection = MyDikeOptimizer.GetBuildingDirection(firstLayerResult.First(), MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.Last());

                var endpointForFirstDirection = MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.First();
                endpointForFirstDirection.y = MyDikeOptimizer.SandbagModel.Height / 2.0f;

                var endpointForSecondDirection = MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.Last();
                endpointForSecondDirection.y = MyDikeOptimizer.SandbagModel.Height / 2.0f;

                float HalfASandbagLength = MyDikeOptimizer.SandbagModel.Length / 2.0f;

                // Act.
                MyDikeOptimizer.AddSandbagsUntilWaypointsAreCovered(firstLayerResult, firstDirection, secondDirection);


                var distanceToWaypointForFinalElementInFirstDirection =
                    Vector3.Distance(firstLayerResult[firstLayerResult.Count - 2], endpointForFirstDirection);

                var distanceToWaypointForFinalElementInSecondDirection =
                    Vector3.Distance(firstLayerResult[firstLayerResult.Count - 1], endpointForSecondDirection);

                bool terminatesWhenWaypointsAreCovered = distanceToWaypointForFinalElementInFirstDirection <= HalfASandbagLength
                                                         && distanceToWaypointForFinalElementInSecondDirection <= HalfASandbagLength;


                // Assert.
                Assert.IsTrue(terminatesWhenWaypointsAreCovered);

            }



            [Test]
            public void AddSandbagsUntilWaypointsAreCovered_WhenCalled_PenultimateElementsDontCoverWaypoints()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();


                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(x * 10, x * 10, x * 10)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 1);

                var firstLayerResult = new List<Vector3>();
                MyDikeOptimizer.GetFirstBagPosition(firstLayerResult);

                Vector3 firstDirection = MyDikeOptimizer.GetBuildingDirection(firstLayerResult.First(), MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.First()),
                    secondDirection = MyDikeOptimizer.GetBuildingDirection(firstLayerResult.First(), MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.Last());

                var endpointForFirstDirection = MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.First();
                endpointForFirstDirection.y = MyDikeOptimizer.SandbagModel.Height / 2.0f;

                var endpointForSecondDirection = MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.Last();
                endpointForSecondDirection.y = MyDikeOptimizer.SandbagModel.Height / 2.0f;

                float HalfASandbagLength = MyDikeOptimizer.SandbagModel.Length / 2.0f;

                // Act.
                MyDikeOptimizer.CompleteFirstLayerFromCenter(firstLayerResult);

                var distanceToWaypointForPenultimateElementInFirstDirection =
                        Vector3.Distance(firstLayerResult[firstLayerResult.Count - 4], endpointForFirstDirection);

                var distanceToWaypointForPenultimateElementInSecondDirection =
                        Vector3.Distance(firstLayerResult[firstLayerResult.Count - 3], endpointForSecondDirection);

                bool penultimateSandbagsDontCoverWaypoints = distanceToWaypointForPenultimateElementInFirstDirection > HalfASandbagLength
                                                             && distanceToWaypointForPenultimateElementInSecondDirection > HalfASandbagLength;

                // Assert.
                Assert.IsTrue(penultimateSandbagsDontCoverWaypoints);

            }
        }

        public class AddNeccesaryStabilizerBagsForUpperLayersTests
        {

            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void AddNeccesaryStabilizerBagsForUpperLayers_WhenCalled_AddsExtraPositionsToFirstLayer()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(10 * x, 10 * x, 12f * x)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 6);

                var firstLayerResult = new List<Vector3>();

                MyDikeOptimizer.GetFirstBagPosition(firstLayerResult);

                Vector3 firstDirection = MyDikeOptimizer.GetBuildingDirection(firstLayerResult.First(),
                                         MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.First()),
                        secondDirection = MyDikeOptimizer.GetBuildingDirection(firstLayerResult.First(),
                                          MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.Last());

                MyDikeOptimizer.AddSandbagsUntilWaypointsAreCovered(firstLayerResult, firstDirection, secondDirection);

                var countBeforeCall = firstLayerResult.Count;

                // Act.
                MyDikeOptimizer.AddNeccesaryStabilizerBagsForUpperLayers(firstLayerResult, firstDirection, secondDirection);

                var numberOfBagsAdded = firstLayerResult.Count - countBeforeCall;

                // Assert.
                Assert.AreEqual(numberOfBagsAdded, 6);

            }
        }

        public class BuildFurtherTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void BuildFurther_WhenOnlyFirstBagInList_AddsTwoElements()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();


                var adjustedHeight = MyDikeOptimizer.SandbagModel.Height / 2;

                var firstBagPosition = new Vector3(0, adjustedHeight, 0);

                var firstLayerResult = new List<Vector3>();
                firstLayerResult.Add(firstBagPosition);


                var firstDirection = new Vector3(-100, adjustedHeight, 100);
                var secondDirection = new Vector3(100, adjustedHeight, -100);


                // Act.
                MyDikeOptimizer.BuildFurther(firstDirection, secondDirection, firstLayerResult);


                // Assert.
                Assert.AreEqual(firstLayerResult.Count, 3);

            }


            [Test]
            public void BuildFurther_WhenNotOnlyFirstBagInList_AddsTwoElements()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();


                var adjustedHeight = MyDikeOptimizer.SandbagModel.Height / 2;

                var firstBagPosition = new Vector3(0, adjustedHeight, 0);

                var firstLayerResult = new List<Vector3>();
                firstLayerResult.Add(firstBagPosition);
                firstLayerResult.Add(firstBagPosition);
                firstLayerResult.Add(firstBagPosition);


                var firstDirection = new Vector3(-100, adjustedHeight, 100);
                var secondDirection = new Vector3(100, adjustedHeight, -100);


                // Act.
                MyDikeOptimizer.BuildFurther(firstDirection, secondDirection, firstLayerResult);


                // Assert.
                Assert.AreEqual(firstLayerResult.Count, 5);

            }


            [Test]
            public void BuildFurther_WhenOnlyFirstBagInList_FirstAddedElementIsOneSandbagLengthCloserToFirstEndpoint()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();


                var adjustedHeight = MyDikeOptimizer.SandbagModel.Height / 2;

                var firstBagPosition = new Vector3(0, adjustedHeight, 0);

                var firstLayerResult = new List<Vector3>();
                firstLayerResult.Add(firstBagPosition);


                var firstDirection = new Vector3(-100, adjustedHeight, 100);
                var secondDirection = new Vector3(100, adjustedHeight, -100);

                var firstBagDistance = Vector3.Distance(firstLayerResult.First(), firstDirection);


                // Act.
                MyDikeOptimizer.BuildFurther(firstDirection, secondDirection, firstLayerResult);

                var firstAddedBagDistance = Vector3.Distance(firstLayerResult[1], firstDirection);

                var distanceMoved = Math.Abs(firstBagDistance - firstAddedBagDistance);

                // Assert.
                Assert.AreEqual(distanceMoved, MyDikeOptimizer.SandbagModel.Length, 0.01);

            }


            [Test]
            public void BuildFurther_WhenOnlyFirstBagInList_SecondAddedElementIsOneSandbagLengthCloserToSecondEndpoint()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var adjustedHeight = MyDikeOptimizer.SandbagModel.Height / 2;

                var firstBagPosition = new Vector3(0, adjustedHeight, 0);

                var firstLayerResult = new List<Vector3>();
                firstLayerResult.Add(firstBagPosition);


                var firstDirection = new Vector3(-100, adjustedHeight, 100);
                var secondDirection = new Vector3(100, adjustedHeight, -100);

                var firstBagDistance = Vector3.Distance(firstLayerResult.First(), secondDirection);


                // Act.
                MyDikeOptimizer.BuildFurther(firstDirection, secondDirection, firstLayerResult);

                var secondAddedBagDistance = Vector3.Distance(firstLayerResult[2], secondDirection);

                var distanceMoved = Math.Abs(firstBagDistance - secondAddedBagDistance);

                // Assert.
                Assert.AreEqual(distanceMoved, MyDikeOptimizer.SandbagModel.Length, 0.01);

            }


            [Test]
            public void BuildFurther_WhenNotOnlyFirstBagInList_FirstAddedElementIsOneSandbagLengthCloserToFirstEndpoint()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var adjustedHeight = MyDikeOptimizer.SandbagModel.Height / 2;

                var firstBagPosition = new Vector3(0, adjustedHeight, 0);

                var firstLayerResult = new List<Vector3>();
                firstLayerResult.Add(firstBagPosition);


                var firstDirection = new Vector3(-100, adjustedHeight, 100);
                var secondDirection = new Vector3(100, adjustedHeight, -100);

                MyDikeOptimizer.BuildFurther(firstDirection, secondDirection, firstLayerResult);

                var anchorPointBagDistance = Vector3.Distance(firstLayerResult[1], firstDirection);


                // Act.
                MyDikeOptimizer.BuildFurther(firstDirection, secondDirection, firstLayerResult);

                var firstAddedBagDistance = Vector3.Distance(firstLayerResult[3], firstDirection);

                var distanceMoved = Math.Abs(anchorPointBagDistance - firstAddedBagDistance);


                // Assert.
                Assert.AreEqual(distanceMoved, MyDikeOptimizer.SandbagModel.Length, 0.01);

            }


            [Test]
            public void BuildFurther_WhenNotOnlyFirstBagInList_SecondAddedElementIsOneSandbagLengthCloserToSecondEndpoint()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();


                var adjustedHeight = MyDikeOptimizer.SandbagModel.Height / 2;

                var firstBagPosition = new Vector3(0, adjustedHeight, 0);

                var firstLayerResult = new List<Vector3>();
                firstLayerResult.Add(firstBagPosition);


                var firstDirection = new Vector3(-100, adjustedHeight, 100);
                var secondDirection = new Vector3(100, adjustedHeight, -100);

                MyDikeOptimizer.BuildFurther(firstDirection, secondDirection, firstLayerResult);

                var anchorPointBagDistance = Vector3.Distance(firstLayerResult[2], firstDirection);


                // Act.
                MyDikeOptimizer.BuildFurther(firstDirection, secondDirection, firstLayerResult);

                var secondAddedBagDistance = Vector3.Distance(firstLayerResult[4], firstDirection);

                var distanceMoved = Math.Abs(anchorPointBagDistance - secondAddedBagDistance);


                // Assert.
                Assert.AreEqual(distanceMoved, MyDikeOptimizer.SandbagModel.Length, 0.01);

            }


            [Test]
            public void BuildFurther_WhenOnlyFirstBagInList_HeightOfAddedElementsDontChange()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var adjustedHeight = MyDikeOptimizer.SandbagModel.Height / 2;

                var firstBagPosition = new Vector3(0, adjustedHeight, 0);

                var firstLayerResult = new List<Vector3>();
                firstLayerResult.Add(firstBagPosition);


                var firstDirection = new Vector3(-100, adjustedHeight, 100);
                var secondDirection = new Vector3(100, adjustedHeight, -100);


                // Act.
                MyDikeOptimizer.BuildFurther(firstDirection, secondDirection, firstLayerResult);


                // Assert.
                Assert.IsTrue(firstLayerResult[1].y == adjustedHeight && firstLayerResult[2].y == adjustedHeight);

            }


            [Test]
            public void BuildFurther_WhenNotOnlyFirstBagInList_HeightOfAddedElementsDontChange()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var adjustedHeight = MyDikeOptimizer.SandbagModel.Height / 2;

                var firstBagPosition = new Vector3(0, adjustedHeight, 0);

                var firstLayerResult = new List<Vector3>();
                firstLayerResult.Add(firstBagPosition);


                var firstDirection = new Vector3(-100, adjustedHeight, 100);
                var secondDirection = new Vector3(100, adjustedHeight, -100);


                // Act.
                MyDikeOptimizer.BuildFurther(firstDirection, secondDirection, firstLayerResult);
                MyDikeOptimizer.BuildFurther(firstDirection, secondDirection, firstLayerResult);

                // Assert.
                Assert.IsTrue(firstLayerResult[3].y == adjustedHeight && firstLayerResult[4].y == adjustedHeight);

            }
        }

        public class NumberOfExtraStabilizingSandbagsRequiredTests
        {

            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void NumberOfExtraStabilizingSandbagsRequired_WhenUnevenNumberOfRemainingLayersAndFinalBagBeyondWaypoint_ReturnsCorrectValue()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(10 * x, 10 * x, 10 * x)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 6);

                var firstLayerResult = new List<Vector3>();

                MyDikeOptimizer.GetFirstBagPosition(firstLayerResult);

                Vector3 firstDirection = MyDikeOptimizer.GetBuildingDirection(firstLayerResult.First(),
                                         MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.First()),
                        secondDirection = MyDikeOptimizer.GetBuildingDirection(firstLayerResult.First(),
                                          MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.Last());

                MyDikeOptimizer.AddSandbagsUntilWaypointsAreCovered(firstLayerResult, firstDirection, secondDirection);

                // Act.
                var result = MyDikeOptimizer.NumberOfExtraStabilizingSandbagsRequired(firstLayerResult);


                // Assert.
                Assert.AreEqual(result, 2);

            }


            [Test]
            public void NumberOfExtraStabilizingSandbagsRequired_WhenEvenNumberOfRemainingLayersAndFinalBagBeyondWaypoint_ReturnsCorrectValue()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(10 * x, 10 * x, 10 * x)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 5);

                var firstLayerResult = new List<Vector3>();

                MyDikeOptimizer.GetFirstBagPosition(firstLayerResult);

                Vector3 firstDirection = MyDikeOptimizer.GetBuildingDirection(firstLayerResult.First(),
                                         MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.First()),
                        secondDirection = MyDikeOptimizer.GetBuildingDirection(firstLayerResult.First(),
                                          MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.Last());

                MyDikeOptimizer.AddSandbagsUntilWaypointsAreCovered(firstLayerResult, firstDirection, secondDirection);

                // Act.
                var result = MyDikeOptimizer.NumberOfExtraStabilizingSandbagsRequired(firstLayerResult);


                // Assert.
                Assert.AreEqual(result, 2);

            }


            [Test]
            public void NumberOfExtraStabilizingSandbagsRequired_WhenUnevenNumberOfRemainingLayersAndFinalBagWithinWaypoint_ReturnsCorrectValue()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(10 * x, 10 * x, 12f * x)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 6);

                var firstLayerResult = new List<Vector3>();

                MyDikeOptimizer.GetFirstBagPosition(firstLayerResult);

                Vector3 firstDirection = MyDikeOptimizer.GetBuildingDirection(firstLayerResult.First(),
                                         MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.First()),
                        secondDirection = MyDikeOptimizer.GetBuildingDirection(firstLayerResult.First(),
                                          MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.Last());

                MyDikeOptimizer.AddSandbagsUntilWaypointsAreCovered(firstLayerResult, firstDirection, secondDirection);

                // Act.
                var result = MyDikeOptimizer.NumberOfExtraStabilizingSandbagsRequired(firstLayerResult);


                // Assert.
                Assert.AreEqual(result, 3);

            }


            [Test]
            public void NumberOfExtraStabilizingSandbagsRequired_WhenEvenNumberOfRemainingLayersAndFinalBagWithinWaypoint_ReturnsCorrectValue()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(10 * x, 10 * x, 12f * x)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 5);

                var firstLayerResult = new List<Vector3>();

                MyDikeOptimizer.GetFirstBagPosition(firstLayerResult);

                Vector3 firstDirection = MyDikeOptimizer.GetBuildingDirection(firstLayerResult.First(),
                                         MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.First()),
                        secondDirection = MyDikeOptimizer.GetBuildingDirection(firstLayerResult.First(),
                                          MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.Last());

                MyDikeOptimizer.AddSandbagsUntilWaypointsAreCovered(firstLayerResult, firstDirection, secondDirection);

                // Act.
                var result = MyDikeOptimizer.NumberOfExtraStabilizingSandbagsRequired(firstLayerResult);


                // Assert.
                Assert.AreEqual(result, 2);

            }
        }

        public class OffsetNeededDueToFinalBagExtensionBeyondWaypointTests
        {

            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void OffsetNeededDueToFinalBagExtensionBeyondWaypoint_FinalBagPositionLiesBeyondWaypoint_ReturnedOffsetEqualsOne()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(10 * x, 10 * x, 10 * x)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 2);

                var firstLayerResult = new List<Vector3>();

                MyDikeOptimizer.GetFirstBagPosition(firstLayerResult);

                Vector3 firstDirection = MyDikeOptimizer.GetBuildingDirection(firstLayerResult.First(), 
                                         MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.First()),
                        secondDirection = MyDikeOptimizer.GetBuildingDirection(firstLayerResult.First(), 
                                          MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.Last());

                MyDikeOptimizer.AddSandbagsUntilWaypointsAreCovered(firstLayerResult, firstDirection, secondDirection);


                // Act.
                var result = MyDikeOptimizer.OffsetNeededDueToFinalBagExtensionBeyondWaypoint(firstLayerResult);


                // Assert.
                Assert.AreEqual(result, 1);

            }


            [Test]
            public void OffsetNeededDueToFinalBagExtensionBeyondWaypoint_FinalBagPositionLiesWithinWaypoint_ReturnedOffsetEqualsZero()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(10 * x, 10 * x, 12f * x)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 2);

                var firstLayerResult = new List<Vector3>();

                MyDikeOptimizer.GetFirstBagPosition(firstLayerResult);

                Vector3 firstDirection = MyDikeOptimizer.GetBuildingDirection(firstLayerResult.First(),
                                         MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.First()),
                        secondDirection = MyDikeOptimizer.GetBuildingDirection(firstLayerResult.First(),
                                          MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.Last());

                MyDikeOptimizer.AddSandbagsUntilWaypointsAreCovered(firstLayerResult, firstDirection, secondDirection);


                // Act.
                var result = MyDikeOptimizer.OffsetNeededDueToFinalBagExtensionBeyondWaypoint(firstLayerResult);


                // Assert.
                Assert.AreEqual(result, 0);

            }
        }

        public class ProximityToOneOfTheWaypointsTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void ProximityToOneOfTheWaypoints_WhenCalled_ReturnsDistanceBetweenPositionAndWaypoint()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(x * 10, x * 10, x * 10)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 1);

                var position = new Vector3(34, 64, 12);

                var expectedDistance = Vector3.Distance(position, MyDikeOptimizer.EvaluatedBlueprint.ConstructionNodes.First());

                // Act.
                var resultDistance = MyDikeOptimizer.ProximityToOneOfTheWaypoints(position);

                // Assert.
                Assert.AreEqual(resultDistance, expectedDistance);

            }
        }

        public class GetAllRemainingLayersTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void GetAllRemainingLayers_WhenCalled_ReturnListConcatenatesFoundExtraLayers()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();


                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(x * 10, x * 10, x * 10)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 3);

                List<Vector3> firstLayer = MyDikeOptimizer.GetFirstOptimalLayer();
                var sortedFirstLayer = MyDikeOptimizer.GetFirstOptimalLayer().OrderBy(MyDikeOptimizer.ProximityToOneOfTheWaypoints).ToList();
                var secondLayer = MyDikeOptimizer.BuildAnotherLayerOnTop(sortedFirstLayer, 2);
                var thirdLayer = MyDikeOptimizer.BuildAnotherLayerOnTop(secondLayer, 3);
                var expectedResultList = secondLayer.Concat(thirdLayer).ToList();

                // Act.
                var actualResultList = MyDikeOptimizer.GetAllRemainingLayers(firstLayer);

                // Assert.
                Assert.IsTrue(expectedResultList.SequenceEqual(actualResultList));

            }
        }

        public class BuildAnotherLayerOnTopTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void BuildAnotherLayerOnTop_WhenCalled_ReturnListHasOneLessElement()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var layerToBeBuiltUpon = Enumerable.Range(0, 10).Select(x => new Vector3(x * 10, x * 10, x * 10)).ToList();
                var newLayerNumber = 3;


                // Act.
                var resultList = MyDikeOptimizer.BuildAnotherLayerOnTop(layerToBeBuiltUpon, newLayerNumber);

                // Assert.
                Assert.AreEqual(resultList.Count, (layerToBeBuiltUpon.Count - 1));

            }


            [Test]
            public void BuildAnotherLayerOnTop_WhenCalled_ReturnListHasNewAssignedHeight()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var layerToBeBuiltUpon = Enumerable.Range(0, 10).Select(x => new Vector3(x * 10, x * 10, x * 10)).ToList();
                var newLayerNumber = 3;


                var expectedHeight = MyDikeOptimizer.SandbagModel.Height * 2.5f;

                // Act.
                var resultList = MyDikeOptimizer.BuildAnotherLayerOnTop(layerToBeBuiltUpon, newLayerNumber);

                // Assert.
                Assert.AreEqual(resultList.First().y, expectedHeight);

            }


            [Test]
            public void BuildAnotherLayerOnTop_WhenCalledOnFirstLayer_ReturnListElementsAreEquidistantToTheirClosestElementsInTheFirstLayer()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(x * 10, x * 10, x * 10)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 2);


                var firstLayer = MyDikeOptimizer.GetFirstOptimalLayer();
                var layerToBeBuiltUpon = firstLayer.OrderBy(MyDikeOptimizer.ProximityToOneOfTheWaypoints).ToList();
                var newLayerNumber = 3;


                // Act.
                var resultList = MyDikeOptimizer.BuildAnotherLayerOnTop(layerToBeBuiltUpon, newLayerNumber);

                var distanceToFirstClosestElement = Vector3.Distance(resultList.First(), layerToBeBuiltUpon[0]);
                var distanceToOtherClosestElement = Vector3.Distance(resultList.First(), layerToBeBuiltUpon[1]);

                // Assert.
                Assert.AreEqual(distanceToFirstClosestElement, distanceToOtherClosestElement, 0.05);

            }


            [Test]
            public void BuildAnotherLayerOnTop_WhenCalledOnNonFirstLayer_ReturnListElementsAreEquidistantToTheirClosestElementsInTheFirstLayer()
            {
                // Arrange. 
                var MyDikeOptimizer = new DikeOptimizer();

                var constructionNodes = Enumerable.Range(0, 2).Select(x => new Vector3(x * 10, x * 10, x * 10)).ToList();
                MyDikeOptimizer.EvaluatedBlueprint = new Blueprint(constructionNodes, 2);

                var firstLayer = MyDikeOptimizer.GetFirstOptimalLayer().OrderBy(MyDikeOptimizer.ProximityToOneOfTheWaypoints).ToList();
                var secondLayer = MyDikeOptimizer.BuildAnotherLayerOnTop(firstLayer, 2);

                var newLayerNumber = 3;

                // Act.
                var resultList = MyDikeOptimizer.BuildAnotherLayerOnTop(secondLayer, newLayerNumber);

                var distanceToFirstClosestElement = Vector3.Distance(resultList.First(), secondLayer[0]);
                var distanceToOtherClosestElement = Vector3.Distance(resultList.First(), secondLayer[1]);

                // Assert.
                Assert.AreEqual(distanceToFirstClosestElement, distanceToOtherClosestElement, 0.05);

            }
        }

        #endregion
    }
}
