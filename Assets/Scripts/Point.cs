﻿using System.Collections;
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
    // Udregner afstanden fra et punkt til en linje, og afgør om det er inden for en acceptabel afstand. Der ses bort fra højden.
    // Kilde: Randy Gaul https://www.randygaul.net/2014/07/23/distance-point-to-line-segment/
    // Postet 23/07-2014. Senest tilgået 10/05-2019
    public bool OnLine(Blueprint blueprint, float maxDistance)
    {
        Vector3 firstNode = blueprint.ConstructionNodes.First();
        Vector3 lastNode = blueprint.ConstructionNodes.Last();

        // Find længden af diget
        Vector2 length = new Vector2(lastNode.x, lastNode.z) - new Vector2(firstNode.x, firstNode.z);

        // Find vektor fra den ene ende af diget til positionen
        Vector2 positionToFirstNode = new Vector2(firstNode.x, firstNode.z) - new Vector2(Position.x, Position.z);

        // Find den ortogonale vektor på linjen
        Vector2 orthogonalVector = length * (Vector2.Dot(length, positionToFirstNode) / Vector2.Dot(length, length));

        // Find afstanden mellem linjen og punktet
        Vector2 distanceFromLine = positionToFirstNode - orthogonalVector;

        // Returner om afstanden mellem linjen og punktet er under den maksimale afstand
        return distanceFromLine.magnitude <= maxDistance ? true : false;
    }

    // Afgør om punktet er under højden givet af blueprint.
    public bool WithinHeight(Blueprint blueprint, float sandbagHeight)
    {
        return Position.y / sandbagHeight > blueprint.DikeHeight ? false : true;
    }

    // Returnerer et array af tilstødende positioner til punktet.
    public Point[] Adjecent(Blueprint blueprint, SandbagMeasurements sandbag)
    {
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
