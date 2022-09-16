using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralPositionsGenerator : PositionsGenerator
{
    private int balanceMagnitude;
    private float baseRotation;
    public SpiralPositionsGenerator(Vector2 mapDimensions, int n) : base(mapDimensions, n)
    {
        balanceMagnitude = randInt(1, n);
        baseRotation = randFloat(0f, 360);
    }

    public override Vector2[] GenerateZonesPosition()
    {
        List<Vector2> posList = new List<Vector2>();
        Vector2 pos = Vector2.zero;
        Vector2 middle = ((bottomLeft + topRight)/2 + new Vector2(2, 2));
        for (int l=0; l<n; l++){
            if (l == 0)
                pos = middle;
            else
            {
                float offset = n * .1f;
                float current = (float)(l + offset) / (n + offset);
                float rot = 360f * current * balanceMagnitude + baseRotation;
                Vector2 dir =(Vector2)(Quaternion.Euler(0f,0f, rot) * new Vector2(0, (topRight.y - bottomLeft.y)/2*current));
                pos = middle + new Vector2(dir.x*(topRight.x-bottomLeft.x)/(topRight.y-bottomLeft.y)*.75f, dir.y);
            }
            posList.Add(pos);
        }
        return posList.ToArray();
    }

    public override Vector2[] GenerateBorderPositions()
    {
        Vector2[] positions = new Vector2[37];
        Vector2 middle = (bottomLeft + topRight)/2 + new Vector2(2, 2);
        for (int i=0; i<positions.Length; i++)
        {
            float rot = 360f * i / positions.Length;
            Vector2 dir =(Vector2)(Quaternion.Euler(0f,0f, rot) * new Vector2(7,(topRight.y - bottomLeft.y)*.7f));
            positions[i] = middle + new Vector2(dir.x*(topRight.x-bottomLeft.x)/(topRight.y-bottomLeft.y)*.7f, dir.y);
        }
        return positions;
    }
}
