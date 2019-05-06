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
    public class EvaluationReporterTests
    {
        public class GetEvaluationReportTests
        {
            [SetUp]
            public void ResetScene()
            {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            }


            [Test]
            public void GetEvaluationReport_BothTestsAreNonsignificant_ReturnsCorrectEvaluationReport()
            {
                // Arrange. 
                var MyEvaluationReporter = new EvaluationReporter();
                var MyTTester = new TTester();
                var MyOtherTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var otherListOfErrors = new List<double>();
                    otherListOfErrors.AddRange(listOfErrors);


                MyTTester.RunTTest(listOfErrors);
                MyOtherTTester.RunTTest(otherListOfErrors);


                // Act.
                string resultString = MyEvaluationReporter.GetEvaluationReport(MyTTester, MyOtherTTester);


                // Assert.
                //Debug.Log(resultString);
            }


            [Test]
            public void GetEvaluationReport_SampleSizeIsTooSmall_ReturnsCorrectEvaluationReport()
            {
                // Arrange. 
                var MyEvaluationReporter = new EvaluationReporter();
                var MyTTester = new TTester();
                var MyOtherTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 29)
                    .Select(x => 0.0)
                    .ToList();

                var otherListOfErrors = new List<double>();
                otherListOfErrors.AddRange(listOfErrors);


                MyTTester.RunTTest(listOfErrors);
                MyOtherTTester.RunTTest(otherListOfErrors);


                // Act.
                var resultString = MyEvaluationReporter.GetEvaluationReport(MyTTester, MyOtherTTester);


                // Assert.
                //Debug.Log(resultString);
            }


            [Test]
            public void GetEvaluationReport_FirstTestsIsNonsignificantAndSecondHasSmallEffect_ReturnsCorrectEvaluationReport()
            {
                // Arrange. 
                var MyEvaluationReporter = new EvaluationReporter();
                var MyTTester = new TTester();
                var MyOtherTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var otherListOfErrors = new List<double>();
                otherListOfErrors.AddRange(listOfErrors);

                var smallEffectMisses = Enumerable.Range(1, 5)
                    .Select(x => 50.0)
                    .ToList();

                otherListOfErrors.AddRange(smallEffectMisses);

                MyTTester.RunTTest(listOfErrors);
                MyOtherTTester.RunTTest(otherListOfErrors);


                // Act.
                var resultString = MyEvaluationReporter.GetEvaluationReport(MyTTester, MyOtherTTester);


                // Assert.
                //Debug.Log(resultString);
            }


            [Test]
            public void GetEvaluationReport_FirstTestsIsNonsignificantAndSecondHasMediumEffect_ReturnsCorrectEvaluationReport()
            {
                // Arrange. 
                var MyEvaluationReporter = new EvaluationReporter();
                var MyTTester = new TTester();
                var MyOtherTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var otherListOfErrors = new List<double>();
                otherListOfErrors.AddRange(listOfErrors);

                var mediumEffectMisses = Enumerable.Range(1, 15)
                    .Select(x => 50.0)
                    .ToList();

                otherListOfErrors.AddRange(mediumEffectMisses);

                MyTTester.RunTTest(listOfErrors);
                MyOtherTTester.RunTTest(otherListOfErrors);

                // Act.
                var resultString = MyEvaluationReporter.GetEvaluationReport(MyTTester, MyOtherTTester);


                // Assert.
                //Debug.Log(resultString);
            }


            [Test]
            public void GetEvaluationReport_FirstTestsIsNonsignificantAndSecondHasLargeEffect_ReturnsCorrectEvaluationReport()
            {
                // Arrange. 
                var MyEvaluationReporter = new EvaluationReporter();
                var MyTTester = new TTester();
                var MyOtherTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var otherListOfErrors = new List<double>();
                otherListOfErrors.AddRange(listOfErrors);

                var largeEffectMisses = Enumerable.Range(1, 50)
                    .Select(x => 50.0)
                    .ToList();

                otherListOfErrors.AddRange(largeEffectMisses);

                MyTTester.RunTTest(listOfErrors);
                MyOtherTTester.RunTTest(otherListOfErrors);

                // Act.
                var resultString = MyEvaluationReporter.GetEvaluationReport(MyTTester, MyOtherTTester);


                // Assert.
                //Debug.Log(resultString);
            }


            [Test]
            public void GetEvaluationReport_FirstTestsHasSmallEffectAndSecondIsNonsignificant_ReturnsCorrectEvaluationReport()
            {
                // Arrange. 
                var MyEvaluationReporter = new EvaluationReporter();
                var MyTTester = new TTester();
                var MyOtherTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var otherListOfErrors = new List<double>();
                otherListOfErrors.AddRange(listOfErrors);

                var smallEffectMisses = Enumerable.Range(1, 5)
                    .Select(x => 50.0)
                    .ToList();


                listOfErrors.AddRange(smallEffectMisses);

                MyTTester.RunTTest(listOfErrors);
                MyOtherTTester.RunTTest(otherListOfErrors);


                // Act.
                var resultString = MyEvaluationReporter.GetEvaluationReport(MyTTester, MyOtherTTester);


                // Assert.
                //Debug.Log(resultString);
            }


            [Test]
            public void GetEvaluationReport_FirstTestsHasMediumEffectAndSecondIsNonsignificant_ReturnsCorrectEvaluationReport()
            {
                // Arrange. 
                var MyEvaluationReporter = new EvaluationReporter();
                var MyTTester = new TTester();
                var MyOtherTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var otherListOfErrors = new List<double>();
                otherListOfErrors.AddRange(listOfErrors);

                var mediumEffectMisses = Enumerable.Range(1, 15)
                    .Select(x => 50.0)
                    .ToList();

                listOfErrors.AddRange(mediumEffectMisses);

                MyTTester.RunTTest(listOfErrors);
                MyOtherTTester.RunTTest(otherListOfErrors);


                // Act.
                var resultString = MyEvaluationReporter.GetEvaluationReport(MyTTester, MyOtherTTester);


                // Assert.
                //Debug.Log(resultString);
            }


            [Test]
            public void GetEvaluationReport_FirstTestsHasLargeEffectAndSecondIsNonsignificant_ReturnsCorrectEvaluationReport()
            {
                // Arrange. 
                var MyEvaluationReporter = new EvaluationReporter();
                var MyTTester = new TTester();
                var MyOtherTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var otherListOfErrors = new List<double>();
                otherListOfErrors.AddRange(listOfErrors);

                var largeEffectMisses = Enumerable.Range(1, 50)
                    .Select(x => 50.0)
                    .ToList();


                listOfErrors.AddRange(largeEffectMisses);

                MyTTester.RunTTest(listOfErrors);
                MyOtherTTester.RunTTest(otherListOfErrors);

                // Act.
                var resultString = MyEvaluationReporter.GetEvaluationReport(MyTTester, MyOtherTTester);


                // Assert.
                //Debug.Log(resultString);
            }


            [Test]
            public void GetEvaluationReport_FirstTestsHasSmallEffectAndSecondHasSmallEffect_ReturnsCorrectEvaluationReport()
            {
                // Arrange. 
                var MyEvaluationReporter = new EvaluationReporter();
                var MyTTester = new TTester();
                var MyOtherTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var otherListOfErrors = new List<double>();
                otherListOfErrors.AddRange(listOfErrors);

                // d lille effekt
                var smallEffectMisses = Enumerable.Range(1, 5)
                    .Select(x => 50.0)
                    .ToList();


                listOfErrors.AddRange(smallEffectMisses);
                otherListOfErrors.AddRange(smallEffectMisses);

                MyTTester.RunTTest(listOfErrors);
                MyOtherTTester.RunTTest(otherListOfErrors);


                // Act.
                var resultString = MyEvaluationReporter.GetEvaluationReport(MyTTester, MyOtherTTester);


                // Assert.
                //Debug.Log(resultString);
            }


            [Test]
            public void GetEvaluationReport_FirstTestsHasSmallEffectAndSecondHasMediumEffect_ReturnsCorrectEvaluationReport()
            {
                // Arrange. 
                var MyEvaluationReporter = new EvaluationReporter();
                var MyTTester = new TTester();
                var MyOtherTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var otherListOfErrors = new List<double>();
                otherListOfErrors.AddRange(listOfErrors);

                var smallEffectMisses = Enumerable.Range(1, 5)
                    .Select(x => 50.0)
                    .ToList();

                var mediumEffectMisses = Enumerable.Range(1, 15)
                    .Select(x => 50.0)
                    .ToList();


                listOfErrors.AddRange(smallEffectMisses);
                otherListOfErrors.AddRange(mediumEffectMisses);

                MyTTester.RunTTest(listOfErrors);
                MyOtherTTester.RunTTest(otherListOfErrors);

                // Act.
                var resultString = MyEvaluationReporter.GetEvaluationReport(MyTTester, MyOtherTTester);


                // Assert.
                //Debug.Log(resultString);
            }


            [Test]
            public void GetEvaluationReport_FirstTestsHasSmallEffectAndSecondHasLargeEffect_ReturnsCorrectEvaluationReport()
            {
                // Arrange. 
                var MyEvaluationReporter = new EvaluationReporter();
                var MyTTester = new TTester();
                var MyOtherTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var otherListOfErrors = new List<double>();
                otherListOfErrors.AddRange(listOfErrors);

                var smallEffectMisses = Enumerable.Range(1, 5)
                    .Select(x => 50.0)
                    .ToList();

                var largeEffectMisses = Enumerable.Range(1, 50)
                    .Select(x => 50.0)
                    .ToList();


                listOfErrors.AddRange(smallEffectMisses);
                otherListOfErrors.AddRange(largeEffectMisses);

                MyTTester.RunTTest(listOfErrors);
                MyOtherTTester.RunTTest(otherListOfErrors);

                // Act.
                var resultString = MyEvaluationReporter.GetEvaluationReport(MyTTester, MyOtherTTester);


                // Assert.
                //Debug.Log(resultString);
            }


            [Test]
            public void GetEvaluationReport_FirstTestsHasMediumEffectAndSecondHasSmallEffect_ReturnsCorrectEvaluationReport()
            {
                // Arrange. 
                var MyEvaluationReporter = new EvaluationReporter();
                var MyTTester = new TTester();
                var MyOtherTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var otherListOfErrors = new List<double>();
                otherListOfErrors.AddRange(listOfErrors);

                var smallEffectMisses = Enumerable.Range(1, 5)
                    .Select(x => 50.0)
                    .ToList();

                var mediumEffectMisses = Enumerable.Range(1, 15)
                    .Select(x => 50.0)
                    .ToList();


                listOfErrors.AddRange(mediumEffectMisses);
                otherListOfErrors.AddRange(smallEffectMisses);

                MyTTester.RunTTest(listOfErrors);
                MyOtherTTester.RunTTest(otherListOfErrors);

                // Act.
                var resultString = MyEvaluationReporter.GetEvaluationReport(MyTTester, MyOtherTTester);


                // Assert.
                //Debug.Log(resultString);
            }


            [Test]
            public void GetEvaluationReport_FirstTestsHasMediumEffectAndSecondHasMediumEffect_ReturnsCorrectEvaluationReport()
            {
                // Arrange. 
                var MyEvaluationReporter = new EvaluationReporter();
                var MyTTester = new TTester();
                var MyOtherTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var otherListOfErrors = new List<double>();
                otherListOfErrors.AddRange(listOfErrors);

                // d mellem effekt
                var mediumEffectMisses = Enumerable.Range(1, 15)
                    .Select(x => 50.0)
                    .ToList();


                listOfErrors.AddRange(mediumEffectMisses);
                otherListOfErrors.AddRange(mediumEffectMisses);

                MyTTester.RunTTest(listOfErrors);
                MyOtherTTester.RunTTest(otherListOfErrors);

                // Act.
                var resultString = MyEvaluationReporter.GetEvaluationReport(MyTTester, MyOtherTTester);


                // Assert.
                //Debug.Log(resultString);
            }


            [Test]
            public void GetEvaluationReport_FirstTestsHasMediumEffectAndSecondHasLargeEffect_ReturnsCorrectEvaluationReport()
            {
                // Arrange. 
                var MyEvaluationReporter = new EvaluationReporter();
                var MyTTester = new TTester();
                var MyOtherTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var otherListOfErrors = new List<double>();
                otherListOfErrors.AddRange(listOfErrors);

                var mediumEffectMisses = Enumerable.Range(1, 15)
                    .Select(x => 50.0)
                    .ToList();

                var largeEffectMisses = Enumerable.Range(1, 50)
                    .Select(x => 50.0)
                    .ToList();


                listOfErrors.AddRange(mediumEffectMisses);
                otherListOfErrors.AddRange(largeEffectMisses);

                MyTTester.RunTTest(listOfErrors);
                MyOtherTTester.RunTTest(otherListOfErrors);

                // Act.
                var resultString = MyEvaluationReporter.GetEvaluationReport(MyTTester, MyOtherTTester);


                // Assert.
                //Debug.Log(resultString);
            }


            [Test]
            public void GetEvaluationReport_FirstTestsHasLargeEffectAndSecondHasSmallEffect_ReturnsCorrectEvaluationReport()
            {
                // Arrange. 
                var MyEvaluationReporter = new EvaluationReporter();
                var MyTTester = new TTester();
                var MyOtherTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var otherListOfErrors = new List<double>();
                otherListOfErrors.AddRange(listOfErrors);

                var smallEffectMisses = Enumerable.Range(1, 5)
                    .Select(x => 50.0)
                    .ToList();

                var largeEffectMisses = Enumerable.Range(1, 50)
                    .Select(x => 50.0)
                    .ToList();


                listOfErrors.AddRange(largeEffectMisses);
                otherListOfErrors.AddRange(smallEffectMisses);

                MyTTester.RunTTest(listOfErrors);
                MyOtherTTester.RunTTest(otherListOfErrors);


                // Act.
                var resultString = MyEvaluationReporter.GetEvaluationReport(MyTTester, MyOtherTTester);


                // Assert.
                //Debug.Log(resultString);
            }


            [Test]
            public void GetEvaluationReport_FirstTestsHasLargeEffectAndSecondHasMediumEffect_ReturnsCorrectEvaluationReport()
            {
                // Arrange. 
                var MyEvaluationReporter = new EvaluationReporter();
                var MyTTester = new TTester();
                var MyOtherTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var otherListOfErrors = new List<double>();
                otherListOfErrors.AddRange(listOfErrors);

                var mediumEffectMisses = Enumerable.Range(1, 15)
                    .Select(x => 50.0)
                    .ToList();

                var largeEffectMisses = Enumerable.Range(1, 50)
                    .Select(x => 50.0)
                    .ToList();


                listOfErrors.AddRange(largeEffectMisses);
                otherListOfErrors.AddRange(mediumEffectMisses);

                MyTTester.RunTTest(listOfErrors);
                MyOtherTTester.RunTTest(otherListOfErrors);


                // Act.
                var resultString = MyEvaluationReporter.GetEvaluationReport(MyTTester, MyOtherTTester);


                // Assert.
                //Debug.Log(resultString);
            }


            [Test]
            public void GetEvaluationReport_FirstTestsHasLargeEffectAndSecondHasLargeEffect_ReturnsCorrectEvaluationReport()
            {
                // Arrange. 
                var MyEvaluationReporter = new EvaluationReporter();
                var MyTTester = new TTester();
                var MyOtherTTester = new TTester();

                List<double> listOfErrors = Enumerable.Range(1, 30)
                    .Select(x => 0.0)
                    .ToList();

                var otherListOfErrors = new List<double>();
                otherListOfErrors.AddRange(listOfErrors);

                var largeEffectMisses = Enumerable.Range(1, 50)
                    .Select(x => 50.0)
                    .ToList();

                listOfErrors.AddRange(largeEffectMisses);
                otherListOfErrors.AddRange(largeEffectMisses);

                MyTTester.RunTTest(listOfErrors);
                MyOtherTTester.RunTTest(otherListOfErrors);


                // Act.
                var resultString = MyEvaluationReporter.GetEvaluationReport(MyTTester, MyOtherTTester);


                // Assert.
                //Debug.Log(resultString);
            }
        }
    }
}
