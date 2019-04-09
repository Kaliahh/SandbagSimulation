using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SandbagSimulation
{
    public class DroneMovement : MonoBehaviour
    {
        public float Speed;
        private float Step;

        // Flytter GameObject et Step mod Vector3 target i løbet af en enkelt frame.
        public void FlyTo(Vector3 target)
        {
            Step = Speed * Time.deltaTime;

            this.transform.position = Vector3.MoveTowards(this.transform.position, target, Step);

            if (this.GetComponent<DroneController>().MySandbag != null)
            {
                Vector3 sandbagTarget = new Vector3(this.transform.position.x, this.transform.position.y - 1f, this.transform.position.z);

                this.GetComponent<DroneController>().MySandbag.transform.position = sandbagTarget;
                this.GetComponent<DroneController>().MySandbag.transform.rotation = Quaternion.identity;
            }
        }
    }
}
