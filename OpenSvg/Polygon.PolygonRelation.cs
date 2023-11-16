//File Polygon.PolygonRelation.cs

namespace OpenSvg;

public partial class Polygon
{
    /// <summary>
    ///     Determines the relationship between two polygons.
    /// </summary>
    /// <param name="target">The target polygon.</param>
    /// <returns>A <see cref="PolygonRelation" /> value indicating the relationship between the polygons.</returns>
    public PolygonRelation RelationTo(Polygon target)
    {
        if (ArePolygonsDisjointByBoundingBox(target))
            return PolygonRelation.Disjoint;

        int insideCount = CountVerticesInsidePolygon(target);
        int neutralCount = CountNeutralVertices(target);

        if (insideCount == Count - neutralCount)
            return PolygonRelation.Inside;

        if (DoEdgesIntersectIgnoringNeutral(target))
            return PolygonRelation.Intersect;

        return PolygonRelation.Disjoint;
    }

    /// <summary>
    ///     Determines if the given point is on the edge of this polygon.
    /// </summary>
    /// <param name="point">The point to check.</param>
    /// <returns>True if the point is on the edge of this polygon, false otherwise.</returns>
    public bool IsOnEdge(Point point)
    {
        for (int i = 0; i < Count; i++)
        {
            Point a = this[i];
            Point b = this[(i + 1) % Count];
            if (point.IsOnLineSegment(a, b)) return true;
        }

        return false;
    }

    private bool ArePolygonsDisjointByBoundingBox(Polygon target)
    {
        BoundingBox thisBoundingBox = BoundingBox;
        BoundingBox otherBoundingBox = target.BoundingBox;
        return !thisBoundingBox.Intersects(otherBoundingBox);
    }

    private int CountVerticesInsidePolygon(Polygon target) => this.Count(point => target.RelationTo(point) == PointRelation.Inside);

    private int CountNeutralVertices(Polygon target) => this.Count(point => target.RelationTo(point) == PointRelation.Disjoint && target.IsOnEdge(point));

    private bool DoEdgesIntersectIgnoringNeutral(Polygon otherPolygon)
    {
        for (int i = 0; i < Count; i++)
        {
            Point a1 = this[i];
            Point a2 = this[(i + 1) % Count];

            for (int j = 0; j < otherPolygon.Count; j++)
            {
                Point b1 = otherPolygon[j];
                Point b2 = otherPolygon[(j + 1) % otherPolygon.Count];

                if (EdgesIntersect(a1, a2, b1, b2) && !IsNeutralIntersection(a1, a2, b1, b2)) return true;
            }
        }

        return false;
    }

    private static bool EdgesIntersect(Point a1, Point a2, Point b1, Point b2)
    {
        int o1 = Orientation(a1, a2, b1);
        int o2 = Orientation(a1, a2, b2);
        int o3 = Orientation(b1, b2, a1);
        int o4 = Orientation(b1, b2, a2);

        if (o1 != o2 && o3 != o4) return true;

        return false;
    }

    private static bool IsNeutralIntersection(Point a1, Point a2, Point b1, Point b2)
    {
        Point? intersection = ComputeIntersection(a1, a2, b1, b2);
        if (intersection == null) return false;

        Point interPoint = intersection.Value;
        return interPoint.Equals(a1) || interPoint.Equals(a2) || interPoint.Equals(b1) || interPoint.Equals(b2);
    }

    private static Point? ComputeIntersection(Point a1, Point a2, Point b1, Point b2)
    {
        double A1 = a2.Y - a1.Y;
        double B1 = a1.X - a2.X;
        double C1 = A1 * a1.X + B1 * a1.Y;

        double A2 = b2.Y - b1.Y;
        double B2 = b1.X - b2.X;
        double C2 = A2 * b1.X + B2 * b1.Y;

        double determinant = A1 * B2 - A2 * B1;

        if (determinant == 0)
        {
            return null; // parallel lines
        }

        double x = (B2 * C1 - B1 * C2) / determinant;
        double y = (A1 * C2 - A2 * C1) / determinant;
        return new Point(x, y);
    }
}