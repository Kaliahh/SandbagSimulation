using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SandbagSimulation
{
    public class UI : MonoBehaviour
    {
        //UI sider
        private GameObject MainMenu; //Startmenu UI
        private GameObject RunTime; //UI under simulationen
        private GameObject Results; //Resultatvindue når simulationen sluttes

        private List<GameObject> UIs; //Liste med alle sider. Nemmere at kontrollere siderne via en liste

        void Start()
        {
            MainMenu = transform.Find("MainMenu").gameObject; //Reference til StartMenu UI
            RunTime = transform.Find("RunTime").gameObject; //Reference til runtime UI
            Results = transform.Find("Results").gameObject; //Reference til resultatvinduet

            UIs = new List<GameObject> { MainMenu, RunTime, Results }; //Siderne lægges ind i listen

            StartMainMenu(); //Vis start menuen når programmet startes
        }

        public void StartMainMenu() //Show main menu. Is called when program starts or user returns to main menu
        {
            foreach (GameObject ui in UIs)
            {
                if (ui.name == "MainMenu") //Kun vis start menuen
                    ui.SetActive(true);
                else
                    ui.SetActive(false);
            }
        }

        public void StartRunTime() //Begin simulation
        {
            foreach (GameObject ui in UIs)
            {
                if (ui.name == "RunTime") //Kun vis start menuen
                    ui.SetActive(true);
                else
                    ui.SetActive(false);
            }
        }

        public void ShowResults() //End simulation. Is called when all sandbags are placed
        {
            Results.SetActive(true);
        }
        public void HideResults()
        {
            Debug.Log("Tes");
            Results.SetActive(false);
        }
    }
}

