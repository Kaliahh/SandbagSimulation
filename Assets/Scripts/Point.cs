using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SandbagSimulation;

public class Point
{
    public Vector3 Position { get; set; }

    public Point(Vector3 position) => Position = position;

    /*
     * Parameters: Vector3 placementLocation, Vector3 position, float viewDistance.
     * 
     * Return: Bool whether or not the drone at given position access the given placementLocation.
     */
    public bool Access(Vector3 dronePosition, float viewDistance, float minDistance)
    {
        // Find Droner inden for viewdistance
        GameObject[] dronesInView = GameObject
            .FindGameObjectsWithTag("Drone")
            .Where(v => (Vector3.Distance(v.transform.position, dronePosition) <= viewDistance))
            .ToArray();

        if (dronesInView.Length < 1)
            return true;

        else
            return dronesInView
                .FirstOrDefault(v => (Vector3.Distance(v.transform.position, Position) <= minDistance)) == null ? true : false;
    }

    /*
    * Parameters: Vector3 position of the drone, Vector3 position to evaluate.
    * 
    * Return: Bool whether or not the target position is empty.
    * 
    * Vigtigt at sandsække med tagget "PickedUpSandbag" sættes i lag 2 der ignorer raycast, for at undgå at blokerer dronens syn
    */
    public bool Empty(Vector3 dronePosition)
    {
        // Tegner blå linjer hvis position er tom, og en rød linje hvis den ikke er tom.
        //if (Physics.Linecast(dronePosition, Position))
        //    Debug.DrawLine(dronePosition, Position, Color.red, 0.75f);
        //else
        //    Debug.DrawLine(dronePosition, Position, Color.blue, 0.75f);

        // Bruger linecast til at afgøre om der er et objekt positionen.
        //Collider[] intersecting = Physics.OverlapSphere(Position, 0.01f);
        //return intersecting.Length == 0;
        return !Physics.Linecast(dronePosition, Position);
    }

    /*
     * Parameters: Vector3 position of the drone, Vector3 position to evaluate, float viewDistance of the drone, float height of a sandbag
     * 
     * Return: Bool whether or not the target position is in view of the drone
     */
    public bool InView(Vector3 dronePosition, float viewDistance, float sandbagHeight)
    {
        // Check at raycast ikke finder noget lige over sandsækken
        float additionalHeight = 0.1f;
        Vector3 target = new Vector3(Position.x, Position.y + sandbagHeight + additionalHeight, Position.z);
        float distance = Vector3.Distance(dronePosition, target);

        // True hvis der er lineOfSight og er inden for viewDistance
        return (distance < viewDistance) ? !Physics.Linecast(dronePosition, target) : false;
    }

    // Undersøger om den givne Vector3 er inden for de given grænser?
    /*
     * Parameters: Vector3 Position to evaluate List<Vector3> List with constructionNodes that determine the border.
     * 
     * Return: Bool whether or not the given position is within the given constructionNodes.
     */
    public bool WithinBorder(Blueprint blueprint, float sandbagHeight, float maxDistance)
    {
        Vector3 firstNode = blueprint.ConstructionNodes.First();
        Vector3 lastNode = blueprint.ConstructionNodes.Last();

        if (OnLine(blueprint, maxDistance) && WithinHeight(blueprint, sandbagHeight))
            return true;

        else
            return false;
    }

    // Er et givent punkt på en linje, +/- maxdistance, givet ved blueprint. 
    public bool OnLine(Blueprint blueprint, float maxDistance)
    {
        Vector3 firstNode = blueprint.ConstructionNodes.First();
        Vector3 lastNode = blueprint.ConstructionNodes.Last();

        // TODO: Skal gøres mere læsbar, en masse magiske variabel navne
        Vector2 n = new Vector2(lastNode.x, lastNode.z) - new Vector2(firstNode.x, firstNode.z);
        Vector2 pa = new Vector2(firstNode.x, firstNode.z) - new Vector2(Position.x, Position.z);

        Vector2 c = n * (Vector2.Dot(n, pa) / Vector2.Dot(n, n));
        Vector2 d = pa - c;

        return (float)System.Math.Sqrt(Vector2.Dot(d, d)) <= maxDistance ? true : false;
    }

    // Afgør om punktet er under højden givet af blueprint.
    public bool WithinHeight(Blueprint blueprint, float sandbagHeight)
    {
        return Position.y / sandbagHeight > blueprint.DikeHeight ? false : true;
    }

    // Returnerer et array af tilstødende positioner til punktet.
    public Point[] Adjecent(Blueprint blueprint, SandbagController sandbag)
    {
        // TODO: Kunne lave retur-typen om til et dictionary for at gøre det mere læsevenligt
        Point[] adjecent = new Point[2];

        Vector3 ad1 = (blueprint.ConstructionNodes.First() - blueprint.ConstructionNodes.Last()).normalized * sandbag.Length;
        ad1 = Position + ad1;

        Vector3 ad2 = (blueprint.ConstructionNodes.Last() - blueprint.ConstructionNodes.First()).normalized * sandbag.Length;
        ad2 = Position + ad2;

        adjecent[0] = new Point(ad1);
        adjecent[1] = new Point(ad2);

        return adjecent;
    }
}
