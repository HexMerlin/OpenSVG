using GeoJSON.Net.Feature;
using OpenSvg;
using OpenSvg.Config;
using OpenSvg.SvgNodes;

using System;

namespace OpenSvg.GeoJson.Converters;


public static class SvgVisualConverter
{

    /// <summary>
    /// Converts an SvgVisual to a GeoJSON feature sequence.
    /// </summary>
    /// <param name="svgVisual">The SvgLine to convert.</param>
    /// <param name="transform">The transformation object for the SvgPath.</param>
    /// <param name="converter">The converter to use for converting coordinates.</param>
    /// <returns>The resulting GeoJSON feature collection.</returns>

    public static IEnumerable<Feature> ToFeatures(this SvgVisual svgVisual, Transform parentTransform, PointConverter converter)
    {
       
        if (svgVisual is SvgVisualContainer svgVisualContainer) 
        {
            Transform composedTransform = parentTransform.ComposeWith(svgVisual.Transform);
            IEnumerable<Feature> features = svgVisualContainer.Children().OfType<SvgVisual>().SelectMany(c => c.ToFeatures(composedTransform, converter));
            foreach (Feature feature in features)
                yield return feature;           
        }
        else yield return svgVisual switch
        {
            SvgPath svgPath => svgPath.ToFeature(parentTransform, converter),
            SvgPolygon svgPolygon => svgPolygon.ToFeature(parentTransform, converter),
            SvgPolyline svgPolyline => svgPolyline.ToFeature(parentTransform, converter),
            SvgLine svgLine => svgLine.ToFeature(parentTransform, converter),
            SvgText svgText => svgText.ToFeature(parentTransform, converter),
          
            _ => throw new NotSupportedException($"SvgElement type {svgVisual.GetType().Name} is not supported.")
        };
    }


    public static SvgVisual ToSvgVisual(this Feature feature, PointConverter converter)
    {
        SvgVisual svgVisual = feature.Geometry switch
        {
            GeoJSON.Net.Geometry.LineString lineString when lineString.Coordinates.Count == 2 => SvgLineConverter.ToSvgLine(feature, converter).ApplyProperties(feature, Constants.DefaultConfigLines),
            GeoJSON.Net.Geometry.LineString lineString when lineString.Coordinates.Count > 2 => SvgPolylineConverter.ToSvgPolyline(feature, converter).ApplyProperties(feature, Constants.DefaultConfigLines),
            GeoJSON.Net.Geometry.Polygon polygon when polygon.Coordinates.Count == 1 => SvgPolygonConverter.ToSvgPolygon(feature, converter).ApplyProperties(feature, Constants.DefaultConfigPolygon),
            GeoJSON.Net.Geometry.Polygon polygon when polygon.Coordinates.Count > 1 => EnclosedPolygonGroupConverter.ToSvgPath(polygon, converter).ApplyProperties(feature, Constants.DefaultConfigPath),
            GeoJSON.Net.Geometry.MultiPolygon multiPolygon => MultiPolygonConverter.ToSvgPath(multiPolygon, converter).ApplyProperties(feature, Constants.DefaultConfigPath),
            GeoJSON.Net.Geometry.Point when feature.IsTextFeature() => SvgTextConverter.ToSvgText(feature, converter).ApplyProperties(feature, Constants.DefaultConfigText),
            GeoJSON.Net.Geometry.Point when !feature.IsTextFeature() => throw new NotSupportedException("Single points that are not text element are not supported by SVG"),
            _ => throw new NotSupportedException($"Unsupported GeoJSON Feature with geometry type {feature.Geometry.GetType().Name}"),
        };

        return svgVisual;
    }
}
