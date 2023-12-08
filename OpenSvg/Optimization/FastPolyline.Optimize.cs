using System.Numerics;

namespace OpenSvg.Optimization;
public partial class FastPolyline
{

    public FastPolyline RemoveSharpTurns()
    {
        if (Length < 3)
            return this;

        var result = new List<Vector2> { this[0], this[1] };
       
        for (int i = 2; i < Length; i++)
            if (!IsSharpTurn(result[^2], result[^1], this[i]))
                result.Add(this[i]);
        
        Point lastPoint = this[^1];

        if (lastPoint != result[^1]) //check if last point was excluded, since it would create a sharp turn
        {
            //remove last point in list until it does not create a sharp turn
            while (result.Count >= 2 && IsSharpTurn(result[^2], result[^1], lastPoint))
                result.RemoveAt(result.Count - 1); 

            result.Add(lastPoint);
        }

        return new FastPolyline(result);
    }

    public bool ContainsSharpTurns()
    {
        for (int i = 2; i < Length - 1; i++)
            if (IsSharpTurn(Points[^2], Points[^1], Points[i])) return true;
        return false;
    }

    private static bool IsSharpTurn(Point a, Point b, Point c)
    {
        Vector2 v1 = b - a;
        Vector2 v2 = c - b;
        float angle = MathF.Acos(Vector2.Dot(v1, v2) / (v1.Length() * v2.Length()));
        angle = NormalizeAngle(angle * 180 / MathF.PI); // Convert to normalized degrees ]-180, 180]
        
        return angle < -160 || angle > 160; // Threshold angle, adjust as needed
    }

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

    public static FastPolyline CreatePolyline(Point startingPoint, float[] turnAngles, float moveDistance = 10)
    {
        List<Point> points = new List<Point> { startingPoint };
        float currentAngle = 0; // Starting direction, adjust as needed

        foreach (float turnAngle in turnAngles)
        {
            currentAngle += turnAngle;
       
            Point lastPoint = points[^1];
            Point newPoint = lastPoint + Vector2FromAngle(currentAngle, moveDistance);
            points.Add(newPoint);
        }

        return new FastPolyline(points);
    }

    private static Point Vector2FromAngle(float angleInDegrees, float length)
    {
        float angleInRadians = angleInDegrees * MathF.PI / 180f;

        return new Point(length * MathF.Cos(angleInRadians), length * MathF.Sin(angleInRadians));
    }
}
