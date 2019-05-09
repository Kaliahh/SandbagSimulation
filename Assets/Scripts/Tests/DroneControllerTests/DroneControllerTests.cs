using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SandbagSimulation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DroneControllerTests
    {
        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        #region State

        //[UnityTest]
        //public IEnumerator DroneController_InitialStateIsFlyToSandbagPickUpLocationState_ReturnTrue()
        //{
        //    // Arrange
        //    GameObject drone = SetupMethods.CreateDrone();

        //    IDroneState state = new FlyToSandbagPickUpLocationState(SetupMethods.CreateDrone().GetComponent<DroneController>());

        //    // Act
        //    yield return null;

        //    // Assert
        //    Assert.AreSame(state, drone.GetComponent<DroneController>().State);
        //}

        #endregion


    }
}
