using NetTopologySuite.Features;
using OpenSvg.Config;
using SkiaSharp;

namespace OpenSvg.GeoJson.Converters;
public static class DrawConfigConverter
{

    public static AttributesTable ToAttributesTable(this DrawConfig drawConfig)
    {
        static string GeoJsonColorString(SkiaSharp.SKColor color) =>
            color.IsTransparent() ? "rgba(0, 0, 0, 0)" : color.ToHexColorString();

        AttributesTable attributesTable = new()
        {
            { GeoJsonNames.Stroke, GeoJsonColorString(drawConfig.StrokeColor) },
            { GeoJsonNames.StrokeWidth, drawConfig.StrokeWidth.ToXmlString() },

            { GeoJsonNames.Fill, GeoJsonColorString(drawConfig.FillColor) },
            { GeoJsonNames.FillOpacity, 1 }
        };
        return attributesTable;
    }

    public static DrawConfig ToDrawConfig(this IAttributesTable attributesTable)
    {
        SKColor fillColor = attributesTable.Exists(GeoJsonNames.Fill) ? SKColor.Parse((string)attributesTable[GeoJsonNames.Fill]) : DrawConfig.DefaultFillColor;
        SKColor strokeColor = attributesTable.Exists(GeoJsonNames.Stroke) ? SKColor.Parse((string)attributesTable[GeoJsonNames.Stroke]) : DrawConfig.DefaultStrokeColor;
        double strokeWidth = attributesTable.Exists(GeoJsonNames.StrokeWidth) ? float.Parse((string)attributesTable[GeoJsonNames.StrokeWidth]) : DrawConfig.DefaultStrokeWidth;
        return new DrawConfig(fillColor, strokeColor, strokeWidth);
    }
}
