using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WideGameManager : MonoBehaviour
{
    public Transform map_parent;
    public Material material;
    public int zones;
    public int lakes;
    public Vector2 mapDimensions;
    public bool readFile;
    public string fileName;
    public string seed;
    void Start()
    {
        if (seed == "") this.seed = MathUtility.StringGenerator(10);
        SeedSingleton.InitSeed(seed);
        Debug.Log("Seed: " + SeedSingleton.instance.seedString);
        OpenMap(zones, lakes);
    }

    void OpenMap(int zones, int lakes)
    {
        Map m;
        if (!readFile)
        {
            Debug.Log("generating...");
            MapGenerator generator = new MapGenerator(mapDimensions);
            m = generator.GenerateMap(zones, lakes);
            FileManager<Map, MapSerializable>.WriteClass(m, "map", fileName, "map2d");
        }
        else
        {
            try {
                m = FileManager<Map, MapSerializable>.ReadClass("map", fileName, "map2d");
            } catch (FileNotFoundException e) {
                Debug.LogError("Error: " + e.Message);
                return;
            }
        }
        MapPainter.GenerateMeshes(m, map_parent, material);
    }

}
