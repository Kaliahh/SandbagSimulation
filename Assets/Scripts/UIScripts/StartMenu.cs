//Denne klasse bruges til at styre GUI for startskærmen og føre information videre til simulationcontrolleren som har variablerne for simulationen samt
//kommandoer som at afslutte programmet
//using SandbagSimulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SandbagSimulation
{
    public class StartMenu : MonoBehaviour
    {
        //Initialisering af sider
        private GameObject TitleScreen; //Introduktionsside (Nr 0 i listen)
        private GameObject NewSimulationScreen; //Indstillingsside før ny simulation (Nr 1 i listen)
        private GameObject TutorialScreen; //Brugsanvisningsside (Nr 2 i listen)
        private GameObject OptionsScreen; //Simulationsinstillingesside (Nr 3 i listen)
        private List<GameObject> Screens; //En list med overstående sider for at kontrollere visning af de forskellige

        //Initialisering af simulation controller
        private GameObject SimulationController;

        void Start()
        {
            //Tildelning af sider
            TitleScreen = transform.Find("TitleScreen").gameObject; //Genererer reference til introduktionsside
            NewSimulationScreen = transform.Find("NewSimulationScreen").gameObject; //Genererer reference til indstillingside
            TutorialScreen = transform.Find("TutorialScreen").gameObject; //Genererer reference til brugsanvisningsside
            OptionsScreen = transform.Find("OptionsScreen").gameObject; //Genererer reference til brugsanvisningsside
            Screens = new List<GameObject>(); //Genererer en ny liste til siderne
            Screens.Add(TitleScreen); Screens.Add(NewSimulationScreen); Screens.Add(TutorialScreen); Screens.Add(OptionsScreen); //Ligger siderne ind i listen

            //Initialisering af simulation controller
            SimulationController = GameObject.FindWithTag("SimulationController"); //Genererer reference til simulationControlleren

            //Opsætning
            SelectScreen(0); //Fremvis introduktionsside
        }

        //Menu Methods
        public void SelectScreen(int selectedScreen) //Kaldt fra en knap med arguementet for den ønskede side (Dette argument kommer fra knap GameObjektet)
        {
            int ScreenIndex = 0; //Variabel for indesering (introduktionsside(0), indstillingsside(1), brugsanvisningside(2)
            foreach (GameObject screen in Screens) //Gennemgår alle sider i listen
            {
                if (ScreenIndex == selectedScreen) //Hvis den ønskede side forfindes
                    screen.SetActive(true);//så vis siden
                else //ellers
                    screen.SetActive(false);//gæm siden væk
                ScreenIndex++; //Næste side
            }
        }

        public void Quit()
        {
            SimulationController.GetComponent<SimulationController>().Quit();
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
            SimulationController.GetComponent<SimulationController>().NumberOfDrones = Value;
        }

        public void SliderSpeed(float value)
        {
            int Value = Mathf.RoundToInt(value); //Laver float værdien fra slideren til et helt antal af droner
                                                 //Ændrer teksten så brugeren ved hvor meget den står på
            GameObject gameobject = NewSimulationScreen.transform.Find("Speed").gameObject; //Reference til GameObjectet med slideren
            Text text = gameobject.transform.Find("Text").GetComponent<Text>(); //Finder teksten
            text.text = "Drone Speed: " + Value.ToString(); //Ændrer teksten

            //Sender værdien til simulationkontrolleren
            SimulationController.GetComponent<SimulationController>().DroneSpeed = Value;
        }

        public void StartSimulation()
        {
            SimulationController.GetComponent<SimulationController>().StartSimulation();
        }
    }
}

