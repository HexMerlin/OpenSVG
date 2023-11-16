namespace OpenSvg;


/// <summary>
///     Represents a convex hull of points.
///     Provides specialized methods for manipulating and querying convex hulls.
/// </summary>
public class ConvexHull : Polygon
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ConvexHull" /> class with the specified collection of points.
    /// </summary>
    /// <param name="points">The collection of points.</param>
    public ConvexHull(IEnumerable<Point> points) : base(ExtractHullPoints(points.ToArray()))
    {
    }


    /// <summary>
    ///     Initializes a new instance of the <see cref="ConvexHull" /> class by merging multiple convex hulls.
    /// </summary>
    /// <param name="convexHulls">The collection of convex hulls to merge.</param>
    public ConvexHull(IEnumerable<ConvexHull> convexHulls) : this(convexHulls.SelectMany(p => p))
    {
    }


    /// <summary>
    ///     Initializes a new instance of the <see cref="ConvexHull" /> class with a set of points known to form a convex hull.
    /// </summary>
    /// <remarks>
    ///     This specialized private constructor is intended for internal optimization purposes. It bypasses the Graham's scan
    ///     algorithm.
    ///     It is used by methods that transform an existing <see cref="ConvexHull" />, preserving its convex nature.
    /// </remarks>
    /// <param name="points">A collection of points that are already in the form of a convex hull.</param>
    /// <param name="_">A marker parameter indicating that Graham's scan should be skipped.</param>
    private ConvexHull(IEnumerable<Point> points, SkipGrahamScan _) : base(points)
    {
    }

    /// <summary>
    /// Transforms the convex hull by applying the specified transform.
    /// </summary>
    /// <param name="transform">The transform to apply.</param>
    /// <returns>A new convex hull that is the result of applying the transform.</returns>
    public ConvexHull Transform(Transform transform) => new(this.Select(p => p.Transform(transform)), SkipGrahamScan.Yes);


    /// <summary>
    ///     Gets the bounding box of the polygon.
    /// </summary>
    /// <returns>The bounding box as a <see cref="BoundingBox" /> object.</returns>
    public BoundingBox BoundingBox()
    {
        if (Count == 0) return new BoundingBox(); // return empty bounding box

        const double min = double.MinValue;
        const double max = double.MaxValue;

        var bounds = this.Aggregate(
            new { UpperLeft = new Point(max, max), LowerRight = new Point(min, min) },
            (acc, p) => new
            {
                UpperLeft = new Point(Math.Min(acc.UpperLeft.X, p.X), Math.Min(acc.UpperLeft.Y, p.Y)),
                LowerRight = new Point(Math.Max(acc.LowerRight.X, p.X), Math.Max(acc.LowerRight.Y, p.Y))
            });

        return new BoundingBox(bounds.UpperLeft, bounds.LowerRight);
    }


    /// <summary>
    ///     Extracts the convex hull for a collection of points using Graham's scan algorithm.
    /// </summary>
    /// <param name="points">An input collection of points.</param>
    /// <returns>The convex hull as a subset of the input points.</returns>
    /// <remarks>
    ///     The algorithm is based on selecting a pivot point, sorting the
    ///     remaining points based on their polar angle with respect to the pivot, and
    ///     iterating through the sorted points to extract the convex hull.
    ///     <para></para>
    ///     For the edge cases where the input is [0..2] points, the input points are returned.
    ///     The results would denote a convex hull of the empty set, a single point, and a line segment, respectively.
    /// </remarks>
    private static IEnumerable<Point> ExtractHullPoints(Point[] points)
    {
        if (points.Length <= 2) return points;

        Point pivot = points.Min();
        Array.Sort(points,
            (a, b) => Math.Atan2(a.Y - pivot.Y, a.X - pivot.X).CompareTo(Math.Atan2(b.Y - pivot.Y, b.X - pivot.X)));
        List<Point> hull = new() { pivot };

        for (int i = 1; i < points.Length; i++) // Start from 1 to skip the pivot
        {
            Point point = points[i];
            while (hull.Count >= 2 && CrossProduct(hull[^2], hull[^1], point) <= 0)
                hull.RemoveAt(hull.Count - 1);

            hull.Add(point);
        }

        return hull;
    }

    /// <summary>
    /// Determines whether the specified collection of points forms a convex hull.
    /// </summary>
    /// <param name="points">The collection of points to check.</param>
    /// <returns>True if the collection of points forms a convex hull, otherwise false.</returns>
    public static bool IsConvexHull(IEnumerable<Point> points)
    {
        var pointList = points.ToList();
        int n = pointList.Count;

        Point pivot = pointList.OrderBy(p => p.Y).ThenBy(p => p.X).First();

        // Sort remaining points based on polar angle with respect to pivot
        var sortedByPolarAngle = pointList.Where(p => p != pivot)
            .OrderBy(p => Math.Atan2(p.Y - pivot.Y, p.X - pivot.X))
            .ToList();

        // Initialize stack with pivot and two points with smallest polar angles
        var hull = new Stack<Point>();
        hull.Push(pivot);
        hull.Push(sortedByPolarAngle[0]);
        hull.Push(sortedByPolarAngle[1]);

        // Validate convexity
        for (int i = 2; i < n - 1; i++)
        {
            Point top = hull.Pop();
            if (CrossProduct(hull.Peek(), top, sortedByPolarAngle[i]) <= 0)
                return false;

            hull.Push(top);
            hull.Push(sortedByPolarAngle[i]);
        }

        return true;
    }

    /// <summary>
    /// Calculates the cross product of three points.
    /// </summary>
    /// <param name="a">The first point.</param>
    /// <param name="b">The second point.</param>
    /// <param name="c">The third point.</param>
    /// <returns>The cross product of the three points.</returns>
    private static double CrossProduct(Point a, Point b, Point c) => (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);

    private enum SkipGrahamScan
    {
        Yes
    }
}
