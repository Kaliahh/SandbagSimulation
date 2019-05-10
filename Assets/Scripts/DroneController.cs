using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static SandbagSimulation.DroneTools;

namespace SandbagSimulation
{
    // Overordnet klasse der styrer dronens beslutninger og bevægelser
    public class DroneController : MonoBehaviour
    {
        #region Fields

        public bool IsHomeAndDone { get; private set; }
        public bool HasBuildingBegun { get; set; }
        public bool IsRightDrone { get; set; }
        public bool IsFinishedBuilding { get; set; }

        public float ViewDistance { get; private set; }
        public float SafeHeight { get; private set; }
        public float DroneSandbagDistance { get; private set; }

        // Section
        public Section MySection { get; private set; }
        public Vector3[] PossiblePlaces { get; set; }
        public Vector3 AboveSection { get; set; }

        // Blueprint
        public Blueprint MyBlueprint { get; private set; }
        public Vector3 BlueprintCentre { get; private set; }

        // Sandbag
        public GameObject MySandbag { get; set; }
        public GameObject LocatedSandbag { get; private set; }
        public SandbagMeasurements SandbagReference { get; set; }
        public Point SandbagTargetPoint { get; private set; }
        public Vector3 SandbagPickUpLocation { get; private set; }

        // Eventen bruges til at stoppe simuleringen, når alle droner er færdige med at bygge
        public event EventHandler FinishedBuilding;

        // Positioner
        public Vector3 DroneTargetPoint { get; private set; }
        public Vector3 AboveTarget { get; set; }
        public Vector3 HomePosition { get; private set; }

        public Action<Vector3> FlyTo { get; private set; }

        // Dronens egen tilstand
        public IDroneState State;

        // Tilstandene dikterer hvad dronen foretager sig
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

        void Start()
        {
            // Initialisering af dronens tilstand
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

            // Den tilstand dronen starter i
            State = FlyToSandbagPickUpLocationState;

            IsFinishedBuilding = false;
            HasBuildingBegun = false;
            IsHomeAndDone = false;

            DroneSandbagDistance = 0.5f;
            SafeHeight = 1f;

            MySandbag = null;
            SandbagTargetPoint = new Point(Vector3.zero);
            SandbagReference = new SandbagMeasurements();

            MySection = new Section();
            MySection.CurrentSection = Vector3.zero;

            // Henter FlyTo metode fra DroneMover
            FlyTo = this.GetComponent<DroneMover>().FlyTo;

            HomePosition = this.transform.position;

            BlueprintCentre = Vector3.Lerp(MyBlueprint.ConstructionNodes[0], MyBlueprint.ConstructionNodes[1], 0.5f);
            AboveSection = new Vector3(BlueprintCentre.x, MyBlueprint.DikeHeight + SafeHeight, BlueprintCentre.z);

            // Kun visuelt, modellen skal lige drejes 90 grader
            this.transform.Rotate(new Vector3(90, 0, 0));
        }

        /* Kaldes i hver frame. Hvis dronen ikke er færdig med at bygge på diget, opdateres dens tilstand.
         * Når dronen er færdig med at bygge, flyver den tilbage til dens startposition (HomePosition) 
         * og bliver der indtil den bliver stoppet af SimulationController */
        private void Update()
        {
            if (IsHomeAndDone == false)
            {
                if (IsFinishedBuilding == false)
                {
                    UpdateState();
                }

                else
                {
                    FlyTo(HomePosition);

                    if (InVicinityOf(this.transform.position, HomePosition))
                    {
                        IsHomeAndDone = true;
                        FinishedBuilding(this, null);
                    }
                }
            }
        }

        // Beslutter hvad dronen skal foretage sig for hver frame
        private void UpdateState()
        {
            /* Hvis dronen har fundet en sandsæk, men den ikke er tagged med "Sandbag" sættes LocatedSandbag til null
             * Dette kan ske hvis en anden drone samler sandsækken op, før denne drone gør */
            if (LocatedSandbag != null && LocatedSandbag.tag != "Sandbag")
            {
                LocatedSandbag = null;
                State = FindSandbagLocationState;
            }

            // Hvis dronen skulle få en ErrorVector, søger den igen efter et sted at placere sandsækken
            if (SandbagTargetPoint.Position == MySection.ErrorVector)
            {
                Debug.Log("Error vector");
                SandbagTargetPoint.Position = Vector3.zero;
                State = SearchForSandbagPlaceState;
            }

            // Eksekverer den nuværende tilstand
            State.Execute();
        }

        /* Tjekker om den placering dronen har fundet stadigvæk er til rådighed,
         * altså at der ikke er blevet placeret en sandsæk i tiden mellem den har fundet den
         * og den faktisk er kommet derhen. Returnerer sand hvis placeringen er tom */
        public bool IsPlaceStillAvailable(Vector3 dronePosition, Point targetPoint)
        {
            // Linecast returnerer sand hvis den rammer noget. Det vil sige at !Linecast returnerer sand hvis der ikke er noget
            bool isTargetPointEmpty = !Physics.Linecast(dronePosition, targetPoint.Position);
            Debug.DrawLine(dronePosition, targetPoint.Position, (isTargetPointEmpty) ? Color.white : Color.red, 0.5f);

            // Hvis pladsen er tom, tjek de to pladser under, hvis sandsækken skal lægges oven på andre sandsække
            if (isTargetPointEmpty == true && targetPoint.Position.y > SandbagReference.Height * 0.75f)
            {
                return PlaceHasFoundation(dronePosition, targetPoint.Position);
            }

            else
            {
                return isTargetPointEmpty;
            }
        }

        /* Tjekker om der er sandsække under det punkt der er ved at blive undersøgt for om det stadigvæk er til rådighed
         * Returnerer sand hvis placeringen af understøttet af sandsække */
        private bool PlaceHasFoundation(Vector3 dronePosition, Vector3 targetPoint)
        {
            Vector3 foundationPoint1 = FindFoundationPoint(targetPoint, MyBlueprint.ConstructionNodes.First(), MyBlueprint.ConstructionNodes.Last());
            Vector3 foundationPoint2 = FindFoundationPoint(targetPoint, MyBlueprint.ConstructionNodes.Last(), MyBlueprint.ConstructionNodes.First());

            bool isFoundationOneThere = Physics.Linecast(dronePosition, foundationPoint1);
            bool isFoundationTwoThere = Physics.Linecast(dronePosition, foundationPoint2);

            Debug.DrawLine(dronePosition, foundationPoint1, (isFoundationOneThere) ? Color.white : Color.red, 0.5f);
            Debug.DrawLine(dronePosition, foundationPoint2, (isFoundationTwoThere) ? Color.white : Color.red, 0.5f);

            return isFoundationOneThere && isFoundationTwoThere;
        }

        /* Regner punktet skråt ned under et andet punkt, i retning af en given Blueprint Node
         * Returnerer dette punkt */
        private Vector3 FindFoundationPoint(Vector3 targetPoint, Vector3 blueprintNodeFrom, Vector3 blueprintNodeTo)
        {
            Vector3 foundationPoint = (blueprintNodeTo - blueprintNodeFrom).normalized * SandbagReference.Length / 2;

            foundationPoint = targetPoint + foundationPoint;
            foundationPoint.y = targetPoint.y - SandbagReference.Height;

            return foundationPoint;
        }


        #region Sættere
        
        // Sætter dronens MyBlueprint til at være et givent Blueprint
        public void SetBlueprint(Blueprint blueprint) => MyBlueprint = blueprint;

        // Sætter dronens DroneMover's Speed til at være en given hastighed
        public void SetSpeed(float speed) => this.GetComponent<DroneMover>().Speed = speed;

        // Sætter dronens synsvidde (ViewDistance) til at være en given distance i form af en float
        public void SetViewDistance(float distance) => ViewDistance = distance;

        // Sætter stedet hvor dronen kan samle sandsække op, til at være et givent punkt (Vector3)
        public void SetSandbagPickUpLocation(Vector3 point) => SandbagPickUpLocation = point;

        // Finder det sted dronen skal placere dens sandsæk ved brug af Section.FindBestPlace()
        public void SetSandbagTargetPoint()
        {
            SandbagTargetPoint.Position = MySection.FindBestPlace(PossiblePlaces, this.transform.position, ViewDistance);

            if (SandbagTargetPoint.Position != new Vector3(-100, -100, -100))
            {
                SetDroneTargetPoint(SandbagTargetPoint.Position);
                AboveTarget = CalculateAbovePoint(SandbagTargetPoint.Position, MyBlueprint, SafeHeight);

                Debug.DrawLine(this.transform.position, SandbagTargetPoint.Position, Color.cyan);
            }
        }

        // Sætter DroneTargetPoint til at være et givet punkt, plus afstanden mellem dronen og sandsækken
        public void SetDroneTargetPoint(Vector3 point) => DroneTargetPoint = new Vector3(point.x, point.y + DroneSandbagDistance, point.z);

        // Sætter dronens LocatedSandbag til at været et givet GameObject
        public void SetLocatedSandbag(GameObject sandbag) => LocatedSandbag = sandbag;

        #endregion
    }
}


