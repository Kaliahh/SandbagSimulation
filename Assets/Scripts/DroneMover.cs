using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace SandbagSimulation
{
    // Bevæger dronen hen 
    public class DroneMover : MonoBehaviour
    {
        public float Speed;

        /* Flytter dronen mod et givent punkt.
         * Afstanden udregnes ud fra Speed og deltaTime */
        public void FlyTo(Vector3 target)
        {
            Vector3 direction = target - this.transform.position;

            this.transform.position += ((direction.magnitude > 1) ? direction.normalized : direction) * Speed * Time.deltaTime;

            // Hvis der er blevet samlet en sandsæk op, følger den med dronen
            if (this.GetComponent<DroneController>().MySandbag != null)
            {
                MoveSandbag();
            }
        }

        /* Sætter dronens sandsæk til at være i et punkt under den,
         * og sørger for at sandsækken ikke roterer */
        private void MoveSandbag()
        {
            Vector3 sandbagTarget = new Vector3(this.transform.position.x, this.transform.position.y - this.GetComponent<DroneController>().DroneSandbagDistance, this.transform.position.z);

            this.GetComponent<DroneController>().MySandbag.transform.position = sandbagTarget;
            this.GetComponent<DroneController>().MySandbag.transform.rotation = Quaternion.identity;
        }
    }
}
