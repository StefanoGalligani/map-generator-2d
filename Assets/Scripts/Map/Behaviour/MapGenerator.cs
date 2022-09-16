using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator
{
    private Vector2 mapDimensions;
    private List<Node_Zone> zone_list;
    private List<Node_Vertex> vertex_list;

    public MapGenerator(Vector2 mapDimensions){
        this.mapDimensions = mapDimensions;
        
        zone_list = new List<Node_Zone>();
        vertex_list = new List<Node_Vertex>();
    }

    public Map GenerateMap(int zones, int lakes)
    {
        GenerateZones(zones + lakes);
        GenerateLakes(lakes);
        //MapPainter.Paint(zone_list, vertex_list);
        OrderAllVertices();

        return new Map(zone_list, vertex_list);
    }

    private void GenerateZones(int n)
    {
        PositionsGenerator pg = PositionsGenerator.GetRandomGenerator(mapDimensions, n);
        _generateBigTriangle();
        Vector2[] zonePositions = pg.GenerateZonesPosition();
        for (int i=0; i<n; i++)
        {
            Node_Zone newZone = new Node_Zone(zonePositions[i]);
            _applyDelaunay(newZone);
            zone_list.Add(newZone);
        }
        Vector2[] borders = pg.GenerateBorderPositions();
        for (int i=0; i<borders.Length; i++){
            Node_Zone newZone = new Node_Zone(borders[i]);
            _applyDelaunay(newZone);
            newZone.mark = true;
            zone_list.Add(newZone);
        }
        _calculateCyclesMiddle();
        _removeBorderZones();
    }

    private void _generateBigTriangle()
    {
        zone_list.Add(new Node_Zone(new Vector2(-1000, -800)));
        zone_list.Add(new Node_Zone(new Vector2(1000, -800)));
        zone_list.Add(new Node_Zone(new Vector2(0, 950)));
        vertex_list.Add(new Node_Vertex(zone_list.ToList()));
        zone_list.ForEach(z => z.mark = true);
    }

    private void _applyDelaunay(Node_Zone z)
    {
        Node_Vertex v1 = _findTriangleColliding(z);
        v1.coveredZones.ForEach(z1 => _connectZones(z, z1));
        _createNewTriangles(z, v1);
        Queue<Edge> q = new Queue<Edge>();
        q.Enqueue(new Edge(v1.coveredZones[0], v1.coveredZones[1]));
        q.Enqueue(new Edge(v1.coveredZones[2], v1.coveredZones[1]));
        q.Enqueue(new Edge(v1.coveredZones[0], v1.coveredZones[2]));
        _removeVertex(v1);
        while (q.Count > 0)
        {
            Edge e = q.Dequeue();
            v1 = _findTriangleByEdge(e, z);
            if (v1 != null)
            {
                List<Node_Zone> triangleVertices = v1.coveredZones.ToList();
                triangleVertices.Remove(e.z1);
                triangleVertices.Remove(e.z2);
                Node_Vertex v2 = _findTriangleByEdge(e, triangleVertices[0]);
                if (MathUtility.CalculateAngle(e.z1.euclideanPosition, e.z2.euclideanPosition, z.euclideanPosition)
                  + MathUtility.CalculateAngle(e.z1.euclideanPosition, e.z2.euclideanPosition, triangleVertices[0].euclideanPosition)
                  > 180)
                {
                    _flip(v1, v2, e, z, triangleVertices[0]);
                    q.Enqueue(new Edge(e.z1, triangleVertices[0]));
                    q.Enqueue(new Edge(e.z2, triangleVertices[0]));
                }
                else
                {
                    _connectVertices(v1, v2);
                }
            }
        }
    }

    private void _flip (Node_Vertex v1, Node_Vertex v2, Edge e, Node_Zone zA, Node_Zone zB)
    {
        _removeVertex(v1);
        _removeVertex(v2);
        _disconnectZones(e.z1, e.z2);

        Node_Vertex newV1 = new Node_Vertex(e.z1, zA, zB);
        Node_Vertex newV2 = new Node_Vertex(e.z2, zA, zB);
        _connectZones(zA, zB);
        _connectVertices(newV1, newV2);
        _connectVertices(newV1, _findTriangleByEdge(new Edge(e.z1, zA), zB));
        _connectVertices(newV2, _findTriangleByEdge(new Edge(e.z2, zA), zB));

        vertex_list.Add(newV1);
        vertex_list.Add(newV2);
    }

    private Node_Vertex _findTriangleColliding(Node_Zone z)
    {
        foreach (Node_Vertex v in vertex_list)
        {
            if (MathUtility.TriPoint(v.coveredZones[0].euclideanPosition, v.coveredZones[1].euclideanPosition, v.coveredZones[2].euclideanPosition, z.euclideanPosition))
            {
                return v;
            }
        }
        Debug.Log("triangolo non trovato wtf");
        return null;
    }

    private void _createNewTriangles(Node_Zone z, Node_Vertex v)
    {
        Node_Vertex v1 = new Node_Vertex(v.coveredZones[0], v.coveredZones[1], z);
        Node_Vertex v2 = new Node_Vertex(v.coveredZones[2], v.coveredZones[1], z);
        Node_Vertex v3 = new Node_Vertex(v.coveredZones[0], v.coveredZones[2], z);
        vertex_list.Add(v1);
        vertex_list.Add(v2);
        vertex_list.Add(v3);
        _connectVertices(v1, v2);
        _connectVertices(v1, v3);
        _connectVertices(v3, v2);
    }

    private void _removeVertex(Node_Vertex v)
    {
        vertex_list.Remove(v);
        v.coveredZones.ForEach(z => z.RemoveVertex(v));
        v.connectedVertices.ForEach(v1 => v1.RemoveConnected(v));
    }

    private Node_Vertex _findTriangleByEdge(Edge e, Node_Zone exclude)
    {
        foreach(Node_Vertex v in vertex_list)
        {
            if (v.coveredZones.Contains(e.z1) && v.coveredZones.Contains(e.z2) && !v.coveredZones.Contains(exclude))
            {
                return v;
            }
        }
        return null;
    }

    private void GenerateLakes(int l)
    {
        for (int i=0; i<l; i++) {
            Node_Zone z = zone_list[SeededNoiseRNG.GetIntValueBetween(0, zone_list.Count, i)];
            foreach (Node_Vertex v in z.vertices){
                v.RemoveZone(z);
                if (v.coveredZones.Count == 0) vertex_list.Remove(v);
            }
            zone_list.Remove(z);
        }
    }

    private void OrderAllVertices(){
        foreach (Node_Zone z in zone_list)
        {
            z.OrderVertices();
        }
    }

    private void _connectZones(Node_Zone a, Node_Zone b)
    {
        a.AddAdiacent(b);
        b.AddAdiacent(a);
    }

    private void _disconnectZones(Node_Zone a, Node_Zone b)
    {
        a.RemoveAdiacent(b);
        b.RemoveAdiacent(a);
    }

    private void _connectVertices(Node_Vertex a, Node_Vertex b)
    {
        a.AddConnected(b);
        b.AddConnected(a);
    }

    private void _calculateCyclesMiddle()
    {
        foreach (Node_Vertex v in vertex_list)
        {
            v.CalculateVoronoiPosition();
        }
    }

    private void _removeBorderZones()
    {
        foreach (Node_Zone z in zone_list.ToList()) {
            if (z.mark) {
                zone_list.Remove(z);
                foreach (Node_Zone zAd in z.adiacentZones) {
                    zAd.RemoveAdiacent(z);
                }
            }
        }
        foreach (Node_Vertex v in vertex_list.ToList()) {
            v.RemoveBorders();
            if (v.coveredZones.Count == 0) {
                vertex_list.Remove(v);
                foreach (Node_Vertex v1 in v.connectedVertices) {
                    v1.RemoveConnected(v);
                }
            }
        }
    }
}
