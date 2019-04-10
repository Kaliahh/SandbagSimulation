﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{

    public float Factor;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 newScale = new Vector3(this.transform.localScale.x * Factor, this.transform.localScale.y * Factor, this.transform.localScale.z * Factor);

        this.transform.localScale = newScale;
    }
}
