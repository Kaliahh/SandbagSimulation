using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SandbagSimulation;
using System.Linq;

namespace SandbagSimulation
{
    public class Section
    {
        // Properties and fields
        public Vector3 CurrentSection { get; set; }
        public float MinimumSeperation { get; set; }
        public float MaximumPlacementDeviation { get; set; }

        private Vector3 ErrorVector = new Vector3(-100f, -100f, -100f);

        // Constructor
        //public Section(Vector3 location) { CurrentSection = location; }

        // Public Methods

        /*
         * Parameters: float with the drones viewDistance, Vector3 with the drones position.
         * 
         * Return: Vector3[] containing the locations where it is possible for a drone to place a sandbag.
         */
        public Vector3[] FindPlace(float viewDistance, Vector3 position, Blueprint blueprint)
        {
            // List of alle the possible places
            List<Vector3> places = new List<Vector3>();

            // Bliver kun brugt til at finde højde og bredde af sandsække, skal findes på en bedre måde (Statiske properties i SandbagController?)
            SandbagController sandbag = Object.FindObjectOfType<SandbagController>();

            // Find en sandsæk til at bruge som udgangspunkt
            Vector3 startingBag = FindStartingPlace(position, viewDistance, sandbag.Height);

            if (startingBag.Equals(ErrorVector))
            {
                Debug.Log("No startingpoint found");
                Debug.DrawLine(position, position + new Vector3(0, 10, 0), Color.red, 1);
                return null;
            }

            MaximumPlacementDeviation = 0.5f; // TODO: Magisk tal

            Queue<Vector3> pointQueue = new Queue<Vector3>();
            List<Vector3> visited = new List<Vector3>();

            pointQueue.Enqueue(startingBag);

            // Breadth first search
            while (pointQueue.Count > 0)
            {
                Point current = new Point(pointQueue.Dequeue());
                Vector3 temp = new Vector3
                (
                    (float)System.Math.Round(current.Position.x, 1),
                    (float)System.Math.Round(current.Position.y, 1),
                    (float)System.Math.Round(current.Position.z, 1)
                );

                if (!visited.Contains(temp)
                    && current.WithinBorder(blueprint, sandbag.Height, MaximumPlacementDeviation)
                    && Vector3.Distance(position, current.Position) <= viewDistance)
                {
                    Point[] adjecent = current.Adjecent(blueprint, sandbag);

                    // Centrum af de sandsække der er til højre og venstre i lagene over og under
                    Vector3 leftMiddle = Vector3.Lerp(current.Position, adjecent[0].Position, 0.5f);
                    Vector3 rightMiddle = Vector3.Lerp(current.Position, adjecent[1].Position, 0.5f);

                    // Tilføj til liste hvis plads er tom og inden for viewDistance og ikke allerede gennemgået
                    if (current.Empty(position)) 
                    {
                        if (current.Position.y >= sandbag.Height)
                        {
                            Point belowLeft = new Point(new Vector3(leftMiddle.x, current.Position.y - sandbag.Height, leftMiddle.z));
                            Point belowRight = new Point(new Vector3(rightMiddle.x, current.Position.y - sandbag.Height, rightMiddle.z));

                            if (!belowLeft.Empty(position) && !belowRight.Empty(position))
                                places.Add(current.Position);

                            // Tiføj positioner under, hvis de er tomme
                            else if (belowLeft.Empty(position))
                                pointQueue.Enqueue(belowLeft.Position);

                            else if (belowRight.Empty(position))
                                pointQueue.Enqueue(belowRight.Position); 
                        }
                        else
                            places.Add(current.Position);
                    }

                    else
                    {
                        // Enqueue omkringliggende sandsække positioner
                        // Ved siden af
                        for (int i = 0; i < adjecent.Length; i++)
                        {
                            pointQueue.Enqueue(adjecent[i].Position);
                        }
                            
                        // Over
                        if (!adjecent[0].Empty(position))
                            pointQueue.Enqueue(new Vector3(leftMiddle.x, current.Position.y + sandbag.Height, leftMiddle.z));

                        if (!adjecent[1].Empty(position))
                            pointQueue.Enqueue(new Vector3(rightMiddle.x, current.Position.y + sandbag.Height, rightMiddle.z));
                    }
                    visited.Add(temp);
                }
            }

            if (places.Count == 0)
                return null;

            return places.ToArray();
        }

        /*
         * Parameters: Vector3[] containing all the possible locations, Vector3 containing the drones postion, float with the drones viewDistance.
         * 
         * Return: Vector3 containing the "best" location to place the sandbag
         */
        public Vector3 FindBestPlace(Vector3[] places, Vector3 position, float viewDistance)
        {
            // Check om det givne array er gyldigt
            if (places.Length < 1 || places == null)
            {
                Debug.Log("No places found");
                return ErrorVector;
            }

            // Return the first place the drone can access.
            for (int i = 0; i < places.Length; i++)
            {
                Point point = new Point(places[i]);
                if (point.Access(position, viewDistance, MinimumSeperation))
                    return places[i];
            }

            // No place could be accessed, return (What should be returned?)
            return ErrorVector;
        }

        /*
         * Parameters:
         * 
         * Return:
         */
        public Vector3 FindNextSection(float viewDistance, Vector3 position, bool isRightDrone, Blueprint blueprint)
        {
            Vector3 targetNode = isRightDrone ? blueprint.ConstructionNodes.Last() : blueprint.ConstructionNodes.First();

            // Samme højde som nuværende section
            targetNode.y = CurrentSection.y;

            // Udregn position der er viewDistance tættere på enden. Hvilken ende afgøres af isRightDrone.
            if (CurrentSection.Equals(targetNode))
                return CurrentSection;

            else
                return Vector3.MoveTowards(position, targetNode, viewDistance);
        }

        // Private Methods
        private Vector3 FindStartingPlace(Vector3 position, float viewDistance, float sandbagHeight)
        {
            GameObject result = GameObject.FindGameObjectsWithTag("PlacedSandbag")
                                       .FirstOrDefault(v => Vector3.Distance(position, v.transform.position) < viewDistance 
                                                            && new Point(v.transform.position).InView(position, viewDistance, sandbagHeight));

            return result == null ? ErrorVector : result.transform.position;
        }
    }

}