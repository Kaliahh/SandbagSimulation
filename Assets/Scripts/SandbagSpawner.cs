using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SandbagSimulation
{
    public class SandbagSpawner : MonoBehaviour
    {
        public GameObject Sandbag;
        public Vector3 SpawnPoint;

        float Counter = 0;
        public float WaitTime = 0.5f;

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

        void SpawnSandbag() => Instantiate(Sandbag, SpawnPoint, Quaternion.identity); // Sandsækken får ingen rotation

        // Tjekker om der er en sandsæk i spawnpointet
        // Returnerer sand hvis der ingen sandsæk er
        bool SpawnPointIsFree()
        {
            GameObject[] sandbags = GameObject.FindGameObjectsWithTag("Sandbag");

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


