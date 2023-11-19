using OpenSvg.Config;
using SkiaSharp;

namespace OpenSvg.GeoJson.Converters;
public static class DrawConfigConverter
{

    public static Dictionary<string, object> ToDictionary(this DrawConfig drawConfig)
    {
        static string GeoJsonColorString(SkiaSharp.SKColor color) =>
           color.IsTransparent() ? "rgba(0, 0, 0, 0)" : color.ToHexColorString();

        var properties = new Dictionary<string, object>
        {
            { GeoJsonNames.Fill, GeoJsonColorString(drawConfig.FillColor) },
            { GeoJsonNames.Stroke, GeoJsonColorString(drawConfig.StrokeColor) },
            { GeoJsonNames.StrokeWidth, drawConfig.StrokeWidth },
            { GeoJsonNames.FillOpacity, 1 }
        };

        return properties;
    }


    public static DrawConfig ToDrawConfig(this Dictionary<string, object> properties)
    {
        SKColor fillColor = (properties.GetValueOrDefault(GeoJsonNames.Fill) as string)?.ToColor() ?? DrawConfig.DefaultFillColor;
        SKColor strokeColor = (properties.GetValueOrDefault(GeoJsonNames.Stroke) as string)?.ToColor() ?? DrawConfig.DefaultStrokeColor;
        double strokeWidth = (properties.GetValueOrDefault(GeoJsonNames.StrokeWidth) as string)?.ToDouble() ?? DrawConfig.DefaultStrokeWidth;
        
        return new DrawConfig(fillColor, strokeColor, strokeWidth);
    }

   


}
