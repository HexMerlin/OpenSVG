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

        var insideCount = CountVerticesInsidePolygon(target);
        var neutralCount = CountNeutralVertices(target);

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
        for (var i = 0; i < Count; i++)
        {
            var a = this[i];
            var b = this[(i + 1) % Count];
            if (point.IsOnLineSegment(a, b)) return true;
        }

        return false;
    }

    private bool ArePolygonsDisjointByBoundingBox(Polygon target)
    {
        var thisBoundingBox = BoundingBox();
        var otherBoundingBox = target.BoundingBox();
        return !thisBoundingBox.Intersects(otherBoundingBox);
    }

    private int CountVerticesInsidePolygon(Polygon target)
    {
        return this.Count(point => target.RelationTo(point) == PointRelation.Inside);
    }

    private int CountNeutralVertices(Polygon target)
    {
        return this.Count(point => target.RelationTo(point) == PointRelation.Disjoint && target.IsOnEdge(point));
    }

    private bool DoEdgesIntersectIgnoringNeutral(Polygon otherPolygon)
    {
        for (var i = 0; i < Count; i++)
        {
            var a1 = this[i];
            var a2 = this[(i + 1) % Count];

            for (var j = 0; j < otherPolygon.Count; j++)
            {
                var b1 = otherPolygon[j];
                var b2 = otherPolygon[(j + 1) % otherPolygon.Count];

                if (EdgesIntersect(a1, a2, b1, b2) && !IsNeutralIntersection(a1, a2, b1, b2)) return true;
            }
        }

        return false;
    }

    private static bool EdgesIntersect(Point a1, Point a2, Point b1, Point b2)
    {
        var o1 = Orientation(a1, a2, b1);
        var o2 = Orientation(a1, a2, b2);
        var o3 = Orientation(b1, b2, a1);
        var o4 = Orientation(b1, b2, a2);

        if (o1 != o2 && o3 != o4) return true;

        return false;
    }

    private static bool IsNeutralIntersection(Point a1, Point a2, Point b1, Point b2)
    {
        var intersection = ComputeIntersection(a1, a2, b1, b2);
        if (intersection == null) return false;

        var interPoint = intersection.Value;
        return interPoint.Equals(a1) || interPoint.Equals(a2) || interPoint.Equals(b1) || interPoint.Equals(b2);
    }

    private static Point? ComputeIntersection(Point a1, Point a2, Point b1, Point b2)
    {
        var A1 = a2.Y - a1.Y;
        var B1 = a1.X - a2.X;
        var C1 = A1 * a1.X + B1 * a1.Y;

        var A2 = b2.Y - b1.Y;
        var B2 = b1.X - b2.X;
        var C2 = A2 * b1.X + B2 * b1.Y;

        var determinant = A1 * B2 - A2 * B1;

        if (determinant == 0)
        {
            return null; // parallel lines
        }

        var x = (B2 * C1 - B1 * C2) / determinant;
        var y = (A1 * C2 - A2 * C1) / determinant;
        return new Point(x, y);
    }
}