using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SandbagSimulation
{
    public class DroneController : MonoBehaviour
    {
        bool IsFinished;
        bool IsRightDrone;
        bool NoSandbagFound;

        float ViewDistance;

        Section MySection;
        Blueprint MyBlueprint;
        DroneMovement MyMovement;

        public GameObject MySandbag { get; private set; }
        public GameObject LocatedSandbag { get; private set; }

        GameObject obj1;
        GameObject obj2;

        Vector3 StartingPosition;
        Vector3 TargetPoint;

        void Start()
        {
            IsFinished = false;
            NoSandbagFound = false;
            IsRightDrone = (UnityEngine.Random.Range(0, 2) == 1) ? true : false; // Halvdelen af alle droner flyver til høre, resten til venstre
            ViewDistance = 10f;

            MySandbag = null;
            MyBlueprint = new Blueprint(new List<Vector3>(), 4);
            MySection = new Section(Vector3.zero);

            MyMovement = this.GetComponent<DroneMovement>();
            StartingPosition = this.transform.position;


            // Dige nodes
            obj1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj1.transform.position = new Vector3(2, 0.5f, 0);
            MyBlueprint.ConstructionNodes.Add(obj1.transform.position);

            obj2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj2.transform.position = new Vector3(-2, 0.5f, 0);
            MyBlueprint.ConstructionNodes.Add(obj2.transform.position);
        }

        void Update()
        {
            MyBlueprint.ConstructionNodes[0] = obj1.transform.position;
            MyBlueprint.ConstructionNodes[1] = obj2.transform.position;

            Debug.DrawLine(MyBlueprint.ConstructionNodes[0], MyBlueprint.ConstructionNodes[1], Color.red);

            if (IsFinished == false)
            {
                DoStuff();
            }

            else
            {
                MyMovement.FlyTo(StartingPosition);
            }
        }

        void DoStuff()
        {
            if (LocatedSandbag == null && MySandbag == null && NoSandbagFound == false)
            {
                LocateNearestSandbag();

                if (LocatedSandbag != null)
                {
                    TargetPoint = LocatedSandbag.transform.position;
                }

                else
                {
                    NoSandbagFound = true;
                }
            }

            else if (LocatedSandbag == null && MySandbag == null && NoSandbagFound == true)
            {
                //TODO: Implementer det her
                SearchForSandbags();
            }

            else if (LocatedSandbag != null && MySandbag == null && InVicinityOf(TargetPoint) == false)
            {
                FlyToSandbag(TargetPoint);
            }

            else if (LocatedSandbag != null && MySandbag == null && InVicinityOf(TargetPoint) == true)
            {
                PickUpSandbag();
            }
        }

        bool InVicinityOf(Vector3 position)
        {
            if ((this.transform.position - position).sqrMagnitude < 0.5f)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        void FlyToSandbag(Vector3 sandbagPosition)
        {
            MyMovement.FlyTo(new Vector3(sandbagPosition.x, sandbagPosition.y + 1f, sandbagPosition.z));
        }

        void SearchForSandbags()
        {
            throw new NotImplementedException();
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
    }
}


