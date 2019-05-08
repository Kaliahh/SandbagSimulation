//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using System.Linq;

//namespace SandbagSimulation
//{
//    public class DroneActions
//    {
//        public float DroneSandbagDistance { get; private set; }

//        public DroneActions()
//        {
//            DroneSandbagDistance = 1f;
//        }

//        #region Handlinger

//        // Checker om den første sandsæk diget er placeret
//        // Returnerer true hvis der er en sa indsæk placeret, falsk hvis den ikke kan finde en
//        public bool IsFirstSandbagPlaced(Vector3 dronePosition, float viewDistance)
//        {
//            GameObject placedSandbag = GameObject.FindGameObjectWithTag("PlacedSandbag");

//            if (placedSandbag != null && Vector3.Distance(dronePosition, placedSandbag.transform.position) <= viewDistance)
//            {
//                return true;
//            }

//            else //TODO: Det her er måske farligt
//                return false;
//        }

//        // Finder det sted dronen skal placere sandsækken. Tager højde for om den første sandsæk er placeret
//        public void FindSandbagPlace(Vector3 dronePosition, float viewDistance, Vector3 BlueprintCentre,
//                                     Func<float, Vector3, Blueprint, Vector3[]> FindPlace, Blueprint blueprint,
//                                     ref bool HasBuildingBegun, ref Vector3[] possiblePlaces,
//                                     ref Vector3 SandbagTargetPoint, ref Vector3 AboveTarget)
//        {
//            if (HasBuildingBegun == false && IsFirstSandbagPlaced(dronePosition, viewDistance) == false)
//            {
//                FindFirstSandbagPlace(dronePosition, viewDistance, BlueprintCentre, FindPlace, blueprint, ref HasBuildingBegun, ref possiblePlaces, ref SandbagTargetPoint, ref AboveTarget);
//            }

//            else if (HasBuildingBegun == false && IsFirstSandbagPlaced(dronePosition, viewDistance) == true)
//            {
//                HasBuildingBegun = true;
//                FindNextSandbagPlace();
//            }

//            else
//            {
//                FindNextSandbagPlace();
//            }
//        }

//        // Finder det sted hvor den første sandsæk skal placeres i diget
//        public void FindFirstSandbagPlace(Vector3 dronePosition, float viewDistance, Vector3 BlueprintCentre, 
//                                          Func<float, Vector3, Blueprint, Vector3[]> FindPlace, Blueprint blueprint, 
//                                          ref bool HasBuildingBegun, ref Vector3 [] possiblePlaces, 
//                                          ref Vector3 SandbagTargetPoint, ref Vector3 AboveTarget)
//        {
//            possiblePlaces = FindPlace(viewDistance, dronePosition, blueprint);

//            if (possiblePlaces == null)
//            {
//                // 
//                SandbagTargetPoint = new Vector3(BlueprintCentre.x, 0.5f, BlueprintCentre.z);
//                SetDroneTargetPoint(SandbagTargetPoint);
//                AboveTarget = CalculateAbovePoint(SandbagTargetPoint);
//                HasBuildingBegun = true;
//            }

//            else if (possiblePlaces != null)
//            {
//                HasBuildingBegun = true;
//            }
//        }

//        // Finder det sted hvor sandsækken skal placeres, ud fra de sandsække der allerede er placeret
//        // Hvis den ikke kan finde en mulig placering, finder den den næste sektion dronen skal arbejde på
//        public void FindNextSandbagPlace(Vector3 dronePosition, float viewDistance, Vector3 BlueprintCentre,
//                                         Func<float, Vector3, Blueprint, Vector3[]> FindPlace, Blueprint blueprint,
//                                         ref bool HasBuildingBegun, ref Vector3[] possiblePlaces,
//                                         ref Vector3 SandbagTargetPoint, ref Vector3 AboveTarget)
//        {
//            possiblePlaces = FindPlace(viewDistance, this.transform.position, MyBlueprint);

//            if (possiblePlaces == null)
//            {
//                MySection.CurrentSection = MySection.FindNextSection(viewDistance, this.transform.position, IsRightDrone, MyBlueprint);
//                AboveSection = CalculateAbovePoint(MySection.CurrentSection);
//            }
//            else
//            {
//                SandbagTargetPoint = MySection.FindBestPlace(possiblePlaces, this.transform.position, viewDistance);
//                SetDroneTargetPoint(SandbagTargetPoint);
//                AboveTarget = CalculateAbovePoint(SandbagTargetPoint);
//            }
//        }

//        // Checker om dronens aktuelle sektion er midten af diget
//        // Returnerer sand hvis det er
//        public bool IsCurrentSectionCenter()
//        {
//            return MySection.CurrentSection == BlueprintCentre;
//        }

//        // Input: Vector 3 point
//        // Output: Returnerer points x og z, men lægger digets højde og en sikkerhedshøjde til points z
//        public Vector3 CalculateAbovePoint(Vector3 point)
//        {
//            return new Vector3(point.x, 0.5f * MyBlueprint.DikeHeight + SafeHeight, point.z);
//        }

//        // Checker om dronen er i nærheden af en Vector3 position
//        // Hvis den er, returnerer den sand, ellers returnerer den falsk
//        public bool InVicinityOf(Vector3 position)
//        {
//            if ((this.transform.position - position).magnitude < 0.1f)
//                return true;

//            else
//                return false;
//        }

//        // Finder den nærmeste sandsæk, og gemmer den i LocatedSandbag
//        public void LocateNearestSandbag()
//        {
//            // 
//            GameObject[] sandbags = GameObject.FindGameObjectsWithTag("Sandbag");

//            float distance = ViewDistance;

//            foreach (GameObject sandbag in sandbags)
//            {
//                Vector3 diff = sandbag.transform.position - this.transform.position;
//                float currentDistance = diff.magnitude;

//                if (currentDistance < distance)
//                {
//                    LocatedSandbag = sandbag;
//                    distance = currentDistance;
//                }
//            }

//            if (LocatedSandbag != null)
//            {
//                SetDroneTargetPoint(LocatedSandbag.transform.position);
//            }
//        }

//        // Gemmer den fundne sandsæk i MySandbag, og den skal nu transporteres
//        // Den fundne sandsæk (LocatedSandbag) sættes til null
//        public void PickUpSandbag()
//        {
//            MySandbag = LocatedSandbag;
//            MySandbag.tag = "PickedUpSandbag";
//            MySandbag.layer = 2;
//            this.gameObject.layer = 0; // Sørger for at dronerne ikke kommer alt for meget i vejen for hinanden

//            MySandbag.GetComponent<Rigidbody>.isKinematic = true;
//            LocatedSandbag = null;
//        }

//        // Placerer MySandbag i et givent punkt, og sætter referencen til sandsækken (MySandbag) til null
//        public void PlaceSandbag()
//        {
//            MySandbag.tag = "PlacedSandbag";
//            MySandbag.layer = 0;
//            this.gameObject.layer = 2; // Sørger for at dronerne ikke kommer alt for meget i vejen for hinanden

//            // MySandbag.GetComponent<SandbagController>().rb.velocity = Vector3.zero;
//            RotateSandbag(this.transform.position);

//            // MySandbag.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - DroneSandbagDistance, this.transform.position.z);

//            MySandbag.GetComponent<Rigidbody>.isKinematic = false;

//            MySandbag = null;
//        }

//        // Roterer sandsækken i forhold til diget og det sted sandsækken skal placeres
//        public void RotateSandbag(Vector3 position)
//        {
//            Vector3 point1 = MyBlueprint.ConstructionNodes.First();
//            Vector3 point2 = MyBlueprint.ConstructionNodes.Last();

//            Vector3 dikeVector = point1 - point2;

//            float angle = Vector3.SignedAngle(Vector3.right, dikeVector, Vector3.up);

//            MySandbag.transform.Rotate(new Vector3(0, angle, 0));
//        }

//        public void SetDroneTargetPoint(Vector3 point)
//        {
//            DroneTargetPoint = new Vector3(point.x, point.y + DroneSandbagDistance, point.z);
//        }

//        public bool IsPlaceStillAvailable() // TODO: Skal også tjekke om der er en sandsæk under
//        {
//            return !Physics.Linecast(this.transform.position, SandbagTargetPoint);

//            //return !Physics.Linecast(this.transform.position, SandbagTargetPoint, 10) && !Physics.Linecast(this.transform.position, SandbagTargetPoint, 9);
//        }

//        #endregion
//    }
//}