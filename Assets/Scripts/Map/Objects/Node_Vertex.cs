using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node_Vertex
{
    public Vector2 euclideanPosition {get; internal set;}
    public List<Node_Vertex> connectedVertices {get; private set;}
    public List<Node_Zone> coveredZones {get; private set;}
    public int index = -1;

    public Node_Vertex(List<Node_Zone> zones, bool updateZones = true){
        connectedVertices = new List<Node_Vertex>();
        coveredZones = new List<Node_Zone>();

        coveredZones.AddRange(zones);
        
        _connectZones(updateZones);
    }

    public Node_Vertex(Node_Zone z1, Node_Zone z2, Node_Zone z3, bool updateZones = true)
    {
        connectedVertices = new List<Node_Vertex>();
        coveredZones = new List<Node_Zone>();
        
        coveredZones.Add(z1);
        coveredZones.Add(z2);
        coveredZones.Add(z3);
        
        _connectZones(updateZones);
    }

    private void _connectZones(bool updateZones) {
        foreach(Node_Zone z in coveredZones) {
            if (updateZones) z.AddVertex(this);
            euclideanPosition += z.euclideanPosition;
        }
        if (coveredZones.Count > 0) euclideanPosition /= coveredZones.Count;
    }

    public void AddConnected(Node_Vertex newVertex){
        if (!connectedVertices.Contains(newVertex)) connectedVertices.Add(newVertex);
    }

    public void RemoveConnected(Node_Vertex vertex){
        connectedVertices.Remove(vertex);
    }

    public void AddCovered(Node_Zone newZone){
        if (!coveredZones.Contains(newZone)) coveredZones.Add(newZone);
    }

    public void RemoveBorders() {
        foreach (Node_Zone z in coveredZones.ToList()) {
            if (z.mark) coveredZones.Remove(z);
        }
    }

    public void RemoveZone(Node_Zone z){
        coveredZones.Remove(z);
        foreach (Node_Vertex v in connectedVertices.ToList()) {
            if (!coveredZones.Intersect(v.coveredZones).Any()){
                v.RemoveConnected(this);
                RemoveConnected(v);
            }
        }
    }

    public void CalculateVoronoiPosition(){
        int commonPoint = 0;
        int pointLine1 = 1;
        int pointLine2 = 2;
        float[] ys = {coveredZones[0].euclideanPosition.y, coveredZones[1].euclideanPosition.y, coveredZones[2].euclideanPosition.y};
        if (ys[0] == ys[1]) {commonPoint = 2; pointLine2 = 0;}
        if (ys[0] == ys[2]) {commonPoint = 1; pointLine1 = 0;}

        float m1 = -(coveredZones[pointLine1].euclideanPosition.x - coveredZones[commonPoint].euclideanPosition.x)/(coveredZones[pointLine1].euclideanPosition.y - coveredZones[commonPoint].euclideanPosition.y);
        float m2 = -(coveredZones[pointLine2].euclideanPosition.x - coveredZones[commonPoint].euclideanPosition.x)/(coveredZones[pointLine2].euclideanPosition.y - coveredZones[commonPoint].euclideanPosition.y);
        Vector2 middle1 = (coveredZones[pointLine1].euclideanPosition + coveredZones[commonPoint].euclideanPosition)/2;
        Vector2 middle2 = (coveredZones[pointLine2].euclideanPosition + coveredZones[commonPoint].euclideanPosition)/2;
        float q1 = middle1.y - middle1.x * m1;
        float q2 = middle2.y - middle2.x * m2;

        float x = (q2-q1)/(m1-m2);
        float y = m1*x + q1;
        euclideanPosition = new Vector2(x, y);
    }
    
}
