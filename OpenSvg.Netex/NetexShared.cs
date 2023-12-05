using System.Globalization;
using System.Xml.Linq;
using OpenSvg;
using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;
using SkiaSharp;

namespace OpenSvg.Netex;

public class NetexShared
{
    public XDocument XDocument { get; private set; }

    public NetexShared(string netexXmlFile)
    {
        static bool IsBus(XElement pointList)
            => pointList.Parent.Parent.Attributes("id").Any(a => a.Value.Contains("BUS"));

        this.XDocument = XDocument.Load(netexXmlFile);
        //XDocument.Save(@"D:\Downloads\Test\Netex\NetexShared2.xml");

        List<XElement> posLists = XDocument.Descendants().Where(e => e.Name.LocalName == "posList").Where(e => IsBus(e)).ToList();
        Console.WriteLine("All posLists: " + posLists.Count());
        //var ids = posLists.Select(e => e.Parent.Attributes("id").Select(a => a.Value)).Where(a => a != null).ToList();
        //foreach (var id in ids.Take(20))
        //{
        //    Console.WriteLine("Id: " + id);
        //}


        Coordinate[][] coordinates = posLists.Select(ParsePointList).Select(list => list.Where(c => IsWithinBoundingBox(c)).ToArray()).ToArray();

        GeoBoundingBox geoBoundingBox = new GeoBoundingBox(coordinates.SelectMany(c => c));

        var metersPerPixels = PointConverter.MetersPerPixels(1000, geoBoundingBox);
        PointConverter converter = new PointConverter(geoBoundingBox.TopLeft, metersPerPixels, 10);
        FastPolyline[] polylines = coordinates.Where(c => c.Count() >=2).Select(c => new FastPolyline(c.Select(converter.ToPoint))).ToArray();

        HashSet<Point> endPoints = GetEndPoints(polylines);

  
        Console.WriteLine("End point count " + endPoints.Count());
        Console.WriteLine("Polylines count " + polylines.Count());

        PolylineSet polylineSet = new PolylineSet(polylines);
      
        Console.WriteLine("Polylines count after hashing " + polylineSet.Count);
        polylineSet.Optimize(); 
        Console.WriteLine("Polylines count after optimization " + polylineSet.Count);
        //*** Generate SVG shapes ***//
              
        SKColor[] colors = ColorHelper.GetDistinctColors();

        int index = 0;

        SvgGroup lineGroup = new SvgGroup();
        foreach (FastPolyline polyline in polylineSet.Polylines)
        {
            SvgPolyline svgPolyline = polyline.ToSvgPolyline();
            svgPolyline.StrokeColor = colors[index % colors.Length];
            lineGroup.ChildElements.Add(svgPolyline);
            index++;

        }
   

        SvgGroup stopGroup = new SvgGroup();
        foreach (Point endPoint in endPoints)
        {
            var svgShape = new SvgCircle() { Center = endPoint, Radius = 1f, FillColor = SKColors.Red };
            stopGroup.ChildElements.Add(svgShape);
        }

    

        SvgDocument svgDocument = new SvgDocument();
        svgDocument.Add(stopGroup);
        svgDocument.Add(lineGroup);

       // svgDocument.StrokeColor = SKColors.Black;
        svgDocument.FillColor = SKColors.Transparent;
        svgDocument.StrokeWidth = 0.5f;
        svgDocument.SetViewBoxToActualSizeAndDefaultViewPort();
        svgDocument.Save(@"D:\Downloads\Test\Netex\NetexShared.svg");

    }

    private HashSet<Point> GetEndPoints(FastPolyline[] polylines)
    {
        HashSet<Point> endPoints = new HashSet<Point>();
        endPoints.UnionWith(polylines.Select(p => p.Points[0]));
        endPoints.UnionWith(polylines.Select(p => p.Points[^1]));
        return endPoints;
    }
     
    static bool IsWithinBoundingBox(Coordinate coordinate)
    {
        var topLeft = new Coordinate(12.873657850104392, 55.64814360262774);
        var bottomRight = new Coordinate(13.068633658027395, 55.56195788858898);
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
