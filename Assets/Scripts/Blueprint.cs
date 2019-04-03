using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    private List<Vector3> _constructionNodes; //Dike nodes
    private int _height; //Dike height

    public Blueprint(List<Vector3> nodes, int height) //Contructor
    {
        _constructionNodes = nodes;
        _height = height;
    }
}
