using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunTime : MonoBehaviour
{
    private Text Time; //Tid
    private Text SandbagsLeft; //Amount of sandbags left before simulation is ended

    void Start() 
    {
        
    }

    public void ChangeTime(float seconds) 
    {

        Time.text = seconds.ToString();
    }
}
