using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Denne datatype bruges til at repræsentere evalueringsdata om sandsækkenes rotation og position.
public class SandbagData
{
    public Vector3 Position,
                   Right,
                   Up,
                   Forward;

    // Denne constructor bruges til at repræsentere elementer i Dike.
    public SandbagData(GameObject obj)
    {
        Position = obj.transform.position;
        Right = obj.transform.right;
        Up = obj.transform.up;
        Forward = obj.transform.forward;
    }

    // Denne constructor bruges til elementer, hvor kun rotation er relevant.
    public SandbagData(Vector3 right, Vector3 up, Vector3 forward)
    {
        Position = Vector3.negativeInfinity;
        Right = right;
        Up = up;
        Forward = forward;
    }

    // Denne constructor bruges udelukkende til at gøre tests mere bekvemme.
    public SandbagData(Vector3 position)
    {
        Position = position;
        Right = Vector3.negativeInfinity;
        Up = Vector3.negativeInfinity;
        Forward = Vector3.negativeInfinity;
    }
}
