using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SandbagSimulation
{
    //public class PlaceFinder
    //{
    //    bool HasBuildingBegun;

    //    Vector3[] PossiblePlaces;
    //    Vector3 Position;

    //    float ViewDistance;

    //    SandbagPlacement sandbagPlacement;

    //    public PlaceFinder(ref bool hasBuildingBegun, )
    //    {
    //        sandbagPlacement = new SandbagPlacement(0f);
    //        HasBuildingBegun = hasBuildingBegun;
    //    }

    //    // Finder det sted dronen skal placere sandsækken. Tager højde for om den første sandsæk er placeret
    //    public void FindSandbagPlace()
    //    {
    //        if (HasBuildingBegun == false && IsFirstSandbagPlaced() == false)
    //        {
    //            FindFirstSandbagPlace();
    //        }

    //        else if (HasBuildingBegun == false && IsFirstSandbagPlaced() == true)
    //        {
    //            HasBuildingBegun = true;
    //            FindNextSandbagPlace();
    //        }

    //        else
    //        {
    //            FindNextSandbagPlace();
    //        }
    //    }

    //    // Finder det sted hvor den første sandsæk skal placeres i diget
    //    private void FindFirstSandbagPlace()
    //    {
    //        PossiblePlaces = sandbagPlacement.FindPlace(ViewDistance, Position, MyBlueprint);

    //        if (PossiblePlaces == null)
    //        {
    //            SandbagTargetPoint.Position = new Vector3(BlueprintCentre.x, SandbagReference.Height / 2, BlueprintCentre.z);
    //            SetDroneTargetPoint(SandbagTargetPoint.Position);
    //            AboveTarget = DroneTools.CalculateAbovePoint(SandbagTargetPoint.Position, MyBlueprint, SafeHeight);
    //            HasBuildingBegun = true;
    //        }

    //        else if (PossiblePlaces != null)
    //        {
    //            HasBuildingBegun = true;
    //        }
    //    }

    //    // Finder det sted hvor sandsækken skal placeres, ud fra de sandsække der allerede er placeret
    //    // Hvis den ikke kan finde en mulig placering, finder den den næste sektion dronen skal arbejde på
    //    private void FindNextSandbagPlace()
    //    {
    //        PossiblePlaces = sandbagPlacement.FindPlace(ViewDistance, Position, MyBlueprint);

    //        if (PossiblePlaces == null)
    //        {
    //            MySection.CurrentSection = MySection.FindNextSection(ViewDistance, Position, IsRightDrone, MyBlueprint);
    //            AboveSection = DroneTools.CalculateAbovePoint(MySection.CurrentSection, MyBlueprint, SafeHeight);
    //        }
    //        else
    //        {
    //            SetSandbagTargetPoint();
    //        }
    //    }

    //    // Checker om den første sandsæk diget er placeret
    //    // Returnerer true hvis der er en sa indsæk placeret, falsk hvis den ikke kan finde en
    //    public bool IsFirstSandbagPlaced()
    //    {
    //        GameObject placedSandbag = GameObject.FindGameObjectWithTag("PlacedSandbag");

    //        return (placedSandbag != null && Vector3.Distance(Position, placedSandbag.transform.position) <= ViewDistance) ? true : false;
    //    }
    //}
}


