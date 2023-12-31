﻿using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using OpenSvg.Config;
using OpenSvg.SvgNodes;

namespace OpenSvg.Geographics.GeoJson.Converters;


public static class SvgPolylineConverter
{

    /// <summary>
    ///     Converts an <see cref="SvgPolyline"/> to a GeoJSON feature.
    /// </summary>
    /// <param name="svgPolyline">The <see cref="SvgPolyline"/> to convert.</param>
    /// <param name="parentTransform">The transformation set by the parent element.</param>
    /// <param name="converter">A converter for converting points and coordinates</param>
    /// <returns>The resulting GeoJSON feature.</returns>
    public static Feature ToFeature(this SvgPolyline svgPolyline, Transform parentTransform, PointConverter converter)
    {
        Transform composedTransform = parentTransform.ComposeWith(svgPolyline.Transform);
        Polyline polyline = svgPolyline.Polyline;

        LineString lineString = polyline.ToLineString(composedTransform, converter);

        var properties = svgPolyline.DrawConfig.ToDictionary();

        return new Feature(lineString, properties);
    }

    /// <summary>
    /// Converts a GeoJSON feature to an <see cref="SvgPolyline"/>.
    /// </summary>
    /// <param name="feature">The GeoJSON feature to convert.</param>
    /// <param name="converter">A converter for converting points and coordinates</param>
    /// <returns>The resulting <see cref="SvgPolyline"/>.</returns>
    public static SvgPolyline ToSvgPolyline(this Feature feature, PointConverter converter)
    {
        if (feature.Geometry is not LineString lineString || lineString.Coordinates.Count <= 2)
            throw new ArgumentException($"Feature must have a LineString geometry with more than two points to convert to {nameof(SvgPolyline)}.");

        Polyline polyline = lineString.ToPolyline(converter);

        SvgPolyline svgPolyline = new SvgPolyline();
        svgPolyline.Polyline = polyline;

        return svgPolyline;
    }

}
