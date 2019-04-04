using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint // : MonoBehaviour
{
    private List<Vector3> ConstructionNodes;
    private int DikeHeight;

    public Blueprint(List<Vector3> nodes, int height)
    {
        ConstructionNodes = nodes;
        DikeHeight = height;
    }
}
