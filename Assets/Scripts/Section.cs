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
        public float MaximumPlacementDeviation { get; private set; }
        public Vector3 ErrorVector { get; private set; }

        public Section()
        {
            ErrorVector = new Vector3(-100, -100, -100);
            MinimumSeperation = 1;
            MaximumPlacementDeviation = 0.5f;
        }

        // Returnerer et array med alle punkter hvor det er muligt for en drone med en given position, at placere en sandsæk.
        public Vector3[] FindPlace(float viewDistance, Vector3 position, Blueprint blueprint)
        {
            List<Vector3> places = new List<Vector3>();
            SandbagMeasurements sandbag = new SandbagMeasurements();

            // Find en sandsæk til at bruge som udgangspunkt
            Vector3 startingBag = FindStartingPlace(position, viewDistance, sandbag.Height);

            if (startingBag.Equals(ErrorVector))
            {
                Debug.Log("No startingpoint found");
                return null;
            }

            Queue<Vector3> pointQueue = new Queue<Vector3>();
            List<Vector3> visited = new List<Vector3>();

            pointQueue.Enqueue(startingBag);

            // Breadth first search
            while (pointQueue.Count > 0)
            {
                Point current = new Point(pointQueue.Dequeue());
                Vector3 temp = Round(current.Position);

                if (!visited.Contains(temp)
                    && current.WithinBorder(blueprint, sandbag.Height, MaximumPlacementDeviation)
                    && Vector3.Distance(position, current.Position) <= viewDistance)
                {
                    Point[] adjecent = current.Adjecent(blueprint, sandbag);

                    // Tilføj til liste hvis plads er tom og inden for viewDistance og ikke allerede gennemgået.
                    if (current.Empty(position)) 
                    {
                        if (current.Position.y >= sandbag.Height)
                        {
                            Point[] belowPoints = current.Below(adjecent, sandbag);
                            if (!belowPoints[0].Empty(position) && !belowPoints[1].Empty(position))
                                places.Add(current.Position);
                            else
                                EnqueueBelow(ref pointQueue, position, belowPoints);
                        }
                        else
                            places.Add(current.Position);
                    }
                    else
                    {
                        Point[] abovePoints = current.Above(adjecent, sandbag);
                        for (int i = 0; i < adjecent.Length; i++)
                            pointQueue.Enqueue(adjecent[i].Position);
                        EnqueueAbove(ref pointQueue, position, abovePoints, adjecent);
                    }
                    visited.Add(temp);
                }
            }
            if (places.Count == 0)
                return null;

            return places.ToArray();
        }

        // Indsætter de underliggende positioner i den givne kø, hvis de ikke er optaget.
        private void EnqueueBelow(ref Queue<Vector3> queue, Vector3 position, Point[] belowPoints)
        {
            for (int i = 0; i < belowPoints.Length; i++)
                if (belowPoints[i].Empty(position))
                    queue.Enqueue(belowPoints[i].Position);
        }

        // Indsætter de overliggende positioner, hvis der er sandsække i begge underliggende positioner.
        private void EnqueueAbove(ref Queue<Vector3> queue, Vector3 position, Point[] abovePoints, Point[] adjecent)
        {
            for (int j = 0; j < abovePoints.Length; j++)
                if (!adjecent[j].Empty(position))
                    queue.Enqueue(abovePoints[j].Position);
        }

        // Returnerer det første element i et givent array af mulige placeringer, som en drone med en given position har adgang.
        public Vector3 FindBestPlace(Vector3[] places, Vector3 position, float viewDistance)
        {
            // Check om det givne array er gyldigt.
            if (places.Length < 1 || places == null)
            {
                //Debug.Log("No places found");
                return ErrorVector;
            }

            for (int i = 0; i < places.Length; i++)
            {
                Point point = new Point(places[i]);
                if (point.Access(position, viewDistance, MinimumSeperation))
                    return places[i];
            }

            // Ingen mulige placeringer.
            return ErrorVector;
        }

        // Afrunder en given vektors værdier til én decimal, og returner den afrundede vektor.
        private Vector3 Round(Vector3 vector)
        {
            return new Vector3
                (
                    (float)System.Math.Round(vector.x, 1),
                    (float)System.Math.Round(vector.y, 1),
                    (float)System.Math.Round(vector.z, 1)
                );
        }

        // Finder den section, som en drone med en given position skal arbjede i.
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

        private Vector3 FindStartingPlace(Vector3 position, float viewDistance, float sandbagHeight)
        {
            GameObject result = GameObject.FindGameObjectsWithTag("PlacedSandbag")
                                       .Where(v => new Point(v.transform.position).InView(position, viewDistance, sandbagHeight))
                                       .OrderBy(v => Vector3.Distance(v.transform.position, position))
                                       .FirstOrDefault();

            return result == null ? ErrorVector : result.transform.position;
        }
    }

}