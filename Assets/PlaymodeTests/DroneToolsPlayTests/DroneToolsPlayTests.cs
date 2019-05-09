using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SandbagSimulation;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;
using static SandbagSimulation.DroneTools;
using static SandbagSimulation.SetupMethods;

namespace Tests
{
    public class DroneToolsPlayTests
    {
        [SetUp]
        public void ResetScene()
        {
            var list = GameObject.FindObjectsOfType<GameObject>();

            foreach (var item in list)
            {
                GameObject.Destroy(item);
            }
        }

        #region ReturnTargetNode

        [UnityTest]
        public IEnumerator DroneTools_ReturnTargetNode_IsRightDroneTrue_TargetNodeLast()
        {
            // Arrange
            Blueprint blueprint = SetupMethods.CreateBlueprint(new Vector3(-10, 0, 0), new Vector3(10, 0, 0), 4);

            bool isRightDrone = true;

            // Act
            yield return null;
            Vector3 node = DroneTools.ReturnTargetNode(isRightDrone, blueprint);

            // Assert
            Assert.AreEqual(blueprint.ConstructionNodes.Last(), node);
        }

        [UnityTest]
        public IEnumerator DroneTools_ReturnTargetNode_IsRightDroneFalse_TargetNodeFirst()
        {
            // Arrange
            Blueprint blueprint = SetupMethods.CreateBlueprint(new Vector3(-10, 0, 0), new Vector3(10, 0, 0), 4);

            bool isRightDrone = false;

            // Act
            yield return null;
            Vector3 node = DroneTools.ReturnTargetNode(isRightDrone, blueprint);

            // Assert
            Assert.AreEqual(blueprint.ConstructionNodes.First(), node);
        }

        #endregion

        #region CalculateAbovePoint

        [UnityTest]
        public IEnumerator DroneTools_CalculateAbovePoint_AllParametersNormal()
        {
            // Arrange
            Vector3 point = Vector3.zero;

            float safeHeight = 2f;

            Vector3 node1 = new Vector3(10, 0, 0);
            Vector3 node2 = new Vector3(-10, 0, 0);
            int height = 5;

            Blueprint blueprint = new Blueprint(new List<Vector3> { node1, node2 }, height);

            // Act
            Vector3 actual = CalculateAbovePoint(point, blueprint, safeHeight);

            Vector3 expected = point + new Vector3(0, height * new SandbagMeasurements().Height + safeHeight, 0);
            yield return null;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [UnityTest]
        public IEnumerator DroneTools_CalculateAbovePoint_BlueprintIsNull_ThrowNullReferenceException()
        {
            // Arrange
            Vector3 point = Vector3.zero;

            float safeHeight = 2f;

            Blueprint blueprint = null;

            // Act
            try
            {
                Vector3 actual = CalculateAbovePoint(point, blueprint, safeHeight);
                Assert.Fail();
            }

            // Assert
            catch (NullReferenceException)
            {
                Assert.Pass();
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneTools_CalculateAbovePoint_SafeHeightIsNegative()
        {
            // Arrange
            Vector3 point = Vector3.zero;

            float safeHeight = -2f;

            Vector3 node1 = new Vector3(10, 0, 0);
            Vector3 node2 = new Vector3(-10, 0, 0);
            int height = 5;

            Blueprint blueprint = new Blueprint(new List<Vector3> { node1, node2 }, height);

            // Act
            Vector3 actual = CalculateAbovePoint(point, blueprint, safeHeight);

            Vector3 expected = point + new Vector3(0, height * new SandbagMeasurements().Height + safeHeight, 0);
            yield return null;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region InVicinityOf

        [UnityTest]
        public IEnumerator DroneTools_InVicinityOf_WithinRange_ReturnTrue()
        {
            // Arrange
            Vector3 position1 = new Vector3(0, 0, 0);
            Vector3 position2 = new Vector3(0, 0, 0.05f);

            // Act
            bool actual = InVicinityOf(position1, position2);

            yield return null;

            // Assert
            Assert.IsTrue(actual);
        }

        [UnityTest]
        public IEnumerator DroneTools_InVicinityOf_OnRange_ReturnFalse()
        {
            // Arrange
            Vector3 position1 = new Vector3(0, 0, 0);
            Vector3 position2 = new Vector3(0, 0, 0.1f);

            // Act
            bool actual = InVicinityOf(position1, position2);

            yield return null;

            // Assert
            Assert.IsFalse(actual);
        }

        [UnityTest]
        public IEnumerator DroneTools_InVicinityOf_OutOfRange_ReturnFalse()
        {
            // Arrange
            Vector3 position1 = new Vector3(0, 0, 0);
            Vector3 position2 = new Vector3(0, 0, 0.2f);

            // Act
            bool actual = InVicinityOf(position1, position2);

            yield return null;

            // Assert
            Assert.IsFalse(actual);
        }


        #endregion

        #region RotateSandbag

        //public static void RotateSandbag(GameObject sandbag, Blueprint blueprint) => sandbag.transform.Rotate(new Vector3(0, CalculateAngleToDike(blueprint), 0));

        [UnityTest]
        public IEnumerator DroneTools_RotateSandbag_NormalParameters_45DegreeAngle()
        {
            // Arrange
            GameObject sandbag = CreateSandbag();

            Vector3 node1 = new Vector3(10, 0, 0);
            Vector3 node2 = new Vector3(0, 0, 10);
            int height = 5;

            Blueprint blueprint = new Blueprint(new List<Vector3> { node1, node2 }, height);

            // Act
            yield return null;
            RotateSandbag(sandbag, blueprint);
            Vector3 actual = sandbag.transform.rotation.eulerAngles;

            Vector3 expected = Quaternion.Euler(0, 45, 0).eulerAngles;
            yield return null;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [UnityTest]
        public IEnumerator DroneTools_RotateSandbag_SandbagIsNull_ThrowNullReferenceException()
        {
            // Arrange
            GameObject sandbag = null;

            Vector3 node1 = new Vector3(10, 0, 0);
            Vector3 node2 = new Vector3(-10, 0, 0);
            int height = 5;

            Blueprint blueprint = new Blueprint(new List<Vector3> { node1, node2 }, height);

            // Act
            try
            {
                RotateSandbag(sandbag, blueprint);
                Assert.Fail();
            }

            // Assert
            catch (NullReferenceException)
            {
                Assert.Pass();
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneTools_RotateSandbag_BlueprintIsNull_ThrowNullReferenceException()
        {
            // Arrange
            GameObject sandbag = CreateSandbag();

            Blueprint blueprint = null;

            // Act
            try
            {
                RotateSandbag(sandbag, blueprint);
                Assert.Fail();
            }

            // Assert
            catch (NullReferenceException)
            {
                Assert.Pass();
            }

            yield return null;
        }

        #endregion

        #region IsLastSandbagPlaced

        // public static bool IsLastSandbagPlaced(Vector3 position, Vector3 targetNode, float viewDistance, Blueprint blueprint, SandbagMeasurements sandbag)

        [UnityTest]
        public IEnumerator DroneTools_IsLastSandbagPlaced_LastIsPlaced_WithinViewDistance_ReturnTrue()
        {
            // Arrange
            Vector3 position = Vector3.zero;

            float viewDistance = 8;

            Vector3 node1 = new Vector3(5, 0, 0);
            Vector3 node2 = new Vector3(-5, 0, 0);
            int height = 5;

            Blueprint blueprint = new Blueprint(new List<Vector3> { node1, node2 }, height);
            Vector3 targetNode = blueprint.ConstructionNodes.Last();

            GameObject sandbag = CreateSandbag();
            sandbag.transform.position = new Vector3(targetNode.x, (blueprint.DikeHeight - 0.5f) * new SandbagMeasurements().Height, targetNode.z);

            // Act
            yield return null;
            bool actual = IsLastSandbagPlaced(position, targetNode, viewDistance, blueprint, new SandbagMeasurements());

            // Assert
            Assert.IsTrue(actual);

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneTools_IsLastSandbagPlaced_LastIsNotPlaced_WithinViewDistance_ReturnFalse()
        {
            // Arrange
            Vector3 position = Vector3.zero;

            float viewDistance = 8;

            Vector3 node1 = new Vector3(5, 0, 0);
            Vector3 node2 = new Vector3(-5, 0, 0);
            int height = 5;

            Blueprint blueprint = new Blueprint(new List<Vector3> { node1, node2 }, height);
            Vector3 targetNode = blueprint.ConstructionNodes.Last();

            // Act
            yield return null;
            bool actual = IsLastSandbagPlaced(position, targetNode, viewDistance, blueprint, new SandbagMeasurements());

            // Assert
            Assert.IsFalse(actual);

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneTools_IsLastSandbagPlaced_LastIsPlaced_OutOfViewDistance_ReturnFalse()
        {
            // Arrange
            Vector3 position = Vector3.zero;

            float viewDistance = 8;

            Vector3 node1 = new Vector3(10, 0, 0);
            Vector3 node2 = new Vector3(-10, 0, 0);
            int height = 5;

            Blueprint blueprint = new Blueprint(new List<Vector3> { node1, node2 }, height);
            Vector3 targetNode = blueprint.ConstructionNodes.Last();

            GameObject sandbag = CreateSandbag();
            sandbag.transform.position = new Vector3(targetNode.x, (blueprint.DikeHeight - 0.5f) * new SandbagMeasurements().Height, targetNode.z);

            // Act
            yield return null;
            bool actual = IsLastSandbagPlaced(position, targetNode, viewDistance, blueprint, new SandbagMeasurements());

            // Assert
            Assert.IsFalse(actual);

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneTools_IsLastSandbagPlaced_LastIsNotPlaced_OutOfViewDistance_ReturnFalse()
        {
            // Arrange
            Vector3 position = Vector3.zero;

            float viewDistance = 8;

            Vector3 node1 = new Vector3(10, 0, 0);
            Vector3 node2 = new Vector3(-10, 0, 0);
            int height = 5;

            Blueprint blueprint = new Blueprint(new List<Vector3> { node1, node2 }, height);
            Vector3 targetNode = blueprint.ConstructionNodes.Last();

            // Act
            yield return null;
            bool actual = IsLastSandbagPlaced(position, targetNode, viewDistance, blueprint, new SandbagMeasurements());

            // Assert
            Assert.IsFalse(actual);

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneTools_IsLastSandbagPlaced_LastIsPlaced_LineOfSightBroken_ReturnFalse()
        {
            // Arrange
            Vector3 position = Vector3.zero;

            float viewDistance = 8;

            Vector3 node1 = new Vector3(5, 0, 0);
            Vector3 node2 = new Vector3(-5, 0, 0);
            int height = 5;

            Blueprint blueprint = new Blueprint(new List<Vector3> { node1, node2 }, height);
            Vector3 targetNode = blueprint.ConstructionNodes.Last();

            GameObject sandbag = CreateSandbag();
            sandbag.transform.position = new Vector3(targetNode.x, (blueprint.DikeHeight - 0.5f) * new SandbagMeasurements().Height, targetNode.z);

            GameObject blockingSandbag = CreateSandbag();
            blockingSandbag.transform.position = Vector3.MoveTowards(blockingSandbag.transform.position, sandbag.transform.position, 2);

            // Act
            yield return null;
            bool actual = IsLastSandbagPlaced(position, targetNode, viewDistance, blueprint, new SandbagMeasurements());

            // Assert
            Assert.IsFalse(actual);

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneTools_IsLastSandbagPlaced_BlueprintIsNull_ThrowNullReferenceException()
        {
            // Arrange
            Vector3 position = Vector3.zero;

            float viewDistance = 8;

            Blueprint blueprint = null;
            Vector3 targetNode = new Vector3(5, 0, 0);

            // Act
            yield return null;

            try
            {
                IsLastSandbagPlaced(position, targetNode, viewDistance, blueprint, new SandbagMeasurements());
                Assert.Fail();
            }

            // Assert
            catch (NullReferenceException)
            {
                Assert.Pass();
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneTools_IsLastSandbagPlaced_SandbagMeasurementsIsNull_ThrowNullReferenceException()
        {
            Vector3 position = Vector3.zero;

            float viewDistance = 8;

            Vector3 node1 = new Vector3(5, 0, 0);
            Vector3 node2 = new Vector3(-5, 0, 0);
            int height = 5;

            Blueprint blueprint = new Blueprint(new List<Vector3> { node1, node2 }, height);
            Vector3 targetNode = blueprint.ConstructionNodes.Last();

            // Act
            yield return null;

            try
            {
                IsLastSandbagPlaced(position, targetNode, viewDistance, blueprint, null);
                Assert.Fail();
            }

            // Assert
            catch (NullReferenceException)
            {
                Assert.Pass();
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneTools_IsLastSandbagPlaced_LastIsNotPlaced_DroneIsInLastSandbagPosition_ReturnFalse()
        {
            // Arrange
            float viewDistance = 8;

            Vector3 node1 = new Vector3(5, 0, 0);
            Vector3 node2 = new Vector3(-5, 0, 0);
            int height = 5;

            Blueprint blueprint = new Blueprint(new List<Vector3> { node1, node2 }, height);
            Vector3 targetNode = blueprint.ConstructionNodes.Last();

            GameObject sandbag = CreateSandbag();
            sandbag.transform.position = new Vector3(targetNode.x, (blueprint.DikeHeight - 0.5f) * new SandbagMeasurements().Height, targetNode.z);

            Vector3 position = sandbag.transform.position;

            // Act
            yield return null;
            bool actual = IsLastSandbagPlaced(position, targetNode, viewDistance, blueprint, new SandbagMeasurements());

            // Assert
            Assert.IsFalse(actual);

            yield return null;
        }

        #endregion
    }
}
