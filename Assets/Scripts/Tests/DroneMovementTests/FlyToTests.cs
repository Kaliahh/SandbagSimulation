using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class FlyToTests
    {
        /* Simulationens brug af FlyTo valideres gennem tests af Unity-funktionen Vector3.MoveTowards, 
         * som udgør FlyTo's krop. Dette kompromis tænkes at isolere metodens adfærd bedre end alternativerne. */
        
        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        
        [Test]
        public void FlyTo_WhenNotAtTarget_GetsCloser()
        {
            //Arrange. 
            GameObject obj = new GameObject();

            Vector3 start = obj.transform.position = new Vector3(0.0f, 0.0f, 0.0f),
                    target = new Vector3(100.0f, 100.0f, 100.0f);

            //deltaTime svarer her til 60 FPS.
            float deltaTime = 1.0f / 60.0f;
            float step = 30.0f * deltaTime;

            //Act. NB: Svarer til FlyTo's krop.
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, step);
            Vector3 current = obj.transform.position;

            //Assert.
            Assert.IsTrue(Vector3.Distance(current, target) < Vector3.Distance(start, target));
        }


        [Test]
        public void FlyTo_WhenWithinStepLength_DoesNotOvershootTarget()
        {
            //Arrange. 
            GameObject obj = new GameObject();

            Vector3 start = obj.transform.position = new Vector3(99.99f, 99.99f, 99.99f),
                    target = new Vector3(100.0f, 100.0f, 100.0f);

            //deltaTime svarer her til 60 FPS.
            float deltaTime = 1.0f / 60.0f;
            float step = 30.0f * deltaTime;

            //Act. NB: Svarer til FlyTo's krop.
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, step);
            Vector3 current = obj.transform.position;

            //Assert.
            Assert.IsTrue(Vector3.Distance(current, target) < Vector3.Distance(start, target));
        }


        [Test]
        public void FlyTo_WhenAtTarget_Remains()
        {
            //Arrange. 
            GameObject obj = new GameObject();

            Vector3 start = obj.transform.position = new Vector3(0.0f, 0.0f, 0.0f),
                    target = new Vector3(0.0f, 0.0f, 0.0f);

            //deltaTime svarer her til 60 FPS.
            float deltaTime = 1.0f / 60.0f;
            float step = 30.0f * deltaTime;

            //Act. NB: Svarer til FlyTo's krop.
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, step);
            Vector3 current = obj.transform.position;

            //Assert.
            Assert.IsTrue(Vector3.Distance(current, target) == Vector3.Distance(start, target));
        }

    }
}
