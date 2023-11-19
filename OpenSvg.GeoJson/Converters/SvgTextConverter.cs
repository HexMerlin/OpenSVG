using Microsoft.AspNetCore.Http.Features;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using OpenSvg;
using OpenSvg.Config;
using OpenSvg.SvgNodes;

using System;

namespace OpenSvg.GeoJson.Converters;


public static class SvgTextConverter
{

    /// <summary>
    /// Converts an SvgText to a GeoJSON feature.
    /// </summary>
    /// <param name="svgText">The SvgText to convert.</param>
    /// <param name="transform">The transformation object for the SvgPath.</param>
    /// <returns>The resulting GeoJSON feature.</returns>
    public static Feature ToFeature(this SvgText svgText, Transform parentTransform, PointConverter converter)
    {
        Transform composedTransform = parentTransform.ComposeWith(svgText.Transform.Get());
      
        var textPoint = new Point(svgText.X.Get(), svgText.Y.Get());
        NetTopologySuite.Geometries.Coordinate geoJsonTextCoordinate = converter.ToNativeCoordinate(textPoint, composedTransform);

        NetTopologySuite.Geometries.Point textPointNative = new(geoJsonTextCoordinate);

        TextConfig textConfig = svgText.TextConfig;

        double fontSizeScaled = textConfig.FontSize * converter.MetersPerPixel;

        // Create a GeoJSON feature for the text
        Feature feature = new(textPointNative, new AttributesTable
                    {
                        { GeoJsonNames.Text, textConfig.Text },
                        { GeoJsonNames.FontName, textConfig.SvgFont.FontName },
                        { GeoJsonNames.FontSize, fontSizeScaled.ToXmlString()  },
                        { GeoJsonNames.Fill, textConfig.DrawConfig.FillColor.ToHexColorString() },  //using fill color for text color in this element
                        { GeoJsonNames.FillOpacity, 1 }
        });

        return feature;
    }

}
