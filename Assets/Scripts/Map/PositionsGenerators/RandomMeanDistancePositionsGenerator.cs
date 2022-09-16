using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMeanDistancePositionsGenerator : PositionsGenerator
{
    private int balanceMagnitude;
    public RandomMeanDistancePositionsGenerator(Vector2 mapDimensions, int n) : base(mapDimensions, n)
    {
        balanceMagnitude = randInt(1, 5);
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
                pos = new Vector2(randFloat(bottomLeft.x, topRight.x), randFloat(bottomLeft.y, topRight.y));
            else
            {            
                for (int i=0; i<balanceMagnitude; i++) {
                    pos = new Vector2(randFloat(bottomLeft.x, topRight.x), randFloat(bottomLeft.y, topRight.y));
                    float meanDist = 0;
                    foreach (Vector2 z in posList) {
                        meanDist += Vector2.Distance(pos, z);
                    }
                    meanDist /= l;
                    if (meanDist > maxMinDist) {
                        maxMinDist = meanDist;
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
        Vector2[] positions = new Vector2[60];
        Vector2 middle = (bottomLeft + topRight)/2 + new Vector2(2, 2);
        for (int i=0; i< 20; i++)
        {
            positions[i] = new Vector2(bottomLeft.x + (topRight.x - bottomLeft.x)*i/19, bottomLeft.y -25);
        }
        for (int i=0; i< 10; i++)
        {
            positions[i + 20] = new Vector2(bottomLeft.x - 20 , bottomLeft.y + (topRight.y - bottomLeft.y)*i/9);
        }
        for (int i=0; i< 10; i++)
        {
            positions[i + 30] = new Vector2(topRight.x + 20, bottomLeft.y + (topRight.y - bottomLeft.y)*i/9);
        }
        for (int i=0; i< 20; i++)
        {
            positions[i+40] = new Vector2(bottomLeft.x + (topRight.x - bottomLeft.x)*i/19, topRight.y +30);
        }
        return positions;
    }
       
}
