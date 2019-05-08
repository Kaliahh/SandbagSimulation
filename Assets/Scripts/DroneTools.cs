using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SandbagSimulation
{
    public static class DroneTools
    {
        public static Vector3 ReturnTargetNode(bool isRightDrone, Blueprint blueprint) => (isRightDrone) ? blueprint.ConstructionNodes.Last() : blueprint.ConstructionNodes.First();

        // Input: Vector3 point
        // Output: Returnerer points x og z, men lægger digets højde og en sikkerhedshøjde til points z
        public static Vector3 CalculateAbovePoint(Vector3 point, Blueprint blueprint, float safeHeight)
        {
             return new Vector3(point.x, blueprint.DikeHeight * 0.1f + safeHeight, point.z);
        }

        // Checker om to positioner er i nærheden af hinanden
        // Hvis den er, returnerer den sand, ellers returnerer den falsk
        public static bool InVicinityOf(Vector3 position1, Vector3 position2) => ((position1 - position2).magnitude < 0.1f) ? true : false;

        // Roterer sandsækken i forhold til diget og det sted sandsækken skal placeres
        public static void RotateSandbag(GameObject sandbag, Blueprint blueprint) => sandbag.transform.Rotate(new Vector3(0, CalculateAngleToDike(blueprint), 0));

        private static float CalculateAngleToDike(Blueprint blueprint)
        {
            Vector3 point1 = blueprint.ConstructionNodes.First();
            Vector3 point2 = blueprint.ConstructionNodes.Last();

            Vector3 dikeVector = point1 - point2;

            return Vector3.SignedAngle(Vector3.right, dikeVector, Vector3.up);
        }

        public static bool IsLastSandbagPlaced(Vector3 position, Vector3 targetNode, float viewDistance, Blueprint blueprint, SandbagController sandbag)
        {
            if (Vector3.Distance(position, targetNode) < viewDistance)
            {
                targetNode.y = (blueprint.DikeHeight - 0.5f) * sandbag.Height;

                bool isPlaceEmpty = Physics.Linecast(position, targetNode, out RaycastHit hitInfo);

                if (hitInfo.distance < Vector3.Distance(position, targetNode) - sandbag.Length)
                {
                    return false;
                }

                else
                {
                    return isPlaceEmpty;
                }
            }

            else
            {
                return false;
            }
        }
    }
}


