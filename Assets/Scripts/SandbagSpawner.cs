using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandbagSpawner : MonoBehaviour
{
    public GameObject Sandbag;
    public Vector3 SpawnPoint;

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
            SpawnSandbag();
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
