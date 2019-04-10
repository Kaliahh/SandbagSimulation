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

        float ViewDistance;

        public Section MySection { get; private set; }
        public Blueprint MyBlueprint { get; private set; }
        public DroneMovement MyMovement; //{ get; private set; }

        public GameObject MySandbag;
        public GameObject LocatedSandbag { get; private set; }

        public Vector3 SandbagPickUpLocation { get; private set; }

        Vector3 CurrentSection;
        Vector3 DroneTargetPoint;
        Vector3 HomePosition;
        Vector3 TargetPoint;

        private void Start()
        {
            IsFinishedBuilding = false;
            IsRightDrone = (UnityEngine.Random.Range(0, 2) == 1) ? true : false; // Halvdelen af alle droner flyver til høre, resten til venstre
            ViewDistance = 50f;

            MySandbag = null;
            MySection = new Section();

            MyMovement = this.GetComponent<DroneMovement>();

            HomePosition = this.transform.position;

            CurrentSection = Vector3.zero;
            TargetPoint = Vector3.zero;
        }

        private void Update()
        {
            //Debug.DrawLine(MyBlueprint.ConstructionNodes[0], MyBlueprint.ConstructionNodes[1], Color.red);

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

            else if (LocatedSandbag == null && MySandbag == null && InVicinityOf(SandbagPickUpLocation) == true)
            {
                LocateNearestSandbag();
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
                CurrentSection = Vector3.Lerp(MyBlueprint.ConstructionNodes[0], MyBlueprint.ConstructionNodes[1], 0.5f);
            }

            else if (MySandbag != null && CurrentSection != Vector3.zero && InVicinityOf(CurrentSection) == false && TargetPoint == Vector3.zero)
            {
                MyMovement.FlyTo(CurrentSection);
            }

            else if (MySandbag != null && InVicinityOf(CurrentSection) == true && TargetPoint == Vector3.zero)
            {
                TargetPoint = FindPlace();
                DroneTargetPoint = new Vector3(TargetPoint.x, TargetPoint.y + 1, TargetPoint.z);
            }

            else if (MySandbag != null && TargetPoint != Vector3.zero && InVicinityOf(DroneTargetPoint) == false)
            {
                MyMovement.FlyTo(DroneTargetPoint);
            }

            else if (MySandbag != null && TargetPoint != Vector3.zero && InVicinityOf(DroneTargetPoint) == true)
            {
                PlaceSandbag();
                TargetPoint = Vector3.zero;
                DroneTargetPoint = Vector3.zero;
            }

            else
            {
                MyMovement.FlyTo(HomePosition);
            }
        }

        private Vector3 FindPlace()
        {
            return Vector3.Lerp(MyBlueprint.ConstructionNodes[0], MyBlueprint.ConstructionNodes[1], UnityEngine.Random.Range(0f, 1f));
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

        //private void SearchForSandbags()
        //{
        //    LocateNearestSandbag();

        //    if (LocatedSandbag != null)
        //    {
        //        NoSandbagFound = false;
        //    }
        //}
        /* */

        // Finder den nærmeste sandsæk, og gemmer den i LocatedSandbag
        public void LocateNearestSandbag()
        {
            GameObject[] sandbags = GameObject.FindGameObjectsWithTag("Sandbag");

            float distance = ViewDistance;

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

            MySandbag.GetComponent<Rigidbody>().isKinematic = true;
            LocatedSandbag = null;
        }

        // Placerer MySandbag i et givent punkt, og sætter referencen til sandsækken (MySandbag) til null
        public void PlaceSandbag()
        {
            MySandbag.tag = "PlacedSandbag";

            // MySandbag.GetComponent<SandbagController>().rb.velocity = Vector3.zero;
            RotateSandbag(this.transform.position);

            // MySandbag.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 1f, this.transform.position.z);

            MySandbag.GetComponent<Rigidbody>().isKinematic = false;

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

        //public void SetSpeed(float speed) => MyMovement.Speed = speed;
        public void SetSpeed(float speed)
        {
            MyMovement.Speed = speed;
        }

        public void SetSandbagPickUpLocation(Vector3 point) => SandbagPickUpLocation = point;
    }
}


