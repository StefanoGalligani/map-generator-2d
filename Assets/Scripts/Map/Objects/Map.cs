using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public List<Node_Zone> zones {get; private set;}
    public List<Node_Vertex> vertices {get; private set;}

    public Map(List<Node_Zone> zones, List<Node_Vertex> vertices)
    {
        this.zones = zones;
        this.vertices = vertices;
        IndexLists();
        Debug.Log("Map created, zones: " + zones.Count);
    }

    private void IndexLists()
    {
        for (int i=0; i< zones.Count; i++)
        {
            zones[i].index = i;
        }
        for (int i=0; i< vertices.Count; i++)
        {
            vertices[i].index = i;
        }
    }
}
