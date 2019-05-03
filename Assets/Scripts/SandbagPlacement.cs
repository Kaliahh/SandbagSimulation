﻿//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace SandbagSimulation
//{
//    public class SandbagPlacement
//    {
//        private Vector3 ErrorVector = new Vector3(-100f, -100f, -100f);

//        public float MaximumPlacementDeviation { get; set; }
//        public float MinimumSeperation { get; set; }

//        public SandbagPlacement(float minimumSeperation)
//        {
//            MinimumSeperation = minimumSeperation;
//        }



//        /*
//             * Parameters: float with the drones viewDistance, Vector3 with the drones position.
//             * 
//             * Return: Vector3[] containing the locations where it is possible for a drone to place a sandbag.
//             */
//        public Vector3[] FindPlace(float viewDistance, Vector3 position, Blueprint blueprint)
//        {
//            // List of alle the possible places
//            List<Vector3> places = new List<Vector3>();

//            // Bliver kun brugt til at finde højde og bredde af sandsække, skal findes på en bedre måde (Statiske properties i SandbagController?)
//            SandbagController sandbag = Object.FindObjectOfType<SandbagController>();

//            // Find en sandsæk til at bruge som udgangspunkt
//            Vector3 startingBag = FindStartingPlace(position, viewDistance, sandbag.Height);

//            if (startingBag.Equals(ErrorVector))
//            {
//                Debug.Log("No startingpoint found");
//                Debug.DrawLine(position, position + new Vector3(0, 10, 0), Color.red, 1);
//                return null;
//            }

//            MaximumPlacementDeviation = 0.5f; // TODO: Magisk tal

//            Queue<Vector3> pointQueue = new Queue<Vector3>();
//            List<Vector3> visited = new List<Vector3>();

//            pointQueue.Enqueue(startingBag);

//            // Breadth first search
//            while (pointQueue.Count > 0)
//            {
//                Point current = new Point(pointQueue.Dequeue());
//                Vector3 temp = Round(current.Position);

//                if (!visited.Contains(temp)
//                    && current.WithinBorder(blueprint, sandbag.Height, MaximumPlacementDeviation)
//                    && Vector3.Distance(position, current.Position) <= viewDistance)
//                {
//                    Point[] adjecent = current.Adjecent(blueprint, sandbag);

//                    // Tilføj til liste hvis plads er tom og inden for viewDistance og ikke allerede gennemgået
//                    if (current.Empty(position))
//                    {
//                        if (current.Position.y >= sandbag.Height)
//                        {
//                            Point[] belowPoints = current.Below(adjecent, sandbag);
//                            if (!belowPoints[0].Empty(position) && !belowPoints[1].Empty(position))
//                                places.Add(current.Position);
//                            else
//                                EnqueueBelow(ref pointQueue, position, belowPoints);
//                        }
//                        else
//                            places.Add(current.Position);
//                    }
//                    else
//                    {
//                        Point[] abovePoints = current.Above(adjecent, sandbag);
//                        for (int i = 0; i < adjecent.Length; i++)
//                            pointQueue.Enqueue(adjecent[i].Position);
//                        EnqueueAbove(ref pointQueue, position, abovePoints, adjecent);
//                    }
//                    visited.Add(temp);
//                }
//            }
//            if (places.Count == 0)
//                return null;

//            return places.ToArray();
//        }

//        // Indsætter de underliggende positioner i den givne kø, hvis de ikke er optaget
//        private void EnqueueBelow(ref Queue<Vector3> queue, Vector3 position, Point[] belowPoints)
//        {
//            for (int i = 0; i < belowPoints.Length; i++)
//                if (belowPoints[i].Empty(position))
//                    queue.Enqueue(belowPoints[i].Position);
//        }

//        // Indsætter de overliggende positioner, hvis der er sandsække i begge underliggende positioner
//        private void EnqueueAbove(ref Queue<Vector3> queue, Vector3 position, Point[] abovePoints, Point[] adjecent)
//        {
//            for (int j = 0; j < abovePoints.Length; j++)
//                if (!adjecent[j].Empty(position))
//                    queue.Enqueue(abovePoints[j].Position);
//        }

//        /*
//         * Parameters: Vector3[] containing all the possible locations, Vector3 containing the drones postion, float with the drones viewDistance.
//         * 
//         * Return: Vector3 containing the "best" location to place the sandbag
//         */
//        public Vector3 FindBestPlace(Vector3[] places, Vector3 position, float viewDistance)
//        {
//            // Check om det givne array er gyldigt
//            if (places.Length < 1 || places == null)
//            {
//                Debug.Log("No places found");
//                return ErrorVector;
//            }

//            // Return the first place the drone can access.
//            for (int i = 0; i < places.Length; i++)
//            {
//                Point point = new Point(places[i]);
//                if (point.Access(position, viewDistance, MinimumSeperation))
//                    return places[i];
//            }

//            // No place could be accessed, return (What should be returned?)
//            return ErrorVector;
//        }

//        private Vector3 Round(Vector3 vector)
//        {
//            return new Vector3
//                (
//                    (float)System.Math.Round(vector.x, 1),
//                    (float)System.Math.Round(vector.y, 1),
//                    (float)System.Math.Round(vector.z, 1)
//                );
//        }

//        private Vector3 FindStartingPlace(Vector3 position, float viewDistance, float sandbagHeight)
//        {
//            GameObject result = GameObject.FindGameObjectsWithTag("PlacedSandbag")
//                                       .FirstOrDefault(v => new Point(v.transform.position).InView(position, viewDistance, sandbagHeight));

//            return result == null ? ErrorVector : result.transform.position;
//        }



//    }
//}
