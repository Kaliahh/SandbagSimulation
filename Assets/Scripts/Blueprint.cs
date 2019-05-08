using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SandbagSimulation
{
    // Indeholder informationer om de punkter diget skal gå igennem
    public class Blueprint
    {
        public List<Vector3> ConstructionNodes { get; private set; }
        public int DikeHeight;

        // Constructor for et blueprint
        public Blueprint(List<Vector3> constructionNodes, int height)
        {
            this.ConstructionNodes = constructionNodes;
            DikeHeight = height;
        }
    }
}


