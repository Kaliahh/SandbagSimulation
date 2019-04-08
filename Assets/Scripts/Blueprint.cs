using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SandbagSimulation
{
    public class Blueprint // : MonoBehaviour
    {
        public List<Vector3> ConstructionNodes { get; private set; }
        public int DikeHeight;

        public Blueprint(List<Vector3> nodes, int height)
        {
            ConstructionNodes = nodes;
            DikeHeight = height;
        }
    }
}


