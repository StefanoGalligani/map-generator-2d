using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Node_Zone z1 {get; private set;}
    public Node_Zone z2 {get; private set;}
    public Edge(Node_Zone z1, Node_Zone z2)
    {
        this.z1 = z1;
        this.z2 = z2;
    }
}
