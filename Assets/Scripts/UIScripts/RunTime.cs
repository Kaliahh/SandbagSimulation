using SandbagSimulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunTime : MonoBehaviour
{
    private Text Time; //Tid
    private Text SandbagsLeft; //Amount of sandbags left before simulation is ended

    private GameObject Menu; //Menu under simulation kørsel
    void Start() 
    {
        //Referencer
        Time = GameObject.Find("Time").GetComponent<Text>(); //Tid
        SandbagsLeft = GameObject.Find("SandBags").GetComponent<Text>(); //Antallet af sandsække
        Menu = GameObject.Find("Menu");
        Menu.SetActive(false);
    }

    void Update() 
    {
        if (Input.GetKeyDown("escape"))
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
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
    
    public void ChangeTime(float seconds) 
    {
        //Time.text = seconds.ToString();
    }

    public void ChangeSandBagsLeft(float sandbags)
    {
        //SandBagsLeft.text = seconds.ToString();
    }

    //Menu Methods

    public void ChangeSpeed(float speed) 
    {
        //GameObject a = Menu.Find("Slider");
    }



}
