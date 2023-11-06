using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;

namespace OpenSvg.Tests.GeoJson;

public class GeoJsonTestHelper
{
    public static void GenerateTestGeoJsonFile(string filePath)
    {
        SvgImageConfig config = new SvgImageConfig();
        SvgDocument svgDocument = config.CreateSvgDocument();
        Coordinate startLocation = new Coordinate(15.575, 58.416);
        GeoJsonDocument geoJsonDocument = new GeoJsonDocument(svgDocument, startLocation);
        geoJsonDocument.SaveToGeoJsonFile(filePath);
    }
}
