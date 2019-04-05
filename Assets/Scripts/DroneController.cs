using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SandbagSimulation
{
    public class DroneController : MonoBehaviour
    {
        bool IsFinished;
        bool HasSandbag;
        bool IsRightDrone;

        float ViewDistance;

        Section MySection;
        Blueprint MyBlueprint;
        //DroneMovement MyMovement;

        public GameObject MySandbag { get; private set; }
        public GameObject LocatedSandbag { get; private set; }

        delegate void Movement(Vector3 target);
        Movement FlyTo;

        delegate float Rotate();

        void Start()
        {
            IsFinished = false;
            HasSandbag = false;
            IsRightDrone = (UnityEngine.Random.Range(0, 2) == 1) ? true : false; // Halvdelen af alle droner flyver til høre, resten til venstre
            ViewDistance = 10f;

            MySandbag = null;
            MyBlueprint = null;
            MySection = new Section(Vector3.zero);

            FlyTo = new DroneMovement().FlyTo;

            LocateNearestSandbag();
            PickUpSandbag();
            PlaceSandbag(new Vector3(0, 2, 0));
        }

        void Update()
        {
            //transform.Rotate(new Vector3(20, 0, 0));

        }

        // Finder den nærmeste sandsæk, og gemmer den i LocatedSandbag
        public void LocateNearestSandbag()
        {
            GameObject[] sandbags = GameObject.FindGameObjectsWithTag("Sandbag");

            float distance = Mathf.Infinity;

            foreach (GameObject sandbag in sandbags)
            {
                Vector3 diff = sandbag.transform.position - this.transform.position;
                float currentDistance = diff.sqrMagnitude;

                if (currentDistance < distance)
                {
                    LocatedSandbag = sandbag;
                    distance = currentDistance;
                }
            }
        }

        // Gemmer den fundne sandsæk i MySandbag, og den skal nu transporteres
        public void PickUpSandbag() => MySandbag = LocatedSandbag;

        // Placerer MySandbag i et givent punkt, og sætter referencen til sandsækken (MySandbag) til null
        public void PlaceSandbag(Vector3 position)
        {
            MySandbag.tag = "PlacedSandbag";

            float xAngle = RotateParameterAroundAxis("x");
            float yAngle = RotateParameterAroundAxis("y");
            float zAngle = RotateParameterAroundAxis("z");

            MySandbag.transform.Rotate(xAngle, yAngle, zAngle);

            MySandbag.transform.position = position;
            MySandbag = null;
        }

        // TODO: Kom tilbage hertil!
        public float RotateParameterAroundAxis(string axis)
        {
            Vector3 point1 = new Vector3(10, 0, 0);
            Vector3 point2 = new Vector3(10, 5, 0);

            Vector3 dike = point1 - point2;
            Vector3 sandbagRotation = MySandbag.transform.rotation.eulerAngles;

            if (axis.CompareTo("x") == 0)
            {
                dike = new Vector3(dike.x, 0, 0);
                sandbagRotation = new Vector3(sandbagRotation.x, 0, 0);

                return Vector3.Angle(dike, sandbagRotation);
            } 

            else if (axis.CompareTo("y") == 0)
            {
                dike = new Vector3(0, dike.y, 0);
                sandbagRotation = new Vector3(0, sandbagRotation.y, 0);

                return Vector3.Angle(dike, sandbagRotation);
            }

            else if (axis.CompareTo("z") == 0)
            {
                dike = new Vector3(0, 0, dike.z);
                sandbagRotation = new Vector3(0, 0, sandbagRotation.y);

                return Vector3.Angle(dike, sandbagRotation);
            }

            else
            {
                throw new Exception("Det her burde virkelig ikke kunne lade sig gøre...");
            }
        }

        public Vector3 CalculateRelativeRotation()
        {
            Vector3 rotation = Vector3.zero;

            Vector3 point1 = new Vector3(10, 0, 0);
            Vector3 point2 = new Vector3(10, 5, 0);

            Vector3 dike = point1 - point2;

            float angle = Vector3.Angle(dike, MySandbag.transform.localEulerAngles);

            return rotation;
        }
    }
}


