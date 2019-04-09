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
        float WaitTime = 1;

        // Start is called before the first frame update
        void Start()
        {
            SpawnPoint = new Vector3(2, 0.5f, 2);
        }

        // Update is called once per frame
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


                //Invoke("SpawnSandbag", 2); // Der sker noget mærkeligt hvis man gør det her
            }
        }

        void SpawnSandbag()
        {
            Instantiate(Sandbag, SpawnPoint, Quaternion.identity); // Sandsækken får ingen rotation
        }

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


