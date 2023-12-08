using System.Globalization;
using System.Xml.Linq;
using OpenSvg;
using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;
using SkiaSharp;
using OpenSvg.Optimization;
using System.Collections.Immutable;

namespace OpenSvg.Netex;

public class NetexShared
{
    public XDocument XDocument { get; private set; }

    public NetexShared(string netexXmlFile)
    {
        bool runOptimization = true;

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


        Coordinate[][] coordinates = posLists.Select(ParsePointList).Select(list => list.Where(c => IsWithinBoundingBox(c)).ToArray()).ToArray();

        GeoBoundingBox geoBoundingBox = new GeoBoundingBox(coordinates.SelectMany(c => c));

        //var metersPerPixels = PointConverter.MetersPerPixels(1000, geoBoundingBox);
        PointConverter converter = new PointConverter(geoBoundingBox, 1000, 10);
        Console.WriteLine("Meters per pixel: " + converter.MetersPerPixel);

        FastPolyline[] polylines = coordinates.Where(c => c.Count() >= 2).Select(c => new FastPolyline(c.Select(converter.ToPoint))).ToArray();

        Console.WriteLine("Polylines with duplicated points: " + polylines.Count(p => p.HasDuplicatedPoints()));


        polylines = polylines.Select(p => p.RemoveEquivalentAdjacentPoints()).ToArray();
        Console.WriteLine("Polylines with duplicated points: " + polylines.Count(p => p.HasDuplicatedPoints()));


        Console.WriteLine("Polylines count " + polylines.Count());

        int DEBUG_uniquePointCount = polylines.SelectMany(p => p.Points).Distinct().Count();
        Console.WriteLine("DEBUG Original unique points: " + DEBUG_uniquePointCount);


        Console.WriteLine("Creating line set");
        LineSet lineSet = new LineSet(polylines);

        Console.WriteLine("Lines: " + lineSet.ByMinPoint.Count);
   
        int DEBUG_lineSetPointCount = lineSet.ByMinPoint.Select(l => l.MinPoint).Distinct().Count();
        
        Console.WriteLine("DEBUG LineSet NON-OPTIMIZED unique points: " + lineSet.TotalPointCount());
        if (runOptimization)
            lineSet.Optimize();

        Console.WriteLine("DEBUG LineSet OPTIMIZED unique points: " + lineSet.TotalPointCount());


        FastPolyline[] optimizedLines = lineSet.OptimizedPolylines.ToArray();
        Console.WriteLine("Optimized lines: " + optimizedLines.Count());

        FastPolyline[] nonOptimizedLines = lineSet.NonOptimizedPolylines.ToArray();
        Console.WriteLine("Non-Optimized lines: " + nonOptimizedLines.Count());
        //*** Generate SVG shapes ***//

        SKColor[] colors = ColorHelper.GetBrightDistinctColors();

        int index = 0;

        SvgGroup lineGroup = new SvgGroup();
        lineGroup.FillColor = SKColors.Transparent;
        lineGroup.StrokeWidth = 0.03f;
        foreach (FastPolyline polyline in lineSet.NonOptimizedPolylines)
        {
            SvgPolyline svgPolyline = polyline.ToSvgPolyline();
            svgPolyline.StrokeColor = SKColors.DarkGray;
            lineGroup.ChildElements.Add(svgPolyline);

        }

        foreach (FastPolyline polyline in optimizedLines)
        {
            SvgPolyline svgPolyline = polyline.ToSvgPolyline();
            svgPolyline.StrokeColor = colors[index % colors.Length];
            lineGroup.ChildElements.Add(svgPolyline);
            index++;
        }

   


        SvgGroup stopGroup = new SvgGroup();
        stopGroup.FillColor = SKColors.Transparent;
        stopGroup.StrokeWidth = 0;
        foreach (Point endPoint in lineSet.AddedEndPoints)
        {
            var svgShape = new SvgCircle() { Center = endPoint, Radius = 0.2f, FillColor = SKColors.Purple };
            stopGroup.ChildElements.Add(svgShape);
        }
        foreach (Point endPoint in lineSet.OriginalEndPoints)
        {
            var svgShape = new SvgCircle() { Center = endPoint, Radius = 0.2f, FillColor = SKColors.Yellow };
            stopGroup.ChildElements.Add(svgShape);
        }
     


        SvgDocument svgDocument = new SvgDocument();
        SvgRectangleAsRect backgroundFill = new SvgRectangleAsRect();
        backgroundFill.FillColor = SKColors.Black;
        backgroundFill.DefinedWidth = AbsoluteOrRatio.Ratio(1);
        backgroundFill.DefinedHeight = AbsoluteOrRatio.Ratio(1);
        svgDocument.Add(backgroundFill);
        svgDocument.Add(lineGroup);
        svgDocument.Add(stopGroup);
       


        svgDocument.SetViewBoxToActualSizeAndDefaultViewPort();
        svgDocument.Save($@"D:\Downloads\Test\Netex\{(runOptimization ? "2" : "1")}.svg");

    }

   
     
    static bool IsWithinBoundingBox(Coordinate coordinate)
    {
        
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

        Coordinate[] coordinates = new Coordinate[count];
        for (int i = 0; i < count; i+=2)
        {
            double latitude = double.Parse(coordinatesText[i], CultureInfo.InvariantCulture);
            double longitude = double.Parse(coordinatesText[i + 1], CultureInfo.InvariantCulture);
            coordinates[i] = new Coordinate(longitude, latitude);
        }

        return coordinates;
    }
}
