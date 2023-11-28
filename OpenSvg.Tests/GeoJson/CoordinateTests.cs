//File CoorinateTests.cs

using OpenSvg.GeoJson;

namespace OpenSvg.Tests.GeoJson;

public class CoordinateTests
{
    /// <summary>
    /// Tests the translation of a coordinate by a given distance in meters to the north, south, east and west directions, respectively.
    /// The distance to the destination is then measured in meters to check if it matches the expected distance
    /// </summary>
    [Fact]
    public void CoordinateTranslateTest()
    {
        const int precision = 0;
        const float distance = 10000; //distance in meters
        Coordinate origin = new Coordinate(15.614f, 58.408f);

        Coordinate eastBy10000m = origin.Translate(distance, 0);
        Coordinate westBy10000m = origin.Translate(-distance, 0);
        Coordinate northBy10000m = origin.Translate(0, distance);
        Coordinate southBy10000m = origin.Translate(0, -distance);

        Assert.Equal(distance, origin.DistanceTo(eastBy10000m), precision);
        Assert.Equal(distance, origin.DistanceTo(westBy10000m), precision);
        Assert.Equal(distance, origin.DistanceTo(northBy10000m), precision);
        Assert.Equal(distance, origin.DistanceTo(southBy10000m), precision);
    }
}