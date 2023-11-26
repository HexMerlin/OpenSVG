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
    public ImmutableArray<GtfsShapePoint> Coordinates { get; }


    public GtfsShape(string id, IEnumerable<GtfsShapePoint> shapePoints)
    {
        ID = id;
        Coordinates = shapePoints.ToImmutableArray();
    }


}
