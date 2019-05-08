using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SandbagSimulation;

public class TotalTime : MonoBehaviour
{
    public bool Run { get; set; } //Skal tiden køre?
    public float RunTime { get; set; } //Tid simulationen har kørt

    void Update()
    {
        if (Run) //Hvis tiden skal køre 
        {
            RunTime += Time.deltaTime; //Tæl tiden op
        }
    }
}
