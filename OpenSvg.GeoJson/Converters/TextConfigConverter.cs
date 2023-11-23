using OpenSvg.Config;
using OpenSvg.SvgNodes;

namespace OpenSvg.GeoJson.Converters;
public static class TextConfigConverter
{

    public static Dictionary<string, object> ToDictionary(this TextConfig textConfig, PointConverter converter)
    {
        double fontSizeScaled = textConfig.FontSize * converter.MetersPerPixel;

        var properties = textConfig.DrawConfig.ToDictionary();
        properties.Add(GeoJsonNames.Text, textConfig.Text);
        properties.Add(GeoJsonNames.FontName, textConfig.SvgFont.FontName);
        properties.Add(GeoJsonNames.FontSize, fontSizeScaled.ToXmlString());
     
        return properties;
    }

    //public static TextConfig ToTextConfig(this Dictionary<string, object> properties, PointConverter converter)
    //{
    //    DrawConfig drawConfig = properties.ToDrawConfig();

    //    string text = properties.GetValueOrDefault(GeoJsonNames.Text) as string ?? string.Empty;
    //    string fontName = properties.GetValueOrDefault(GeoJsonNames.FontName) as string ?? SvgNames.DefaultFontName;
    //    SvgFont svgFont = SvgFont.GetSystemFont(fontName);
    //    double fontSize = (properties.GetValueOrDefault(GeoJsonNames.FontSize) as string)?.ToDouble() / converter.MetersPerPixel ?? SvgNames.DefaultFontSize;

    //    return new  TextConfig(text, svgFont, fontSize, drawConfig);

    //}

}
