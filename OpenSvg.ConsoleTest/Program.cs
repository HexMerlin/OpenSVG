using OpenSvg.Config;
using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;
using SkiaSharp;

namespace OpenSvg.ConsoleTest;

internal class Program
{

    public static void Main()
    {

        GeoJsonDocument geoJsonDocument1 = GeoJsonDocument.Load(@"D:\Downloads\Test\legend.geojson");
        GeoJsonDocument geoJsonDocument2 = GeoJsonDocument.Load(@"D:\Downloads\Test\otraf.geojson");
        SvgDocument svgDocument1 = geoJsonDocument1.ToSvgDocument();
        SvgDocument svgDocument2 = geoJsonDocument2.ToSvgDocument();

        svgDocument1.Save(@"D:\Downloads\Test\legend.svg");
        svgDocument2.Save(@"D:\Downloads\Test\otraf.svg");


        //GeoJsonDocument geoJsonDocument2 = GeoJsonDocument.Load(@"D:\Downloads\Test\otraf.geojson");
        //SvgDocument svgDocument2 = geoJsonDocument2.ToSvgDocument();

        //svgDocument2.Save(@"D:\Downloads\Test\otraf.svg");

    }
}