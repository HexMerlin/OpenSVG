using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using OpenSvg;
using OpenSvg.Config;
using OpenSvg.SvgNodes;

using System;

namespace OpenSvg.GeoJson.Converters;


public static class SvgPolylineConverter
{

    public static Feature ToFeature(this SvgPolyline svgPolyline, Transform parentTransform, PointConverter converter)
    {
        Transform composedTransform = parentTransform.ComposeWith(svgPolyline.Transform.Get());
        Polyline polyline = svgPolyline.Polyline.Get();
        LineString lineString = polyline.ToLinearRing(composedTransform, converter);
        NetTopologySuite.Geometries.Polygon nativePolygon = new(linearRing);
        DrawConfig drawConfig = svgPolygon.DrawConfig;
        NetTopologySuite.Features.AttributesTable attributesTable = drawConfig.ToAttributesTable();
        return new Feature(nativePolygon, attributesTable);
    }


}
