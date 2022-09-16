using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCirclePositionsGenerator : PositionsGenerator
{
    private int balanceMagnitude;
    public RandomCirclePositionsGenerator(Vector2 mapDimensions, int n) : base(mapDimensions, n)
    {
        balanceMagnitude = randInt(1, 11);
    }

    public override Vector2[] GenerateZonesPosition()
    {
        List<Vector2> posList = new List<Vector2>();
        Vector2 pos = Vector2.zero;
        Vector2 middle = ((bottomLeft + topRight)/2 + new Vector2(2, 2));
        for (int l=0; l<n; l++){
            float maxMinDist = 0;
            Vector2 maxMinDistPoint = Vector2.zero;
            if (l == 0)
                pos = middle;
            else
            {
                float current = (float)(l-1) / (n-1);
                float rot = 360f * current + randFloat(-360 / (n-1) /2, 360f / (n-1) /2);
                Vector2 dir = Vector2.zero;
                for (int i=0; i<balanceMagnitude; i++) {
                    dir =(Vector2)(Quaternion.Euler(0f,0f, rot) * new Vector2(0, randFloat((topRight.y - bottomLeft.y)/4, (topRight.y - bottomLeft.y)/2)));
                    pos = middle + new Vector2(dir.x*(topRight.x-bottomLeft.x)/(topRight.y-bottomLeft.y)*.8f, dir.y);
                    float minDist = float.PositiveInfinity;
                    foreach (Vector2 z in posList) {
                        if (Vector2.Distance(pos, z) < minDist)
                            minDist = Vector2.Distance(pos, z);
                    }
                    if (minDist > maxMinDist) {
                        maxMinDist = minDist;
                        maxMinDistPoint = pos;
                    }
                }
                pos = maxMinDistPoint; 
            }
            posList.Add(pos);
        }
        return posList.ToArray();
    }

    public override Vector2[] GenerateBorderPositions()
    {
        Vector2[] positions = new Vector2[23];
        Vector2 middle = (bottomLeft + topRight)/2 + new Vector2(2, 2);
        for (int i=0; i<positions.Length; i++)
        {
            float rot = 360f * i / positions.Length;
            Vector2 dir =(Vector2)(Quaternion.Euler(0f,0f, rot) * new Vector2(7,(topRight.y - bottomLeft.y)*.8f));
            positions[i] = middle + new Vector2(dir.x*(topRight.x-bottomLeft.x)/(topRight.y-bottomLeft.y)*.8f, dir.y);
        }
        return positions;
    }
}
