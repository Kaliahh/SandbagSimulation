using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace SandbagSimulation
{
    public class DroneMover : MonoBehaviour
    {
        public float Speed;
        private float Step;

        public List<GameObject> OtherDrones;

        // Flytter GameObject et Step mod Vector3 target i løbet af en enkelt frame.
        public void FlyTo(Vector3 target)
        {
            Step = Speed * Time.deltaTime;

            //if (IsPathBlocked(target) == true)
            //{
            //    // Flyv udenom eller bliv
            //}

            //else
            //{
            //    // this.transform.position = Vector3.MoveTowards(this.transform.position, target, Step);
            //}

            this.transform.position = Vector3.MoveTowards(this.transform.position, target, Step);

            if (this.GetComponent<DroneController>().MySandbag != null)
            {
                Vector3 sandbagTarget = new Vector3(this.transform.position.x, this.transform.position.y - 1f, this.transform.position.z);

                this.GetComponent<DroneController>().MySandbag.transform.position = sandbagTarget;
                this.GetComponent<DroneController>().MySandbag.transform.rotation = Quaternion.identity;
            }
        }

        private bool IsPathBlocked(Vector3 target)
        {
            int blockingDrones = OtherDrones
                .Where(p => (this.transform.position - p.transform.position).magnitude < 4)
                .Where(p => (Vector3.Project((this.transform.position - p.transform.position), target) - p.transform.position).magnitude < 4)
                .Count();

            return blockingDrones > 0;
        }
    }
}
