using OpenSvg.Geographics;
using OpenSvg.SvgNodes;
using SkiaSharp;
using System.Collections.Immutable;
using System.IO.Compression;

namespace OpenSvg.Gtfs;
public class GtfsFeed
{
   
    public required ImmutableSortedDictionary<string, GtfsStop> RealStops { get; init; }

    public required ImmutableSortedDictionary<string, GtfsStop> OtherStops { get; init; }

    public required ImmutableArray<GtfsStopTime> StopTimes { get; init; }
    public required ImmutableSortedDictionary<string, GtfsShape> Shapes { get; init; }

    public required ImmutableSortedDictionary<string, GtfsTrip> Trips { get; init; }

    public ImmutableArray<GtfsStop> RealStopsWithTraffic => RealStops.Values.Where(s => s.HasTraffic).ToImmutableArray();

    public GtfsFeed()
    {
      
    }

    public GtfsFeed CreateFiltered()
    {
        
        var stops = this.RealStopsWithTraffic.Take(20).ToImmutableSortedDictionary(s => s.StopID, s => s);
        var shapes = stops.Values.SelectMany(s => s.Shapes.Values).Select(s => s.shape).Distinct().ToImmutableSortedDictionary(s => s.ID, s => s);
        GtfsFeed filtered = new GtfsFeed()
        {
            RealStops = stops,
            OtherStops = ImmutableSortedDictionary<string, GtfsStop>.Empty,
            StopTimes = ImmutableArray<GtfsStopTime>.Empty,
            Shapes = shapes,
            Trips = ImmutableSortedDictionary<string, GtfsTrip>.Empty,
        };
        return filtered;
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
            Console.WriteLine("Loaded " + entry.Name);
        }
        GtfsFeed gtfsFeed = new GtfsFeed()
        {
            RealStops = stops.Values.Where(s => s.LocationType == 0).ToImmutableSortedDictionary(s => s.StopID, s => s),
            OtherStops = stops.Values.Where(s => s.LocationType != 0).ToImmutableSortedDictionary(s => s.StopID, s => s),
            StopTimes = stopTimes,
            Shapes = shapes,
            Trips = trips,

        };
        gtfsFeed.JoinDataSources();
        
        return gtfsFeed;
    }



    public SvgDocument ToSvgDocument()
    {

        var geoBoundingBox = ComputeGeoBoundingBox(); 

        var topLeftCoordinate = geoBoundingBox.TopLeft;
        double metersPerPixel = PointConverter.MetersPerPixels(1000, geoBoundingBox);
        PointConverter converter = new PointConverter(topLeftCoordinate, metersPerPixel, 10);

        

        //LineSet lineSet = new LineSet();
        Console.WriteLine("ORIGINAL GTFS POINT COUNT: " + Shapes.Values.SelectMany(s => s.ShapePoints).Count());
        Console.WriteLine("Creating raw polylines...");

        var shapeGroup = new SvgGroup()
        {
            StrokeColor = SKColors.DarkGreen,
            StrokeWidth = 0.5f,
            FillColor = SKColors.Transparent
        };
        Console.WriteLine("Creating SvgPolylines...");
        foreach (GtfsShape shape in Shapes.Values)
        {
            var svgPolyline = shape.ToPolyline(converter).ToSvgPolyline();
            shapeGroup.ChildElements.Add(svgPolyline);

        }

        SvgGroup stopGroup = new SvgGroup()
        {
            StrokeColor = SKColors.DarkRed,
            StrokeWidth = 2f,
            FillColor = SKColors.Transparent,
            
        };
        foreach (GtfsStop stop in RealStops.Values)
        {
            var svgShape = stop.ToSvgShape(converter);
            stopGroup.ChildElements.Add(svgShape);
        }
       
        SvgDocument svgDocument = new SvgDocument();
        svgDocument.Add(stopGroup);
        svgDocument.Add(shapeGroup);

        svgDocument.SetViewBoxToActualSizeAndDefaultViewPort();

        return svgDocument;
    }

    public GeoBoundingBox ComputeGeoBoundingBox()
    {
        var coordinates = Shapes.SelectMany(s => s.Value.ShapePoints).Select(sp => sp.Coordinate)
            .Concat(RealStops.Select(s => s.Value.Coordinate)); ;
   
        return new GeoBoundingBox(coordinates);
    }


    public void JoinDataSources()
    {
      
        Console.WriteLine("Processing...");
        foreach (GtfsStopTime stopTime in StopTimes)
        {
            string tripID = stopTime.TripID;
            string stopID = stopTime.StopID;
            GtfsTrip trip = Trips[tripID];
            GtfsShape shape = Shapes[trip.ShapeID];
            GtfsStop stop = RealStops[stopID];
            stop.HasTraffic = true;

            if (stopTime.StopSequence == 0)
            {
                stop.HasDistTraveled = true;
            }
            if (stopTime.StopSequence > 0 && stopTime.ShapeDistTraveled > 0)
            {
                stop.HasDistTraveled = true;
            }
            stop.AssociateWith(shape, stopTime);

            //Coordinate coordByCloseness = shape.FindClosestCoordinateOnShape(stop.Coordinate);
            //Coordinate? coordByDistTraveled = shape.CoordinateUsingGtfsDistance(stopTime.ShapeDistTraveled);
            //stop.AddShape(shape, coordByDistTraveled, coordByCloseness);

        }
    }

}
