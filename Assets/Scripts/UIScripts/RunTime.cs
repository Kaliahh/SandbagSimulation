using SandbagSimulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunTime : MonoBehaviour
{
    private Text Time; //Tid
    private Text SandbagsLeft; //Antallet af sandsække tilbage

    private GameObject Menu; //Kørselsmenu

    void Start() 
    {
        //Referencer
        Time = GameObject.Find("Time").GetComponent<Text>(); //Tid
        SandbagsLeft = GameObject.Find("SandBags").GetComponent<Text>(); //Antallet af sandsække tilbage
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
        //Time.text = seconds.ToString();
    }

    public void ChangeSandBagsLeft(float sandbags) //Opdater sandsække tilbage
    {
        //SandBagsLeft.text = seconds.ToString();
    }

    public void ChangeSpeed(float speed) //Sæt hastigheden på simulationen fra slideren
    {
        //GameObject a = Menu.Find("Slider");
    }
}
