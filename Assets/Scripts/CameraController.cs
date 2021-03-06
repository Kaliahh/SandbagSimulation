﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SandbagSimulation
    {

    public class CameraController : MonoBehaviour
    {
        // Referencer
        
        public Camera MainCamera; // Kameraet som skal flyttes rundt
        private Vector2 rotation = new Vector2(0, 0); // Vektor til fri kamara med varibler på hvor musen har rykket sig
        public float CameraSpeed;

        // Indlæser bruger input
        void Update() 
        {
            // Standardkamara
            if (Input.GetKeyDown("1"))
            {
                SelectCameraPos1(); // Vælg standardpostion 1 som ser diget forfra
            }

            if (Input.GetKeyDown("2")) 
            {
                SelectCameraPos2(); // Vælg standardpostion 2 som ser diget ovenfra
            }

            if (Input.GetKeyDown("3")) 
            {
                SelectCameraPos3(); // Vælg standardpostion 3 som ser diget fra siden
            }

            // Fri kamera knapper
            if (Input.GetButton("Horizontal")) 
            {
                MainCamera.transform.Translate(Input.GetAxis("Horizontal")*CameraSpeed, 0, 0);
            }

            if (Input.GetButton("Vertical")) 
            {
                MainCamera.transform.Translate(0, 0, Input.GetAxis("Vertical")*CameraSpeed);
            }

            if (Input.GetMouseButton(1)) //Hvis venstremuseknap holdes nede, kan man ændre kameravinklen
            {
                rotation = MainCamera.transform.eulerAngles; //Tag vinklen ud og gem den som en vector2
                rotation.y += Input.GetAxis("Mouse X"); //Adder den ønskede nye vinkel y gennem input fra musen
                rotation.x += -Input.GetAxis("Mouse Y"); //Adder den ønskede nye vinkel x gennem input fra musen
                MainCamera.transform.eulerAngles = rotation; //Tildel den nye vektor til vinkelen
            }
        }

        #region  Metoder til udregning af kameraets postition og vinkel henholdsvis til diget
    
        // Finder midten af et givent dige, i form af et blueprint
        private Vector3 FindMiddleOfDike()
        {
            // https://www.purplemath.com/modules/midpoint.htm midtpunkt formel
            Blueprint blueprint = GetComponent<SimulationController>().Blueprint;
            Vector3 result;

            // Finder midten af diget ud fra alle punkter
            float SumX = 0;
            float SumY = 0;
            float SumZ = 0;

            foreach (Vector3 node in blueprint.ConstructionNodes)
            {
                SumX += node.x;
                SumY += node.y;
                SumZ += node.z;
            }

            int Count = blueprint.ConstructionNodes.Count(); // Antal elementer i listen

            // Udregner og returnerer
            result = new Vector3(SumX / Count, SumY / Count, SumZ / Count);

            // Finder midten af højden og lægger højden til midtpunktet
            result += new Vector3(0, blueprint.DikeHeight / 2, 0);
            return result;

        }

        public float FindDikeLength()
        {
            //Udregner længden af diget i z aksen
            float Result = Math.Abs(GetComponent<SimulationController>().Node1.z - GetComponent<SimulationController>().Node2.z);
            return Result;
        }
        


        // Udregn placering af standardkamera 1
        private Vector3 CalculateCamera1Pos() 
        {
            // Kameraet bliver placeret ved sandsækkes genereringspunkt
            Vector3 result = GetComponent<SandbagSpawner>().SpawnPoint;
            result.y = GetComponent<SimulationController>().Blueprint.DikeHeight / 2;

            return result;
        }

        // Udregn placering af standardkamera 2
        private Vector3 CalculateCamera2() 
        {
            // Kameratet bliver placeret i midten af diget med en højde så kameraet svæver over diget
            Vector3 result = FindMiddleOfDike();

            // Tilføjer en højde på kameret baseret på diget længden
            float Dikelenght = Math.Abs(GetComponent<SimulationController>().Node1.z - GetComponent<SimulationController>().Node2.z); //Udregner længden af diget i z aksen
            result = result + (new Vector3(0, 1.5f, 0) * Dikelenght);

            return result;
        }

        // Udregn placering af standardkamera 3
        private Vector3 CalculateCamera3() 
        {
            // Kameratet bliver placeret i diget midte og roteres således at diget ses fra siden af
            Vector3 result = FindMiddleOfDike() + new Vector3(0, 0, -1.25f) * (float)Math.Pow(FindDikeLength(), 0.90); //Skalerer. Potens fordi der kræves mindre og mindre zoom når diget bliver længere

            return result;
        }

        #endregion

        #region Metoder til valg af standard kamera

        // Standardposition 1: Forfra
        private void SelectCameraPos1() 
        {
            MainCamera.transform.position = CalculateCamera1Pos(); //Sætter kameraet i midten af hvor sandsække genereres
            MainCamera.transform.LookAt(FindMiddleOfDike()); //Vender kameraet således at det kigger på diget
            MainCamera.transform.position += new Vector3(-1, 1, 0); //Offset for at den ikke sidder inde i dronerne.
        }

        // Stardardposition 2: Ovenfra
        private void SelectCameraPos2() 
        {
            MainCamera.transform.position = CalculateCamera2();
            MainCamera.transform.eulerAngles = new Vector3(90f, 0f, 0f);
        }

        // Standardposition 3: Sidefra
        private void SelectCameraPos3() 
        {
            MainCamera.transform.position = CalculateCamera3();
            MainCamera.transform.rotation.SetLookRotation(GetComponent<SandbagSpawner>().SpawnPoint);
            MainCamera.transform.eulerAngles = new Vector3(0f, 0f, 0f); //Rotation sættes til 0 så kameraet vender mod diget
        }
        #endregion
    }
}
