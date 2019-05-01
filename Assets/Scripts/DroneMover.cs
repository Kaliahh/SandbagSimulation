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
            Vector3 direction = target - this.transform.position;

            //direction += AvoidCollision(direction);

            //Debug.DrawLine(this.transform.position, this.transform.position + direction.normalized);

            this.transform.position += ((direction.magnitude > 1) ? direction.normalized : direction) * Speed * Time.deltaTime;

            // Hvis der er blevet samlet en sandsæk op, følger den med dronen
            if (this.GetComponent<DroneController>().MySandbag != null)
            {
                MoveSandbag();
            }
        }

        private Vector3 AvoidCollision(Vector3 destination)
        {
            Vector3 avoidanceVector = Vector3.zero;
            Vector3 direction = Vector3.zero;

            float distance = 0;

            var a = OtherDrones
                .Where(d => Vector3.Distance(d.transform.position, this.transform.position) < 2)
                .ToList();

            foreach (GameObject drone in a)
            {
                direction = this.transform.position - drone.transform.position;
                distance = direction.magnitude;

                avoidanceVector += ((2 / distance) * direction);
            }

            //Vector3 resultant = Vector3.zero;

            //foreach (GameObject drone in OtherDrones)
            //{
            //    resultant += (this.transform.position - drone.transform.position) * 1 / (Vector3.Distance(this.transform.position, drone.transform.position));
            //}

            return avoidanceVector;
        }

        private void MoveSandbag()
        {
            Vector3 sandbagTarget = new Vector3(this.transform.position.x, this.transform.position.y - this.GetComponent<DroneController>().DroneSandbagDistance, this.transform.position.z);

            this.GetComponent<DroneController>().MySandbag.transform.position = sandbagTarget;
            this.GetComponent<DroneController>().MySandbag.transform.rotation = Quaternion.identity;
        }


        //private bool IsPathBlocked(Vector3 target)
        //{
        //    int blockingDrones = OtherDrones
        //        .Where(p => (this.transform.position - p.transform.position).magnitude < 4)
        //        .Where(p => (Vector3.Project((this.transform.position - p.transform.position), target) - p.transform.position).magnitude < 4)
        //        .Count();

        //    return blockingDrones > 0;
        //}
    }
}
