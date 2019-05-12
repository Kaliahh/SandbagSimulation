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
    public class DroneMoverPlayTests
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

        [UnityTest]
        public IEnumerator DroneMover_FlyTo_TargetFarAway_LimitDistance()
        {
            // Arrange
            GameObject drone = CreateDrone();

            yield return null;

            DroneMover mover = drone.GetComponent<DroneMover>();

            drone.SetActive(false);

            mover.transform.position = Vector3.zero;

            Vector3 target = new Vector3(10, 0, 0);

            // Act
            yield return null;

            Vector3 expected = new Vector3(mover.Speed * Time.deltaTime, 0, 0);

            mover.FlyTo(target);

            Vector3 actual = mover.transform.position;

            // Assert
            Assert.AreEqual(expected, actual);

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneMover_FlyTo_TargetClose_DontLimitDistance()
        {
            // Arrange
            GameObject drone = CreateDrone();

            yield return null;

            DroneMover mover = drone.GetComponent<DroneMover>();

            drone.SetActive(false);

            mover.transform.position = Vector3.zero;

            Vector3 target = new Vector3(0.5f, 0, 0);

            // Act
            yield return null;

            Vector3 expected = target * mover.Speed * Time.deltaTime;

            mover.FlyTo(target);

            Vector3 actual = mover.transform.position;

            // Assert
            Assert.AreEqual(expected, actual);

            yield return null;
        }

        [UnityTest]
        public IEnumerator DroneMover_MoveSandbag_SandbagFollowsIfNotNull()
        {
            // Arrange
            GameObject drone = CreateDrone();

            yield return null;

            drone.transform.position = Vector3.zero;

            drone.GetComponent<DroneController>().MySandbag = CreateSandbag();

            Vector3 target = new Vector3(10, 0, 0);

            // Act
            yield return null;

            drone.GetComponent<DroneController>().FlyTo(target);

            Vector3 expected = drone.transform.position - new Vector3(0, drone.GetComponent<DroneController>().DroneSandbagDistance, 0);

            Vector3 actual = drone.GetComponent<DroneController>().MySandbag.transform.position;

            // Assert
            Assert.AreEqual(expected, actual);

            yield return null;
        }
    }
}
