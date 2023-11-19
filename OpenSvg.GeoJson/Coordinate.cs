using DotSpatial.Projections;

namespace OpenSvg.GeoJson;

/// <summary>
///     A world coordinate (in WGS84 datum) defined by longitude and latitude
/// </summary>
public readonly struct Coordinate
{
    public readonly double Long;
    
    public readonly double Lat;

    public Coordinate(double longitude, double latitude)
    {
        this.Long = Math.Round(longitude, Constants.CoordinateDecimalPrecision);
        this.Lat = Math.Round(latitude, Constants.CoordinateDecimalPrecision);
    }

    
    /// <summary>
    /// Translates the coordinate by a specified distance in meters along the X and Y axes.
    /// </summary>
    /// <param name="dxMeters">East-west translation in meters. Positive values move east, negative values move west.</param>
    /// <param name="dyMeters">North-south translation in meters. Positive values move north, negative values move south.</param>
    /// <remarks>Note that the y-coordinates increase upwards (north), whereas SVG Y-coordinates increase downwards</remarks>
    /// <returns>A new Coordinate object representing the translated position.</returns>

    public Coordinate Translate(double dxMeters, double dyMeters)
    {
        // Define WGS84 ellipsoid
        ProjectionInfo wgs84 = KnownCoordinateSystems.Geographic.World.WGS1984;

        // Transform the point to Mercator projection (meters)
        double[] z = { 0 };
        double[] xy = { Long, Lat };
        Reproject.ReprojectPoints(xy, z, wgs84, KnownCoordinateSystems.Projected.World.WebMercator, 0, 1);

        // Add the dxMeters and dyMeters
        xy[0] += dxMeters;
        xy[1] += dyMeters;

        // Transform the point back to WGS84
        Reproject.ReprojectPoints(xy, z, KnownCoordinateSystems.Projected.World.WebMercator, wgs84, 0, 1);

        // the xy array now holds the new coordinates
        var result = new Coordinate(xy[0], xy[1]);

        return result;
    }

    /// <summary>
    /// Calculates the Cartesian offset to another coordinate in meters, as a tuple of X (dx) and Y (dy) distances.
    /// Positive dx indicates eastward distance, negative dx indicates westward distance.
    /// Positive dy indicates northward distance, negative dy indicates southward distance.
    /// This method assumes a flat Earth projection to compute the distances.
    /// </summary>
    /// <param name="coordinate">The coordinate to calculate the offset to.</param>
    /// <returns>
    /// A tuple containing the X (dx) and Y (dy) distances in meters between the current coordinate and the specified coordinate.
    /// </returns>
    public (double dx, double dy) CartesianOffset(Coordinate coordinate)
    {
        // Define WGS84 ellipsoid
        ProjectionInfo wgs84 = KnownCoordinateSystems.Geographic.World.WGS1984;

        // Transform the points to Mercator projection (meters)
        double[] z = new double[2];
        double[] xy1 = { Long, Lat };
        double[] xy2 = { coordinate.Long, coordinate.Lat };
        Reproject.ReprojectPoints(xy1, z, wgs84, KnownCoordinateSystems.Projected.World.WebMercator, 0, 1);
        Reproject.ReprojectPoints(xy2, z, wgs84, KnownCoordinateSystems.Projected.World.WebMercator, 0, 1);

        // Calculate distance on a flat surface
        double dx = xy2[0] - xy1[0];
        double dy = xy2[1] - xy1[1];
        return (dx, dy);

    }


    /// <summary>
    /// Calculates the distance to another coordinate.
    /// </summary>
    /// <param name="coordinate">The coordinate to calculate the distance to.</param>
    /// <returns>The distance between the two coordinates in meters.</returns>

    public double DistanceTo(Coordinate coordinate)
    {
        var (dx, dy) = CartesianOffset(coordinate);
        double distance = Math.Sqrt(dx * dx + dy * dy);
        return distance;
    }

    /// <summary>
    ///     A world coordinate as a string in a JSON friendly format
    /// </summary>
    /// <returns>The coordinate as a string in a JSON friendly format.</returns>
    public override string ToString() => $"{{ \"long\": {Long.ToXmlString()}, \"lat\": {Lat.ToXmlString()} }}";
}