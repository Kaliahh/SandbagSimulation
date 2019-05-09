using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SandbagMeasurements
{
    public float Height { get; private set; }
    public float Width { get; private set; }
    public float Length { get; private set; }

    public SandbagMeasurements()
    {
        Height = 0.10f;
        Width = 0.35f;
        Length = 0.60f;
    }
}
