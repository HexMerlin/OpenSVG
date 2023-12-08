using OpenSvg.Gtfs;
using OpenSvg.SvgNodes;
using OpenSvg.Netex;
using System.Xml.Linq;
using System.Numerics;
using System.Collections.Immutable;
using OpenSvg.GeoJson;
using OpenSvg.Config;
using OpenSvg.Optimization;
namespace OpenSvg.ConsoleTest;

internal class Program
{
 
    private static void SaveSvg(FastPolyline polyline, int index)
    {
        var svgPolyline = polyline.ToSvgPolyline();
        svgPolyline.StrokeColor = SkiaSharp.SKColors.Red;
        svgPolyline.StrokeWidth = 0.2f;
        svgPolyline.FillColor = SkiaSharp.SKColors.Transparent;
        var doc1 = svgPolyline.ToSvgDocument();
        doc1.SetViewBoxToActualSizeAndDefaultViewPort();

        doc1.Save($@"D:\Downloads\Polyline{index}.svg");
    }

    public static void Main()
    {
        float[] angles = new float[] { 20, 15, -175, 1, 175, 40, 40, 40, -20, -20, -20 };

        var polyline = FastPolyline.CreatePolyline(new Point(100, 100), angles, 10);
        SaveSvg(polyline, 1);
        polyline = polyline.RemoveSharpTurns();
        SaveSvg(polyline, 2);
       
        return;

        //var svgRect = new SvgRectangleAsRect();
        //svgRect.X = 10;
        //svgRect.Y = 10;
        //svgRect.DefinedWidth = AbsoluteOrRatio.Ratio(1);
        //var doc = svgRect.ToSvgDocument();
        //doc.Save(@"D:\Downloads\Test\rect.svg");
        //return;

      //  var topLeft = new Coordinate(9.437420, 61.310728);
      //  var bottomRight = new Coordinate(45.784249, 8.150650);
      //  var topLeftWebMercator = topLeft.ToWebMercator();

      ////  var bottomLeftWebMercator = new Coordinate(topLeft.Long, bottomRight.Lat).ToWebMercator();
      //  var bottomLeftWebMercator = new Coordinate(topLeft.Long, bottomRight.Lat).ToWebMercator();

    

      //  var bottomRightWebMercator = bottomRight.ToWebMercator();
      //  Console.WriteLine("Top-left WGS84           :  " + topLeft);
      //  Console.WriteLine("Bottom-right WGS84       :  " + bottomRight);

      //  Console.WriteLine("Top-Left web mercator    : " + topLeftWebMercator.ToString());
      //  Console.WriteLine("Bottom-right web mercator: " + bottomRightWebMercator.ToString());

      //  Console.WriteLine("Bottom-left web mercator: " + bottomLeftWebMercator.ToString());

      // // GeoBoundingBox box = new GeoBoundingBox(new Point[] { topLeftWebMercator, bottomRight });
      //  var converter = new PointConverter(box, 1000, 10);
      //  Console.WriteLine(converter.MetersPerPixel);
      //  Console.WriteLine(1d / converter.MetersPerPixel);
      //  var topLeftPoint = converter.ToPoint(topLeft);
      //  var bottomRightPoint = converter.ToPoint(bottomRight);
      //  Polygon polygon = new Polygon([topLeftPoint,
      //      new Point(bottomRightPoint.X, topLeftPoint.Y),
      //      bottomRightPoint,
      //      new Point(topLeftPoint.X, bottomRightPoint.Y)
      //                    ]);
      //  var svgPolygon = polygon.ToSvgPolygon();
      //  svgPolygon.StrokeColor = SkiaSharp.SKColors.Red;
      //  svgPolygon.StrokeWidth = 1;
      //  svgPolygon.FillColor = SkiaSharp.SKColors.Red;
      //  var svgDoc = svgPolygon.ToSvgDocument();
      //  svgDoc.SetViewBoxToActualSizeAndDefaultViewPort();
      //  svgDoc.Save(@"D:\Downloads\Test\temp.svg");
      //  return;


        NetexShared netexShared = new NetexShared(@"D:\Downloads\Test\Netex\skane\_shared_data.xml");
        //XDocument xDoc = netexShared.XDocument;
        //List<XElement> posLists = xDoc.Descendants().Where(e => e.Name.LocalName == "posList").ToList();

        
        //Console.WriteLine(posLists.Count());

        return;

        GtfsFeed gtfs = GtfsFeed.Load(@"D:\Downloads\Test\GTFS\skane.zip");
        gtfs = gtfs.CreateFiltered();

        //gtfsFeed.JoinDataSources();

       // AllPoints allPoints = new AllPoints(gtfsFeed);

        SvgDocument svgDocument = gtfs.ToSvgDocument();
        svgDocument.Save(@"D:\Downloads\Test\GTFS\skane.svg");

        Console.WriteLine("Real stops: " + gtfs.RealStops.Count());
        Console.WriteLine("Real stops with traffic: " + gtfs.RealStopsWithTraffic.Count());
        Console.WriteLine("Real stops with traffic and distance traveled: " + gtfs.RealStopsWithTraffic.Where(s => s.HasDistTraveled).Count());

        //GeoJsonDocument geoJsonDocument1 = GeoJsonDocument.Load(@"D:\Downloads\Test\legend.geojson");
        //GeoJsonDocument geoJsonDocument2 = GeoJsonDocument.Load(@"D:\Downloads\Test\otraf.geojson");
        //SvgDocument svgDocument1 = geoJsonDocument1.ToSvgDocument();
        //SvgDocument svgDocument2 = geoJsonDocument2.ToSvgDocument();

        //svgDocument1.Save(@"D:\Downloads\Test\legend.svg");
        //svgDocument2.Save(@"D:\Downloads\Test\otraf.svg");



    }
}