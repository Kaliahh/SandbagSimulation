using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SandbagSimulation
{
    public class UI : MonoBehaviour
    {
        private Text Time; //Tid
        private Text SandbagsLeft; //Amount of sandbags left before simulation is ended

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
        public void ChangeTime(float seconds) //Metode til at opdatere køretiden som vises på skærmen over tid
        {
            int Seconds = ((int)seconds);
           RunTime.transform.Find("Time").GetComponent<Text>().text = "Time: " + Seconds; //Sætter ui teksten
        }

        public void SimulationStarted()
        {
            StartMenu.SetActive(false);
            RunTime.SetActive(true);
        }
    }
}

