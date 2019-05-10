using SandbagSimulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunTime : MonoBehaviour
{
    // Variabler
    private Text TimeText; //Tid
    private Text DronesText; //Antallet af sandsække tilbage

    //Referencer
    private GameObject Menu; //Kørselsmenu
    public GameObject SimulationController; //Simulationskontrol

    void Start() 
    {
        //Referencer
        TimeText = GameObject.Find("Time").GetComponent<Text>(); // Tid
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

    // Vis eller gem kørselsmenuen
    private void ToggleMenu() 
    {
        // Hvis Menuen allerede er aktiv 
        if (Menu.activeSelf) 
        {
            // Fjern menuen
            Menu.SetActive(false); 
        }
        else 
        {
            // Åben menuen
            Menu.SetActive(true); 
        }
    }

    // Opdater tiden
    public void ChangeTime(float seconds) 
    {
        int TextMinuttes = (int)(seconds / 60);
        int TextSeconds = (int)(seconds % 60);

        TimeText.text = 
            (TextMinuttes < 10 ? "0" + TextMinuttes.ToString() : TextMinuttes.ToString()) + 
            ":" + 
            (TextSeconds < 10 ? "0" + TextSeconds.ToString() : TextSeconds.ToString());
    }

    // Sæt hastigheden på simulationen fra slideren
    public void ChangeFinishedDrones(int finishedDrones, int totalDrones) 
    {
        DronesText.text = "FinishedDrones: " + finishedDrones + "/" + totalDrones;
    }

    // Sæt hastigheden på simulationen fra slideren
    public void ChangeSimulationSpeed(float speed) 
    {
        Menu.gameObject.transform.Find("Slider").gameObject.transform.Find("Text").GetComponent<Text>().text = speed + "x";
        Time.timeScale = speed;
    }
}
