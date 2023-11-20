﻿using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using GeoJSON;
using OpenSvg.SvgNodes;

namespace OpenSvg.GeoJson.Converters;

public static class MultiPolygonConverter
{
    /// <summary>
    ///     Converts an <see cref="MultiPolygon"/> to a GeoJSON MultiPolygon.
    /// </summary>
    /// <param name="multiPolygon">The <see cref="SvgPath"/> to convert.</param>
    /// <param name="transform">The transformation set by a parent element.</param>
    /// <param name="converter">A converter for converting points and coordinates</param>
    /// <returns>The resulting GeoJSON feature.</returns>
    public static GeoJSON.Net.Geometry.MultiPolygon ToGeoJsonMultiPolygon(this MultiPolygon multiPolygon, Transform transform, PointConverter converter)
    {
        var polygons = multiPolygon.Select(group => group.ToGeoJsonPolygon(transform, converter)).ToList();

        return new GeoJSON.Net.Geometry.MultiPolygon(polygons);
    }

    public static MultiPolygon ToMultiPolygon(this GeoJSON.Net.Geometry.MultiPolygon geoJsonMultiPolygon, PointConverter converter)
    {
        IEnumerable<EnclosedPolygonGroup> enclosedPolygonGroups = geoJsonMultiPolygon.Coordinates.Select(polygon => polygon.ToEnclosedPolygonGroup(converter));

        return new MultiPolygon(enclosedPolygonGroups);

    }

}
