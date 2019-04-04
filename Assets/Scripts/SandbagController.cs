using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandbagController : MonoBehaviour
{
    float Height;
    float Width;
    float Length;


    // Start is called before the first frame update
    void Start()
    {
        Height = 2.0f;
        Width = 3.5f;
        Length = 6.0f;
        
        this.transform.localScale = new Vector3(Length, Height, Width);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
