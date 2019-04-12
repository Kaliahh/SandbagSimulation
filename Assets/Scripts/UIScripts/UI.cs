using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SandbagSimulation
{
    public class UI : MonoBehaviour
    {

        //UI sider
        private GameObject StartMenu; //Startmenu
        private GameObject RunTime; //UI når simulationen kører
        

        void Start()
        {
            RunTime = transform.Find("RunTime").gameObject; //Reference UI for runtime
            StartMenu = transform.Find("StartMenu").gameObject; //Reference til StartMenu UI
            StartMenu.SetActive(true);
            RunTime.SetActive(false);
        }
        public void ChangeTime(float value) //Metode til at opdatere køretiden som vises på skærmen over tid
        {
            RunTime.transform.Find("Time").GetComponent<Text>().text = "Time: " + value; //Sætter ui teksten
        }

        public void SimulationStarted()
        {
            StartMenu.SetActive(false);
            RunTime.SetActive(true);
        }
    }
}

