using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using OpenSvg;
using OpenSvg.Config;
using OpenSvg.SvgNodes;

using System;

namespace OpenSvg.GeoJson.Converters;


public static class SvgLineConverter
{

    /// <summary>
    /// Converts an SvgLine to a GeoJSON feature.
    /// </summary>
    /// <param name="svgLine">The SvgLine to convert.</param>
    /// <param name="transform">The transformation object for the SvgPath.</param>
    /// <returns>The resulting GeoJSON feature.</returns>
    public static Feature ToFeature(this SvgLine svgLine, Transform parentTransform, PointConverter converter)
    {
        Transform composedTransform = parentTransform.ComposeWith(svgLine.Transform.Get());
        NetTopologySuite.Geometries.Coordinate p1 = converter.ToNativeCoordinate(svgLine.P1, composedTransform);
        NetTopologySuite.Geometries.Coordinate p2 = converter.ToNativeCoordinate(svgLine.P2, composedTransform);
        LineString lineString = new LineString(new[] { p1, p2 });

        DrawConfig drawConfig = svgLine.DrawConfig;
        AttributesTable attributesTable = drawConfig.ToAttributesTable();
        Feature feature = new(lineString, attributesTable);
        return feature;
    }

    public static SvgLine ToSvgLine(this Feature feature, PointConverter converter)
    {
        LineString lineString = (LineString)feature.Geometry;
        DrawConfig drawConfig = feature.Attributes.ToDrawConfig();

        Point p1 = converter.ToPoint(lineString.Coordinates[0]);
        Point p2 = converter.ToPoint(lineString.Coordinates[1]);
        SvgLine svgLine = new SvgLine
        {
            P1 = p1,
            P2 = p2,
            DrawConfig = drawConfig
        };
        return svgLine;
    }

}
