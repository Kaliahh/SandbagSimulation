//Denne klasse håndterer inputs i start menuen og hvilke sider i start menuen som skal vises.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SandbagSimulation;

public class MainMenu : MonoBehaviour
{
    //Initialisering af sider
    private GameObject TitleScreen; //Introduktionsside (Nr 0 i listen)
    private GameObject NewSimulationScreen; //Nysimulationsside(Nr 1 i listen)
    private GameObject TutorialScreen; //Brugsanvisningsside (Nr 2 i listen)
    private List<GameObject> Screens; //En list med overstående sider for at kontrollere visning af de forskellige

    //Initialisering af simulation controller
    private GameObject UI;

    void Start()
    {
        //Referencer
        TitleScreen = transform.Find("TitleScreen").gameObject; //Reference til introduktionsside
        NewSimulationScreen = transform.Find("NewSimulationScreen").gameObject; //Reference til indstillingside
        TutorialScreen = transform.Find("TutorialScreen").gameObject; //Reference til brugsanvisningsside

        Screens = new List<GameObject> { TitleScreen, NewSimulationScreen, TutorialScreen}; //Siderne lægges ind i listen

        UI = GameObject.Find("UI"); //Reference til UI

        //Opsætning
        SelectScreen(0); //Fremvis introduktionsside
    }

    //Menu Methods
    public void SelectScreen(int selectedScreen) //Kaldt fra en knap med arguementet for den ønskede side
    {
        int ScreenIndex = 0; //Variabel for indesering (introduktionsside(0), indstillingsside(1), brugsanvisningside(2)
        foreach (GameObject screen in Screens) //Gennemgår alle sider i listen og viser den ønskede side og gemmer resten væk
        {
            if (ScreenIndex == selectedScreen)
                screen.SetActive(true);
            else
                screen.SetActive(false);
            ScreenIndex++;
        }
    }

    public void Quit()
    {
        Debug.Log("Quitting, not implemented");
        //UI.GetComponent<UI>().Quit();
    }

    //New Simulation Methods
    public void SliderNumberOfDrones(float value)
    {
        int Value = Mathf.RoundToInt(value); //Laver float værdien fra slideren til et helt antal af droner
                                             //Ændrer teksten så brugeren ved hvor meget den står på
        GameObject gameobject = NewSimulationScreen.transform.Find("NumberOfDrones").gameObject; //Reference til GameObjectet med slideren
        Text text = gameobject.transform.Find("Text").GetComponent<Text>(); //Finder teksten
        text.text = "Number of Drones: " + Value.ToString(); //Ændrer teksten

        //Sender værdien til simulationkontrolleren
        Debug.Log("Ændrer Antalet af droner. IKKE IMPLEMTERET");
        //SimulationController.GetComponent<SimulationController>().NumberOfDrones = Value;
    }

    public void SliderSpeed(float value)
    {
        int Value = Mathf.RoundToInt(value); //Laver float værdien fra slideren til et helt antal af droner
                                             //Ændrer teksten så brugeren ved hvor meget den står på
        GameObject gameobject = NewSimulationScreen.transform.Find("Speed").gameObject; //Reference til GameObjectet med slideren
        Text text = gameobject.transform.Find("Text").GetComponent<Text>(); //Finder teksten
        text.text = "Drone Speed: " + Value.ToString(); //Ændrer teksten

        //Sender værdien til simulationkontrolleren
        Debug.Log("Ændrer droners hastighed. IKKE IMPLEMTERET");
        //SimulationController.GetComponent<SimulationController>().DroneSpeed = Value;
    }

    public void StartSimulation() //Kaldt fra start simulationsknappen fra nysimulationssiden
    {
        UI.GetComponent<UI>().StartRunTime();
    }
}

