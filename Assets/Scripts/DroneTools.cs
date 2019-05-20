using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SandbagSimulation
{
    // En "værktøjskasse" af metoder som dronen kan bruge
    public static class DroneTools
    {
        // Returnerer den Blueprint Node dronen skal arbejde hen imod
        public static Vector3 ReturnTargetNode(bool isRightDrone, Blueprint blueprint) => (isRightDrone) ? blueprint.ConstructionNodes.Last() : blueprint.ConstructionNodes.First();

        // Returnerer points x og z, men lægger digets højde og en sikkerhedshøjde til points z
        public static Vector3 CalculateAbovePoint(Vector3 point, Blueprint blueprint, float safeHeight)
        {
             return new Vector3(point.x, blueprint.DikeHeight * new SandbagMeasurements().Height + safeHeight, point.z);
        }

        // Checker om to positioner er i nærheden af hinanden
        // Hvis den er, returnerer den sand, ellers returnerer den falsk
        public static bool InVicinityOf(Vector3 position1, Vector3 position2) => ((position1 - position2).magnitude < 0.1f) ? true : false;

        // Roterer sandsækken i forhold til diget og det sted sandsækken skal placeres
        public static void RotateSandbag(GameObject sandbag, Blueprint blueprint) => sandbag.transform.Rotate(new Vector3(0, CalculateAngleToDike(blueprint), 0));

        // Udregner vinklen der er parallel med diget
        private static float CalculateAngleToDike(Blueprint blueprint)
        {
            Vector3 point1 = blueprint.ConstructionNodes.First();
            Vector3 point2 = blueprint.ConstructionNodes.Last();

            Vector3 dikeVector = point1 - point2;

            return Vector3.SignedAngle(Vector3.right, dikeVector, Vector3.up);
        }

        /* Linecaster til den sidste sandsæk der skal placeres i diget.
         * Hvis den rammer noget, returneres sand */
        public static bool IsLastSandbagPlaced(Vector3 position, Vector3 targetNode, float viewDistance, Blueprint blueprint, SandbagMeasurements sandbag)
        {
            if (Vector3.Distance(position, targetNode) < viewDistance)
            {
                targetNode.y = (blueprint.DikeHeight - 0.5f) * sandbag.Height;

                bool isPlaceEmpty = Physics.Linecast(position, targetNode, out RaycastHit hitInfo);

                if (hitInfo.distance < Vector3.Distance(position, targetNode) - sandbag.Length / 2)
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


