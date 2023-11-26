using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;
using System.Collections.Immutable;
using System.IO.Compression;

namespace OpenSvg.Gtfs;
public class GtfsFeed
{
    public required ImmutableArray<GtfsStop> Stops { get; init; }

    public required ImmutableArray<GtfsShape> Shapes { get; init; }

    public GtfsFeed()
    {
       
    }

    public static GtfsFeed Load(string gtfsFilePath)
    {
        using var gtsFile = ZipFile.OpenRead(gtfsFilePath);
        ImmutableArray<GtfsStop> stops = ImmutableArray<GtfsStop>.Empty;
        ImmutableArray<GtfsShape> shapes = ImmutableArray<GtfsShape>.Empty;

        foreach (ZipArchiveEntry entry in gtsFile.Entries)
        {
            switch (entry.Name)
            {
                case "stops.txt":
                    stops = StopsParser.ReadStops(entry).ToImmutableArray();
                    break;
                case "shapes.txt":
                    shapes = ShapesParser.ReadShapes(entry).ToImmutableArray();
                    break;  
                
            }           
        }
        return new GtfsFeed()
        {
            Stops = stops,
            Shapes = shapes
        };

    }

    public SvgDocument ToSvgDocument()
    {

        var geoBoundingBox = ComputeGeoBoundingBox(); 

        var topLeftCoordinate = geoBoundingBox.TopLeft;
        double metersPerPixel = PointConverter.MetersPerPixels(800, geoBoundingBox);
        PointConverter converter = new PointConverter(topLeftCoordinate, metersPerPixel, 10);

        SvgGroup svgGroupStops = Stops.ToSvgGroup(converter);
        SvgGroup svgGroupShapes = Shapes.ToSvgGroup(converter);

        SvgDocument svgDocument = new SvgDocument();
        svgDocument.Add(svgGroupStops);
        svgDocument.Add(svgGroupShapes);
        svgDocument.SetViewPortToActualSize();

        return svgDocument;
    }

    public GeoBoundingBox ComputeGeoBoundingBox()
    {
        var stopsBounds = new GeoBoundingBox(Stops.Select(s => s.Coordinate));
        var shapesBounds = new GeoBoundingBox(Shapes.SelectMany(s => s.Coordinates).Select(sp => sp.Coordinate));
        return GeoBoundingBox.Union(stopsBounds, shapesBounds);
    } 


 
}
