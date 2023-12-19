using OpenSvg.Config;
using OpenSvg.SvgNodes;

namespace OpenSvg.Geographics.GeoJson.Converters;
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



}
