using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
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
            Transform composedTransform = parentTransform.ComposeWith(svgVisual.Transform.Get());
            IEnumerable<Feature> features = svgVisualContainer.Children().OfType<SvgVisual>().SelectMany(c => c.ToFeatures(composedTransform, converter));
            foreach (Feature feature in features)
                yield return feature;           
        }
        else yield return svgVisual switch
        {
            SvgPath svgPath => svgPath.ToFeature(parentTransform, converter),
            SvgPolygon svgPolygon => svgPolygon.ToFeature(parentTransform, converter),
            SvgLine svgLine => svgLine.ToFeature(parentTransform, converter),
            SvgText svgText => svgText.ToFeature(parentTransform, converter),
          
            _ => throw new NotSupportedException($"SvgElement type {svgVisual.GetType().Name} is not supported.")
        };
    }

}
