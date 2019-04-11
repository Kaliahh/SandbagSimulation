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
            if (startingBag.Equals(new Vector3(-100, -100, -100)))
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
                    // Centrum af de sandsække der er til højre og venstre i lagene over og under
                    Vector3 rightMiddle = Vector3.Lerp(new Vector3(current.x, current.y, current.z + sandbag.Length), current, 0.5f);
                    Vector3 leftMiddle = Vector3.Lerp(new Vector3(current.x, current.y, current.z - sandbag.Length), current, 0.5f);
                    //Debug.Log("Current: " + current.ToString());
                    // Tilføj til liste hvis plads er tom og inden for viewDistance og ikke allerede gennemgået
                    if (IsEmpty(position, current))
                    {
                        // Tilføj kun hvis der er sække eller jord under dem (Midten af sækken er lavere end højden af en enkelt sandsæk)

                        Vector3 below1 = new Vector3(current.x, current.y - sandbag.Height, rightMiddle.z);
                        Vector3 below2 = new Vector3(current.x, current.y - sandbag.Height, leftMiddle.z);
                        // Hvis jorden ikke er ved y = 0, så skal jordens starthøjde lægges til sandbag.height i udtrykket (Current.y er højden af sandsækkens midte)
                        //Debug.Log("Below: " + below1.ToString() + " " + below2.ToString());
                        //Debug.Log(IsInView(position, below1, viewDistance, sandbag.Height).ToString() + " " + IsInView(position, below2, viewDistance, sandbag.Height).ToString());
                        if (IsInView(position, below1, viewDistance, sandbag.Height) && IsInView(position, below2, viewDistance, sandbag.Height) || current.y < sandbag.Height)
                        {
                            //Debug.Log("Adding: " + current.ToString());
                            places.Add(current);
                        }
                        // Tilføj placeringer under, hvis de kan ses 
                        if (IsInView(position, below1, viewDistance, sandbag.Height) && below1.y > 0)
                            q.Enqueue(below1);
                        if (IsInView(position, below2, viewDistance, sandbag.Height) && below2.y > 0)
                            q.Enqueue(below2);

                    }
                    else
                    {
                        // Enqueue omkringliggende sandsække positioner (Udregning burde gøres mere sikker, da der kan være regnefelj/approximationer)
                        // Ved siden af, x-akse
                        q.Enqueue(new Vector3(current.x, current.y, current.z + sandbag.Length));
                        q.Enqueue(new Vector3(current.x, current.y, current.z - sandbag.Length));
                        // Ved siden af, z-akse
                        //q.Enqueue(new Vector3(current.x, current.y, current.z + sandbag.Width));
                        //q.Enqueue(new Vector3(current.x, current.y, current.z - sandbag.Width));
                        // Over - Overvej at bruge Vector3.lerp
                        q.Enqueue(new Vector3(current.x, current.y + sandbag.Height, rightMiddle.z));
                        q.Enqueue(new Vector3(current.x, current.y + sandbag.Height, leftMiddle.z));

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
            Vector3 errorVector = new Vector3(-100f, -100f, -100f);

            // Check om det givne array er gyldigt
            if (places.Length < 1 || places == null)
                return errorVector;

            // Return the first place the drone can access.
            for (int i = 0; i < places.Length; i++)
                if (IsAccess(places[i], position, viewDistance))
                    return places[i];

            // No place could be accessed, return (What should be returned?)
            return errorVector;
        }

        /*
         * Parameters:
         * 
         * Return:
         */
        public Vector3 FindNextSection(float viewDistance, Vector3 position, bool isRightDrone, Blueprint blueprint)
        {
            // Sorter efter x-værdi
            //Vector3[] sortedVectors = blueprint.ConstructionNodes.OrderBy(v => v.x).ToArray<Vector3>();     // Måske unødvendig
            Vector3 firstNode = blueprint.ConstructionNodes[0];
            Vector3 lastNode = blueprint.ConstructionNodes[blueprint.ConstructionNodes.Count - 1];
            Vector3 nextSection;
            // Udregn position der er viewDistance tættere på enden. Hvilken ende afgøres af isRightDrone.
            if (isRightDrone)
                if (CurrentSection.Equals(lastNode))
                    return CurrentSection;
                else
                    nextSection = position + ((lastNode - position).normalized * viewDistance);
            else
                if (CurrentSection.Equals(firstNode))
                return CurrentSection;
            else
                nextSection = position + ((firstNode - position).normalized * viewDistance);

            // Behold den samme højde
            nextSection.y = position.y;
            // behold samme z-koordinat (Skal ændres hvis der bliver mulighed for brede diger)
            if (firstNode.x == lastNode.x)
                nextSection.x = firstNode.x;

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
            // Der gøres ikke nogne forsøg på at forudsige om der er adgang ved ankomst

            // Array of all drone-Gameobjects
            GameObject[] drones = GameObject.FindGameObjectsWithTag("Drone");

            // List of drones that are within viewDistance, does not contain the drone with the position given af parameter.
            List<Vector3> releventPositions = new List<Vector3>();

            // her er skrevet to forskellige tilgange, der burde virke ens(Bekræft!), afgør hvilken der er hurtigere og anvend denne.
            // Begge tilgange består umiddelbart tests.

            /*
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
            */

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
            Vector3[] sortedVectors = blueprint.ConstructionNodes.OrderBy(v => v.z).ToArray<Vector3>();     // Måske unødvendig
            if (position.y + sandbagHeight >= blueprint.DikeHeight * sandbagHeight || position.y >= blueprint.DikeHeight * sandbagHeight)
            {
                return position.z > sortedVectors[0].z && position.z < sortedVectors[sortedVectors.Length - 1].z ? true : false;
            }
            else
                return true;
        }

        private Vector3 FindStartingPlace(Vector3 position, float viewDistance)
        {
            GameObject[] sandbags = GameObject.FindGameObjectsWithTag("PlacedSandbag");

            Vector3 bagPosition;
            for(int i = 0; i < sandbags.Length; i++)
            {
                bagPosition = sandbags[i].transform.position;
                if (Vector3.Distance(position, bagPosition) < viewDistance)
                    return bagPosition;
            }

            return new Vector3(-100, -100, -100);
        }
    }

}