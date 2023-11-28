using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;
using System.Collections.Immutable;
using System.IO.Compression;

namespace OpenSvg.Gtfs;
public class GtfsFeed
{
    public required ImmutableSortedDictionary<string, GtfsStop> Stops { get; init; }

    public required ImmutableArray<GtfsStopTime> StopTimes { get; init; }
    public required ImmutableSortedDictionary<string, GtfsShape> Shapes { get; init; }

    public required ImmutableSortedDictionary<string, GtfsTrip> Trips { get; init; }

    public GtfsFeed()
    {
       
    }

    public static GtfsFeed Load(string gtfsFilePath)
    {
        using var gtsFile = ZipFile.OpenRead(gtfsFilePath);
        ImmutableSortedDictionary<string, GtfsStop> stops = ImmutableSortedDictionary<string, GtfsStop>.Empty;
        var stopTimes = ImmutableArray<GtfsStopTime>.Empty;
        ImmutableSortedDictionary<string, GtfsShape> shapes = ImmutableSortedDictionary<string, GtfsShape>.Empty;
        ImmutableSortedDictionary<string, GtfsTrip> trips = ImmutableSortedDictionary<string, GtfsTrip>.Empty;

        foreach (ZipArchiveEntry entry in gtsFile.Entries)
        {
            Console.WriteLine(entry.Name);
        }
        foreach (ZipArchiveEntry entry in gtsFile.Entries)
        {
            switch (entry.Name)
            {
                case "shapes.txt":
                     shapes = ShapesParser.Read(entry).ToImmutableSortedDictionary(s => s.ID, s => s);
                    break;
                case "stops.txt":
                    stops = StopsParser.Read(entry).ToImmutableSortedDictionary(s => s.StopID, s => s);
                    break;
                case "stop_times.txt":
                    stopTimes = StopTimesParser.Read(entry).ToImmutableArray();
                    break;
                case "trips.txt":
                    trips = TripsParser.Read(entry).ToImmutableSortedDictionary(t => t.TripID, t => t);
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

    public void JoinDataSources()
    {
        IEnumerable<(string TripID, string StopID)> tripsAndStops = StopTimes.Select(st => (st.TripID, st.StopID));

        foreach ((string tripID, string stopID) in tripsAndStops)
        {
            
            GtfsTrip trip = Trips[tripID];
            GtfsShape shape = Shapes[trip.ShapeID];

            GtfsStop? stop = Stops.ContainsKey(stopID) ? Stops[stopID] : null;
            if (stop != null)
            {
                stop.AddShape(shape);
            }
            else
            {
                Console.WriteLine($"Stop {stopID} was referenced but not found.");
            }

        }

        foreach (GtfsStop stop in Stops.Values)
        {
            Console.WriteLine("Stop " + stop.StopID + " " + stop.Shapes.Count());
        }

    }

    public SvgDocument ToSvgDocument()
    {

        var geoBoundingBox = ComputeGeoBoundingBox(); 

        var topLeftCoordinate = geoBoundingBox.TopLeft;
        double metersPerPixel = PointConverter.MetersPerPixels(1000, geoBoundingBox);


        PointConverter converter = new PointConverter(topLeftCoordinate, metersPerPixel, 10);

        SvgGroup svgGroupStops = Stops.Values.ToSvgGroup(converter);
        SvgGroup svgGroupShapes = Shapes.Values.ToSvgGroup(converter);

        SvgDocument svgDocument = new SvgDocument();
        svgDocument.Add(svgGroupStops);
        svgDocument.Add(svgGroupShapes);
        svgDocument.SetViewBoxToActualSizeAndDefaultViewPort();

        return svgDocument;
    }

    public GeoBoundingBox ComputeGeoBoundingBox()
    {
        var stopsBounds = new GeoBoundingBox(Stops.Select(s => s.Value.Coordinate));
        var shapesBounds = new GeoBoundingBox(Shapes.SelectMany(s => s.Value.ShapePoints).Select(sp => sp.Coordinate));
        return GeoBoundingBox.Union(stopsBounds, shapesBounds);
    } 


 
}
