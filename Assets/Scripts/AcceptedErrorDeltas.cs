using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Denne datatype indeholder den accepterede fejlmargin for hhv. rotation og position.
public class AcceptedErrorDeltas
{
    public double Rotation { get; private set; }
    public double Position { get; private set; }

    public AcceptedErrorDeltas()
    {
        Rotation = 5.0;
        Position = 0.03;
}
}
