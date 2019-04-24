using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SandbagSimulation {
    public class CameraController : MonoBehaviour {
        //Referencer
        public Camera mainCamera; //Kamaraet som skal flyttes rundt

        private Vector2 rotation = new Vector2(0, 0); //Vektor til fri kamara med varibler på hvor musen har rykket sig
        void Update() //Indlæser bruger input
        {
            //Standardkamara
            if (Input.GetKeyDown("1"))
            {
                SelectCameraPos1(); //Vælg standardpostion 1 som ser diget forfra
            }
            if (Input.GetKeyDown("2")) 
            {
                SelectCameraPos2(); //Vælg standardpostion 2 som ser diget ovenfra
            }
            if (Input.GetKeyDown("3")) 
            {
                SelectCameraPos3(); //Vælg standardpostion 3 som ser diget fra siden
            }

            //Fri kamera knapper
            if (Input.GetButton("Horizontal")) 
            {
                mainCamera.transform.Translate(Input.GetAxis("Horizontal"), 0, 0);
            }
            if (Input.GetButton("Vertical")) 
            {
                mainCamera.transform.Translate(0, 0, Input.GetAxis("Vertical"));
            }

            if (Input.GetMouseButton(0)) 
            {
                rotation.y += Input.GetAxis("Mouse X");
                rotation.x += -Input.GetAxis("Mouse Y");
                mainCamera.transform.eulerAngles = rotation;
            }

        }

        //Metoder til udregning af kameraets postition og vinkel henholdsvis til diget.
        private Vector3 FindMiddleOfDike()
        {
            //https://www.purplemath.com/modules/midpoint.htm midpoint formel
            Blueprint blueprint = GetComponent<SimulationController>().blueprint;
            Vector3 result;

            //Finder midten af diget ud fra alle punkter
            float SumX = 0;
            float SumY = 0;
            float SumZ = 0;

            foreach (Vector3 node in blueprint.ConstructionNodes)
            {
                SumX += node.x;
                SumY += node.y;
                SumZ += node.z;
            }
            int Count = blueprint.ConstructionNodes.Count(); //Amount of elements in the list

            //Udregner og returnerer
            result = new Vector3(SumX / Count, SumY / Count, SumZ / Count);

            //Finder midten af højden og lægger højden til midtpunktet
            result += new Vector3(0, blueprint.DikeHeight / 2, 0);
            return result;

        } //Finder digets position som bruges alle standardkameraer

        private Vector3 CalculateCamera1Pos() //Udregn placering af standardkamera 1
        {
            //Kameraet bliver placeret ved sandsækkes genereringspunkt
            Vector3 result = GetComponent<SandbagSpawner>().SpawnPoint;
            result.y = GetComponent<SimulationController>().blueprint.DikeHeight / 2;
            return result;
        }

        private Vector3 CalculateCamera2() //Udregn placering af standardkamera 2
        {
            //Kameratet bliver placeret i midten af diget med en højde så kameraet svæver over diget
            Vector3 result = FindMiddleOfDike();

            //Tilføjer en højde
            result = result + new Vector3(0, 20, 0);
            return result;

            //https://www.purplemath.com/modules/midpoint.htm midpoint formel
        }

        private Vector3 CalculateCamera3() //Udregn placering af standardkamera 3
        {
            //Kameratet bliver placeret i diget midte og roteres således at diget ses fra siden af
            Vector3 result = FindMiddleOfDike();

            Vector3 spawn = GetComponent<SandbagSpawner>().SpawnPoint;


            //Pej kamaratet mod sandsække generingspunkt
            mainCamera.transform.LookAt(spawn);

            //Roter kameratet 90 grader
            mainCamera.transform.LookAt(spawn);


            return result;
        }

        //Metoder til valg af standard kamera
        private void SelectCameraPos1() //Standardposition 1: Forfra
        {
            mainCamera.transform.position = CalculateCamera1Pos();
            mainCamera.transform.LookAt(FindMiddleOfDike());
        }

        private void SelectCameraPos2() //Stardardposition 2: Ovenfra
        {
            mainCamera.transform.position = CalculateCamera2();
            mainCamera.transform.eulerAngles = new Vector3(90f, 0f, 0f);
        }

        private void SelectCameraPos3() //Standardposition 3: Sidefra
        {
            mainCamera.transform.position = CalculateCamera3();
            mainCamera.transform.rotation.SetLookRotation(GetComponent<SandbagSpawner>().SpawnPoint);

            //Højde
            float height = 20;
            mainCamera.transform.position += new Vector3(0, 1, -15);
            mainCamera.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }
}
