using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;
using System.Collections.Immutable;
using System.IO.Compression;

namespace OpenSvg.Gtfs;
public class GtfsFeed
{
    public required ImmutableArray<GtfsStop> Stops { get; init; }

    public required ImmutableArray<GtfsStopTime> StopTimes { get; init; }
    public required ImmutableArray<GtfsShape> Shapes { get; init; }

    public required ImmutableArray<GtfsTrip> Trips { get; init; }

    public GtfsFeed()
    {
       
    }

    public static GtfsFeed Load(string gtfsFilePath)
    {
        using var gtsFile = ZipFile.OpenRead(gtfsFilePath);
        ImmutableArray<GtfsStop> stops = ImmutableArray<GtfsStop>.Empty;
        ImmutableArray<GtfsStopTime> stopTimes = ImmutableArray<GtfsStopTime>.Empty;
        ImmutableArray<GtfsShape> shapes = ImmutableArray<GtfsShape>.Empty;
        ImmutableArray<GtfsTrip> trips = ImmutableArray<GtfsTrip>.Empty;

        foreach (ZipArchiveEntry entry in gtsFile.Entries)
        {
            switch (entry.Name)
            {
                case "shapes.txt":
                    shapes = ShapesParser.Read(entry).ToImmutableArray();
                    break;
                case "stops.txt":
                    stops = StopsParser.Read(entry).ToImmutableArray();
                    break;
                case "stops_times.txt":
                    stopTimes = StopTimesParser.Read(entry).ToImmutableArray();
                    break;
                case "trips.txt":
                    trips = TripsParser.Read(entry).ToImmutableArray();
                    break;

            }           
        }
        return new GtfsFeed()
        {
            Stops = stops,
            StopTimes = stopTimes,
            Shapes = shapes,
            Trips = trips,
        };

    }

    public SvgDocument ToSvgDocument()
    {

        var geoBoundingBox = ComputeGeoBoundingBox(); 

        var topLeftCoordinate = geoBoundingBox.TopLeft;
        double metersPerPixel = PointConverter.MetersPerPixels(1000, geoBoundingBox);


        PointConverter converter = new PointConverter(topLeftCoordinate, metersPerPixel, 10);

        SvgGroup svgGroupStops = Stops.ToSvgGroup(converter);
        SvgGroup svgGroupShapes = Shapes.ToSvgGroup(converter);

        SvgDocument svgDocument = new SvgDocument();
        svgDocument.Add(svgGroupStops);
        svgDocument.Add(svgGroupShapes);
        svgDocument.SetViewBoxToActualSizeAndDefaultViewPort();

        return svgDocument;
    }

    public GeoBoundingBox ComputeGeoBoundingBox()
    {
        var stopsBounds = new GeoBoundingBox(Stops.Select(s => s.Coordinate));
        var shapesBounds = new GeoBoundingBox(Shapes.SelectMany(s => s.ShapePoints).Select(sp => sp.Coordinate));
        return GeoBoundingBox.Union(stopsBounds, shapesBounds);
    } 


 
}
