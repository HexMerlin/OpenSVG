﻿using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using OpenSvg;
using OpenSvg.Config;
using OpenSvg.SvgNodes;


namespace OpenSvg.GeoJson.Converters;


public static class SvgTextConverter
{

    /// <summary>
    ///     Converts an SvgText to a GeoJSON feature.
    /// </summary>
    /// <param name="svgText">The SvgText to convert.</param>
    /// <param name="parentTransform">The transformation set by the parent element.</param>
    /// <param name="converter">A converter for converting points and coordinates</param>
    /// <returns>The resulting GeoJSON feature.</returns>
    public static Feature ToFeature(this SvgText svgText, Transform parentTransform, PointConverter converter)
    {
        Transform composedTransform = parentTransform.ComposeWith(svgText.Transform.Get());
        Point textPoint = new Point(svgText.X.Get(), svgText.Y.Get());
        Position position = converter.ToPosition(textPoint, composedTransform);
        TextConfig svgTextConfig = svgText.TextConfig;
        var properties = svgTextConfig.ToDictionary(converter); 
        GeoJSON.Net.Geometry.Point geoJsonPoint = new GeoJSON.Net.Geometry.Point(position);
        return new Feature(geoJsonPoint, properties);
    }

    /// <summary>
    /// Converts a GeoJSON feature to an <see cref="SvgText"/>.
    /// </summary>
    /// <param name="feature">The GeoJSON feature to convert.</param>
    /// <param name="converter">A converter for converting points and coordinates</param>
    /// <returns>The resulting SvgText.</returns>
    public static SvgText ToSvgText(this Feature feature, PointConverter converter)
    {
        if (feature.Geometry is not GeoJSON.Net.Geometry.Point geoJsonPoint)
            throw new ArgumentException($"Feature must have a Point geometry to convert to {nameof(SvgText)}.");

        Point point = converter.ToPoint(geoJsonPoint.Coordinates);
        Dictionary<string, object>? properties = feature.Properties as Dictionary<string, object>;
        TextConfig textConfig = properties?.ToTextConfig(converter) ?? new TextConfig();

        var svgText = new SvgText
        {
            Point = point,
            TextConfig = textConfig
        };

        return svgText;
    }

    public static bool IsTextFeature(this Feature feature)
    {
        return feature.Geometry is GeoJSON.Net.Geometry.Point
            && feature.Properties != null
            && feature.Properties.ContainsKey(GeoJsonNames.Text);
    }
}