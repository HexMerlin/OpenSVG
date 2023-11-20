using OpenSvg.Config;
using SkiaSharp;

namespace OpenSvg.GeoJson.Converters;
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

    public static DrawConfig ToDrawConfig(this Dictionary<string, object> properties)
    {
        SKColor fillColor = (properties.GetValueOrDefault(GeoJsonNames.Fill) as string)?.ToOpenSvgColor() ?? Constants.DefaultFillColor;
        SKColor strokeColor = (properties.GetValueOrDefault(GeoJsonNames.Stroke) as string)?.ToOpenSvgColor() ?? Constants.DefaultStrokeColor;
        double strokeWidth = (properties.GetValueOrDefault(GeoJsonNames.StrokeWidth) as string)?.ToDouble() ?? Constants.DefaultStrokeWidth;
        
        return new DrawConfig(fillColor, strokeColor, strokeWidth);
    }

   


}
