using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace OpenSvg.Gtfs;
public class GtfsShape
{
    public string ID { get; }
    public ImmutableArray<GtfsShapePoint> ShapePoints { get; }


    public GtfsShape(string id, IEnumerable<GtfsShapePoint> shapePoints)
    {
        ID = id;
        ShapePoints = shapePoints.ToImmutableArray();
    }


}
