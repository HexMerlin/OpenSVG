using DotSpatial.Projections;

namespace OpenSvg.GeoJson;

/// <summary>
///     A world coordinate (in WGS84 datum) defined by longitude and latitude
/// </summary>
public record Coordinate(double Long, double Lat)
{
    /// <summary>
    /// Translates the coordinate by a specified distance in meters along the X and Y axes.
    /// </summary>
    /// <param name="dxMeters">East-west translation in meters. Positive values move east, negative values move west.</param>
    /// <param name="dyMeters">North-south translation in meters. Positive values move north, negative values move south.</param>
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
    ///     Gets the distance between two coordinates.
    /// </summary>
    /// <param name="coordinate">The coordinate.</param>
    /// <returns>A double.</returns>
    public double DistanceTo(Coordinate coordinate)
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
        double distance = Math.Sqrt(dx * dx + dy * dy);

        return distance;
    }

    /// <summary>
    ///     A world coordinate as a string in a JSON friendly format
    /// </summary>
    /// <returns>The coordinate as a string in a JSON friendly format.</returns>
    public override string ToString() => $"{{ \"long\": {Math.Round(Long, 3).ToXmlString()}, \"lat\": {Math.Round(Lat, 3).ToXmlString()} }}";
}