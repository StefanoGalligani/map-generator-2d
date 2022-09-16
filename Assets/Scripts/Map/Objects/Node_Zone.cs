using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node_Zone
{
    public Vector2 euclideanPosition {get; private set;}
    public List<Node_Zone> adiacentZones {get; private set;}
    public List<Node_Vertex> vertices {get; private set;}
    public bool mark = false;
    public int index = -1;

    public Node_Zone (Vector2 pos){
        euclideanPosition = pos;
        adiacentZones = new List<Node_Zone>();
        vertices = new List<Node_Vertex>();
    }

    public void AddAdiacent(Node_Zone newZone){
        if (!adiacentZones.Contains(newZone)) adiacentZones.Add(newZone);
    }

    public void AddVertex(Node_Vertex newVertex){
        if (!vertices.Contains(newVertex)) vertices.Add(newVertex);
    }
    
    public void RemoveAdiacent(Node_Zone zone){
        adiacentZones.Remove(zone);
    }
    
    public void RemoveVertex(Node_Vertex vertex){
        vertices.Remove(vertex);
    }

    public void OrderVertices(){
        for (int i=0; i < vertices.Count-1; i++) {
            List<Node_Vertex> near = vertices.Intersect(vertices[i].connectedVertices).ToList();
            if (near.Count < 2) {
                Debug.LogError("vertex connected only to " + near.Count+ ", index: " + i + " in " + vertices.Count + " vertices\nzone: " + euclideanPosition
                + "\nvertex: " + vertices[i].euclideanPosition);
                //vertices.Remove(vertices[i]);
                //vertices[i].RemoveZone(this);
                //OrderVertices();
                return;
            }
            Node_Vertex next = null;
            if (i == 0){
                if (MathUtility.PointSideRelativeToLine(near[0].euclideanPosition, euclideanPosition, vertices[i].euclideanPosition) > 0)
                    next = near[0];
                else next = near[1];
            }
            else next = near[0].Equals(vertices[i-1]) ? near[1]:near[0];
            
            vertices[vertices.IndexOf(next)] = vertices[i+1];
            vertices[i+1] = next;
        }
    }
    
}
