namespace OpenSvg.GeoJson;

/// <summary>
///     A class for converting a point to a geographic coordinate.
/// </summary>
public record PointCoordinateConverter
{
    /// <summary>
    ///     Constructor for PointCoordinateConverter class
    /// </summary>
    /// <param name="startLocation">The starting location to use for the conversion</param>
    /// <param name="metersPerPixel">The meters per pixel value used to convert coordinates</param>
    public PointCoordinateConverter(Coordinate startLocation, double metersPerPixel)
    {
        StartLocation = startLocation;
        MetersPerPixel = metersPerPixel;
    }

    /// <summary>
    ///     The starting point for the coordinate conversion process
    /// </summary>
    public Coordinate StartLocation { get; init; }

    /// <summary>
    ///     The meters per pixel conversion factor.
    /// </summary>
    public double MetersPerPixel { get; init; }

    /// <summary>
    ///     Converts a point to a world coordinate.
    /// </summary>
    /// <param name="point">The point to convert.</param>
    /// <returns>The geographic coordinate.</returns>
    public Coordinate ConvertToCoordinate(Point point)
    {
        double dxMeters = point.X * MetersPerPixel;
        double dyMeters = -point.Y * MetersPerPixel; // Invert Y-value

        Coordinate coordinate = StartLocation.Translate(dxMeters, dyMeters);
            
        return coordinate;
    }


    /// <summary>
    ///     Converts a world coordinate to a point.
    /// </summary>
    /// <param name="coordinate">The world coordinate to convert.</param>
    /// <returns>A point converted from the input coordinate.</returns>
    public Point ConvertToPoint(Coordinate coordinate)
    {
        var (dx, dy) = StartLocation.CartesianOffset(coordinate);

        double x = dx / MetersPerPixel;
        double y = -dy / MetersPerPixel; // Invert Y-axis

        return new Point(x, y);
    }
}