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
        public IEnumerator DroneController_State_1_InitialIsFlyToSandbagPickUpLocationState()
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
        public IEnumerator DroneController_State_2_FlyToSandbagPickUpLocationState_To_FindSandbagLocationState()
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

            yield return null;

            drone.GetComponent<DroneController>().State.Execute();

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().FindSandbagLocationState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_3_FindSandbagLocationState_To_FlyToLocatedSandbagState()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().FindSandbagLocationState;

            GameObject sandbag = CreateSandbag();

            yield return null;

            sandbag.transform.position = new Vector3(2, 0, 0);

            // Act
            drone.GetComponent<DroneController>().State.Execute();
            
            yield return null;

            IDroneState actual = drone.GetComponent<DroneController>().State;

            IDroneState expected = drone.GetComponent<DroneController>().FlyToLocatedSandbagState;

            // Assert
            Assert.AreEqual(expected.GetType(), actual.GetType());

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_4_FlyToLocatedSandbagState_To_PickUpLocatedSandbagState()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().FlyToLocatedSandbagState;

            GameObject sandbag = CreateSandbag();

            yield return null;

            sandbag.transform.position = new Vector3(10, 0, 0);

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
        public IEnumerator DroneController_State_5_PickUpLocatedSandbagState_To_FlyToSectionState()
        {
            // Arrange 
            GameObject drone = CreateDrone();

            yield return null;

            drone.GetComponent<DroneController>().State = drone.GetComponent<DroneController>().PickUpLocatedSandbagState;

            GameObject sandbag = CreateSandbag();

            yield return null;

            sandbag.transform.position = new Vector3(10, 0, 0);

            drone.GetComponent<DroneController>().SetLocatedSandbag(sandbag);

            // Act
            yield return null;

            // Assert
            Assert.Fail();

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_6_FlyToSectionState_To_SearchForSandbagPlaceState()
        {
            // Arrange 

            // Act
            yield return null;

            // Assert
            Assert.Fail();

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_7_SearchForSandbagPlaceState_To_FlyToAboveTargetState()
        {
            // Arrange 

            // Act
            yield return null;

            // Assert
            Assert.Fail();

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_8_FlyToAboveTargetState_To_FlyToDroneTargetState()
        {
            // Arrange 

            // Act
            yield return null;

            // Assert
            Assert.Fail();

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_9_FlyToDroneTargetState_To_PlaceMySandbagState()
        {
            // Arrange 

            // Act
            yield return null;

            // Assert
            Assert.Fail();

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_10_PlaceMySandbagState_To_ReturnToAboveTargetState()
        {
            // Arrange 

            // Act
            yield return null;

            // Assert
            Assert.Fail();

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneController_State_11_ReturnToAboveTargetState_To_FlyToSandbagPickUpLocationState()
        {
            // Arrange 

            // Act
            yield return null;

            // Assert
            Assert.Fail();

            yield return null;
        }


        // TODO: Husk tilfælde hvor den hopper til en anden tilstand end den næste

        #endregion
    }
}
