namespace OpenSvg.GeoJson;

/// <summary>
///     A class for converting a point to a geographic coordinate.
/// </summary>
public record PointToCoordinateConverter
{
    /// <summary>
    ///     Constructor for PointToCoordinateConverter class
    /// </summary>
    /// <param name="startLocation">The starting location to use for the conversion</param>
    /// <param name="metersPerPixel">The meters per pixel value used to convert coordinates</param>
    /// <param name="imageHeight">The height of the image in pixels</param>
    /// <param name="roundToDecimals">The number of decimals to round coordinates to (optional, default is -1)</param>
    public PointToCoordinateConverter(Coordinate startLocation, double metersPerPixel, double imageHeight,
        int roundToDecimals = -1)
    {
        ImageHeight = imageHeight;
        StartLocation = startLocation;
        MetersPerPixel = metersPerPixel;
        RoundToDecimals = roundToDecimals;
    }

    /// <summary>
    ///     Applies rounding of Coordinate values to a fixed number of decimals.
    ///     If the value is less than 0, no rounding is applied
    ///     This feature can be used during development to get output that is easier to read
    /// </summary>
    public int RoundToDecimals { get; init; } = -1;

    /// <summary>
    ///     The starting point for the coordinate conversion process
    /// </summary>
    public Coordinate StartLocation { get; init; }

    /// <summary>
    ///     The meters per pixel conversion factor.
    /// </summary>
    public double MetersPerPixel { get; init; }

    /// <summary>
    ///     The height of the image is required to invert the Y-value of points,
    ///     with points having its origin (0,0) at the top-left corner and Y-coordinates increase downwards,
    ///     as opposed to world coordinates where y (latitude) increase upwards (towards north)
    /// </summary>
    public double ImageHeight { get; init; }

    /// <summary>
    ///     Converts a point to a world coordinate.
    /// </summary>
    /// <param name="point">The point to convert.</param>
    /// <returns>The geographic coordinate.</returns>
    public Coordinate ConvertToCoordinate(Point point)
    {
        double invertedY = ImageHeight - point.Y;

        double dxMeters = point.X * MetersPerPixel;
        double dyMeters = invertedY * MetersPerPixel;

        Coordinate coordinate = StartLocation.Translate(dxMeters, dyMeters);

        if (RoundToDecimals >= 0)
            coordinate = new Coordinate(Math.Round(coordinate.Long, RoundToDecimals),
                Math.Round(coordinate.Lat, RoundToDecimals));

        return coordinate;
    }
}