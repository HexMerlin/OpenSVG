using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;
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
        return new GtfsFeed()
        {
            RealStops = stops.Values.Where(s => s.LocationType == 0).ToImmutableSortedDictionary(s => s.StopID, s => s),
            OtherStops = stops.Values.Where(s => s.LocationType != 0).ToImmutableSortedDictionary(s => s.StopID, s => s),
            StopTimes = stopTimes,
            Shapes = shapes,
            Trips = trips,
           
    };

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

          
            Coordinate coordByCloseness = shape.FindClosestCoordinateOnShape(stop.Coordinate);
            Coordinate? coordByDistTraveled = shape.CoordinateUsingGtfsDistance(stopTime.ShapeDistTraveled);

            stop.AddShape(shape, coordByDistTraveled, coordByCloseness);
            
            //if (coordinate != null && coordinate.Value.DistanceTo(stop.Coordinate) > 200)
            //{
                //Console.WriteLine("StopTime: " + stopTime);
                //Console.WriteLine("Trip: " + trip);
                //Console.WriteLine("Stop: " + stop);
                //Console.WriteLine("Stop coordinate: " + stop.Coordinate);
                //Console.WriteLine("Selected OnShapeStop: " + coordinate);
                //Console.WriteLine("Distance from stop to onShapeStop (m): " + coordinate.Value.DistanceTo(stop.Coordinate));

                //Console.WriteLine("Direction ID" + trip.DirectionID);
                //Console.WriteLine("Dist: traveled: " + stopTime.ShapeDistTraveled);
                //Console.WriteLine("-- Shape -- ");
                //foreach (GtfsShapePoint shapePoint in shape.ShapePoints)
                //{
                //    var distanceToStop = shapePoint.Coordinate.DistanceTo(stop.Coordinate);
                //    Console.Write($"{distanceToStop} m: ");
                //    Console.WriteLine(shapePoint);
                //}
                //Console.WriteLine("******** End *******");
              
           // }
           
        }

        //foreach (GtfsStop stop in Stops.Values)
        //{
        //    Console.WriteLine("Stop " + stop.StopID + " " + stop.Shapes.Count());
        //}

    }


    public SvgDocument ToSvgDocument()
    {

        var geoBoundingBox = ComputeGeoBoundingBox(); 

        var topLeftCoordinate = geoBoundingBox.TopLeft;
        double metersPerPixel = PointConverter.MetersPerPixels(1000, geoBoundingBox);


        PointConverter converter = new PointConverter(topLeftCoordinate, metersPerPixel, 10);

        SvgGroup svgGroupStops = RealStops.Values.ToSvgGroup(converter);
       // SvgGroup svgGroupShapes = Shapes.Values.ToSvgGroup(converter);

        SvgDocument svgDocument = new SvgDocument();
        svgDocument.Add(svgGroupStops);
      //  svgDocument.Add(svgGroupShapes);
        svgDocument.SetViewBoxToActualSizeAndDefaultViewPort();

        return svgDocument;
    }

    public GeoBoundingBox ComputeGeoBoundingBox()
    {
        var stopsBounds = new GeoBoundingBox(RealStops.Select(s => s.Value.Coordinate));
        var shapesBounds = new GeoBoundingBox(Shapes.SelectMany(s => s.Value.ShapePoints).Select(sp => sp.Coordinate));
        return GeoBoundingBox.Union(stopsBounds, shapesBounds);
    } 


 
}
