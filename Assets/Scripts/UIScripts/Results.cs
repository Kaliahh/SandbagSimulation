//Denne klasse bruges til at vise resultaterne samt en knap om at gemme resultaterne væk
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SandbagSimulation;
public class Results : MonoBehaviour
{
    //Eksempler
    public string EvaluationReport = "1234567890$1234567890\u2265"; //Varible til evalueringsstrengen som bliver hentet fra evaluator
    public int TimeString = 200;

    private Text TotalTime; //Tekstblok til tid
    private Text Position; //Tekstblok til position evaluering. Bruges også hvis diget er optimalt.
    private Text Rotation; //Tekstblok til rotation evaluering

    public GameObject SimulationController;
    public GameObject Runtime;

    // Start is called before the first frame update
    void Start()
    {
        //Referencer
        TotalTime = GameObject.Find("TotalTime").GetComponent<Text>();
        Position = GameObject.Find("Position").GetComponent<Text>();
        Rotation = GameObject.Find("Rotation").GetComponent<Text>();
    }

    private void Update()
    {
        EvaluationReport = SimulationController.GetComponent<SimulationController>().EvaluationReport;
        PrintString();
        Debug.Log(SimulationController.GetComponent<SimulationController>().EvaluationReport);
    }

    private void PrintString() //Resultatet er en enkel streng fra evaluator.
    {
        //Skriver tiden
        TotalTime.text = "Det tog " + TimeString + " sekunder.";

        //Hvis diget er optimal ingen $
        if (!EvaluationReport.Contains("$"))
        {
            Rotation.text = EvaluationReport; //Udskriver hele strengen
            Position.text = null;
        }

        //Hvis diget mangler findes $. Del strengen op der.
        else
        {
            Rotation.text = EvaluationReport.Split('$')[0];
            Position.text = EvaluationReport.Split('$')[1];
            
            Debug.Log("Kørt");
        }

    }
}
