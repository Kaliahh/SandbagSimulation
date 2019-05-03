//Denne klasse bruges til at vise resultaterne samt en knap om at gemme resultaterne væk
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SandbagSimulation;
public class Results : MonoBehaviour
{
    //Eksempler
    private string EvaluationReport = "1234567890$1234567890\u2265"; //Varible til evalueringsstrengen som bliver hentet fra evaluator
    private int TimeString = 200;

    private Text TotalTime; //Tekstblok til tid
    private Text Position; //Tekstblok til position evaluering. Bruges også hvis diget er optimalt.
    private Text Rotation; //Tekstblok til rotation evaluering

    // Start is called before the first frame update
    void Start()
    {
        //Referencer
        TotalTime = GameObject.Find("TotalTime").GetComponent<Text>();
        Position = GameObject.Find("Position").GetComponent<Text>();
        Rotation = GameObject.Find("Rotation").GetComponent<Text>();

        PrintString();
    }
    
    private void PrintString() //Resultatet er en enkel streng fra evaluator.
    {
        //Skriver tiden
        TotalTime.text = "Det tog " + TimeString + " sekunder.";

        //Hvis diget er optimal ingen $
        if (!EvaluationReport.Contains("$"))
            Position.text = EvaluationReport; //Udskriver hele strengen

        //Hvis diget mangler findes $. Del strengen op der.
        else
            Position.text = EvaluationReport.Split('$')[0];
            Rotation.text = EvaluationReport.Split('$')[1];
    }
}
