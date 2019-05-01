using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SandbagSimulation
{
    // Indeholder informationer om sandsækkens dimentioner og bestemmer størrelse og rotation på objkterne i simulationen
    public class SandbagController : MonoBehaviour
    {
        public float Height { get; private set; }
        public float Width { get; private set; }
        public float Length { get; private set; }

        float Scale;

        // Start is called before the first frame update
        void Start()
        {
            Scale = 0.1f;

            Height = 0.10f;
            Width = 0.35f;
            Length = 0.60f;

            this.transform.localScale = new Vector3(Length, Height, Width);

            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
