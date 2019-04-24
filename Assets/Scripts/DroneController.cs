using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SandbagSimulation
{
    public class DroneController : MonoBehaviour
    {
        bool IsFinishedBuilding;
        bool IsHomeAndDone;
        public bool IsRightDrone;
        bool HasBuildingBegun;

        int Step;

        public float ViewDistance { get; set; }
        float SafeHeight;
        public float DroneSandbagDistance { get; private set; }

        public Section MySection { get; private set; }
        public Blueprint MyBlueprint { get; private set; }

        public GameObject MySandbag { get; private set; }
        public GameObject LocatedSandbag { get; private set; }

        private List<GameObject> OtherDrones; // TODO: Implementer undgåelse af andre droner

        public Vector3 SandbagPickUpLocation { get; private set; }

        public event EventHandler FinishedBuilding;

        Vector3 BlueprintCentre;
        Vector3[] PossiblePlaces;

        Vector3 DroneTargetPoint;
        Vector3 SandbagTargetPoint;
        
        Vector3 AboveSection;
        Vector3 AboveTarget;

        Vector3 HomePosition;

        Action<Vector3> FlyTo;

        private void Start()
        {
            IsFinishedBuilding = false;
            HasBuildingBegun = false;
            IsHomeAndDone = false;

            DroneSandbagDistance = 1f;
            SafeHeight = 2f;
            Step = 0;

            MySandbag = null;
            SandbagTargetPoint = Vector3.zero;

            MySection = new Section();
            MySection.CurrentSection = Vector3.zero;
            AboveSection = new Vector3(BlueprintCentre.x, MyBlueprint.DikeHeight + SafeHeight, BlueprintCentre.z);

            FlyTo = this.GetComponent<DroneMovement>().FlyTo;

            HomePosition = this.transform.position;

            BlueprintCentre = Vector3.Lerp(MyBlueprint.ConstructionNodes[0], MyBlueprint.ConstructionNodes[1], 0.5f);
            
            // Kun visuelt
            this.transform.Rotate(new Vector3(90, 0, 0));
        }

        private void Update()
        {
            Debug.DrawLine(MyBlueprint.ConstructionNodes[0], MyBlueprint.ConstructionNodes[1], Color.red);

            if (IsHomeAndDone == false)
            {
                if (IsFinishedBuilding == false)
                {
                    MakeDecision();
                }

                else
                {
                    FlyTo(HomePosition);

                    if (InVicinityOf(HomePosition))
                    {
                        // Gør noget
                        IsHomeAndDone = true;
                        FinishedBuilding(this, null);
                    }
                }
            }
        }

        // Beslutter hvad dronen skal foretage sig for hver frame
        private void MakeDecision()
        {
            // Hvis dronen har fundet en sandsæk, men den ikke er tagged med "Sandbag" sættes LocatedSandbag til null
            if (LocatedSandbag != null && LocatedSandbag.tag != "Sandbag")
            {
                LocatedSandbag = null;
                Step = 1;
            }

            // Det her forudsætter at algoritmen der finder placeringer er perfekt
            if (SandbagTargetPoint == new Vector3(-100f, -100f, -100f))
            {
                IsFinishedBuilding = true;
            }

            else
            {
                // Dikterer rækkefølgen af handlinger dronen skal foretage. En opskrift for diget
                switch (Step)
                {
                    case 0: FlyToSandbagPickUpLocation(); break;

                    case 1: FindSandbagLocation(); break;

                    case 2: FlyToLocatedSandbag(); break;

                    case 3: PickUpLocatedSandbag(); break;

                    case 4: FlyToSection(); break;

                    case 5: SearchForSandbagPlace(); break;

                    case 6: FlyToAboveTarget(); break;

                    case 7: FlyToDroneTarget(); break;

                    case 8: PlaceMySandbag(); break;

                    case 9: ReturnToAboveTarget(); break;

                    default: Step = 0; break;
                }
            }
        }

        #region Handlingsplan

        // Flyver dronen hen til opsamlingssted for sandsække. Når den kommer i nærheden, tælles step op med 1
        private void FlyToSandbagPickUpLocation()
        {
            FlyTo(SandbagPickUpLocation);
            if (InVicinityOf(SandbagPickUpLocation))
                Step++;
        }

        // Finder den nærmeste sandsæk, og tæller step op med 1 hvis den finder en. 
        // Sætter step til 0, hvis den ikke kunne finde en sandsæk
        private void FindSandbagLocation()
        {
            LocateNearestSandbag();
            if (LocatedSandbag != null)
                Step++;
            else
                Step = 0;
        }

        // Flyver dronen hen til den sandsæk den har fundet, tæller step op med 1 når den kommer i nærheden
        private void FlyToLocatedSandbag()
        {
            FlyTo(DroneTargetPoint);
            if (InVicinityOf(DroneTargetPoint))
                Step++;
        }

        // Samler den fundne sandsæk op, og finder en sektion den kan arbejde på, hvis dronen ikke allerede har en
        private void PickUpLocatedSandbag()
        {
            PickUpSandbag();
            if (MySandbag != null)
            {
                Step++;

                if (MySection.CurrentSection == Vector3.zero)
                {
                    MySection.CurrentSection = BlueprintCentre;
                    AboveSection = CalculateAbovePoint(MySection.CurrentSection);
                }
            }
            // TODO: Måske else?
        }

        // Flyver dronen hen til et punkt over dens aktuelle sektion
        // Når den kommer i nærheden, tælles step op med 1, og dronen finder ud af hvor den skal sætte sandsækken
        private void FlyToSection()
        {
            FlyTo(AboveSection);
            if (InVicinityOf(AboveSection))
                Step++;
        }

        private void SearchForSandbagPlace()
        {
            FindSandbagPlace();
            Step++;
        }        

        // Flyver dronen hen over det sted hvor sandsækken skal placeres
        private void FlyToAboveTarget()
        {
            FlyTo(AboveTarget);

            if (InVicinityOf(AboveTarget))
            {
                if (IsPlaceStillAvailable() == false)
                {
                    Step = 5;
                }

                else
                    Step++;
            }
        }

        // Flyver dronen hen til det sted den skal være, for at placere sandsækken rigtigt
        private void FlyToDroneTarget()
        {
            FlyTo(DroneTargetPoint);

            if (InVicinityOf(DroneTargetPoint))
                Step++;
        }

        // Placerer den sandsæk dronen bære rundt på
        private void PlaceMySandbag()
        {


            PlaceSandbag();
            Step++;
        }

        // Flyver tilbage til punktet over der hvor dronen har placeret sandsækken. 
        // Step sættes til 0, processen starter forfra
        private void ReturnToAboveTarget()
        {
            FlyTo(AboveTarget);
            if (InVicinityOf(AboveTarget))
                Step = 0;
        }
        #endregion

        #region Handlinger

        // Checker om den første sandsæk i diget er placeret
        // Returnerer true hvis der er en sandsæk placeret, falsk hvis den ikke kan finde en
        private bool IsFirstSandbagPlaced()
        {
            GameObject placedSandbag = GameObject.FindGameObjectWithTag("PlacedSandbag");

            if (placedSandbag != null && Vector3.Distance(this.transform.position, placedSandbag.transform.position) <= ViewDistance)
            {
                return true;
            }

            else //TODO: Det her er måske farligt
                return false;
        }

        // Finder det sted dronen skal placere sandsækken. Tager højde for om den første sandsæk er placeret
        private void FindSandbagPlace()
        {
            if (HasBuildingBegun == false && IsFirstSandbagPlaced() == false)
            {
                FindFirstSandbagPlace();
            }

            else if (HasBuildingBegun == false && IsFirstSandbagPlaced() == true)
            {
                HasBuildingBegun = true;
                FindNextSandbagPlace();
            }

            else
            {
                FindNextSandbagPlace();
            }
        }

        // Finder det sted hvor den første sandsæk skal placeres i diget
        private void FindFirstSandbagPlace()
        {
            PossiblePlaces = MySection.FindPlace(ViewDistance, this.transform.position, MyBlueprint);

            if (PossiblePlaces == null)
            {
                // TODO: ret 0.5 til sandsæks højde * 0.5
                SandbagTargetPoint = new Vector3(BlueprintCentre.x, 0.5f, BlueprintCentre.z);
                SetDroneTargetPoint(SandbagTargetPoint);
                AboveTarget = CalculateAbovePoint(SandbagTargetPoint);
                HasBuildingBegun = true;
            }

            else if (PossiblePlaces != null)
            {
                HasBuildingBegun = true;
            }
        }

        // Finder det sted hvor sandsækken skal placeres, ud fra de sandsække der allerede er placeret
        // Hvis den ikke kan finde en mulig placering, finder den den næste sektion dronen skal arbejde på
        private void FindNextSandbagPlace()
        {
            PossiblePlaces = MySection.FindPlace(ViewDistance, this.transform.position, MyBlueprint);

            if (PossiblePlaces == null)
            {
                MySection.CurrentSection = MySection.FindNextSection(ViewDistance, this.transform.position, IsRightDrone, MyBlueprint);
                AboveSection = CalculateAbovePoint(MySection.CurrentSection);
            }
            else
            {
                SandbagTargetPoint = MySection.FindBestPlace(PossiblePlaces, this.transform.position, ViewDistance);
                SetDroneTargetPoint(SandbagTargetPoint);
                AboveTarget = CalculateAbovePoint(SandbagTargetPoint);
            }
        }

        // Checker om dronens aktuelle sektion er midten af diget
        // Returnerer sand hvis det er
        private bool IsCurrentSectionCenter()
        {
            return MySection.CurrentSection == BlueprintCentre;
        }

        // Input: Vector 3 point
        // Output: Returnerer points x og z, men lægger digets højde og en sikkerhedshøjde til points z
        private Vector3 CalculateAbovePoint(Vector3 point)
        {
            return new Vector3(point.x, 0.5f * MyBlueprint.DikeHeight + SafeHeight, point.z);
        }

        // Checker om dronen er i nærheden af en Vector3 position
        // Hvis den er, returnerer den sand, ellers returnerer den falsk
        private bool InVicinityOf(Vector3 position)
        {
            if ((this.transform.position - position).magnitude < 0.1f)
                return true;

            else
                return false;
        }

        // Finder den nærmeste sandsæk, og gemmer den i LocatedSandbag
        public void LocateNearestSandbag()
        {
            GameObject[] sandbags = GameObject.FindGameObjectsWithTag("Sandbag");

            float distance = ViewDistance;

            foreach (GameObject sandbag in sandbags)
            {
                Vector3 diff = sandbag.transform.position - this.transform.position;
                float currentDistance = diff.magnitude;

                if (currentDistance < distance)
                {
                    LocatedSandbag = sandbag;
                    distance = currentDistance;
                }
            }

            if (LocatedSandbag != null)
            {
                SetDroneTargetPoint(LocatedSandbag.transform.position);
            }
        }

        // Gemmer den fundne sandsæk i MySandbag, og den skal nu transporteres
        // Den fundne sandsæk (LocatedSandbag) sættes til null
        public void PickUpSandbag()
        {
            MySandbag = LocatedSandbag;
            MySandbag.tag = "PickedUpSandbag";
            MySandbag.layer = 2;
            this.gameObject.layer = 0; // Sørger for at dronerne ikke kommer alt for meget i vejen for hinanden

            MySandbag.GetComponent<Rigidbody>().isKinematic = true;
            LocatedSandbag = null;
        }

        // Placerer MySandbag i et givent punkt, og sætter referencen til sandsækken (MySandbag) til null
        public void PlaceSandbag()
        {
            MySandbag.tag = "PlacedSandbag";
            MySandbag.layer = 0;
            this.gameObject.layer = 2; // Sørger for at dronerne ikke kommer alt for meget i vejen for hinanden

            // MySandbag.GetComponent<SandbagController>().rb.velocity = Vector3.zero;
            RotateSandbag(this.transform.position);

            // MySandbag.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - DroneSandbagDistance, this.transform.position.z);

            MySandbag.GetComponent<Rigidbody>().isKinematic = false;

            MySandbag = null;
        }

        // Roterer sandsækken i forhold til diget og det sted sandsækken skal placeres
        private void RotateSandbag(Vector3 position)
        {
            Vector3 point1 = MyBlueprint.ConstructionNodes[0];
            Vector3 point2 = MyBlueprint.ConstructionNodes[1];

            Vector3 dikeVector = point1 - point2;

            float angle = Vector3.SignedAngle(Vector3.right, dikeVector, Vector3.up);

            MySandbag.transform.Rotate(new Vector3(0, angle, 0));
        }

        private void SetDroneTargetPoint(Vector3 point)
        {
            DroneTargetPoint = new Vector3(point.x, point.y + DroneSandbagDistance, point.z);
        }

        private bool IsPlaceStillAvailable()
        {
            return !Physics.Linecast(this.transform.position, SandbagTargetPoint);

            
            //return !Physics.Linecast(this.transform.position, SandbagTargetPoint, 10) && !Physics.Linecast(this.transform.position, SandbagTargetPoint, 9);
        }

        #endregion

        #region Sættere
        public void SetBlueprint(Blueprint blueprint) => MyBlueprint = blueprint;

        public void SetSpeed(float speed) => this.GetComponent<DroneMovement>().Speed = speed;

        public void SetSandbagPickUpLocation(Vector3 point) => SandbagPickUpLocation = point;

        public void SetViewDistance(float distance) => ViewDistance = distance;

        public void SetOtherDrones(List<GameObject> list) => OtherDrones = list;
        #endregion
    }
}


