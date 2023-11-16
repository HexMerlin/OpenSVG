using OpenSvg.GeoJson;

namespace OpenSvg.ConsoleTest;

internal class Program
{

    public static void Main()
    {
        GeoJsonDocument geoJsonDocument = GeoJsonDocument.Load(@"D:\Downloads\test.geojson");
        Console.WriteLine(geoJsonDocument.ToString());
    }
}