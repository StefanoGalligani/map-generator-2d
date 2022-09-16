using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapSerializable : SerializableClass<Map>
{
    public float[][] zonePositions;
    public int[] zoneIndices;
    public bool[] zoneMarks;
    public int[][] zoneVertices; //non dovrebbe essere riordinato se si passa false nel costruttore dei vertici
    public int[][] zoneAdiacent;
    
    public float[][] vertPositions;
    public int[] vertIndices;
    public int[][] vertZones;
    public int[][] vertAdiacent;
    
    public void InitClassValues(Map m)
    {
        zonePositions = new float[m.zones.Count][];
        zoneIndices = new int[m.zones.Count];
        zoneMarks = new bool[m.zones.Count];
        zoneAdiacent = new int[m.zones.Count][];
        zoneVertices = new int[m.zones.Count][];
        foreach(Node_Zone z in m.zones)
        {
            zonePositions[z.index] = new float[2];
            zonePositions[z.index][0] = z.euclideanPosition.x;
            zonePositions[z.index][1] = z.euclideanPosition.y;
            zoneIndices[z.index] = z.index;
            zoneMarks[z.index] = z.mark;
            
            zoneAdiacent[z.index] = new int[z.adiacentZones.Count];
            zoneVertices[z.index] = new int[z.vertices.Count];
            int i=0;
            foreach (Node_Zone zAd in z.adiacentZones)
            {
                zoneAdiacent[z.index][i] = zAd.index;
                i++;
            }
            i=0;
            foreach (Node_Vertex v in z.vertices)
            {
                zoneVertices[z.index][i] = v.index;
                i++;
            }
        }
        
        vertPositions = new float[m.vertices.Count][];
        vertIndices = new int[m.vertices.Count];
        vertAdiacent = new int[m.vertices.Count][];
        vertZones = new int[m.vertices.Count][];
        foreach(Node_Vertex v in m.vertices)
        {
            vertPositions[v.index] = new float[2];
            vertPositions[v.index][0] = v.euclideanPosition.x;
            vertPositions[v.index][1] = v.euclideanPosition.y;
            vertIndices[v.index] = v.index;
            
            vertAdiacent[v.index] = new int[v.connectedVertices.Count];
            vertZones[v.index] = new int[v.coveredZones.Count];
            int i=0;
            foreach (Node_Vertex vAd in v.connectedVertices)
            {
                vertAdiacent[v.index][i] = vAd.index;
                i++;
            }
            i=0;
            foreach (Node_Zone z in v.coveredZones)
            {
                vertZones[v.index][i] = z.index;
                i++;
            }
        }
    }

    public Map ExtractClassData()
    {
        List<Node_Zone> zones = new List<Node_Zone>();
        List<Node_Vertex> vertices = new List<Node_Vertex>();
        for (int i=0; i<zoneIndices.Length; i++)
        {
            Node_Zone z = new Node_Zone(new Vector2(zonePositions[i][0], zonePositions[i][1]));
            z.mark = zoneMarks[i];
            z.index = i;
            zones.Add(z);
        }
        for (int i=0; i<vertIndices.Length; i++)
        {
            List<Node_Zone> covered = new List<Node_Zone>();
            for (int j=0; j<vertZones[i].Length; j++)
            {
                covered.Add(zones[vertZones[i][j]]);
            }
            Node_Vertex v = new Node_Vertex(covered, false);
            v.euclideanPosition = new Vector2(vertPositions[i][0], vertPositions[i][1]);
            v.index = i;
            vertices.Add(v);
        }
        for (int i=0; i<zones.Count; i++)
        {
            for (int j=0; j<zoneAdiacent[i].Length; j++)
            {
                zones[i].AddAdiacent(zones[zoneAdiacent[i][j]]);
            }
            for (int j=0; j<zoneVertices[i].Length; j++)
            {
                zones[i].AddVertex(vertices[zoneVertices[i][j]]);
            }
        }
        for (int i=0; i<vertices.Count; i++)
        {
            for (int j=0; j<vertAdiacent[i].Length; j++)
            {
                vertices[i].AddConnected(vertices[vertAdiacent[i][j]]);
            }
        }
        return new Map(zones, vertices);
    }
}
