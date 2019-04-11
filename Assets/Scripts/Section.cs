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
            Vector3 startingBag = FindStartingPlace(position, viewDistance);
            if (startingBag.Equals(ErrorVector))
                return null;

            Queue<Vector3> q = new Queue<Vector3>();
            List<Vector3> visited = new List<Vector3>(); 
            q.Enqueue(startingBag);
            // Breadth first search
            while (q.Count > 0)
            {
                Vector3 current = q.Dequeue();
                if (!visited.Contains(current) && IsWithinBorder(current, blueprint, sandbag.Height) && Vector3.Distance(position, current) <= viewDistance)
                {
                    Vector3[] adjecent = FindAdjecent(current, blueprint, sandbag);

                    // Centrum af de sandsække der er til højre og venstre i lagene over og under
                    Vector3 leftMiddle = Vector3.Lerp(current, adjecent[0], 0.5f);
                    Vector3 rightMiddle = Vector3.Lerp(current, adjecent[1], 0.5f);

                    // Tilføj til liste hvis plads er tom og inden for viewDistance og ikke allerede gennemgået
                    if (IsEmpty(position, current))
                    {
                        // Tilføj kun hvis der er sække eller jord under dem (Midten af sækken er lavere end højden af en enkelt sandsæk)
                        Vector3 belowLeft = new Vector3(leftMiddle.x, current.y - sandbag.Height, leftMiddle.z);
                        Vector3 belowRight = new Vector3(rightMiddle.x, current.y - sandbag.Height, rightMiddle.z);
                        // Hvis jorden ikke er ved y = 0, så skal jordens starthøjde lægges til sandbag.height i udtrykket (Current.y er højden af sandsækkens midte)
                        if (IsInView(position, belowLeft, viewDistance, sandbag.Height) && IsInView(position, belowRight, viewDistance, sandbag.Height) || current.y < sandbag.Height)
                            places.Add(current);
                        // Tilføj placeringer under, hvis de kan er synlige
                        if (IsInView(position, belowLeft, viewDistance, sandbag.Height) && belowLeft.y > 0)
                            q.Enqueue(belowLeft);
                        if (IsInView(position, belowRight, viewDistance, sandbag.Height) && belowRight.y > 0)
                            q.Enqueue(belowRight);
                    }
                    else
                    {
                        // Enqueue omkringliggende sandsække positioner
                        // Ved siden af, x-akse
                        for(int i = 0; i < adjecent.Length; i++)
                            q.Enqueue(adjecent[i]);
                        // Over - Overvej at bruge Vector3.lerp
                        q.Enqueue(new Vector3(leftMiddle.x, current.y + sandbag.Height, leftMiddle.z));
                        q.Enqueue(new Vector3(rightMiddle.x, current.y + sandbag.Height, rightMiddle.z));
                    }
                    visited.Add(current);
                }
            }
            // Returner listen af placeringer som array
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
                return ErrorVector;

            // Return the first place the drone can access.
            for (int i = 0; i < places.Length; i++)
                if (IsAccess(places[i], position, viewDistance))
                    return places[i];

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
            // Sorter efter x-værdi
            Vector3 firstNode = blueprint.ConstructionNodes.First();
            Vector3 lastNode = blueprint.ConstructionNodes.Last();
            Vector3 nextSection;
            // Udregn position der er viewDistance tættere på enden. Hvilken ende afgøres af isRightDrone.
            if (isRightDrone)
                if (CurrentSection.Equals(lastNode))
                    return CurrentSection;
                else
                    nextSection = Vector3.MoveTowards(position, lastNode, viewDistance);
            else
                if (CurrentSection.Equals(firstNode))
                    return CurrentSection;
            else
                nextSection = Vector3.MoveTowards(position, firstNode, viewDistance);

            // Behold den samme højde
            nextSection.y = position.y;
            
            return nextSection;
        }

        // Private Methods
        /*
         * Parameters: Vector3 placementLocation, Vector3 position, float viewDistance.
         * 
         * Return: Bool whether or not the drone at given position access the given placementLocation.
         */
        private bool IsAccess(Vector3 placementLocation, Vector3 position, float viewDistance)
        {
            // Linq tilgang
            return GameObject.FindGameObjectsWithTag("Drone").
                FirstOrDefault(v => (Vector3.Distance(v.transform.position, placementLocation) <= MinimumSeperation)) == null ? true : false;

            /*
            // List of drones that are within viewDistance, does not contain the drone with the position given af parameter.
            List<Vector3> releventPositions = new List<Vector3>();

            // her er skrevet to forskellige tilgange, der burde virke ens(Bekræft!), afgør hvilken der er hurtigere og anvend denne.
            // Begge tilgange består umiddelbart tests.

            
            // Approach 1
            for(int i = 0; i < drones.Length; i++)
            {
                Vector3 dronePosition = drones[i].transform.position;
                // Only consider other drones
                if (dronePosition != position)
                    // Only consider if drone is within viewDistance
                    if(Vector3.Distance(dronePosition, position) <= viewDistance)
                        // Determine if drone is too close
                        if (Vector3.Distance(dronePosition, placementLocation) <= MinimumSeperation)
                            return false;
            }
            

            // Approach 2
            // Add position of drones within viewDistance to list
            for (int i = 0; i < drones.Length; i++)
            {
                Vector3 dronePosition = drones[i].transform.position;
                if (Vector3.Distance(dronePosition, position) <= viewDistance && !dronePosition.Equals(position))
                    releventPositions.Add(dronePosition);
            }
            // If any of the drones in view are too close, return false
            for (int i = 0; i < releventPositions.Count; i++)
                if (Vector3.Distance(releventPositions[i], placementLocation) <= MinimumSeperation)
                    return false;

            // Return true if no obstacles were found
            return true;
            */
        }

        /*
         * Parameters: Vector3 position of the drone, Vector3 position to evaluate.
         * 
         * Return: Bool whether or not the target position is empty.
         */
        private bool IsEmpty(Vector3 dronePosition, Vector3 target)
        {
            // Bruger raycast til at afgøre om der er en sandsæk i positionen
            //return !Physics.Linecast(dronePosition, target);
            return !Physics.Raycast(dronePosition, target - dronePosition, Vector3.Distance(dronePosition, target));
        }

        /*
         * Parameters: Vector3 position of the drone, Vector3 position to evaluate, float viewDistance of the drone, float height of a sandbag
         * 
         * Return: Bool whether or not the target position is in view of the drone
         */
        private bool IsInView(Vector3 dronePosition, Vector3 target, float viewDistance, float sandbagHeight)
        {
            // Check at raycast ikke finder noget lige over sandsækken
            target = Vector3.Lerp(target, new Vector3(target.x, target.y + sandbagHeight, target.z), 0.5f);
            float distance = Vector3.Distance(dronePosition, target);
            // True hvis der er lineOfSight og er inden for viewDistance
            return (distance < viewDistance) ? !IsEmpty(dronePosition, target) : false;
            // Samme resultat, forskellige metoder.
            //return (distance < viewDistance) ? Physics.Linecast(dronePosition, target) : false;
        }

        // Er den givne Vector3 inden for de given grænser
        // Understøtter kun dige med bredde af 1 sæk, men kan udvides.
        /*
         * Parameters: Vector3 Position to evaluate List<Vector3> List with constructionNodes that determine the border.
         * 
         * Return: Bool whether or not the given position is within the given constructionNodes.
         */
        private bool IsWithinBorder(Vector3 position, Blueprint blueprint, float sandbagHeight)
        {
            Vector3 firstNode = blueprint.ConstructionNodes.First();
            Vector3 lastNode = blueprint.ConstructionNodes.Last();
            if (position.y + sandbagHeight >= blueprint.DikeHeight * sandbagHeight || position.y >= blueprint.DikeHeight * sandbagHeight)
            {
                if (position.z >= firstNode.z && position.z <= lastNode.z)
                    if (position.x >= firstNode.x && position.x <= lastNode.x)
                        return true;
                return false;
            }
            else
                return true;
        }

        private Vector3 FindStartingPlace(Vector3 position, float viewDistance)
        {
            GameObject result = GameObject.FindGameObjectsWithTag("PlacedSandbag")
                                       .FirstOrDefault(v => Vector3.Distance(position, v.transform.position) < viewDistance);

            return result == null ? ErrorVector : result.transform.position;
        }

        private Vector3[] FindAdjecent(Vector3 position, Blueprint blueprint, SandbagController sandbag)
        {
            Vector3[] adjecent = new Vector3[2];

            adjecent[0] = Vector3.MoveTowards(position, blueprint.ConstructionNodes[0], sandbag.Length);
            adjecent[0].y = position.y;
            adjecent[1] = Vector3.MoveTowards(position, blueprint.ConstructionNodes[blueprint.ConstructionNodes.Count - 1], sandbag.Length);
            adjecent[1].y = position.y;
            return adjecent;
        }
    }

}