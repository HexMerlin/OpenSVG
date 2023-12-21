using System.Globalization;
using System.Xml.Linq;
using OpenSvg;
using OpenSvg.Geographics;
using OpenSvg.SvgNodes;
using SkiaSharp;
using OpenSvg.Optimization;
using System.Collections.Immutable;
using System.Numerics;
using OpenSvg.Geographics.GeoJson;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Feature;

namespace OpenSvg.Netex;

public class NetexShared
{
    public XDocument XDocument { get; private set; }

    public NetexShared(string netexXmlFile)
    {

        const double flattening = 1d / 298.257223563;
        Console.WriteLine(flattening);
        const double flattening2 = 0.0033528106643315072;

        Console.WriteLine(flattening2);

        Coordinate coordinate = new Coordinate(18.062584803276312, 59.33653007566394);
        var c1 = coordinate.TranslateByBearingAndDistance(30, 20);

        var expected1 = new Coordinate(18.062760509, 59.336685555);
        Console.WriteLine("Expected1 : " + expected1);
        Console.WriteLine("Actual1   : " + c1);

        var c2 = coordinate.TranslateByBearingAndDistance(300, 20);

        var expected2 = new Coordinate(18.062280472, 59.336619841);
        Console.WriteLine("Expected2 : " + expected2);
        Console.WriteLine("Actual1   : " + c2);
        throw new NotImplementedException();

        static bool IsTaxi(XElement pointList)
            => pointList.Parent.Parent.Attributes("id").Any(a => a.Value.ToLower(CultureInfo.InvariantCulture).Contains("taxi"));

        this.XDocument = XDocument.Load(netexXmlFile);
        //XDocument.Save(@"D:\Downloads\Test\Netex\NetexShared2.xml");

        List<XElement> posLists = XDocument.Descendants().Where(e => e.Name.LocalName == "posList").Where(e => !IsTaxi(e)).ToList();
        Console.WriteLine("All posLists: " + posLists.Count());

        //var ids = posLists.Select(e => e.Parent.Attributes("id").Select(a => a.Value)).Where(a => a != null).ToList();
        //foreach (var id in ids.Take(20))
        //{
        //    Console.WriteLine("Id: " + id);
        //}

        // Coordinate[][] coordinates = posLists.Select(ParsePointList).ToArray();
      //  Coordinate[][] coordinates = posLists.Select(ParsePointList).Select(list => DEBUG_FILTER_WithinBoundingBox(list)).ToArray();

        Coordinate[][] coordinates = posLists.Select(ParsePointList).Where(list => IsAllWithinBoundingBox(list)).ToArray();

        Foo(coordinates);

        return;

        GeoBoundingBox geoBoundingBox = new GeoBoundingBox(coordinates.SelectMany(c => c));

        //var metersPerPixels = PointConverter.MetersPerPixels(1000, geoBoundingBox);
        PointConverter converter = new PointConverter(geoBoundingBox, 1000, 10);
        Console.WriteLine("Meters per pixel: " + converter.MetersPerPixel);

        //FastPolyline[] polylines = coordinates.Where(c => c.Count() >= 2).Select(c => new FastPolyline(c.Select(converter.ToPoint)))
        //    .Select(p => p.RemoveSharpTurns()).ToArray();

        //FastPolyline[] polylines = coordinates.Where(c => c.Count() >= 2).Select(c => new FastPolyline(c.Select(converter.ToPoint)))
        //.Where(p => p.HasBadEndPoint()).ToArray();

        FastPolyline[] polylines = coordinates.Where(c => c.Count() >= 2).Select(c => new FastPolyline(c.Select(converter.ToPoint)))
   .ToArray();
       

      



        Console.WriteLine("Polylines with sharp turns: " + polylines.Count(p => p.HasSharpTurns()));
        polylines = polylines.Select(polyline => polyline.RemoveEquivalentAdjacentPoints()).ToArray();
        polylines = polylines.Select(p => p.RemoveSharpTurns()).ToArray();
        polylines = polylines.Select(p => p.ApplyRDPA()).ToArray(); 
        //Console.WriteLine("Polylines with sharp turns: " + polylines.Count(p => p.HasSharpTurns()));
        // polylines = polylines.Select(p => p.ReorderMisplacedPoints()).ToArray();
        //  polylines = polylines.Select(polyline => polyline.RemoveEquivalentAdjacentPoints()).ToArray();

        var originalSvg = CreateSvg(polylines, ImmutableArray<FastPolyline>.Empty, GetEndPoints(polylines), ImmutableArray<Point>.Empty);
        originalSvg.Save($@"D:\Downloads\Test\Netex\1.svg");
        // polylines = polylines.Where(p => p.HasDuplicatedPoints()).ToArray();

        // polylines = polylines.Where(p => p.ContainsSharpTurns()).Take(1).ToArray();

        //polylines = polylines.Where(p => p.HasDuplicatedPoints()).Where(p => p.Length < 13).Take(1).ToArray();

        //FastPolyline debugPolyline = polylines.First(p => p.HasDuplicatedPoints());

        //Console.WriteLine("FOCUS LINE SHARPTURNS: " + debugPolyline.HasSharpTurns());

        //Console.WriteLine(debugPolyline.Points[0]);
        //for (int i = 1; i < debugPolyline.Length - 1; i++)
        //{
        //    Console.WriteLine(debugPolyline.Points[i] + "   " + FastPolyline.GetAngle(debugPolyline[i - 1], debugPolyline[i], debugPolyline[i + 1]) + " Dist: " + Vector2.DistanceSquared(debugPolyline.Points[i - 1], debugPolyline.Points[i]));
        //}
        //Console.WriteLine(debugPolyline.Points[^1]);

        //debugPolyline = debugPolyline.RemoveEquivalentAdjacentPoints();

        //Console.WriteLine("After RemoveEquivalentAdjacentPoints");
        //Console.WriteLine(debugPolyline.Points[0]);
        //for (int i = 1; i < debugPolyline.Length - 1; i++)
        //{
        //    Console.WriteLine(debugPolyline.Points[i] + "   " + FastPolyline.GetAngle(debugPolyline[i - 1], debugPolyline[i], debugPolyline[i + 1]) + " Dist: " + Vector2.DistanceSquared(debugPolyline.Points[i - 1], debugPolyline.Points[i]));
        //}
        //Console.WriteLine(debugPolyline.Points[^1]);


        Console.WriteLine("Polylines with sharp turns: " + polylines.Count(p => p.HasSharpTurns()));
        Console.WriteLine("Polylines with duplicated points: " + polylines.Count(p => p.HasDuplicatedPoints()));

        Console.WriteLine("Creating line set");
        LineSet lineSet = new LineSet(polylines);

        lineSet.Optimize(); 

        FastPolyline[] optimizedLines = lineSet.OptimizedPolylines.ToArray();
        FastPolyline[] nonOptimizedLines = lineSet.NonOptimizedPolylines.ToArray();
 

        var optimizedSvg = CreateSvg(nonOptimizedLines, optimizedLines, lineSet.OriginalEndPoints, lineSet.AddedEndPoints);
        optimizedSvg.Save($@"D:\Downloads\Test\Netex\2.svg");

    }

    private static HashSet<Point> GetEndPoints(IEnumerable<FastPolyline> polylines)
    {
        HashSet<Point> endPoints = new HashSet<Point>();
        foreach (var polyline in polylines)
        {
            endPoints.Add(polyline[0]);
            endPoints.Add(polyline[^1]);
        }
        return endPoints;
    }

    public void Foo(Coordinate[][] polylines)
    {
        var lineStrings = polylines.Select(CreateLineString);
      

        GeometryCollection geometryCollection = new GeometryCollection(lineStrings);
        Feature feature = new Feature(geometryCollection);
        FeatureCollection featureCollection = new FeatureCollection();
        featureCollection.Features.Add(feature);
        GeoJsonDocument geoJsonDocument = new GeoJsonDocument(featureCollection);
        geoJsonDocument.Save(@"D:\Downloads\Test\Netex\skane1.geojson");

    }

    private static LineString CreateLineString(Coordinate[] polyline)
    {
        LineString lineString = new LineString(polyline.Select(c => new GeoJSON.Net.Geometry.Position(c.Lat, c.Long)));
        return lineString;
    }
        
    //public static SvgDocument CreateGeoJson(IEnumerable<FastPolyline> nonOptimizedLines, IEnumerable<FastPolyline> optimizedLines, IEnumerable<Point> originalStops, IEnumerable<Point> addedNodes)
    //{
    //    GeoJsonDocument geoJsonDocument = new GeoJsonDocument();


    //}
    public static SvgDocument CreateSvg(IEnumerable<FastPolyline> nonOptimizedLines, IEnumerable<FastPolyline> optimizedLines, IEnumerable<Point> originalStops, IEnumerable<Point> addedNodes)
    {
        SKColor[] colors = ColorHelper.GetBrightDistinctColors();

        SvgDocument svgDocument = new SvgDocument();
        SvgRectangleAsRect backgroundFill = new SvgRectangleAsRect();
        backgroundFill.FillColor = SKColors.Black;
        backgroundFill.DefinedWidth = AbsoluteOrRatio.Ratio(1);
        backgroundFill.DefinedHeight = AbsoluteOrRatio.Ratio(1);
        svgDocument.Add(backgroundFill);

        svgDocument.StrokeWidth = 0.03f;
       
        SvgGroup lineGroup = new SvgGroup();
        lineGroup.FillColor = SKColors.Transparent;
        lineGroup.StrokeWidth = 0.03f;
        foreach (FastPolyline polyline in nonOptimizedLines)
        {
            //SvgPolyline svgPolyline = polyline.ToSvgPolyline();
            var svgElement = polyline.ToSvgPath();
            svgElement.StrokeColor = SKColors.DarkGray;
            svgElement.FillColor = SKColors.Transparent;
            svgDocument.Add(svgElement);

        }

        int index = 0;
        foreach (FastPolyline polyline in optimizedLines)
        {
            var svgElement = polyline.ToSvgPath();
            svgElement.StrokeColor = colors[index % colors.Length];
  
            svgElement.FillColor = SKColors.Transparent;
            // svgElement.FillColor = SKColors.Transparent;
            svgDocument.Add(svgElement);
            index++;
        }




        SvgGroup stopGroup = new SvgGroup();
        stopGroup.FillColor = SKColors.Transparent;
        stopGroup.StrokeWidth = 0;
        //foreach (Point endPoint in addedNodes)
        //{
        //    var svgShape = new SvgCircle() { Center = endPoint, Radius = 0.2f, FillColor = SKColors.Purple };
        //    stopGroup.ChildElements.Add(svgShape);
        //}
        foreach (Point endPoint in originalStops)
        {
            var svgShape = new SvgCircle() { Center = endPoint, Radius = 0.2f, FillColor = SKColors.Yellow };
            stopGroup.ChildElements.Add(svgShape);
        }

        
       // svgDocument.Add(lineGroup);
        svgDocument.Add(stopGroup);



        svgDocument.SetViewBoxToActualSizeAndDefaultViewPort();
        return svgDocument;
    }

    static bool IsAllWithinBoundingBox(Coordinate[] coordinates) => coordinates.All(c => IsWithinBoundingBox(c));

    static bool IsWithinBoundingBox(Coordinate coordinate)
    {
       // return true;
        //larger area
        var topLeft = new Coordinate(12.873657850104392, 55.64814360262774);
        var bottomRight = new Coordinate(13.068633658027395, 55.56195788858898);

        //smaller area
        //var topLeft = new Coordinate(12.923674725904739, 55.57711068790566);
        //var bottomRight = new Coordinate(12.931944300524888, 55.57293422889567);

        return coordinate.IsWithinBoundingBox(topLeft, bottomRight);
    }

    public static Coordinate[] ParsePointList(XElement pointListElement)
    {
        if (pointListElement == null)
        {
            throw new ArgumentNullException(nameof(pointListElement));
        }

        var countAttribute = pointListElement.Attribute("count");
        if (countAttribute == null)
        {
            throw new InvalidOperationException("Count attribute is missing in pointList element.");
        }

        int count = int.Parse(countAttribute.Value);

        var coordinatesText = pointListElement.Value.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        if (coordinatesText.Length != count)
        {
            throw new InvalidOperationException($"Number of coordinates values ({coordinatesText.Length}) does not match the count attribute ({count}).");
        }
        if (count % 2 != 0)
        {
            throw new InvalidOperationException($"Number of coordinates values ({coordinatesText.Length}) is not an even number.");
        }
     
        Coordinate[] coordinates = new Coordinate[count >> 1];
        for (int i = 0; i < count; i+=2)
        {
            double latitude = double.Parse(coordinatesText[i], CultureInfo.InvariantCulture);
            double longitude = double.Parse(coordinatesText[i + 1], CultureInfo.InvariantCulture);
            coordinates[i >> 1] = new Coordinate(longitude, latitude);
        }

        return coordinates;
    }
}
