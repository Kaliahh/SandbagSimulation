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
        public IEnumerator DroneController_State_021_FindSandbagLocationState_To_FlyToLocatedSandbagState_LocatedSandbagIsNotNull()
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
        public IEnumerator DroneController_State_022_FindSandbagLocationState_To_FlyToSandbagPickUpLocationState_LocatedSandbagIsNull()
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
        public IEnumerator DroneController_State_041_PickUpLocatedSandbagState_To_FlyToSectionState_SandbagWithinRange_MySandbagIsNotNull()
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
        public IEnumerator DroneController_State_042_PickUpLocatedSandbagState_To_FindSandbagLocationState_SandbagOutOfRange_MySandbagIsNull()
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
        public IEnumerator DroneController_State_07_FlyToAboveTargetState_To_FlyToDroneTargetState()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            // Act
            yield return null;

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().SearchForSandbagPlaceState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_08_FlyToDroneTargetState_To_PlaceMySandbagState()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            // Act
            yield return null;

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().SearchForSandbagPlaceState;

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

            // Act
            yield return null;

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().SearchForSandbagPlaceState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_10_ReturnToAboveTargetState_To_FlyToSandbagPickUpLocationState()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            // Act
            yield return null;

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().SearchForSandbagPlaceState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());

            yield return null;
        }


        // TODO: Husk tilfælde hvor den hopper til en anden tilstand end den næste

        #endregion
    }
}
