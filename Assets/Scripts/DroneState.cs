using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SandbagSimulation
{
    public interface IDroneState
    {
        void Execute();
    }

    public abstract class DroneState : IDroneState
    {
        // public GameObject Drone { get; private set; }
        public DroneController C { get; private set; }

        public DroneState(DroneController controller)
        {
            C = controller;
        }

        public abstract void Execute();
    }

    // Flyver dronen hen til opsamlingssted for sandsække. Når den kommer i nærheden, opdateres State til at samle den op
    public class FlyToSandbagPickUpLocationState : DroneState
    {
        public FlyToSandbagPickUpLocationState(DroneController controller) : base(controller) { }

        public override void Execute()
        {
            C.FlyTo(C.SandbagPickUpLocation);

            if (DroneTools.InVicinityOf(C.transform.position, C.SandbagPickUpLocation))
                C.State = C.FindSandbagLocationState;
        }
    }

    // Finder den nærmeste sandsæk, og tæller step op med 1 hvis den finder en. 
    // Sætter step til 0, hvis den ikke kunne finde en sandsæk
    public class FindSandbagLocationState : DroneState
    {
        public FindSandbagLocationState(DroneController controller) : base(controller) { }

        public override void Execute()
        {
            LocateNearestSandbag(C.transform.position, C.ViewDistance);

            if (C.LocatedSandbag != null)
                C.State = C.FlyToLocatedSandbagState;

            else
                C.State = C.FlyToSandbagPickUpLocationState;
        }

        // Finder den nærmeste sandsæk, og gemmer den i LocatedSandbag
        public void LocateNearestSandbag(Vector3 position, float viewDistance)
        {
            GameObject locatedSandbag = null;
            
            GameObject[] sandbags = GameObject.FindGameObjectsWithTag("Sandbag");

            float distance = viewDistance;

            foreach (GameObject sandbag in sandbags)
            {
                Vector3 diff = sandbag.transform.position - position;
                float currentDistance = diff.magnitude;

                if (currentDistance < distance)
                {
                    locatedSandbag = sandbag;
                    distance = currentDistance;
                }
            }

            if (locatedSandbag != null)
            {
                C.SetDroneTargetPoint(locatedSandbag.transform.position);
                C.SetLocatedSandbag(locatedSandbag);
            }
        }
    }

    public class FlyToLocatedSandbagState : DroneState
    {
        public FlyToLocatedSandbagState(DroneController controller) : base(controller) { }

        public override void Execute()
        {
            C.FlyTo(C.DroneTargetPoint);

            if (DroneTools.InVicinityOf(this.C.transform.position, C.DroneTargetPoint))
                C.State = C.PickUpLocatedSandbagState;
        }
    }

    public class PickUpLocatedSandbagState : DroneState
    {
        public PickUpLocatedSandbagState(DroneController controller) : base(controller) { }

        public override void Execute()
        {
            GameObject droneSandbag = null;

            PickUpSandbag(ref droneSandbag, C.LocatedSandbag, C);

            C.MySandbag = droneSandbag;
            

            if (C.MySandbag != null)
            {
                C.State = C.FlyToSectionState;

                if (C.MySection.CurrentSection == Vector3.zero)
                {
                    C.MySection.CurrentSection = C.BlueprintCentre;
                    C.AboveSection = DroneTools.CalculateAbovePoint(C.MySection.CurrentSection, C.MyBlueprint, C.SafeHeight);
                }
            }

            else
            {
                C.State = C.FindSandbagLocationState;
            }
        }

        // Gemmer den fundne sandsæk i MySandbag, og den skal nu transporteres
        // Den fundne sandsæk (LocatedSandbag) sættes til null
        public void PickUpSandbag(ref GameObject droneSandbag, GameObject locatedSandbag, DroneController controller)
        {
            droneSandbag = locatedSandbag;
            droneSandbag.tag = "PickedUpSandbag";
            droneSandbag.layer = 2; // Dronen kan Linecaste igennem sandsækken
            controller.gameObject.layer = 0; // Sørger for at dronerne ikke kommer alt for meget i vejen for hinanden

            // Gemmer en reference til SandbagController, så det er nemt at tilgå f.eks. højden af en sandsæk
            if (controller.SandbagReference == null)
            {
                controller.SandbagReference = droneSandbag.GetComponent<SandbagController>();
            }

            droneSandbag.GetComponent<Rigidbody>().isKinematic = true; // Sørger for at dens velocity bliver dræbt, bliver ikke påvirket af tyngdekraft
            
            controller.SetLocatedSandbag(null);
        }
    }

    public class FlyToSectionState : DroneState
    {
        public FlyToSectionState(DroneController controller) : base(controller) { }
        
        public override void Execute()
        {
            C.FlyTo(C.AboveSection);

            if (DroneTools.InVicinityOf(C.transform.position, C.AboveSection))
                C.State = C.SearchForSandbagPlaceState;
        }
    }

    public class SearchForSandbagPlaceState : DroneState
    {
        public SearchForSandbagPlaceState(DroneController controller) : base(controller) { }

        public override void Execute()
        {
            FindSandbagPlace();
            C.State = C.FlyToAboveTargetState;
        }

        // Finder det sted dronen skal placere sandsækken. Tager højde for om den første sandsæk er placeret
        public void FindSandbagPlace()
        {
            if (C.HasBuildingBegun == false && IsFirstSandbagPlaced() == false)
            {
                FindFirstSandbagPlace();
            }

            else if (C.HasBuildingBegun == false && IsFirstSandbagPlaced() == true)
            {
                C.HasBuildingBegun = true;
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
            C.PossiblePlaces = C.MySection.FindPlace(C.ViewDistance, C.transform.position, C.MyBlueprint);

            if (C.PossiblePlaces == null)
            {
                C.SandbagTargetPoint.Position = new Vector3(C.BlueprintCentre.x, C.SandbagReference.Height / 2, C.BlueprintCentre.z);
                C.SetDroneTargetPoint(C.SandbagTargetPoint.Position);
                C.AboveTarget = DroneTools.CalculateAbovePoint(C.SandbagTargetPoint.Position, C.MyBlueprint, C.SafeHeight);
                C.HasBuildingBegun = true;
            }

            else if (C.PossiblePlaces != null)
            {
                C.HasBuildingBegun = true;
            }
        }

        // Finder det sted hvor sandsækken skal placeres, ud fra de sandsække der allerede er placeret
        // Hvis den ikke kan finde en mulig placering, finder den den næste sektion dronen skal arbejde på
        private void FindNextSandbagPlace()
        {
            C.PossiblePlaces = C.MySection.FindPlace(C.ViewDistance, C.transform.position, C.MyBlueprint);

            if (C.PossiblePlaces == null)
            {
                Debug.DrawLine(C.transform.position, C.transform.position + new Vector3(0, 10, 0), Color.blue, 0.5f);
                C.MySection.CurrentSection = C.MySection.FindNextSection(C.ViewDistance, C.transform.position, C.IsRightDrone, C.MyBlueprint);
                C.AboveSection = DroneTools.CalculateAbovePoint(C.MySection.CurrentSection, C.MyBlueprint, C.SafeHeight);
            }

            else
            {
                C.SandbagTargetPoint.Position = C.MySection.FindBestPlace(C.PossiblePlaces, C.transform.position, C.ViewDistance);

                if (C.SandbagTargetPoint.Position != new Vector3(-100, -100, -100))
                {
                    C.SetDroneTargetPoint(C.SandbagTargetPoint.Position);
                    C.AboveTarget = DroneTools.CalculateAbovePoint(C.SandbagTargetPoint.Position, C.MyBlueprint, C.SafeHeight);

                    Debug.DrawLine(C.transform.position, C.SandbagTargetPoint.Position, Color.cyan);
                }
            }
        }

        // Checker om den første sandsæk diget er placeret
        // Returnerer true hvis der er en sa indsæk placeret, falsk hvis den ikke kan finde en
        public bool IsFirstSandbagPlaced()
        {
            GameObject placedSandbag = GameObject.FindGameObjectWithTag("PlacedSandbag");

            return (placedSandbag != null && Vector3.Distance(C.transform.position, placedSandbag.transform.position) <= C.ViewDistance) ? true : false;
        }
    }

    public class FlyToAboveTargetState : DroneState
    {
        public FlyToAboveTargetState(DroneController controller) : base(controller) { }

        public override void Execute()
        {
            C.FlyTo(C.AboveTarget);

            if (DroneTools.InVicinityOf(C.transform.position, C.AboveTarget))
            {
                if (DroneTools.IsLastSandbagPlaced(C.transform.position, DroneTools.ReturnTargetNode(C.IsRightDrone, C.MyBlueprint), C.ViewDistance, C.MyBlueprint, C.SandbagReference) == true)
                {
                    C.IsFinishedBuilding = true;
                    return;
                }

                if (C.IsPlaceStillAvailable(C.transform.position, C.SandbagTargetPoint) == false)
                {
                    C.State = C.SearchForSandbagPlaceState;
                }

                else
                {
                    C.State = C.FlyToDroneTargetState;
                }
            }
        }

        

    }

    public class FlyToDroneTargetState : DroneState
    {
        public FlyToDroneTargetState(DroneController controller) : base(controller) { }

        public override void Execute()
        {
            C.FlyTo(C.DroneTargetPoint);

            if (DroneTools.InVicinityOf(C.transform.position, C.DroneTargetPoint))
                C.State = C.PlaceMySandbagState;
        }
    }

    public class PlaceMySandbagState : DroneState
    {
        public PlaceMySandbagState(DroneController controller) : base(controller) { }

        public override void Execute()
        {
            GameObject droneSandbag = C.MySandbag;

            PlaceSandbag(ref droneSandbag, C.MyBlueprint, C);

            C.MySandbag = droneSandbag;

            C.State = C.ReturnToAboveTargetState;
        }

        // Placerer MySandbag i et givent punkt, og sætter referencen til sandsækken (MySandbag) til null
        public void PlaceSandbag(ref GameObject droneSandbag, Blueprint blueprint, DroneController controller)
        {
            droneSandbag.tag = "PlacedSandbag";
            droneSandbag.layer = 0; // Linecasts rammer igen sandsækken
            controller.gameObject.layer = 2; // Sørger for at dronerne ikke kommer alt for meget i vejen for hinanden

            DroneTools.RotateSandbag(droneSandbag, blueprint);

            droneSandbag.GetComponent<Rigidbody>().isKinematic = false;  // Bliver igen påvirket af tyngdekraft
            droneSandbag = null;
        }
    }

    public class ReturnToAboveTargetState : DroneState
    {
        public ReturnToAboveTargetState(DroneController controller) : base(controller) { }

        public override void Execute()
        {
            C.FlyTo(C.AboveTarget);

            if (DroneTools.InVicinityOf(C.transform.position, C.AboveTarget))
            {
                if (DroneTools.IsLastSandbagPlaced(C.transform.position, DroneTools.ReturnTargetNode(C.IsRightDrone, C.MyBlueprint), C.ViewDistance, C.MyBlueprint, C.SandbagReference) == true)
                {
                    C.IsFinishedBuilding = true;
                    return;
                }

                C.State = C.FlyToSandbagPickUpLocationState;
            }
        }
    }
}



