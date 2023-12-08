

namespace OpenSvg.GeoJson;

/// <summary>
///     A class for converting a point to a geographic coordinate.
/// </summary>
public record PointConverter
{
    /// <summary>
    ///     Constructor for PointCoordinateConverter class
    /// </summary>
    /// <param name="startLocation">The starting location to use for the conversion</param>
    /// <param name="metersPerPixel">The meters per pixel value used to convert coordinates</param>
    public PointConverter(Coordinate startLocation, double metersPerPixel, int segmentCountForCurveApproximation = 10)
    {
        StartLocation = startLocation;
        MetersPerPixel = metersPerPixel;
        SegmentCountForCurveApproximation = segmentCountForCurveApproximation;

    }

    /// <summary>
    ///     Constructor for PointCoordinateConverter class
    /// </summary>
    /// <param name="geoJsonBoundingBox">The geo bounding box covering all coordinates</param>
    /// <param name="desiredSvgWidth">The desired width of the SVG image</param>
    public PointConverter(GeoBoundingBox geoBoundingBox, double desiredSvgWidth, int segmentCountForCurveApproximation = 10) 
        : this(geoBoundingBox.TopLeft, MetersPerPixels(desiredSvgWidth, geoBoundingBox), segmentCountForCurveApproximation)
    {

    }


    /// <summary>
    ///     The starting point for the coordinate conversion process
    /// </summary>
    public Coordinate StartLocation { get; init; }

    /// <summary>
    ///     The meters per pixel conversion factor.
    /// </summary>
    public double MetersPerPixel { get; init; }

    public int SegmentCountForCurveApproximation { get; init; }

    public static double MetersPerPixels(double desiredSvgWidth, GeoBoundingBox geoBoundingBox)
    {
        double widthInMeters = geoBoundingBox.TopLeft.DistanceTo(geoBoundingBox.TopRight);
        double metersPerPixel = widthInMeters / desiredSvgWidth;
        return metersPerPixel;
    }


    /// <summary>
    ///     Converts a point to a world coordinate.
    /// </summary>
    /// <param name="point">The point to convert.</param>
    /// <returns>The geographic coordinate.</returns>
    public Coordinate ToCoordinate(Point point, Transform? transform)
    {
        if (transform != null) point = point.Transform((Transform)transform);
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
    public Point ToPoint(Coordinate coordinate)
    {
        (double dx, double dy) = StartLocation.CartesianOffset(coordinate);

        double x = dx / MetersPerPixel;
        double y = -dy / MetersPerPixel; // Invert Y-axis

        return new Point((float) x, (float) y);
    }
}