using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using OpenSvg;
using OpenSvg.Config;
using OpenSvg.SvgNodes;

using System;

namespace OpenSvg.GeoJson.Converters;


public static class SvgPolygonConverter
{

    public static Feature ToFeature(this SvgPolygon svgPolygon, Transform parentTransform, PointConverter converter)
    {
        Transform composedTransform = parentTransform.ComposeWith(svgPolygon.Transform.Get());
        Polygon polygon = svgPolygon.Polygon.Get();
        LinearRing linearRing = polygon.ToLinearRing(composedTransform, converter);
        NetTopologySuite.Geometries.Polygon nativePolygon = new(linearRing);
        DrawConfig drawConfig = svgPolygon.DrawConfig;
        NetTopologySuite.Features.AttributesTable attributesTable = drawConfig.ToAttributesTable();
        return new Feature(nativePolygon, attributesTable);
    }


}
