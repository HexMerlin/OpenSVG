using System.Numerics;

namespace OpenSvg;


/// <summary>
///     Represents a point in a pixel coordinate system. The origin (0, 0) is at the top-left corner
/// </summary>
public readonly struct Point : IComparable<Point>, IEquatable<Point>
{

    /// <summary>
    /// The X-coordinate of the point.
    /// </summary>
    /// <value>The X-coordinate of the point.</value>
    public float X { get; }

    /// <summary>
    /// The Y-coordinate of the point.
    /// </summary>
    /// <value>The Y-coordinate of the point.</value>
    public float Y { get; }

    /// <summary>
    /// Initializes a new instance of the Point struct with specified X and Y coordinates.
    /// </summary>
    /// <param name="x">The X-coordinate of the point.</param>
    /// <param name="y">The Y-coordinate of the point.</param>
    public Point(float x, float y)
    {
        X = x.Round();
        Y = y.Round();
    }

    /// <summary>
    /// Initializes a new instance of the Point struct with specified X and Y coordinates.
    /// </summary>
    /// <param name="vector">The vector to convert to a point.</param>
    public Point(Vector2 vector) : this(vector.X, vector.Y)
    {
    }

    /// <summary>
    ///     Gets the origin point at (0, 0).
    /// </summary>
    /// <value>The origin point at (0, 0).</value>
    public static Point Origin => new(0, 0);

    /// <summary>
    /// Converts the point to a vector.
    /// </summary>
    /// <value>The point as a vector.</value>
    public Vector2 AsVector => new((float)X, (float)Y);

    /// <summary>
    ///     Points are ordered by Y, then X.
    /// </summary>
    /// <example>
    ///     <para>
    ///         The following example demonstrates use of the <see cref="CompareTo" /> method.
    ///     </para>
    ///     <code><![CDATA[
    /// Point[] points = new Point[] { (0, 10), (20, 5) };
    /// Console.WriteLine(points.Min());
    /// // Output: (0, 10)
    /// ]]></code>
    /// </example>
    /// <param name="other">The point to compare to.</param>
    /// <returns>A value indicating the relative order of the points.</returns>
    public int CompareTo(Point other) => (Y, X).CompareTo((other.Y, other.X));

    /// <summary>
    /// Transforms the point by the specified transform.
    /// </summary>
    /// <param name="transform">The transform to apply to the point.</param>
    /// <returns>The transformed point.</returns>
    public Point Transform(Transform transform) => new(Vector2.Transform(AsVector, transform.Matrix));


    /// <summary>
    /// Calculates the distance to another point.
    /// </summary>
    /// <param name="other">The other point.</param>
    /// <returns>The distance to the other point.</returns>
    public float DistanceTo(Point other)
    {
        float dx = other.X - X;
        float dy = other.Y - Y;
        return MathF.Sqrt(dx * dx + dy * dy);
    }
    

    /// <summary>
    /// Determines whether the point is on a line segment defined by two points.
    /// </summary>
    /// <param name="a">The start point of the line segment.</param>
    /// <param name="b">The end point of the line segment.</param>
    /// <returns>True if the point is on the line segment; otherwise, false.</returns>
    public readonly bool IsOnLineSegment(Point a, Point b)
    {
        const float Tolerance = 1e-5f;

        // Check if point is within the bounding box of the line segment
        if (X < MathF.Min(a.X, b.X) || X > MathF.Max(a.X, b.X)) return false;
        if (Y < MathF.Min(a.Y, b.Y) || Y > MathF.Max(a.Y, b.Y)) return false;

        // Check if point is collinear with line segment using cross-product
        float crossProduct = (Y - a.Y) * (b.X - a.X) - (X - a.X) * (b.Y - a.Y);
        return MathF.Abs(crossProduct) <= Tolerance;
    }

    /// <summary>
    /// Implicitly converts a tuple of floats to a point.
    /// </summary>
    /// <param name="tuple">The tuple to convert.</param>
    /// <returns>The converted point.</returns>
    public static implicit operator Point((float x, float y) tuple) => new(tuple.x, tuple.y);

    /// <summary>
    /// Implicitly converts a point to a tuple of floats.
    /// </summary>
    /// <param name="p">The point to convert.</param>
    /// <returns>The converted tuple.</returns>
    public static implicit operator (float X, float Y)(Point p) => (p.X, p.Y);

    /// <summary>
    ///     Returns a string that represents the point.
    /// </summary>
    /// <returns>A string representation of this object</returns>
    public override string ToString() => $"({float.Round(X, 3).ToXmlString()}, {float.Round(Y, 3).ToXmlString()})";

    ///<inheritdoc/>
    public override bool Equals(object? obj) => obj is Point other && Equals(other);

    /// <summary>
    /// Determines whether the specified point is equal to the current point.
    /// </summary>
    /// <param name="other">The point to compare with the current point.</param>
    /// <returns>True if the specified point is equal to the current point; otherwise, false.</returns>
    public readonly bool Equals(Point other) => X.Equals(other.X) && Y.Equals(other.Y);

    /// <summary>
    /// Returns a hash code for the current point.
    /// </summary>
    /// <returns>A hash code for the current point.</returns>
    public override int GetHashCode() => HashCode.Combine(X, Y);

    /// <summary>
    /// Determines whether two points are equal.
    /// </summary>
    /// <param name="left">The first point to compare.</param>
    /// <param name="right">The second point to compare.</param>
    /// <returns>True if the two points are equal; otherwise, false.</returns>
    public static bool operator ==(Point left, Point right) => left.Equals(right);

    /// <summary>
    /// Determines whether two points are not equal.
    /// </summary>
    /// <param name="left">The first point to compare.</param>
    /// <param name="right">The second point to compare.</param>
    /// <returns>True if the two points are not equal; otherwise, false.</returns>
    public static bool operator !=(Point left, Point right) => !(left == right);

    /// <summary>
    /// Determines whether the first point is less than the second point.
    /// </summary>
    /// <param name="left">The first point to compare.</param>
    /// <param name="right">The second point to compare.</param>
    /// <returns>True if the first point is less than the second point; otherwise, false.</returns>
    public static bool operator <(Point left, Point right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the first point is less than or equal to the second point.
    /// </summary>
    /// <param name="left">The first point to compare.</param>
    /// <param name="right">The second point to compare.</param>
    /// <returns>True if the first point is less than or equal to the second point; otherwise, false.</returns>
    public static bool operator <=(Point left, Point right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the first point is greater than the second point.
    /// </summary>
    /// <param name="left">The first point to compare.</param>
    /// <param name="right">The second point to compare.</param>
    /// <returns>True if the first point is greater than the second point; otherwise, false.</returns>
    public static bool operator >(Point left, Point right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the first point is greater than or equal to the second point.
    /// </summary>
    /// <param name="left">The first point to compare.</param>
    /// <param name="right">The second point to compare.</param>
    /// <returns>True if the first point is greater than or equal to the second point; otherwise, false.</returns>
    public static bool operator >=(Point left, Point right) => left.CompareTo(right) >= 0;

}
