using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapPainter
{
    public static void GenerateMeshes(Map m, Transform map_parent, Material material) {
        foreach(Node_Zone z in m.zones){
            Vector3[] vertices = new Vector3[z.vertices.Count];
            Vector2[] uv = new Vector2[z.vertices.Count];
            //Debug.Log("there will be " + (3 * (z.vertices.Count - 2)) + " triangles");
            int[] triangles = new int[3 * (z.vertices.Count - 2)]; 
            
            for (int i=0; i< vertices.Length; i++) {
                vertices[i] = z.vertices[i].euclideanPosition - z.euclideanPosition;
                uv[i] = new Vector2(0, 0);
            }

            for (int i=0; i< triangles.Length/3; i++) {
                triangles[3*i] = 0;
                triangles[3*i + 1] = i + 1;
                triangles[3*i + 2] = i + 2;
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            
            GameObject zone = new GameObject("zone", typeof(MeshFilter), typeof(MeshRenderer));
            zone.transform.SetParent(map_parent);
            zone.transform.position = z.euclideanPosition;
            zone.GetComponent<MeshFilter>().mesh = mesh;
            zone.GetComponent<MeshRenderer>().material = material;
        }
    }

    public static void Paint(List<Node_Zone> zone_list, List<Node_Vertex> vertex_list) {
        foreach(Node_Zone z in zone_list) {
            GameObject s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            s.transform.position = new Vector3(z.euclideanPosition.x, z.euclideanPosition.y,0);
            s.transform.localScale = new Vector3(3, 3, 1);
            foreach(Node_Zone z1 in z.adiacentZones)
            {
                for (int i=1; i<100; i++)
                {
                   // GameObject c = GameObject.CreatePrimitive(PrimitiveType.Cube);
                   // c.transform.position = (-z.euclideanPosition + z1.euclideanPosition)/100 * i + z.euclideanPosition;
                }
            }
        }
        foreach(Node_Vertex v in vertex_list) {
            GameObject c = GameObject.CreatePrimitive(PrimitiveType.Cube);
            c.transform.position = v.euclideanPosition;
            c.transform.localScale = new Vector3(4, 4, 1);
            //Debug.Log("Vertex of zones: " + v.coveredZones[0] + ", " + v.coveredZones[1] + ", " + v.coveredZones[2]);
           // Debug.Log("The position is: " + v.euclideanPosition);
           // foreach(Node_Zone z in v.coveredZones) {
              //  Debug.Log(z.euclideanPosition);
            //}
            foreach(Node_Vertex z1 in v.connectedVertices) {
                    //Debug.Log("connected to " + z1.euclideanPosition);
                for (int i=1; i<100; i++)
                {
                    GameObject s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    s.transform.position = (-v.euclideanPosition + z1.euclideanPosition) *i/100 + v.euclideanPosition;
                    s.transform.localScale = new Vector3(.5f, .5f, 1);
                }
            }
        }
    }
}
