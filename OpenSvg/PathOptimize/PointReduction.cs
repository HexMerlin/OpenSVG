using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSvg.PathOptimize;

public static class PointReduction
{
    public static Point[] Reduce(Point[] points, double tolerance)
    {
        if (points == null || points.Length < 3 || tolerance <= 0)
            return points;

        bool[] keepPoint = new bool[points.Length];
        keepPoint[0] = keepPoint[points.Length - 1] = true;

        ReducePoints(points, 0, points.Length - 1, tolerance, keepPoint);

        return points.Where((point, index) => keepPoint[index]).ToArray();
    }

    private static void ReducePoints(Point[] points, int firstIndex, int lastIndex, double tolerance, bool[] keepPoint)
    {
        double maxDistance = 0;
        int indexFarthest = 0;

        for (int i = firstIndex + 1; i < lastIndex; i++)
        {
            double distance = PerpendicularDistance(points[firstIndex], points[lastIndex], points[i]);

            if (distance > maxDistance)
            {
                maxDistance = distance;
                indexFarthest = i;
            }
        }

        if (maxDistance > tolerance)
        {
            keepPoint[indexFarthest] = true;
            ReducePoints(points, firstIndex, indexFarthest, tolerance, keepPoint);
            ReducePoints(points, indexFarthest, lastIndex, tolerance, keepPoint);
        }
    }

    private static double PerpendicularDistance(Point lineStart, Point lineEnd, Point point)
    {
        double area = Math.Abs(0.5 * (lineStart.X * lineEnd.Y + lineEnd.X * point.Y + point.X * lineStart.Y
                                      - lineEnd.X * lineStart.Y - point.X * lineEnd.Y - lineStart.X * point.Y));
        double lineLength = Math.Sqrt(Math.Pow(lineEnd.X - lineStart.X, 2) + Math.Pow(lineEnd.Y - lineStart.Y, 2));
        return 2 * area / lineLength;
    }
}