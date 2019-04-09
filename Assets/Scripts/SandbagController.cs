using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SandbagSimulation
{
    public class SandbagController : MonoBehaviour
    {
        public float Height { get; private set; }
        public float Width { get; private set; }
        public float Length { get; private set; }

        float Scale;

        public Rigidbody rb;


        // Start is called before the first frame update
        void Start()
        {
            Scale = 0.025f;

            Height = 20f * Scale;
            Width = 35f * Scale;
            Length = 60f * Scale;

            this.transform.localScale = new Vector3(Length, Height, Width);

            this.transform.rotation = Quaternion.Euler(0, 0, 0);

            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
