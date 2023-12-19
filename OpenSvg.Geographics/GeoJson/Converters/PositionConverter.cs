using GeoJSON.Net.Geometry;

namespace OpenSvg.Geographics.GeoJson.Converters;
internal static class PositionConverter
{
    public static Position ToPosition(this Coordinate coordinate) => new Position(coordinate.Lat, coordinate.Long);

    public static Coordinate ToCoordiate(this IPosition position) => new Coordinate(position.Longitude, position.Latitude);

}
