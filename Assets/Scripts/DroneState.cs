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

            if (C.InVicinityOf(C.transform.position, C.SandbagPickUpLocation))
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
            C.LocateNearestSandbag();

            if (C.LocatedSandbag != null)
                C.State = C.FlyToLocatedSandbagState;

            else
                C.State = C.FlyToSandbagPickUpLocationState;
        }
    }

    public class FlyToLocatedSandbagState : DroneState
    {
        public FlyToLocatedSandbagState(DroneController controller) : base(controller) { }

        public override void Execute()
        {
            C.FlyTo(C.DroneTargetPoint);

            if (C.InVicinityOf(this.C.transform.position, C.DroneTargetPoint))
                C.State = C.PickUpLocatedSandbagState;
        }
    }

    public class PickUpLocatedSandbagState : DroneState
    {
        public PickUpLocatedSandbagState(DroneController controller) : base(controller) { }

        public override void Execute()
        {
            C.PickUpSandbag();

            if (C.MySandbag != null)
            {
                C.State = C.FlyToSectionState;

                if (C.MySection.CurrentSection == Vector3.zero)
                {
                    C.MySection.CurrentSection = C.BlueprintCentre;
                    C.AboveSection = C.CalculateAbovePoint(C.MySection.CurrentSection);
                }
            }

            else
            {
                C.State = C.FindSandbagLocationState;
            }
        }
    }

    public class FlyToSectionState : DroneState
    {
        public FlyToSectionState(DroneController controller) : base(controller) { }
        
        public override void Execute()
        {
            C.FlyTo(C.AboveSection);

            if (C.InVicinityOf(C.transform.position, C.AboveSection))
                C.State = C.SearchForSandbagPlaceState;
        }
    }

    public class SearchForSandbagPlaceState : DroneState
    {
        public SearchForSandbagPlaceState(DroneController controller) : base(controller) { }

        public override void Execute()
        {
            C.FindSandbagPlace();
            C.State = C.FlyToAboveTargetState;
        }
    }

    public class FlyToAboveTargetState : DroneState
    {
        public FlyToAboveTargetState(DroneController controller) : base(controller) { }

        public override void Execute()
        {
            C.FlyTo(C.AboveTarget);

            if (C.InVicinityOf(C.transform.position, C.AboveTarget))
            {
                if (C.IsLastSandbagPlaced(C.ReturnTargetNode()) == true)
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

            if (C.InVicinityOf(C.transform.position, C.DroneTargetPoint))
                C.State = C.PlaceMySandbagState;
        }
    }

    public class PlaceMySandbagState : DroneState
    {
        public PlaceMySandbagState(DroneController controller) : base(controller) { }

        public override void Execute()
        {
            C.PlaceSandbag();
            C.State = C.ReturnToAboveTargetState;
        }
    }

    public class ReturnToAboveTargetState : DroneState
    {
        public ReturnToAboveTargetState(DroneController controller) : base(controller) { }

        public override void Execute()
        {
            C.FlyTo(C.AboveTarget);

            if (C.InVicinityOf(C.transform.position, C.AboveTarget))
            {
                if (C.IsLastSandbagPlaced(C.ReturnTargetNode()) == true)
                {
                    C.IsFinishedBuilding = true;
                    return;
                }

                C.State = C.FlyToSandbagPickUpLocationState;
            }
        }
    }
}

//public IDroneState FlyToSandbagPickUpLocationState;
//public IDroneState FindSandbagLocationState;
//public IDroneState FlyToLocatedSandbagState;
//public IDroneState PickUpLocatedSandbagState;
//public IDroneState FlyToSectionState;
//public IDroneState SearchForSandbagPlaceState;
//public IDroneState FlyToAboveTargetState;
//public IDroneState FlyToDroneTargetState;
//public IDroneState PlaceMySandbagState;
//public IDroneState ReturnToAboveTargetState;



