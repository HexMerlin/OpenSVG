namespace OpenSvg.Optimization;

/// <summary>
/// Represents a class for applying the Ramer-Douglas-Peucker Algorithm (RDPA) to a polyline.
/// </summary>
public partial class FastPolyline
{
    /// <summary>
    /// Applies the Ramer-Douglas-Peucker Algorithm (RDPA) to the polyline.
    /// </summary>
    /// <param name="distanceSquaredThreshold">The distance squared threshold for simplification.</param>
    /// <returns>A new FastPolyline object representing the simplified polyline.</returns>
    public FastPolyline ApplyRDPA(float distanceSquaredThreshold = 0.001f)
    {
        return new FastPolyline(ApplyRDPA(this.Points.ToList(), distanceSquaredThreshold));
    }

    /// <summary>
    /// Applies the Ramer-Douglas-Peucker Algorithm (RDPA) to the given list of points.
    /// </summary>
    /// <param name="points">The list of points to simplify.</param>
    /// <param name="distanceSquaredThreshold">The distance squared threshold for simplification.</param>
    /// <returns>A new list of points representing the simplified polyline.</returns>
    private static List<Point> ApplyRDPA(List<Point> points, float distanceSquaredThreshold)
    {
        if (points.Count < 3)
            return new List<Point>(points);

        int index = FindFurthestPoint(points, distanceSquaredThreshold, out float maxDistanceSquared);

        if (maxDistanceSquared > distanceSquaredThreshold)
        {
            var leftSegment = ApplyRDPA(points.GetRange(0, index + 1), distanceSquaredThreshold);
            var rightSegment = ApplyRDPA(points.GetRange(index, points.Count - index), distanceSquaredThreshold);

            // Merge segments while avoiding duplication of the division point
            leftSegment.RemoveAt(leftSegment.Count - 1);
            leftSegment.AddRange(rightSegment);

            return leftSegment;
        }

        return new List<Point> { points[0], points[^1] };
    }

    /// <summary>
    /// Finds the index of the point furthest from the line segment formed by the first and last points.
    /// </summary>
    /// <param name="points">The list of points to search.</param>
    /// <param name="epsilonSquared">The squared distance threshold for determining the furthest point.</param>
    /// <param name="maxDistanceSquared">The squared distance of the furthest point.</param>
    /// <returns>The index of the furthest point.</returns>
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

    /// <summary>
    /// Calculates the squared perpendicular distance of a point from a line segment.
    /// </summary>
    /// <param name="point">The point to calculate the distance for.</param>
    /// <param name="lineStart">The starting point of the line segment.</param>
    /// <param name="lineEnd">The ending point of the line segment.</param>
    /// <returns>The squared perpendicular distance of the point from the line segment.</returns>
    private static float PerpendicularDistanceSquared(Point point, Point lineStart, Point lineEnd)
    {
        var line = lineEnd - lineStart;
        var projected = Point.Dot(point - lineStart, line) / line.LengthSquared();
        var projectedPoint = lineStart + projected * line;

        return Point.DistanceSquared(point, projectedPoint);
    }
}
