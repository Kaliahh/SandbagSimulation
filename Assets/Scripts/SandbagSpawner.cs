using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SandbagSimulation
{
    /* Instantierer en sandsæk efter et givent antal sekunder, og så længe der ikke er nogen sandsække 
     * i nærheden af det sted de instantieres */
    public class SandbagSpawner : MonoBehaviour
    {
        public GameObject Sandbag;
        public Vector3 SpawnPoint;

        float Counter = 0;
        float WaitTime = 0.3f;

        void Start()
        {
            Sandbag = this.GetComponent<SimulationController>().SandBag;
        }

        void Update()
        {
            if (SpawnPointIsFree())
            {
                if (Counter > WaitTime)
                {
                    Counter = 0;
                    SpawnSandbag();
                }

                else
                {
                    Counter += Time.deltaTime;
                }
            }
        }

        // Instantierer en sandsæk
        void SpawnSandbag() => Instantiate(Sandbag, SpawnPoint, Quaternion.identity); // Sandsækken får ingen rotation

        /* Tjekker om der er en sandsæk i spawnpointet
         * Returnerer sand hvis der ingen sandsæk er */
        bool SpawnPointIsFree()
        {
            GameObject[] sandbags = GameObject.FindGameObjectsWithTag("Sandbag")
                .Where(s => Vector3.Distance(s.transform.position, SpawnPoint) < 1)
                .ToArray();

            if (sandbags.Length == 0)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}


