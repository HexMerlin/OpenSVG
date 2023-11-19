using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using OpenSvg;
using OpenSvg.Config;
using OpenSvg.SvgNodes;

using System;

namespace OpenSvg.GeoJson.Converters;


public static class SvgPathConverter
{

    /// <summary>
    /// Converts an SvgPath to a GeoJSON feature.
    /// </summary>
    /// <param name="svgPath">The SvgPath to convert.</param>
    /// <param name="transform">The transformation object for the SvgPath.</param>
    /// <returns>The resulting GeoJSON feature.</returns>
    public static Feature ToFeature(this SvgPath svgPath, Transform parentTransform, PointConverter converter)
    {
        Transform composedTransform = parentTransform.ComposeWith(svgPath.Transform.Get());
        MultiPolygon multiPolygon = svgPath.ApproximateToMultiPolygon(converter.SegmentCountForCurveApproximation);

        List<NetTopologySuite.Geometries.Polygon> nativePolygons = new();

        foreach (EnclosedPolygonGroup enclosedPolygonGroup in multiPolygon)
        {
            LinearRing exteriorRing = enclosedPolygonGroup.ExteriorPolygon.ToLinearRing(composedTransform, converter);

            List<LinearRing> interiorRings = enclosedPolygonGroup.InteriorPolygons.Select(p => p.ToLinearRing(composedTransform, converter)).ToList();

            NetTopologySuite.Geometries.Polygon nativePolygon = new(exteriorRing, interiorRings.ToArray());

            nativePolygons.Add(nativePolygon);
        }

        // Create a GeoJSON MultiPolygon feature using the polygons
        NetTopologySuite.Geometries.MultiPolygon geoJsonMultiPolygon = new(nativePolygons.ToArray());

        DrawConfig drawConfig = svgPath.DrawConfig;
        AttributesTable attributesTable = drawConfig.ToAttributesTable();
        Feature feature = new(geoJsonMultiPolygon, attributesTable);
        return feature;
    }
}
