using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSvg.Optimization;
public static class SimpleCorrections
{

    public static IEnumerable<Point> RemoveEquivalentAdjacentPoints(IEnumerable<Point> points, float minDistanceSquaredThreshold = 0.00001f)
    {
        Point prevPoint = new Point(float.MinValue, float.MinValue);
        foreach (Point point in points)
        {
            if (Point.DistanceSquared(prevPoint, point) > minDistanceSquaredThreshold)
            {
                yield return point;
                prevPoint = point;
            }
        }   

       

    }

}
