using OpenSvg.GeoJson;
using SkiaSharp;

namespace OpenSvg.ConsoleTest;

internal class Program
{

    public static void Main()
    {
        GeoJsonDocument geoJsonDocument = GeoJsonDocument.Load(@"D:\Downloads\legend.geojson");

        var svgDocument = geoJsonDocument.ToSvgDocument();
        svgDocument.Save(@"D:\Downloads\test.svg");
     
    }
}