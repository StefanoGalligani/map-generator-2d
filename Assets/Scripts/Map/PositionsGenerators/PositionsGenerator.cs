using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PositionsGenerator
{
    protected Vector2 bottomLeft {get; private set;}
    protected Vector2 topRight {get; private set;}
    protected int option {get; private set;}
    protected int n {get; private set;} 
    private StatefulNoiseRNG rand;

    public PositionsGenerator(Vector2 mapDimensions, int n)
    {
        this.rand = new StatefulNoiseRNG();
        this.bottomLeft = -mapDimensions/2;
        this.topRight = mapDimensions/2;
        this.n = n;
    }

    public static PositionsGenerator GetRandomGenerator(Vector2 mapDimensions, int n) {
        string[] names = {"RandomEquidistantPositionsGenerator",
            "RandomMeanDistancePositionsGenerator",
            "RandomCirclePositionsGenerator",
            "SpiralPositionsGenerator",};
        System.Type posGeneratorType = System.Type.GetType(names[SeededNoiseRNG.GetIntValueBetween(0, names.Length, 0)]);
        object[] args = {mapDimensions, n};
        return (PositionsGenerator)System.Activator.CreateInstance(posGeneratorType, args);
    }

    protected float randFloat(float min, float max) {
        return rand.GetFloatValueBetween(min, max);
    }
    
    protected int randInt(int min, int max) {
        return rand.GetIntValueBetween(min, max);
    }

    public abstract Vector2[] GenerateZonesPosition();

    public abstract Vector2[] GenerateBorderPositions();
}
