using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OpenSvg.PathOptimize;

//Ramer-Douglas-Peucker Algorithm (RDPA)
public static class RDPA
{
    public static List<Point> RemoveClosePoints(List<Point> points, float minDistanceThreshold = 0.01f)
    {
        float minDistanceSquared = minDistanceThreshold * minDistanceThreshold;

        bool aboveThreshold(Point p1, Point p2)
            => Point.DistanceSquared(p1, p2) >= minDistanceSquared * minDistanceSquared;
       
        if (points.Count == 1)
            return points;

        while (!aboveThreshold(points[0], points[^1]))
        {
            points.RemoveAt(points.Count - 1);
            if (points.Count == 1)
                return points;
        }

        for (int i = 1; i < points.Count - 1; i++) // Skip the first and last points
        {
            if (!aboveThreshold(points[i], points[i - 1]) || !aboveThreshold(points[i], points[i + 1]))
            {
                points.RemoveAt(i);
                i--;
                if (points.Count == 1)
                    return points;
               
            }

        }
    
        return points;
    }

    // RDPA implementation
    public static List<Point> ApplyRDPA(List<Point> points, float epsilonSquared)
    {
        if (points.Count < 3)
            return new List<Point>(points);

        int index = FindFurthestPoint(points, epsilonSquared, out float maxDistanceSquared);

        if (maxDistanceSquared > epsilonSquared)
        {
            var leftSegment = ApplyRDPA(points.GetRange(0, index + 1), epsilonSquared);
            var rightSegment = ApplyRDPA(points.GetRange(index, points.Count - index), epsilonSquared);

            // Merge segments while avoiding duplication of the division point
            leftSegment.RemoveAt(leftSegment.Count - 1);
            leftSegment.AddRange(rightSegment);

            return leftSegment;
        }

        return new List<Point> { points[0], points[^1] };
    }

    // Helper method to find the point furthest from the line segment formed by the first and last points
    private static int FindFurthestPoint(List<Point> points, float epsilonSquared, out float maxDistanceSquared)
    {
        int furthestPointIndex = -1;
        maxDistanceSquared = 0;

        for (int i = 1; i < points.Count - 1; i++)
        {
            float distanceSquared = PerpendicularDistanceSquared(points[i], points[0], points[^1]);

            if (distanceSquared > maxDistanceSquared)
            {
                furthestPointIndex = i;
                maxDistanceSquared = distanceSquared;
            }
        }

        return furthestPointIndex;
    }

    // Calculate the squared perpendicular distance of a point from a line segment
    private static float PerpendicularDistanceSquared(Point point, Point lineStart, Point lineEnd)
    {
        var line = lineEnd - lineStart;
        var projected = Point.Dot(point - lineStart, line) / line.LengthSquared();
        var projectedPoint = lineStart + projected * line;

        return Point.DistanceSquared(point, projectedPoint);
    }
}
