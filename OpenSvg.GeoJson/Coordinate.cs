using DotSpatial.Projections;

namespace OpenSvg.GeoJson;

/// <summary>
///     A world coordinate (in WGS84 datum) defined by longitude and latitude
/// </summary>
public record Coordinate(double Long, double Lat)
{
    /// <summary>
    ///     Translates the coordinate to a new coordinate by a movement a given distance in meters in X and Y.
    /// </summary>
    /// <param name="dxMeters">
    ///     The east-west translation (movement) in meters. Positive values means to the east, negative
    ///     values means to the west.
    /// </param>
    /// <param name="dyMeters">
    ///     The north-south translation (movement) in meters. Positive values means to the north, negative
    ///     values means to the south.
    /// </param>
    /// <returns>A Coordinate.</returns>
    public Coordinate Translate(double dxMeters, double dyMeters)
    {
        // Define WGS84 ellipsoid
        var wgs84 = KnownCoordinateSystems.Geographic.World.WGS1984;

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
    ///     Gets the distance between two coordinates.
    /// </summary>
    /// <param name="coordinate">The coordinate.</param>
    /// <returns>A double.</returns>
    public double DistanceTo(Coordinate coordinate)
    {
        // Define WGS84 ellipsoid
        var wgs84 = KnownCoordinateSystems.Geographic.World.WGS1984;

        // Transform the points to Mercator projection (meters)
        var z = new double[2];
        double[] xy1 = { Long, Lat };
        double[] xy2 = { coordinate.Long, coordinate.Lat };
        Reproject.ReprojectPoints(xy1, z, wgs84, KnownCoordinateSystems.Projected.World.WebMercator, 0, 1);
        Reproject.ReprojectPoints(xy2, z, wgs84, KnownCoordinateSystems.Projected.World.WebMercator, 0, 1);

        // Calculate distance on a flat surface
        var dx = xy2[0] - xy1[0];
        var dy = xy2[1] - xy1[1];
        var distance = Math.Sqrt(dx * dx + dy * dy);

        return distance;
    }

    /// <summary>
    ///     A world coordinate as a string in a JSON friendly format
    /// </summary>
    /// <returns>The coordinate as a string in a JSON friendly format.</returns>
    public override string ToString()
    {
        return $"{{ \"long\": {Math.Round(Long, 3).ToXmlString()}, \"lat\": {Math.Round(Lat, 3).ToXmlString()} }}";
    }
}