using OpenSvg.Gtfs;
using OpenSvg.SvgNodes;
using OpenSvg.Netex;
using System.Xml.Linq;
using System.Numerics;
using System.Collections.Immutable;

namespace OpenSvg.ConsoleTest;

internal class Program
{
 

    public static void Main()
    {
       
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