﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandbagController : MonoBehaviour
{
    float Height;
    float Width;
    float Length;

    float Scale;


    // Start is called before the first frame update
    void Start()
    {
        Scale = 0.025f;

        Height = 20f * Scale;
        Width = 35f * Scale;
        Length = 60f * Scale;
        
        this.transform.localScale = new Vector3(Length, Height, Width);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
