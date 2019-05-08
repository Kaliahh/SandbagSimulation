//Denne klasse styrer visningen af de forskellige UI sider
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SandbagSimulation;

public class UI : MonoBehaviour
{
    //UI sider
    private GameObject MainMenu; //Startmenu UI
    private GameObject RunTime; //UI under simulationen
    private GameObject Results; //Resultatvindue når simulationen sluttes

    private List<GameObject> UIs; //Liste med alle sider. Nemmere at kontrollere siderne via en liste

    void Start()
    {
        //Referencer
        MainMenu = transform.Find("MainMenu").gameObject; //Reference til StartMenu UI
        RunTime = transform.Find("RunTime").gameObject;  //Reference til runtime UI
        Results = transform.Find("Results").gameObject;

        UIs = new List<GameObject> { MainMenu, RunTime, Results }; //Siderne lægges ind i listen

        //Opsætning
        StartMainMenu(); //Fremvis startmenuen når programmet startes
    }

    public void StartMainMenu() //Start startmenuen
    {
        
        foreach (GameObject ui in UIs)
        {
            if (ui.name == "MainMenu") //Kun vis start menuen
                ui.SetActive(true);
            else
                ui.SetActive(false);
        }
    }

    public void StartRunTime() //Start kørsels UI
    {
        foreach (GameObject ui in UIs)
        {
            Debug.Log(ui.name);
            if (ui.name == "RunTime") //Kun vis kørsels UI
                ui.SetActive(true);
            else
                ui.SetActive(false);
        }
    }

    public void ShowResults() //Vis resultatet
    {
        Results.SetActive(true);
    }

    public void HideResults() //Gæm resultatet væk, hvis der ønskes en forsat visning af simulationen
    {
        Results.SetActive(false);
    }
}

