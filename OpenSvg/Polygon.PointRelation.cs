namespace OpenSvg;

public partial class Polygon
{
    /// <summary>
    ///     Calculates the <see cref="PointRelation" /> of a point to a polygon.
    /// </summary>
    /// <param name="target">The point to check.</param>
    /// <returns>The <see cref="PointRelation" /> of the point to the polygon. Either Inside or Disjoint.</returns>
    public PointRelation RelationTo(Point target)
    {
        // Check if the point is a vertex of the polygon
        if (Contains(target)) return PointRelation.Inside;

        // Check if the point lies on any edge of the polygon
        for (int i = 0; i < Count; i++)
            if (target.IsOnLineSegment(this[i], this[(i + 1) % Count]))
                return PointRelation.Inside;

        bool isInside = false;
        int j = Count - 1; // Last vertex

        for (int i = 0; i < Count; i++)
        {
            if ((this[i].Y < target.Y && this[j].Y >= target.Y) || (this[j].Y < target.Y && this[i].Y >= target.Y))
                if (this[i].X + (target.Y - this[i].Y) / (this[j].Y - this[i].Y) * (this[j].X - this[i].X) < target.X)
                    isInside = !isInside;
            j = i;
        }

        return isInside ? PointRelation.Inside : PointRelation.Disjoint;
    }

    private static int Orientation(Point p, Point q, Point r)
    {
        float val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

        if (val == 0) return 0; // collinear
        return val > 0 ? 1 : 2; // clock or counterclockwise
    }
}