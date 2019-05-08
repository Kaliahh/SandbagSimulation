using SandbagSimulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunTime : MonoBehaviour
{
    //Variabler
    private Text TimeText; //Tid
    private Text DronesText; //Antallet af sandsække tilbage

    //Referencer
    private GameObject Menu; //Kørselsmenu
    public GameObject SimulationController; //Simulationskontrol

    void Start() 
    {
        //Referencer
        TimeText = GameObject.Find("Time").GetComponent<Text>(); //Tid
        DronesText = GameObject.Find("Drones").GetComponent<Text>(); //Antallet af sandsække tilbage
        Menu = GameObject.Find("Menu"); //Kørselsmenu

        //Opsætning
        Menu.SetActive(false); //Gem kørselsmenuen væk
    }

    void Update() 
    {
        if (Input.GetKeyDown("escape"))
        {
            ToggleMenu();
        }
        ChangeTime(SimulationController.GetComponent<SimulationController>().TotalTime);
        ChangeFinishedDrones(SimulationController.GetComponent<SimulationController>().NumOfFinishedDrones, SimulationController.GetComponent<SimulationController>().NumberOfDrones);

    }
        

    private void ToggleMenu() //Vis eller gem kørselsmenuen
    {
        if (Menu.active) //If Menuen allerede er aktiv 
        {
            Menu.SetActive(false); //Fjern menuen
        }
        else //Ellers 
        {
            Menu.SetActive(true); //Åben menuen
        }
    }
    
    public void ChangeTime(float seconds) //Opdater tiden
    {
        int TextMinuttes = (int)(seconds / 60);
        int TextSeconds = (int)(seconds % 60);
        TimeText.text = "Time: " + TextMinuttes + ":" + TextSeconds % 60;
    }

    public void ChangeFinishedDrones(int finishedDrones, int totalDrones) //Sæt hastigheden på simulationen fra slideren
    {
        DronesText.text = "FinishedDrones: " + finishedDrones + "/" + totalDrones;
    }

    public void ChangeSimulationSpeed(float speed) //Sæt hastigheden på simulationen fra slideren
    {
        Menu.gameObject.transform.Find("Slider").gameObject.transform.Find("Text").GetComponent<Text>().text = speed + "x";
        Time.timeScale = speed;
    }
}
