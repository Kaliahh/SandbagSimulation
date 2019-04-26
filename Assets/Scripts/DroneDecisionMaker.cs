//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace SandbagSimulation
//{
//    public class DroneDecisionMaker : MonoBehaviour // TODO: Færdiggør DecisionMaker
//    {
//        int Step;
//        public bool HasBuildingBegun;
//        public bool IsRightDrone;

//        public GameObject LocatedSandbag { get; set; }

//        public Vector3[] PossiblePlaces;
//        public Action<Vector3> FlyTo;
//        private Func<DroneController> GetDC;


//        private DroneActions Actions;

//        public Vector3 SandbagPickUpLocation; // Kommer fra DroneController
//        public Vector3 SandbagTargetPoint;
//        public Vector3 DroneTargetPoint;
//        public Vector3 BlueprintCentre;
//        public Vector3 AboveSection;
//        public Vector3 AboveTarget;

//        void Start()
//        {
//            HasBuildingBegun = false;

//            FlyTo = this.GetComponent<DroneMover>().FlyTo;
//            GetDC = this.GetComponent<DroneController>;

//            Actions = this.GetComponent<DroneActions>();


//            Step = 0;

//        }

//        public void MakeDecision()
//        {
//            // Hvis dronen har fundet en sandsæk, men den ikke er tagged med "Sandbag" sættes LocatedSandbag til null
//            if (LocatedSandbag != null && LocatedSandbag.tag != "Sandbag")
//            {
//                LocatedSandbag = null;
                
//                Step = 1;
//            }

//            // Det her forudsætter at algoritmen der finder placeringer er perfekt
//            if (SandbagTargetPoint == new Vector3(-100f, -100f, -100f))
//            {
//                this.GetComponent<DroneController>().IsFinishedBuilding = true;
//            }

//            else
//            {
//                // Dikterer rækkefølgen af handlinger dronen skal foretage. En opskrift for diget
//                switch (Step)
//                {
//                    case 0: FlyToSandbagPickUpLocation(); break;

//                    case 1: FindSandbagLocation(); break;

//                    case 2: FlyToLocatedSandbag(); break;

//                    case 3: PickUpLocatedSandbag(); break;

//                    case 4: FlyToSection(); break;

//                    case 5: SearchForSandbagPlace(); break;

//                    case 6: FlyToAboveTarget(); break;

//                    case 7: FlyToDroneTarget(); break;

//                    case 8: PlaceMySandbag(); break;

//                    case 9: ReturnToAboveTarget(); break;

//                    default: Step = 0; break;
//                }
//            }
//        }

//        #region Handlingsplan

//        // Flyver dronen hen til opsamlingssted for sandsække. Når den kommer i nærheden, tælles step op med 1
//        private void FlyToSandbagPickUpLocation()
//        {
//            FlyTo(SandbagPickUpLocation);
//            if (Actions.InVicinityOf(SandbagPickUpLocation))
//                Step++;
//        }

//        // Finder den nærmeste sandsæk, og tæller step op med 1 hvis den finder en. 
//        // Sætter step til 0, hvis den ikke kunne finde en sandsæk
//        private void FindSandbagLocation()
//        {
//            Actions.LocateNearestSandbag();
//            if (LocatedSandbag != null)
//                Step++;
//            else
//                Step = 0;
//        }

//        // Flyver dronen hen til den sandsæk den har fundet, tæller step op med 1 når den kommer i nærheden
//        private void FlyToLocatedSandbag()
//        {
//            FlyTo(DroneTargetPoint);
//            if (Actions.InVicinityOf(DroneTargetPoint))
//                Step++;
//        }

//        // Samler den fundne sandsæk op, og finder en sektion den kan arbejde på, hvis dronen ikke allerede har en
//        private void PickUpLocatedSandbag()
//        {
//            Actions.PickUpSandbag();
//            if (this.GetComponent<DroneController>().MySandbag != null)
//            {
//                Step++;

//                if (this.GetComponent<DroneController>().MySection.CurrentSection == Vector3.zero)
//                {
//                    this.GetComponent<DroneController>().MySection.CurrentSection = BlueprintCentre;
//                    AboveSection = Actions.CalculateAbovePoint(this.GetComponent<DroneController>().MySection.CurrentSection);
//                }
//            }
//            // TODO: Måske else?
//        }

//        // Flyver dronen hen til et punkt over dens aktuelle sektion
//        // Når den kommer i nærheden, tælles step op med 1, og dronen finder ud af hvor den skal sætte sandsækken
//        private void FlyToSection()
//        {
//            FlyTo(AboveSection);
//            if (Actions.InVicinityOf(AboveSection))
//                Step++;
//        }

//        private void SearchForSandbagPlace()
//        {
//            Actions.(this.transform.position, GetDC().ViewDistance, ref HasBuildingBegun);
//            Step++;
//        }

//        // Flyver dronen hen over det sted hvor sandsækken skal placeres
//        private void FlyToAboveTarget()
//        {
//            FlyTo(AboveTarget);

//            if (Actions.InVicinityOf(AboveTarget))
//            {
//                if (Actions.IsPlaceStillAvailable() == false)
//                {
//                    Step = 5;
//                }

//                else
//                    Step++;
//            }
//        }

//        // Flyver dronen hen til det sted den skal være, for at placere sandsækken rigtigt
//        private void FlyToDroneTarget()
//        {
//            FlyTo(DroneTargetPoint);

//            if (Actions.InVicinityOf(DroneTargetPoint))
//                Step++;
//        }

//        // Placerer den sandsæk dronen bære rundt på
//        private void PlaceMySandbag()
//        {
//            Actions.PlaceSandbag();
//            Step++;
//        }

//        // Flyver tilbage til punktet over der hvor dronen har placeret sandsækken. 
//        // Step sættes til 0, processen starter forfra
//        private void ReturnToAboveTarget()
//        {
//            FlyTo(AboveTarget);
//            if (Actions.InVicinityOf(AboveTarget))
//                Step = 0;
//        }
//        #endregion
//    }
//}
