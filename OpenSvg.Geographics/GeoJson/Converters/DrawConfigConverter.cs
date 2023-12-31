﻿using GeoJSON.Net.Feature;
using OpenSvg.Config;
using OpenSvg.SvgNodes;
using SkiaSharp;

namespace OpenSvg.Geographics.GeoJson.Converters;
public static class DrawConfigConverter
{

    public static Dictionary<string, object> ToDictionary(this DrawConfig drawConfig)
    {
        static string GeoJsonColorString(SKColor color) =>
           color.IsTransparent() ? Constants.TransparentColorString : color.ToHexColorString();

        var properties = new Dictionary<string, object>
        {
            { GeoJsonNames.Fill, GeoJsonColorString(drawConfig.FillColor) },
            { GeoJsonNames.Stroke, GeoJsonColorString(drawConfig.StrokeColor) },
            { GeoJsonNames.StrokeWidth, drawConfig.StrokeWidth },
            { GeoJsonNames.FillOpacity, 1 }
        };

        return properties;
    }
    public static SKColor ToOpenSvgColor(this string geoJsonColorString) => geoJsonColorString.Equals(Constants.TransparentColorString, StringComparison.OrdinalIgnoreCase)
            ? SKColors.Transparent
            : geoJsonColorString.ToColor();

    public static SvgVisual ApplyProperties(this SvgVisual svgVisual, Feature feature, DrawConfig defaultValues)
    {
        Dictionary<string, object>? properties = feature.Properties as Dictionary<string, object>;
        if (properties is not null)
        {
            SKColor fillColor = (properties.GetValueOrDefault(GeoJsonNames.Fill) as string)?.ToOpenSvgColor() ?? defaultValues.FillColor;
            SKColor strokeColor = (properties.GetValueOrDefault(GeoJsonNames.Stroke) as string)?.ToOpenSvgColor() ?? defaultValues.StrokeColor;
            float strokeWidth = (properties.GetValueOrDefault(GeoJsonNames.StrokeWidth) as string)?.ToFloat() ?? defaultValues.StrokeWidth;
            svgVisual.FillColor = fillColor;
            svgVisual.StrokeColor = strokeColor;
            svgVisual.StrokeWidth = strokeWidth;
        }
        return svgVisual;
    }

  


}
