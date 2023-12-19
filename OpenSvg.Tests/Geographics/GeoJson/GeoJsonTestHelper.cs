using OpenSvg.Geographics;
using OpenSvg.Geographics.GeoJson;
using OpenSvg.SvgNodes;

namespace OpenSvg.Tests.Geographics.GeoJson;

public class GeoJsonTestHelper
{
    public static void GenerateTestGeoJsonFile(string filePath)
    {
        SvgImageConfig config = new SvgImageConfig();
        SvgDocument svgDocument = config.CreateSvgDocument();
        Coordinate startLocation = new Coordinate(15.575f, 58.416f);
        GeoJsonDocument geoJsonDocument = 
            new GeoJsonDocument(svgDocument, startLocation, metersPerPixel:1, segmentCountForCurveApproximation: 8);
        geoJsonDocument.Save(filePath);
    }
}
