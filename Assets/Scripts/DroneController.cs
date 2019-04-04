using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SandbagSimulation
{
    public class DroneController : MonoBehaviour
    {
        bool IsFinished;
        bool HasSandbag;
        bool IsRightDrone;

        float ViewDistance;

        Section MySection;
        Blueprint MyBlueprint;
        DroneMovement MyMovement;

        GameObject MySandbag;
        GameObject LocatedSandbag;

        void Start()
        {
            IsFinished = false;
            HasSandbag = false;
            IsRightDrone = (Random.Range(0, 2) == 1) ? true : false; // Halvdelen af alle droner flyver til høre, resten til venstre
            ViewDistance = 10f;

            MySandbag = null;
            MyBlueprint = null;
            MySection = new Section(Vector3.zero);
            MyMovement = new DroneMovement();
        }

        void Update()
        {

        }

        // Finder den nærmeste sandsæk, og gemmer den i LocatedSandbag
        public void LocateSandbag()
        {
            GameObject[] sandbags = GameObject.FindGameObjectsWithTag("Sandbag");

            float distance = Mathf.Infinity;

            foreach (GameObject sandbag in sandbags)
            {
                Vector3 diff = sandbag.transform.position - this.transform.position;
                float currentDistance = diff.sqrMagnitude;

                if (currentDistance < distance)
                {
                    LocatedSandbag = sandbag;
                    distance = currentDistance;
                }
            }
        }

        // Gemmer den fundne sandsæk i MySandbag, og den skal nu transporteres
        void PickUpSandbag() => MySandbag = LocatedSandbag;

        // Placerer MySandbag i et givent punkt, og sætter referencen til sandsækken (MySandbag) til null
        void PlaceSandbag(Vector3 position)
        {
            MySandbag.tag = "PlacedSandbag";

            MySandbag.transform.position = position;
            MySandbag = null;
        }
    }
}


