//Denne klasse bruges til at vise resultaterne samt en knap om at gemme resultaterne væk
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SandbagSimulation;
public class Results : MonoBehaviour
{
    //Eksempler
    public string EvaluationReport = "1234567890$1234567890\u2265"; // Varible til evalueringsstrengen som bliver hentet fra evaluator

    private Text Position;  // Tekstblok til position evaluering. Bruges også hvis diget er optimalt.
    private Text Rotation;  // Tekstblok til rotation evaluering

    public GameObject SimulationController;
    public GameObject Runtime;

    void Start()
    {
        //Referencer
        Position = GameObject.Find("Position").GetComponent<Text>();
        Rotation = GameObject.Find("Rotation").GetComponent<Text>();

        
        //Debug.Log(SimulationController.GetComponent<SimulationController>().EvaluationReport);
    }

    void Update()
    {
        PrintString();
    }

    //Resultatet er en enkel streng fra evaluator.
    private void PrintString() 
    {
        EvaluationReport = SimulationController.GetComponent<SimulationController>().EvaluationReport;

        // Hvis diget er optimal ingen $, udskriv hele strengen
        if (!EvaluationReport.Contains("$"))
        {
            Rotation.text = EvaluationReport; 
            Position.text = null;
        }

        // Hvis diget mangler findes $. Del strengen op der.
        else
        {
            Rotation.text = EvaluationReport.Split('$')[0];
            Position.text = EvaluationReport.Split('$')[1];
        }

    }
}
