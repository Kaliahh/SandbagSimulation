using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SandbagSimulation
{
    public class SandbagController : MonoBehaviour
    {
        public float Height { get; }
        public float Width { get; }
        public float Length { get; }

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
}
