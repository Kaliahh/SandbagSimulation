using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Vector3[] FindPlace(float viewDistance, Vector3 position)
    {
        // List of alle the possible places
        List<Vector3> places = new List<Vector3>();
        // list of all the sandbags within viewDistance
        List<Vector3> bagsInView = new List<Vector3>();

        // Array of all sandbag-Gameobjects
        GameObject[] sandbags = GameObject.FindGameObjectsWithTag("Sandbag");
        
        // Starter med lag 1
        int layer = 1;

        // Add sandbags within viewDistance to list
        for (int i = 0; i < sandbags.Length; i++)
        {
            Vector3 bagPosition = sandbags[i].transform.position;
            if (Vector3.Distance(bagPosition, position) <= viewDistance)
                bagsInView.Add(bagPosition);
            // Determine the top layer
            //If(bagPosition.y / sandbag.height > layer)
            //Layer = bagPosition / sandbag.height;
        }

        // Identify where it is possible to place one
        // Identify the top layer

        // Layer-by-layer approach
        // Find sandbags that have unoccupied sides.
        // Find the position a sandbag needs to have in order for it to occupy this side.
        // Add this place to list
        // This allows a position to be on list multiple times


        // Return the list of places as array
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
    public Vector3 FindNextSection(float viewDistance, Vector3 position)
    {
        // Take IsRightDrone into consideration?
        // Use construction nodes as guidelines?

        // Udregn position der er viewDistance tættere på enden.
        // if(IsRightDrone)
            // nextSection = position + ((rightMostConstructionNode.position - position).normalized * viewDistance)
        // else
            // nextSection = position + ((leftMostConstructionNode.position - position).normalized * viewdistance)

        // Position can be assumed to be the same as CurrentSection.


        // Find and return the locatíon of the next section to work on 
        // Set currentSection to the found section?
        return new Vector3();
    }

    // Private Methods
    /*
     * Parameters: Vector3 placementLocation, Vector3 position, float viewDistance
     * 
     * Return: Bool whether or not the drone at given position access the given placementLocation.
     */
    private bool IsAccess(Vector3 placementLocation, Vector3 position, float viewDistance)
    {
        // Array of all drone-Gameobjects
        GameObject[] drones = GameObject.FindGameObjectsWithTag("Drone");

        // List of drones that are within viewDistance, does not contain the drone with the position given af parameter.
        List<Vector3> releventPositions = new List<Vector3>();

        // Here are 2 different approaches, they should work the same(Verify!), determine whitch is faster and use that

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
        {
            if (Vector3.Distance(releventPositions[i], placementLocation) <= MinimumSeperation)
                return false;
        }
        
        // Return true if no obstacles were found
        return true;
    }

}
