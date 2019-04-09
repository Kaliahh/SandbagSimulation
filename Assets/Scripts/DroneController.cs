using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SandbagSimulation
{
    public class DroneController : MonoBehaviour
    {
        bool IsFinishedBuilding;
        bool IsRightDrone;
        bool NoSandbagFound;

        float ViewDistance;

        public Section MySection { get; private set; }
        public Blueprint MyBlueprint { get; private set; }
        public DroneMovement MyMovement { get; private set; }

        public GameObject MySandbag;
        public GameObject LocatedSandbag { get; private set; }

        Vector3 HomePosition;
        Vector3 TargetPoint;
        public Vector3 SandbagPickUpLocation { get; private set; }

        Vector3 CurrentSection;

        private void Start()
        {
            IsFinishedBuilding = false;
            NoSandbagFound = false;
            IsRightDrone = (UnityEngine.Random.Range(0, 2) == 1) ? true : false; // Halvdelen af alle droner flyver til høre, resten til venstre
            ViewDistance = 10f;

            MySandbag = null;
            MySection = new Section(Vector3.zero);

            MyMovement = this.GetComponent<DroneMovement>();
            HomePosition = this.transform.position;

            CurrentSection = Vector3.zero;
        }

        private void Update()
        {
            // Debug.DrawLine(MyBlueprint.ConstructionNodes[0], MyBlueprint.ConstructionNodes[1], Color.red);

            if (IsFinishedBuilding == false)
            {
                MakeDecision();
            }

            else
            {
                MyMovement.FlyTo(HomePosition);
            }
        }

        private void MakeDecision()
        {
            if (LocatedSandbag != null && LocatedSandbag.tag != "Sandbag")
            {
                LocatedSandbag = null;
            }

            if (LocatedSandbag == null && MySandbag == null && InVicinityOf(SandbagPickUpLocation) == false)
            {
                MyMovement.FlyTo(SandbagPickUpLocation);
            }

            else if (LocatedSandbag == null && MySandbag == null && InVicinityOf(SandbagPickUpLocation) == true && NoSandbagFound == false)
            {
                LocateNearestSandbag();

                if (LocatedSandbag == null)
                {
                    NoSandbagFound = true;
                }
            }

            else if (LocatedSandbag == null && MySandbag == null && NoSandbagFound == true)
            {
                //TODO: Implementer det her
                SearchForSandbags();
            }

            else if (LocatedSandbag != null && MySandbag == null && InVicinityOf(LocatedSandbag.transform.position) == false)
            {
                FlyToSandbag(LocatedSandbag.transform.position);
            }

            else if (LocatedSandbag != null && MySandbag == null && InVicinityOf(LocatedSandbag.transform.position) == true)
            {
                PickUpSandbag();
            }

            else if (MySandbag != null && CurrentSection == Vector3.zero)
            {
                CurrentSection = Vector3.Lerp(MyBlueprint.ConstructionNodes[0], MyBlueprint.ConstructionNodes[1], UnityEngine.Random.Range(0f, 1f));
            }

            else if (MySandbag != null && CurrentSection != Vector3.zero && InVicinityOf(CurrentSection) == false)
            {
                MyMovement.FlyTo(CurrentSection);
            }

            else if (MySandbag != null && InVicinityOf(CurrentSection) == true)
            {
                Vector3 position = new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, this.transform.position.z);
                MySandbag.GetComponent<SandbagController>().rb.velocity = Vector3.zero;
                PlaceSandbag(position);
                CurrentSection = Vector3.zero;
            }

            else
            {
                MyMovement.FlyTo(HomePosition);
            }
        }

        private bool InVicinityOf(Vector3 position)
        {
            if ((this.transform.position - position).sqrMagnitude < 1.2f)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        private void FlyToSandbag(Vector3 sandbagPosition)
        {
            MyMovement.FlyTo(new Vector3(sandbagPosition.x, sandbagPosition.y + 1f, sandbagPosition.z));
        }

        private void SearchForSandbags()
        {
            LocateNearestSandbag();

            if (LocatedSandbag != null)
            {
                NoSandbagFound = false;
            }
        }

        // Finder den nærmeste sandsæk, og gemmer den i LocatedSandbag
        public void LocateNearestSandbag()
        {
            GameObject[] sandbags = GameObject.FindGameObjectsWithTag("Sandbag");

            //if (sandbags.Length == 0)
            //{
            //    NoSandbagFound = true;
            //    return;
            //}

            //TODO: Det her skal være ViewDistance
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
        // Den fundne sandsæk (LocatedSandbag) sættes til null
        public void PickUpSandbag()
        {
            MySandbag = LocatedSandbag;
            MySandbag.tag = "PickedUpSandbag";
            LocatedSandbag = null;
        }

        // Placerer MySandbag i et givent punkt, og sætter referencen til sandsækken (MySandbag) til null
        public void PlaceSandbag(Vector3 position)
        {
            MySandbag.tag = "PlacedSandbag";

            RotateSandbag(position);

            MySandbag.transform.position = position;
            MySandbag = null;
        }

        private void RotateSandbag(Vector3 position)
        {
            Vector3 point1 = MyBlueprint.ConstructionNodes[0];
            Vector3 point2 = MyBlueprint.ConstructionNodes[1];

            Vector3 dikeVector = point1 - point2;

            float angle = Vector3.SignedAngle(Vector3.right, dikeVector, Vector3.up);

            MySandbag.transform.Rotate(new Vector3(0, angle + 90, 0));
        }

        public void SetBlueprint(Blueprint blueprint) => MyBlueprint = blueprint;

        public void SetSpeed(float speed) => MyMovement.Speed = speed;

        public void SetSandbagPickUpLocation(Vector3 point) => SandbagPickUpLocation = point;
    }
}


