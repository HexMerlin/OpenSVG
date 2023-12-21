
using System.Numerics;

namespace OpenSvg;

/// <summary>
/// Provides extension methods for manipulating points.
/// </summary>
public static class PointExtensions
{
    /// <summary>
    /// Normalizes an angle in degrees to the range ]-180, 180].
    /// </summary>
    /// <remarks>This is useful for using angles to denote turns, where 0 means no turning.
    /// A negative value means a left turn and a positive value means a right turn.
    /// The higher the absolute value, the sharper the turn.
    /// </remarks>
    /// <param name="degrees">The angle in degrees to normalize.</param>
    /// <returns>The normalized angle.</returns>
    public static float NormalizeAngle(float degrees)
    {
        degrees %= 360;
        if (degrees > 180) return degrees - 360;
        if (degrees <= -180) return degrees + 360;
        return degrees;
    }

    /// <summary>
    /// Calculates the angle formed by three points in 2D space, normalized to the range ]-180, 180].
    /// </summary>
    /// <remarks>
    /// This method computes the angle in degrees between two vectors, v1 and v2, formed by the points a, b, and c.
    /// Points a, b, and c are represented as 2D vectors (Point/Vector2).
    /// - If any two of the points are identical (resulting in zero-length vectors), the method returns 0 degrees.
    /// - The angle is normalized using NormalizeAngle to fall within the range ]-180, 180].
    /// - The method is robust against floating-point inaccuracies and avoids returning NaN by clamping the cosine of the angle within the range [-1, 1].
    ///
    /// Note: The method assumes a coordinate system where positive angles indicate a clockwise rotation and negative angles an anticlockwise rotation.
    /// </remarks>
    /// <param name="a">The first point in the sequence, forming vector v1 with point b.</param>
    /// <param name="b">The second point in the sequence, forming vectors v1 with point a and v2 with point c.</param>
    /// <param name="c">The third point in the sequence, forming vector v2 with point b.</param>
    /// <returns>
    /// The normalized angle in degrees formed by the three points, within the range ]-180, 180].
    /// Returns 0 degrees if any two points are identical.
    /// </returns>

    public static float GetAngle(Point a, Point b, Point c)
    {
        Vector2 v1 = b - a;
        Vector2 v2 = c - b;

        if (v1 == Vector2.Zero || v2 == Vector2.Zero)
            return 0f; //returning 0 for zero-length vectors (identical points)

        float dotProduct = Vector2.Dot(v1, v2);
        float magnitudeProduct = v1.Length() * v2.Length();

        // Clamping the value to the range [-1, 1] to avoid NaN
        float cosTheta = Math.Clamp(dotProduct / magnitudeProduct, -1f, 1f);

        float angleRadians = MathF.Acos(cosTheta);

        // Converting radians to degrees
        float angleDegrees = NormalizeAngle((angleRadians * 180) / MathF.PI);
        return angleDegrees;

    }

    /// <summary>
    /// Determines whether the specified point is within a certain distance from the current point.
    /// </summary>
    /// <remarks>
    /// This method is significantly faster than <see cref="Point.Distance(Point, Point)"/> since it avoids computing the actual distance.
    /// </remarks>
    /// <param name="point">The current point.</param>
    /// <param name="other">The point to compare with the current point.</param>
    /// <param name="distance">The distance threshold.</param>
    /// <returns>
    /// <c>true</c> if the specified point is within the given distance from the current point; otherwise, <c>false</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// Point p1 = new Point(0, 0);
    /// Point p2 = new Point(3, 4);
    /// double distance = 5;
    /// bool isWithin = p1.IsWithinDistance(p2, distance);
    /// Console.WriteLine(isWithin);
    /// //Output: <c>True</c> (since p2 is exactly 5 units away from p1)
    /// </code>
    /// </example>
    public static bool IsWithinDistance(this Point point, Point other, double distance) => Point.DistanceSquared(point, other) <= distance * distance;
   
    /// <summary>
    /// Transforms the point by the specified transform.
    /// </summary>
    /// <param name="point">The point to transform.</param>
    /// <param name="transform">The transform to apply to the point.</param>
    /// <returns>The transformed point.</returns>
    public static Point Transform(this Point point, Transform transform) => Vector2.Transform(point, transform.Matrix);

    /// <summary>
    /// Determines whether the point is on a line segment defined by two points.
    /// </summary>
    /// <param name="p">The point to check.</param>
    /// <param name="a">The start point of the line segment.</param>
    /// <param name="b">The end point of the line segment.</param>
    /// <returns>True if the point is on the line segment; otherwise, false.</returns>
    public static bool IsOnLineSegment(this Point p, Point a, Point b)
    {
        const float Tolerance = 1e-5f;

        // Return false if the point is outside the bounding box of the line segment
        if (p.X < MathF.Min(a.X, b.X) || p.X > MathF.Max(a.X, b.X)) return false;
        if (p.Y < MathF.Min(a.Y, b.Y) || p.Y > MathF.Max(a.Y, b.Y)) return false;

        // Check if point is collinear with line segment using cross-product
        float crossProduct = (p.Y - a.Y) * (b.X - a.X) - (p.X - a.X) * (b.Y - a.Y);
        return MathF.Abs(crossProduct) <= Tolerance;

    }

    /// <summary>
    /// Compares a current point with another point, primarily based on their Y coordinates, followed by their X coordinates.
    /// Ordering is similar to how text is read in an English document: left to right, then top to bottom.
    /// </summary>
    /// <remarks>
    /// The comparison is first made using the Y coordinates. If the Y coordinates are equal,
    /// the comparison is then made using the X coordinates. 
    /// </remarks>
    /// <param name="other">The point to compare to this instance.</param>
    /// <returns>
    /// A signed integer that indicates the relative order of the points being compared.
    /// The return value has these meanings: 
    /// Less than zero: This instance precedes 'other' in the sort order.
    /// Zero: This instance occurs in the same position in the sort order as 'other'.
    /// Greater than zero: This instance follows 'other' in the sort order.
    /// </returns>
    public static int CompareTo(this Point p1, Point p2)
    {
        int yComparison = p1.Y.CompareTo(p2.Y);
        if (yComparison != 0)
            return yComparison;

        return p1.X.CompareTo(p2.X);
    }
}
