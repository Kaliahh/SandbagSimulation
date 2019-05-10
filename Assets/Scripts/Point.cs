using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SandbagSimulation;

public class Point
{
    public Vector3 Position { get; set; }

    public Point(Vector3 position) => Position = position;

    // Bedømmer om en drone med en given position har adgang til punktet.
    // Returner falsk hvis der er andre droner for tæt på punktet.
    public bool Access(Vector3 dronePosition, float viewDistance, float minDistance)
    {
        // Find Droner inden for viewdistance.
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

    // Afgør om punktet er tomt.
    public bool Empty(Vector3 dronePosition)
    {
        return !Physics.Linecast(dronePosition, Position);
    }

    // Afgør om en drone med en given position, kan se punktet.
    public bool InView(Vector3 dronePosition, float viewDistance, float sandbagHeight)
    {
        // Check at linecast ikke rammer noget lige over sandsækken
        float additionalHeight = 0.1f;
        Vector3 target = new Vector3(Position.x, Position.y + sandbagHeight + additionalHeight, Position.z);
        float distance = Vector3.Distance(dronePosition, target);

        // Sand hvis der er lineOfSight og er inden for viewDistance.
        return (distance < viewDistance) ? !Physics.Linecast(dronePosition, target) : false;
    }

    // Afgør om punktet er inden for grænserne givet ved blueprint.
    public bool WithinBorder(Blueprint blueprint, float sandbagHeight, float maxDistance)
    {
        Vector3 firstNode = blueprint.ConstructionNodes.First();
        Vector3 lastNode = blueprint.ConstructionNodes.Last();

        if (OnLine(blueprint, maxDistance) && WithinHeight(blueprint, sandbagHeight))
            return true;
        else
            return false;
    }

    // Er et givent punkt på en linje med en acceptabel margen. 
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
    public Point[] Adjecent(Blueprint blueprint, SandbagMeasurements sandbag)
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

    // Finder og returnerer et array af punkter over ([0] = venstre, [1] = højre)
    public Point[] Above(Point[] adjecent, SandbagMeasurements sandbag)
    {
        Point[] abovePoints = new Point[2];

        Vector3 leftMiddle = Vector3.Lerp(Position, adjecent[0].Position, 0.5f);
        Vector3 rightMiddle = Vector3.Lerp(Position, adjecent[1].Position, 0.5f);

        abovePoints[0] = new Point(new Vector3(leftMiddle.x, Position.y + sandbag.Height, leftMiddle.z));
        abovePoints[1] = new Point(new Vector3(rightMiddle.x, Position.y + sandbag.Height, rightMiddle.z));

        return abovePoints;
    }

    // Finder og returnerer et array af punkter nedenunder 
    public Point[] Below(Point[] adjecent, SandbagMeasurements sandbag)
    {
        Point[] belowPoints = new Point[2];

        Vector3 leftMiddle = Vector3.Lerp(Position, adjecent[0].Position, 0.5f);
        Vector3 rightMiddle = Vector3.Lerp(Position, adjecent[1].Position, 0.5f);

        belowPoints[0] = new Point(new Vector3(leftMiddle.x, Position.y - sandbag.Height, leftMiddle.z));
        belowPoints[1] = new Point(new Vector3(rightMiddle.x, Position.y - sandbag.Height, rightMiddle.z));

        return belowPoints;
    }
}
