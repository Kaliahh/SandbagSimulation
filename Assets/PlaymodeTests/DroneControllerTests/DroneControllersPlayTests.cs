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
    public class DroneControllersPlayTests
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

        #region State

        [UnityTest]
        public IEnumerator DroneController_State_00_InitialIsFlyToSandbagPickUpLocationState()
        {
            // Arrange
            GameObject drone = CreateDrone();

            yield return null;

            // Act
            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().FlyToSandbagPickUpLocationState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());
        }

        [UnityTest]
        public IEnumerator DroneController_State_01_FlyToSandbagPickUpLocationState_To_FindSandbagLocationState()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().FlyToSandbagPickUpLocationState;

            // Act
            while (drone.GetComponent<DroneController>().State.GetType() == drone.GetComponent<DroneController>().FlyToSandbagPickUpLocationState.GetType())
            {
                yield return null;
            }

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().FindSandbagLocationState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_02_1_FindSandbagLocationState_To_FlyToLocatedSandbagState_LocatedSandbagIsNotNull()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().FindSandbagLocationState;

            GameObject sandbag = CreateSandbag();

            yield return null;

            sandbag.transform.position = new Vector3(2, 0, 0);

            // Act
            yield return null;

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().FlyToLocatedSandbagState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_02_2_FindSandbagLocationState_To_FlyToSandbagPickUpLocationState_LocatedSandbagIsNull()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().FindSandbagLocationState;

            // Act

            yield return null;

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().FlyToSandbagPickUpLocationState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_03_FlyToLocatedSandbagState_To_PickUpLocatedSandbagState()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().FlyToLocatedSandbagState;

            GameObject sandbag = CreateSandbag();

            yield return null;

            sandbag.transform.position = new Vector3(1, 0, 0);

            drone.GetComponent<DroneController>().SetLocatedSandbag(sandbag);

            // Act
            yield return null;

            while (drone.GetComponent<DroneController>().State.GetType() == drone.GetComponent<DroneController>().FlyToLocatedSandbagState.GetType())
            {
                yield return null;
            }

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().PickUpLocatedSandbagState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_04_1_PickUpLocatedSandbagState_To_FlyToSectionState_SandbagWithinRange_MySandbagIsNotNull()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().PickUpLocatedSandbagState;

            GameObject sandbag = CreateSandbag();

            sandbag.transform.position = new Vector3(0.1f, 0, 0);

            drone.GetComponent<DroneController>().SetLocatedSandbag(sandbag);

            // Act
            yield return null;

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().FlyToSectionState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_04_2_PickUpLocatedSandbagState_To_FindSandbagLocationState_SandbagOutOfRange_MySandbagIsNull()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().PickUpLocatedSandbagState;

            GameObject sandbag = CreateSandbag();

            sandbag.transform.position = new Vector3(10, 0, 0);

            drone.GetComponent<DroneController>().SetLocatedSandbag(sandbag);

            // Act
            yield return null;

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().FindSandbagLocationState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_05_FlyToSectionState_To_SearchForSandbagPlaceState()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().FlyToSectionState;

            drone.GetComponent<DroneController>().AboveSection = new Vector3(1, 0, 0);

            // Act
            while (drone.GetComponent<DroneController>().State.GetType() == drone.GetComponent<DroneController>().FlyToSectionState.GetType())
            {
                yield return null;
            }

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().SearchForSandbagPlaceState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_06_SearchForSandbagPlaceState_To_FlyToAboveTargetState()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().SearchForSandbagPlaceState;

            // Act
            yield return null;

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().FlyToAboveTargetState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_07_1_FlyToAboveTargetState_To_FlyToDroneTargetState_PlaceIsAvailable()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().FlyToAboveTargetState;

            drone.GetComponent<DroneController>().AboveTarget = new Vector3(1, 0, 0);

            // Act
            while (drone.GetComponent<DroneController>().State.GetType() == drone.GetComponent<DroneController>().FlyToAboveTargetState.GetType())
            {
                yield return null;
            }

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().FlyToDroneTargetState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_07_2_FlyToAboveTargetState_To_SearchForSandbagPlaceState_PlaceIsNotAvailable()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().FlyToAboveTargetState;

            drone.GetComponent<DroneController>().AboveTarget = new Vector3(2, 0, 0);

            GameObject sandbag = CreateSandbag();

            sandbag.transform.position = new Vector3(2, 2, 2);

            yield return null;  

            drone.GetComponent<DroneController>().SandbagTargetPoint.Position = sandbag.transform.position;

            // Act
            while (drone.GetComponent<DroneController>().State.GetType() == drone.GetComponent<DroneController>().FlyToAboveTargetState.GetType())
            {
                yield return null;
            }

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().SearchForSandbagPlaceState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_07_3_FlyToAboveTargetState_LastSandbagIsPlaced()
        {
            // Arrange 
            GameObject drone = CreateDrone(Vector3.zero, new Vector3(2, 0, 0), new Vector3(-2, 0, 0), 2);

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().FlyToAboveTargetState;

            drone.GetComponent<DroneController>().AboveTarget = new Vector3(0, 0, 2);

            drone.GetComponent<DroneController>().FinishedBuilding += EventHandlerPlaceholder;

            GameObject sandbag = CreateSandbag();

            Vector3 targetNode = ReturnTargetNode(drone.GetComponent<DroneController>().IsRightDrone, drone.GetComponent<DroneController>().MyBlueprint);

            sandbag.transform.position = new Vector3(targetNode.x, (drone.GetComponent<DroneController>().MyBlueprint.DikeHeight - 0.5f) * new SandbagMeasurements().Height, targetNode.z);

            yield return null;

            // Act
            while (InVicinityOf(drone.transform.position, drone.GetComponent<DroneController>().AboveTarget) == false)
            {
                yield return null;
            }

            bool actual = drone.GetComponent<DroneController>().IsFinishedBuilding;

            // Assert
            Assert.IsTrue(actual);

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_08_FlyToDroneTargetState_To_PlaceMySandbagState()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().FlyToDroneTargetState;

            drone.GetComponent<DroneController>().SetDroneTargetPoint(new Vector3(1, 0, 0));

            GameObject sandbag = CreateSandbag();

            drone.GetComponent<DroneController>().MySandbag = sandbag;

            // Act
            while (drone.GetComponent<DroneController>().State.GetType() == drone.GetComponent<DroneController>().FlyToDroneTargetState.GetType())
            {
                yield return null;
            }

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().PlaceMySandbagState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_09_PlaceMySandbagState_To_ReturnToAboveTargetState()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().PlaceMySandbagState;

            GameObject sandbag = CreateSandbag();

            drone.GetComponent<DroneController>().MySandbag = sandbag;

            // Act
            yield return null;

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().ReturnToAboveTargetState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_10_1_ReturnToAboveTargetState_To_FlyToSandbagPickUpLocationState()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().ReturnToAboveTargetState;

            // Act
            while (drone.GetComponent<DroneController>().State.GetType() == drone.GetComponent<DroneController>().ReturnToAboveTargetState.GetType())
            {
                yield return null;
            }

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().FlyToSandbagPickUpLocationState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_10_2_ReturnToAboveTargetState_LastSandbagPlaced()
        {
            // Arrange 
            GameObject drone = CreateDrone(Vector3.zero, new Vector3(2, 0, 0), new Vector3(-2, 0, 0), 2);

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().ReturnToAboveTargetState;

            drone.GetComponent<DroneController>().AboveTarget = new Vector3(0, 0, 2);

            drone.GetComponent<DroneController>().FinishedBuilding += EventHandlerPlaceholder;

            GameObject sandbag = CreateSandbag();

            Vector3 targetNode = ReturnTargetNode(drone.GetComponent<DroneController>().IsRightDrone, drone.GetComponent<DroneController>().MyBlueprint);

            sandbag.transform.position = new Vector3(targetNode.x, (drone.GetComponent<DroneController>().MyBlueprint.DikeHeight - 0.5f) * new SandbagMeasurements().Height, targetNode.z);

            yield return null;

            // Act
            while (InVicinityOf(drone.transform.position, drone.GetComponent<DroneController>().AboveTarget) == false)
            {
                yield return null;
            }

            bool actual = drone.GetComponent<DroneController>().IsFinishedBuilding;

            // Assert
            Assert.IsTrue(actual);

            yield return null;
        }

        private void EventHandlerPlaceholder(object sender, EventArgs e) { }

        #endregion

        #region SearchForSandbagPlaceState metoder

        [UnityTest]
        public IEnumerator SearchForSandbagPlaceState_FindSandbagPlace_FirstSandbagNotPlaced_HasBuildingBegunFalse()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            Vector3 node1 = new Vector3(4, 0, 0);
            Vector3 node2 = new Vector3(-4, 0, 0);
            int dikeHeight = 3;

            Blueprint blueprint = new Blueprint(new List<Vector3> { node1, node2 }, dikeHeight);

            drone.GetComponent<DroneController>().SetBlueprint(blueprint);

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().SearchForSandbagPlaceState;

            Vector3 blueprintCentre = Vector3.Lerp(blueprint.ConstructionNodes[0], blueprint.ConstructionNodes[1], 0.5f);

            // Act
            yield return null;

            var actual = drone.GetComponent<DroneController>().SandbagTargetPoint.Position;

            var expected = new Vector3(blueprintCentre.x, new SandbagMeasurements().Height / 2, blueprintCentre.z);

            // Assert
            Assert.AreEqual(expected, actual);

            yield return null;
        }

        [UnityTest]
        public IEnumerator SearchForSandbagPlaceState_FindSandbagPlace_FirstSandbagPlaced_HasBuildingBegunFalse()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            drone.transform.position = new Vector3(0, 3, 0);

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().SearchForSandbagPlaceState;

            Vector3 node1 = new Vector3(4, 0, 0);
            Vector3 node2 = new Vector3(-4, 0, 0);
            int dikeHeight = 3;

            Blueprint blueprint = new Blueprint(new List<Vector3> { node1, node2 }, dikeHeight);

            drone.GetComponent<DroneController>().SetBlueprint(blueprint);

            Vector3 blueprintCentre = Vector3.Lerp(blueprint.ConstructionNodes[0], blueprint.ConstructionNodes[1], 0.5f);

            GameObject sandbag = CreateSandbag();

            sandbag.tag = "PlacedSandbag";

            sandbag.transform.position = new Vector3(blueprintCentre.x, new SandbagMeasurements().Height / 2, blueprintCentre.z);

            // Act
            yield return null;

            var actual = drone.GetComponent<DroneController>().SandbagTargetPoint.Position;

            // Det her er den første placering der bliver tjekket af FindPlace, og vil derfor være den første FindBestPlace returnerer til SandbagTargetPoint
            var expected = sandbag.transform.position + (blueprint.ConstructionNodes.First() - blueprint.ConstructionNodes.Last()).normalized * new SandbagMeasurements().Length;

            // Assert
            Assert.AreEqual(expected, actual);

            yield return null;
        }

        [UnityTest]
        public IEnumerator SearchForSandbagPlaceState_FindSandbagPlace_HasBuildingBegunTrue()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            drone.transform.position = new Vector3(0, 3, 0);

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().SearchForSandbagPlaceState;

            drone.GetComponent<DroneController>().HasBuildingBegun = true;

            Vector3 node1 = new Vector3(4, 0, 0);
            Vector3 node2 = new Vector3(-4, 0, 0);
            int dikeHeight = 3;

            Blueprint blueprint = new Blueprint(new List<Vector3> { node1, node2 }, dikeHeight);

            drone.GetComponent<DroneController>().SetBlueprint(blueprint);

            Vector3 blueprintCentre = Vector3.Lerp(blueprint.ConstructionNodes[0], blueprint.ConstructionNodes[1], 0.5f);

            GameObject sandbag = CreateSandbag();

            sandbag.tag = "PlacedSandbag";

            sandbag.transform.position = new Vector3(blueprintCentre.x, new SandbagMeasurements().Height / 2, blueprintCentre.z);

            // Act
            yield return null;

            var actual = drone.GetComponent<DroneController>().SandbagTargetPoint.Position;

            // Det her er den første placering der bliver tjekket af FindPlace, og vil derfor være den første FindBestPlace returnerer til SandbagTargetPoint
            var expected = sandbag.transform.position + (blueprint.ConstructionNodes.First() - blueprint.ConstructionNodes.Last()).normalized * new SandbagMeasurements().Length;

            // Assert
            Assert.AreEqual(expected, actual);

            yield return null;
        }

        [UnityTest]
        public IEnumerator SearchForSandbagPlaceState_FindSandbagPlace_HasBuildingBegunTrue_NoPossiblePlacesNextSection()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            drone.transform.position = new Vector3(0, 3, 0);

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().SearchForSandbagPlaceState;

            drone.GetComponent<DroneController>().HasBuildingBegun = true;

            Vector3 node1 = new Vector3(4, 0, 0);
            Vector3 node2 = new Vector3(-4, 0, 0);
            int dikeHeight = 3;

            Blueprint blueprint = new Blueprint(new List<Vector3> { node1, node2 }, dikeHeight);

            drone.GetComponent<DroneController>().SetBlueprint(blueprint);

            Vector3 blueprintCentre = Vector3.Lerp(blueprint.ConstructionNodes[0], blueprint.ConstructionNodes[1], 0.5f);

            drone.GetComponent<DroneController>().MySection.CurrentSection = blueprintCentre;

            // Act
            yield return null;

            var actual = drone.GetComponent<DroneController>().MySection.CurrentSection;

            var expected = Vector3.MoveTowards(drone.transform.position, ReturnTargetNode(drone.GetComponent<DroneController>().IsRightDrone, blueprint), drone.GetComponent<DroneController>().ViewDistance);

            // Assert
            Assert.AreEqual(expected, actual);

            yield return null;
        }

        #endregion


    }
}
