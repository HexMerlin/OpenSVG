namespace OpenSvg;

/// <summary>
/// A class for creating circular arcs represented as a specified number of equidistant points
/// </summary>

public static class CircularArc
{
    /// <summary>
    /// Creates the points forming a circular arc with a certain startAngle and length in degrees
    /// The function also enable to control the order the arcs are generated
    /// This is useful when the arc points are to be inserted in a bigger polygon, and be connected with more points where the order is important
    /// A positive arcLength means counterclockwise point generation (standard degrees increase counter-clockwise)
    /// A negative arcLenght mean clockwise point generation
    /// </summary>
    /// <example>
    /// Two equivalent circular arcs between 10 and 100 degrees (and thus 90 absolute degrees arcLength) can be generated in both opposite directions:
    /// 1. startAngle = 10, arcLength = 90 (counterclockwise generation from 10 degrees to 100 degrees)
    /// 2. startAngle = 100, arcLength = -90 (clockwise generation from 100 degrees to 10 degrees)
    /// </example>
    /// <param name="circleCenter">The center point of the arc circle</param>
    /// <param name="radius">The radius of the circular arc in points</param>
    /// <param name="startAngle">The start angle</param>
    /// <param name="arcLength">The arc length (positive or negative)</param>
    /// <param name="numberOfPoints">The number of points interpolated on the arc. The startAngle and the computed endAngle will always have a point, and thus 'numberOfPoints' must not be less than 2  </param>
    /// <returns>A list of points for the generated arc</returns>
    public static IEnumerable<Point> CreateArcPoints(Point circleCenter, double radius, double startAngle, double arcLength, int numberOfPoints)
    {
        startAngle = NormalizeDegreesToPositiveBelow360(startAngle);
        arcLength = NormalizeDegreesTo360NegativeOrPositive(arcLength);

        double startAngleRad = startAngle * (Math.PI / 180.0);
        double arcLengthRad = arcLength * (Math.PI / 180.0);

        double angleStep = arcLengthRad / (numberOfPoints - 1);

        return Enumerable.Range(0, numberOfPoints).Select(i =>
        {
            double angle = startAngleRad + i * angleStep;
            double x = circleCenter.X + radius * Math.Cos(angle);
            double y = circleCenter.Y - radius * Math.Sin(angle); // Invert the y-coordinate
            return new Point((float)x, (float)y);
        });
    }

    /// <summary>
    /// Normalizes degrees to a positive value in the range [0..360[.
    /// </summary>
    /// <param name="degrees">The degrees to normalize</param>
    /// <returns>A double.</returns>
    public static double NormalizeDegreesToPositiveBelow360(double degrees)
    {
        degrees %= 360;
        if (degrees < 0)
            degrees += 360;

        return degrees;
    }

    /// <summary>
    /// Normalizes degrees to the range ]-360..360[
    /// </summary>
    /// <param name="degrees">A value in degrees, positive or negative.</param>
    /// <returns>A double.</returns>
    private static double NormalizeDegreesTo360NegativeOrPositive(double degrees) => degrees % 360;
}