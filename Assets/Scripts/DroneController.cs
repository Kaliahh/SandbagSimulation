using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SandbagSimulation
{
    public class DroneController : MonoBehaviour
    {
        #region Fields

        public bool IsHomeAndDone;
        public bool HasBuildingBegun;
        public bool IsRightDrone;
        public bool IsFinishedBuilding;

        public float ViewDistance { get; private set; }
        public float SafeHeight { get; private set; }
        public float DroneSandbagDistance { get; private set; }

        public Section MySection { get; private set; }
        public Blueprint MyBlueprint { get; private set; }

        public GameObject MySandbag { get; set; }
        public GameObject LocatedSandbag { get; private set; }

        public Vector3 SandbagPickUpLocation { get; private set; }

        public SandbagController SandbagReference { get; set; }

        public event EventHandler FinishedBuilding;

        public Vector3 BlueprintCentre { get; private set; }
        public Vector3[] PossiblePlaces { get; set; }

        public Vector3 DroneTargetPoint { get; private set; }
        public Point SandbagTargetPoint { get; private set; }

        public Vector3 AboveSection { get; set; }
        public Vector3 AboveTarget { get; set; }

        public Vector3 HomePosition { get; private set; }

        private Vector3 ErrorVector;

        public Action<Vector3> FlyTo { get; private set; }

        // Dikterer rækkefølgen af handlinger dronen skal foretage. En opskrift for diget
        public IDroneState State;

        public IDroneState FlyToSandbagPickUpLocationState;
        public IDroneState FindSandbagLocationState;
        public IDroneState FlyToLocatedSandbagState;
        public IDroneState PickUpLocatedSandbagState;
        public IDroneState FlyToSectionState;
        public IDroneState SearchForSandbagPlaceState;
        public IDroneState FlyToAboveTargetState;
        public IDroneState FlyToDroneTargetState;
        public IDroneState PlaceMySandbagState;
        public IDroneState ReturnToAboveTargetState;

        #endregion

        private void Start() // TODO: Start skal ryddes lidt op, måske med kommentarer til?
        {
            FlyToSandbagPickUpLocationState = new FlyToSandbagPickUpLocationState(this);
            FindSandbagLocationState = new FindSandbagLocationState(this);
            FlyToLocatedSandbagState = new FlyToLocatedSandbagState(this);
            PickUpLocatedSandbagState = new PickUpLocatedSandbagState(this);
            FlyToSectionState = new FlyToSectionState(this);
            SearchForSandbagPlaceState = new SearchForSandbagPlaceState(this);
            FlyToAboveTargetState = new FlyToAboveTargetState(this);
            FlyToDroneTargetState = new FlyToDroneTargetState(this);
            PlaceMySandbagState = new PlaceMySandbagState(this);
            ReturnToAboveTargetState = new ReturnToAboveTargetState(this);

            State = FlyToSandbagPickUpLocationState;

            IsFinishedBuilding = false;
            HasBuildingBegun = false;
            IsHomeAndDone = false;

            ErrorVector = new Vector3(-100, -100, -100);

            // TODO: Lidt nogle magiske tal
            DroneSandbagDistance = 0.5f;
            SafeHeight = 1f;

            MySandbag = null;
            SandbagTargetPoint = new Point(Vector3.zero);

            // Opsætning af MySection
            MySection = new Section();
            MySection.CurrentSection = Vector3.zero;
            MySection.MinimumSeperation = 1;

            // Henter FlyTo method fra DroneMover, så programmet ikke behøver at referere til en anden class
            FlyTo = this.GetComponent<DroneMover>().FlyTo;

            HomePosition = this.transform.position;

            BlueprintCentre = Vector3.Lerp(MyBlueprint.ConstructionNodes[0], MyBlueprint.ConstructionNodes[1], 0.5f);
            AboveSection = new Vector3(BlueprintCentre.x, MyBlueprint.DikeHeight + SafeHeight, BlueprintCentre.z);

            // Kun visuelt, modellen skal lige drejes 90 grader
            this.transform.Rotate(new Vector3(90, 0, 0));
        }

        private void Update()
        {
            // Synsrækkevidde visualisering
            //Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(0, 0, 1)  * ViewDistance);
            //Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(0, 0, -1) * ViewDistance);
            //Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(0, 1, 0)  * ViewDistance);
            //Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(0, -1, 0) * ViewDistance);
            //Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(1, 0, 0)  * ViewDistance);
            //Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(-1, 0, 0) * ViewDistance);

            // Dronens nuværende sektion
            //Debug.DrawLine(MySection.CurrentSection, MySection.CurrentSection + new Vector3(0, 10, 0), Color.blue);

            if (IsHomeAndDone == false)
            {
                if (IsFinishedBuilding == false)
                {
                    UpdateState();
                }

                else
                {
                    FlyTo(HomePosition);

                    if (DroneTools.InVicinityOf(this.transform.position, HomePosition))
                    {
                        IsHomeAndDone = true;
                        FinishedBuilding(this, null);
                    }
                }
            }
        }

        // DecisionMaker

        // Beslutter hvad dronen skal foretage sig for hver frame
        private void UpdateState()
        {
            // Hvis dronen har fundet en sandsæk, men den ikke er tagged med "Sandbag" sættes LocatedSandbag til null
            if (LocatedSandbag != null && LocatedSandbag.tag != "Sandbag")
            {
                LocatedSandbag = null;
                State = FindSandbagLocationState;
            }

            // Hvis dronen skulle få en ErrorVector, søger den igen efter et sted at placere sandsækken
            if (SandbagTargetPoint.Position == ErrorVector)
            {
                Debug.Log("Error vector");
                SandbagTargetPoint.Position = Vector3.zero;
                State = SearchForSandbagPlaceState;
            }

            State.Execute();
        }


        

        #region SandbagPlaceFinder?

        // Finder det sted dronen skal placere sandsækken. Tager højde for om den første sandsæk er placeret
        public void FindSandbagPlace()
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
                SandbagTargetPoint.Position = new Vector3(BlueprintCentre.x, SandbagReference.Height / 2, BlueprintCentre.z);
                SetDroneTargetPoint(SandbagTargetPoint.Position);
                AboveTarget = DroneTools.CalculateAbovePoint(SandbagTargetPoint.Position, MyBlueprint, SafeHeight);
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
                AboveSection = DroneTools.CalculateAbovePoint(MySection.CurrentSection, MyBlueprint, SafeHeight);
            }
            else
            {
                SetSandbagTargetPoint();
            }
        }

        // Checker om den første sandsæk diget er placeret
        // Returnerer true hvis der er en sa indsæk placeret, falsk hvis den ikke kan finde en
        public bool IsFirstSandbagPlaced()
        {
            GameObject placedSandbag = GameObject.FindGameObjectWithTag("PlacedSandbag");

            return (placedSandbag != null && Vector3.Distance(this.transform.position, placedSandbag.transform.position) <= ViewDistance) ? true : false;
        }


        public bool IsPlaceStillAvailable(Vector3 dronePosition, Point targetPoint)
        {
            // Linecast returnerer sand hvis den rammer noget. Det vil sige at !Linecast returnerer sand hvis der ikke er noget
            bool isTargetPointEmpty = !Physics.Linecast(dronePosition, targetPoint.Position);
            Debug.DrawLine(dronePosition, targetPoint.Position, (isTargetPointEmpty) ? Color.white : Color.red, 0.5f);

            // Hvis pladsen er tom, tjek de to pladser under, hvis sandsækken skal lægges oven på andre sandsække
            if (isTargetPointEmpty == true && targetPoint.Position.y > SandbagReference.Height)
            {
                return PlaceHasFoundation(dronePosition, targetPoint.Position);
            }

            else
            {
                return isTargetPointEmpty;
            }
        }

        private bool PlaceHasFoundation(Vector3 dronePosition, Vector3 targetPoint)
        {
            Vector3 foundationPoint1 = FindFoundationPoint(targetPoint, MyBlueprint.ConstructionNodes.First(), MyBlueprint.ConstructionNodes.Last());
            Vector3 foundationPoint2 = FindFoundationPoint(targetPoint, MyBlueprint.ConstructionNodes.Last(), MyBlueprint.ConstructionNodes.First());

            bool firstHasFoundation = Physics.Linecast(dronePosition, foundationPoint1);
            bool secondHasFoundation = Physics.Linecast(dronePosition, foundationPoint2);

            Debug.DrawLine(dronePosition, foundationPoint1, (firstHasFoundation) ? Color.white : Color.red, 0.5f);
            Debug.DrawLine(dronePosition, foundationPoint2, (secondHasFoundation) ? Color.white : Color.red, 0.5f);

            return firstHasFoundation && secondHasFoundation;
        }

        private Vector3 FindFoundationPoint(Vector3 targetPoint, Vector3 blueprintNodeFrom, Vector3 blueprintNodeTo)
        {
            Vector3 foundationPoint = (blueprintNodeTo - blueprintNodeFrom).normalized * SandbagReference.Length / 2;

            foundationPoint = targetPoint + foundationPoint;
            foundationPoint.y = targetPoint.y - SandbagReference.Height;

            return foundationPoint;
        }

        #endregion




















        #region Sættere
        public void SetBlueprint(Blueprint blueprint) => MyBlueprint = blueprint;

        public void SetSpeed(float speed) => this.GetComponent<DroneMover>().Speed = speed;

        public void SetSandbagPickUpLocation(Vector3 point) => SandbagPickUpLocation = point;

        public void SetViewDistance(float distance) => ViewDistance = distance;

        public void SetOtherDrones(List<GameObject> list) => this.GetComponent<DroneMover>().OtherDrones = list;

        public void SetDroneTargetPoint(Vector3 point) => DroneTargetPoint = new Vector3(point.x, point.y + DroneSandbagDistance, point.z);

        public void SetSandbagTargetPoint()
        {
            SandbagTargetPoint.Position = MySection.FindBestPlace(PossiblePlaces, this.transform.position, ViewDistance);

            if (SandbagTargetPoint.Position != new Vector3(-100, -100, -100))
            {
                SetDroneTargetPoint(SandbagTargetPoint.Position);
                AboveTarget = DroneTools.CalculateAbovePoint(SandbagTargetPoint.Position, MyBlueprint, SafeHeight);

                Debug.DrawLine(this.transform.position, SandbagTargetPoint.Position, Color.cyan);
            }
        }

        public void SetLocatedSandbag(GameObject sandbag) => LocatedSandbag = sandbag;

        #endregion
    }
}


