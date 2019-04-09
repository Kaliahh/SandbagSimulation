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
        public Section(Vector3 location) { CurrentSection = location; }

        // Public Methods

        /*
         * Parameters: float with the drones viewDistance, Vector3 with the drones position.
         * 
         * Return: Vector3[] containing the locations where it is possible for a drone to place a sandbag.
         */
        public Vector3[] FindPlace(float viewDistance, Vector3 position, List<Vector3> constructionNodes)
        {
            // List of alle the possible places
            List<Vector3> places = new List<Vector3>();

            // Bliver kun brugt til at finde højde og bredde af sandsække, skal findes på en bedre måde (Statiske properties i SandbagController?)
            SandbagController sandbag = Object.FindObjectOfType<SandbagController>();

            // Find en sandsæk der er lige under dronen (Problemer hvis der ikke er en)
            RaycastHit sandbagHit;
            Physics.Raycast(position, Vector3.down, out sandbagHit, viewDistance);

            Queue<Vector3> q = new Queue<Vector3>();
            List<Vector3> visited = new List<Vector3>(); 
            q.Enqueue(sandbagHit.transform.position);
            // Breadth first search
            while (q.Count > 0)
            {

                Vector3 current = q.Dequeue();
                Debug.Log(current.ToString());
                if (!visited.Contains(current) && IsWithinBorder(current, constructionNodes) && Vector3.Distance(position, current) <= viewDistance)
                {
                    // Tilføj til liste hvis plads er tom og inden for viewDistance og ikke allerede gennemgået
                    if (IsEmpty(position, current))
                    {
                        // Tilføj kun hvis der er sække eller jord under dem (Midten af sækken er lavere end højden af en enkelt sandsæk)
                        Vector3 below1 = new Vector3(current.x + sandbag.Length / 2, current.y - sandbag.Height, current.z);
                        Vector3 below2 = new Vector3(current.x - sandbag.Length / 2, current.y - sandbag.Height, current.z);
                        // Hvis jorden ikke er ved y = 0, så skal jordens starthøjde lægges til sandbag.height i udtrykket (Current.y er højden af sandsækkens midte)
                        if ((IsInView(position, below1, viewDistance, sandbag.Height) && IsInView(position, below2, viewDistance, sandbag.Height)) || current.y < sandbag.Height)
                        {
                            places.Add(current);
                        }
                        // Tilføj placeringer under, hvis de kan ses (Afgør om det egentlig er sandsække, så den ikke går under jorden)
                        if (IsInView(position, below1, viewDistance, sandbag.Height) && below1.y > 0)
                            q.Enqueue(below1);
                        if (IsInView(position, below2, viewDistance, sandbag.Height) && below2.y > 0)
                            q.Enqueue(below2);
                    }
                    else
                    {
                        // Enqueue omkringliggende sandsække positioner (Udregning burde gøres mere sikker, da der kan være regnefelj/approximationer)
                        // Ved siden af, x-akse
                        q.Enqueue(new Vector3(current.x + sandbag.Length, current.y, current.z));
                        q.Enqueue(new Vector3(current.x - sandbag.Length, current.y, current.z));
                        // Ved siden af, z-akse
                        //q.Enqueue(new Vector3(current.x, current.y, current.z + sandbag.Width));
                        //q.Enqueue(new Vector3(current.x, current.y, current.z - sandbag.Width));
                        // Over
                        //q.Enqueue(new Vector3(current.x + sandbag.Length / 2, current.y + sandbag.Height, current.z));
                        //q.Enqueue(new Vector3(current.x - sandbag.Length / 2, current.y + sandbag.Height, current.z));

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
            // Find the first location where it is possible for a drone to access

            // Need viewDistance

            // For each possible location
            // Determine if any other drones are at, or too close to location, (maybe determine if anyone is heading there)
            // Should it be considered if location currently inaccessible, will be accessible upon arrival?
            // Return the first accessible

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
        public Vector3 FindNextSection(float viewDistance, Vector3 position, bool isRightDrone, List<Vector3> constructionNodes)
        {
            // Sorter efter x-værdi
            Vector3[] sortedVectors = constructionNodes.OrderBy(v => v.x).ToArray<Vector3>();

            Vector3 nextSection;
            // Udregn position der er viewDistance tættere på enden. Hvilken ende afgøres af isRightDrone.
            if (isRightDrone)
                if (CurrentSection.Equals(sortedVectors[sortedVectors.Length - 1]))
                    return CurrentSection;
                else
                    nextSection = position + ((constructionNodes[constructionNodes.Count - 1] - position).normalized * viewDistance);
            else
                if (CurrentSection.Equals(sortedVectors[0]))
                return CurrentSection;
            else
                nextSection = position + ((constructionNodes[0] - position).normalized * viewDistance);

            // Behold den samme højde
            nextSection.y = position.y;
            // Position can be assumed to be the same as CurrentSection. Mulighed for at fjerne en parameter

            // Alternativ måde at skrive det på, en linje, men ikke ligefrem køn at se på, ikke tilpasset nested if-else
            // return ((isRightDrone ? constructionNodes[constructionNodes.Count - 1] : constructionNodes[0]) - position).normalized * viewDistance;

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
            // Array of all drone-Gameobjects
            GameObject[] drones = GameObject.FindGameObjectsWithTag("Drone");

            // List of drones that are within viewDistance, does not contain the drone with the position given af parameter.
            List<Vector3> releventPositions = new List<Vector3>();

            // her er skrevet rto forskellige tilgange, der burde virke ens(Bekræft!), afgør hvilken der er hurtigere og anvend denne.
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
         * Parameters: Vector3 with the center of the sphere, float with the radius of the sphere
         * 
         * Return: List<Vector3> List containing all placed sandbags in the sphere
         */
        private List<Vector3> FindSandbagsInRadius(Vector3 position, float radius)
        {
            // Burde tilpasses så den kun finder sandsække der er i LineOfSight
            List<Vector3> sandbagsInRadius = new List<Vector3>();

            // Array med alle sandsække
            SandbagController[] allSandbags = Object.FindObjectsOfType<SandbagController>();

            // Tilføj alle sandsække inden for viewDistance til liste, og find det øverste lag
            for (int i = 0; i < allSandbags.Length; i++)
            {
                Vector3 bagPosition = allSandbags[i].transform.position;
                float height = allSandbags[i].Height;
                if (Vector3.Distance(bagPosition, position) <= radius)
                    sandbagsInRadius.Add(bagPosition);
            }
            return sandbagsInRadius;
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
            target.y += sandbagHeight / 2;  // Igen, usikkerhed kan skabe problemer
            float distance = Vector3.Distance(dronePosition, target);
            // True hvis der er lineOfSight og er inden for viewDistance
            return (distance < viewDistance) ? !Physics.Linecast(dronePosition, target) : false;
        }

        // Er den givne Vector3 inden for de given grænser
        // Understøtter kun dige med bredde af 1 sæk, men kan udvides.
        /*
         * Parameters: Vector3 Position to evaluate List<Vector3> List with constructionNodes that determine the border.
         * 
         * Return: Bool whether or not the given position is within the given constructionNodes.
         */
        private bool IsWithinBorder(Vector3 position, List<Vector3> constructionNodes)
        {
            Vector3[] sortedVectors = constructionNodes.OrderBy(v => v.x).ToArray<Vector3>();
            return position.x > sortedVectors[0].x && position.x < sortedVectors[sortedVectors.Length - 1].x ? true : false;
        }
    }

}