using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class MathUtility
{
    public static float CalculateAngle(Vector2 start, Vector2 end, Vector2 point){
        Vector2 v1 = start - point;
        Vector2 v2 = end - point;
        //float dp = v1.x * v2.x + v1.y * v2.y;
        //float angle = Mathf.Acos(dp/(v1.magnitude*v2.magnitude));
        //return angle * Mathf.Rad2Deg;
        Vector2 abm = v1*v2.magnitude;
        Vector2 bam = v2*v1.magnitude;
        return 2 * Mathf.Atan2((abm-bam).magnitude, (abm+bam).magnitude) * Mathf.Rad2Deg;
    }

    public static bool TriPoint(Vector2 t1, Vector2 t2, Vector2 t3, Vector2 p) {
        float areaOrig = Mathf.Abs( (t2.x-t1.x)*(t3.y-t1.y) - (t3.x-t1.x)*(t2.y-t1.y) );

        float area1 = Mathf.Abs( (t1.x-p.x)*(t2.y-p.y) - (t2.x-p.x)*(t1.y-p.y) );
        float area2 = Mathf.Abs( (t2.x-p.x)*(t3.y-p.y) - (t3.x-p.x)*(t2.y-p.y) );
        float area3 = Mathf.Abs( (t3.x-p.x)*(t1.y-p.y) - (t1.x-p.x)*(t3.y-p.y) );

        return CloserThan(area1 + area2 + area3, areaOrig, 0.0001f);
    }

    public static float PointSideRelativeToLine(Vector2 point, Vector2 startLine, Vector2 endLine)
    {
        float d = (point.x - startLine.x) * (endLine.y - startLine.y) - (point.y - startLine.y) * (endLine.x - startLine.x);
        return d;
    }
    
    public static bool IsCrossingLines(Vector2 edge1a, Vector2 edge1b, Vector2 edge2a, Vector2 edge2b)
    {
        Vector2 inters = IntersectionPoint(edge1a, edge1b, edge2a, edge2b);

        if (inters.x == float.NegativeInfinity) return false;
        return ((edge1a.x <= inters.x && inters.x <= edge1b.x || edge1b.x <= inters.x && inters.x <= edge1a.x) &&
                (edge1a.y <= inters.y && inters.y <= edge1b.y || edge1b.y <= inters.y && inters.y <= edge1a.y) &&
                (edge2a.x <= inters.x && inters.x <= edge2b.x || edge2b.x <= inters.x && inters.x <= edge2a.x) &&
                (edge2a.y <= inters.y && inters.y <= edge2b.y || edge2b.y <= inters.y && inters.y <= edge2a.y));
    }

    public static Vector2 IntersectionPoint(Vector2 edge1a, Vector2 edge1b, Vector2 edge2a, Vector2 edge2b)
    {
        float x;
        float y;
        if(edge1a.x == edge1b.x && edge2a.x == edge2b.x) //sono entrambe verticali
        {
            if (edge1a.x == edge2a.x) //sono allineate
            {
                Vector2 medium1 = (edge1a + edge1b)/2;
                Vector2 medium2 = (edge2a + edge2b)/2;
                if (medium2.y > medium1.y)
                {
                    Vector2 appo = medium1;
                    medium1 = medium2;
                    medium2 = appo;
                }
                return (new Vector2(medium1.x, medium1.y - medium1.magnitude/2)+new Vector2(medium2.x, medium2.y + medium2.magnitude/2))/2;
            }
            else //sono verticali non allineate
            {
                return new Vector2(float.NegativeInfinity, 0);
            }
        }
        else if(edge1a.x == edge1b.x) //solo la prima è verticale
        {
            float m = (edge2b.y - edge2a.y)/(edge2b.x - edge2a.x);
            float q = edge2a.y - edge2a.x * m;

            x = edge1a.x;
            y = m*x + q;
            return new Vector2(x, y);
        }
        else if(edge2a.x == edge2b.x) //solo la seconda è verticale
        {
            float m = (edge1b.y - edge1a.y)/(edge1b.x - edge1a.x);
            float q = edge1a.y - edge1a.x * m;

            x = edge2a.x;
            y = m*x + q;
            return new Vector2(x, y);
        }

        float m1 = (edge1b.y - edge1a.y)/(edge1b.x - edge1a.x);
        float m2 = (edge2b.y - edge2a.y)/(edge2b.x - edge2a.x);
        float q1 = edge1a.y - edge1a.x * m1;
        float q2 = edge2a.y - edge2a.x * m2;

        if (CloserThan(m1, m2, .0001f))
        {
            if (!CloserThan(q1, q2, .0001f))
            {
                x = float.NegativeInfinity;
                y = 0;
            }
            else
            {
                Vector2[] points = {edge1a, edge1b, edge2a, edge2b};
                points = points.OrderBy(v => v.x).ToArray<Vector2>();
                return (points[1] + points[2])/2;
            }
        }
        else
        {
            x = (q2-q1)/(m1-m2);
            y = m1*x + q1;   
        }
        return new Vector2(x, y);
    }
    
    public static bool CloserThan(float a, float b, float val){
        float absA = Mathf.Abs(a);
        float absB = Mathf.Abs(b);
        float diff = Mathf.Abs(a - b);

        if (a == b) { // shortcut, handles infinities
            return true;
        } else if (a == 0 || b == 0 || absA + absB < float.MinValue) {
            // a or b is zero or both are extremely close to it
            // relative error is less meaningful here
            return diff < (val * float.MinValue);
        } else { // use relative error
            return diff / (absA + absB) < val;
        }
    }
    
    public static string StringGenerator(int l)
    {
        char[] chars = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
            'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
        string s = "";
        for (int i=0; i<l; i++)
        {
            s += chars[Random.Range(0, chars.Length)];
        }
        return s;
    }
}
