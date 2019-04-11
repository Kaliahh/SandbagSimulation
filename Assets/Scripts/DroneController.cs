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
        bool HasBuildingBegun;
        bool IsFirstSandbagPlaced;

        int Step;
        int NumberOfSteps;

        float ViewDistance;
        float SafeHeight;

        public float DroneSandbagDistance { get; private set; }

        public Section MySection { get; private set; }
        public Blueprint MyBlueprint { get; private set; }
        // public DroneMovement MyMovement { get; private set; }

        public GameObject MySandbag;
        public GameObject LocatedSandbag { get; private set; }

        public Vector3 SandbagPickUpLocation { get; private set; }

        Vector3 BlueprintCentre;
        Vector3[] PossiblePlaces;

        //Vector3 CurrentSection;
        Vector3 DroneTargetPoint;
        Vector3 HomePosition;
        Vector3 SandbagTargetPoint;

        Vector3 AboveSection;

        Action<Vector3> FlyTo;

        private void Start()
        {
            IsFinishedBuilding = false;
            HasBuildingBegun = false;
            IsFirstSandbagPlaced = false;
            IsRightDrone = (UnityEngine.Random.Range(0, 2) == 1) ? true : false; // Halvdelen af alle droner flyver til høre, resten til venstre

            ViewDistance = 50f;
            DroneSandbagDistance = 1f;
            SafeHeight = 2f;

            Step = 0;
            NumberOfSteps = 60;

            MySandbag = null;
            MySection = new Section();

            FlyTo = this.GetComponent<DroneMovement>().FlyTo;

            //MyMovement = this.GetComponent<DroneMovement>();

            HomePosition = this.transform.position;

            BlueprintCentre = Vector3.Lerp(MyBlueprint.ConstructionNodes[0], MyBlueprint.ConstructionNodes[1], 0.5f);

            MySection.CurrentSection = Vector3.zero;
            SandbagTargetPoint = Vector3.zero;

            AboveSection = new Vector3(BlueprintCentre.x, MyBlueprint.DikeHeight + SafeHeight, BlueprintCentre.z);

            this.transform.Rotate(new Vector3(90, 0, 0));
        }

        private void Update()
        {
            Debug.DrawLine(MyBlueprint.ConstructionNodes[0], MyBlueprint.ConstructionNodes[1], Color.red);

            if (IsFinishedBuilding == false)
            {
                // MakeDecision();
                MakeDecision2();
            }

            else
            {
                FlyTo(HomePosition);
            }
        }

        private void MakeDecision()
        {
            // Hvis dronen har fundet en sandsæk, men den ikke er tagged med "Sandbag" sættes LocatedSandbag til null
            if (LocatedSandbag != null && LocatedSandbag.tag != "Sandbag")
            {
                LocatedSandbag = null;
            }

            // Dronen har ikke fundet en sandsæk, flyv til opsamlingssted
            if (InVicinityOf(SandbagPickUpLocation) == false && LocatedSandbag == null && MySandbag == null)
            {
                FlyTo(SandbagPickUpLocation);
            }

            // Dronen har ikke fundet en sandsæk, og er kommet til opsamlingssted, finder en sandsæk
            else if (InVicinityOf(SandbagPickUpLocation) == true && LocatedSandbag == null && MySandbag == null)
            {
                LocateNearestSandbag();
            }

            // Dronen har fundet en sandsæk, flyv til sandsækken
            else if (LocatedSandbag != null && MySandbag == null && InVicinityOf(LocatedSandbag.transform.position) == false)
            {
                FlyToSandbag(LocatedSandbag.transform.position);
            }

            // Dronen er kommet hen til den fundne sandsæk, saml den op
            else if (LocatedSandbag != null && MySandbag == null && InVicinityOf(LocatedSandbag.transform.position) == true)
            {
                PickUpSandbag();
            }

            // Dronen har samlet en sandsæk op, men den ved ikke hvor den skal bygge, find midten af diget
            else if (MySection.CurrentSection == Vector3.zero && MySandbag != null)
            {
                MySection.CurrentSection = CalculateAbovePoint(BlueprintCentre);
            }

            // Dronen har samlet en sandsæk op, og ved hvor den skal bygge, flyv til det område
            else if (MySandbag != null && MySection.CurrentSection != Vector3.zero && InVicinityOf(MySection.CurrentSection) == false && SandbagTargetPoint == Vector3.zero)
            {
                FlyTo(MySection.CurrentSection);
            }

            // Konstruktionen af diget er ikke påbegyndt endnu, find midten af diget og placer 
            else if (MySandbag != null && InVicinityOf(MySection.CurrentSection) == true && SandbagTargetPoint == Vector3.zero && IsCurrentSectionCenter() && HasBuildingBegun == false)
            {
                PossiblePlaces = MySection.FindPlace(ViewDistance, this.transform.position, MyBlueprint);

                if (PossiblePlaces == null)
                {
                    // TODO: ret 0.5 til sandsæks højde * 0.5
                    SandbagTargetPoint = new Vector3(BlueprintCentre.x, 0.5f, BlueprintCentre.z);
                    DroneTargetPoint = new Vector3(SandbagTargetPoint.x, SandbagTargetPoint.y + DroneSandbagDistance, SandbagTargetPoint.z);
                    HasBuildingBegun = true;
                }

                else if (PossiblePlaces != null)
                {
                    HasBuildingBegun = true;
                }
            }

            else if (MySandbag != null && SandbagTargetPoint != Vector3.zero && InVicinityOf(DroneTargetPoint) == false)
            {
                FlyTo(DroneTargetPoint);
            }

            else if (MySandbag != null && SandbagTargetPoint != Vector3.zero && InVicinityOf(DroneTargetPoint) == true)
            {
                PlaceSandbag();
                SandbagTargetPoint = Vector3.zero;
                DroneTargetPoint = Vector3.zero;
            }

            else if (MySandbag != null && InVicinityOf(MySection.CurrentSection) == true && SandbagTargetPoint == Vector3.zero && HasBuildingBegun == true)
            {
                PossiblePlaces = MySection.FindPlace(ViewDistance, this.transform.position, MyBlueprint);

                if (PossiblePlaces == null)
                {
                    MySection.CurrentSection = MySection.FindNextSection(ViewDistance, this.transform.position, IsRightDrone, MyBlueprint);
                }
                else
                {
                    SandbagTargetPoint = MySection.FindBestPlace(PossiblePlaces, this.transform.position, ViewDistance);
                    DroneTargetPoint = new Vector3(SandbagTargetPoint.x, SandbagTargetPoint.y + DroneSandbagDistance, SandbagTargetPoint.z);
                }
            }

            else
            {
                FlyTo(HomePosition);
            }
        }

        private void MakeDecision2()
        {
            // Hvis dronen har fundet en sandsæk, men den ikke er tagged med "Sandbag" sættes LocatedSandbag til null
            if (LocatedSandbag != null && LocatedSandbag.tag != "Sandbag")
            {
                LocatedSandbag = null;
                Step = 1;
            }

            switch (Step)
            {
                case 0:
                    FlyTo(SandbagPickUpLocation);
                    if (InVicinityOf(SandbagPickUpLocation))
                        Stepper();
                    break;

                case 1:
                    LocateNearestSandbag();
                    if (LocatedSandbag != null)
                        Stepper();
                    // TODO: Gå til et senere step med else
                    break;

                case 2:
                    FlyToSandbag(LocatedSandbag.transform.position);
                    if (InVicinityOf(LocatedSandbag.transform.position))
                        Stepper();
                    break;

                case 3:
                    PickUpSandbag();
                    if (MySandbag != null)
                    {
                        Stepper();

                        if (MySection.CurrentSection != Vector3.zero)
                        {
                            MySection.CurrentSection = BlueprintCentre;
                        }
                            
                    }
                    // TODO: Måske else?


                    break;

                case 4:
                    

                default:
                    Step = 0;
                    break;
            }
        }

        private void Stepper()
        {
            Step = (Step + 1) % NumberOfSteps; 
        }

        private bool IsCurrentSectionCenter()
        {
            return MySection.CurrentSection == CalculateAbovePoint(BlueprintCentre);
        }

        private Vector3 CalculateAbovePoint(Vector3 point)
        {
            return new Vector3(point.x, MyBlueprint.DikeHeight + SafeHeight, point.z);
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
            FlyTo(new Vector3(sandbagPosition.x, sandbagPosition.y + DroneSandbagDistance, sandbagPosition.z));
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

            // MySandbag.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - DroneSandbagDistance, this.transform.position.z);

            MySandbag.GetComponent<Rigidbody>().isKinematic = false;

            MySandbag = null;
        }

        private void RotateSandbag(Vector3 position)
        {
            Vector3 point1 = MyBlueprint.ConstructionNodes[0];
            Vector3 point2 = MyBlueprint.ConstructionNodes[1];

            Vector3 dikeVector = point1 - point2;

            float angle = Vector3.SignedAngle(Vector3.right, dikeVector, Vector3.up);

            MySandbag.transform.Rotate(new Vector3(0, angle, 0));
        }

        public void SetBlueprint(Blueprint blueprint) => MyBlueprint = blueprint;

        public void SetSpeed(float speed) => this.GetComponent<DroneMovement>().Speed = speed;

        public void SetSandbagPickUpLocation(Vector3 point) => SandbagPickUpLocation = point;
    }
}


