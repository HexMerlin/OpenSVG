using Microsoft.VisualBasic.FileIO;
using OpenSvg.Geographics;
using OpenSvg.SvgNodes;
using SkiaSharp;
using System.IO.Compression;

namespace OpenSvg.Gtfs;

public static class StopsParser
{

    public static IEnumerable<GtfsStop> Read(ZipArchiveEntry stopEntry)
    {
     
        using Stream stopStream = stopEntry.Open();
        using var parser = new TextFieldParser(stopStream);
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;

        if (!parser.EndOfData) parser.ReadLine();

        while (!parser.EndOfData)
        {
            string[]? fields = parser.ReadFields();
            if (fields == null) continue;

            string stop_id = fields.Length > 0 ? fields[0] : string.Empty;
            string stop_name = fields.Length > 1 ? fields[1] : string.Empty;
            float latitude = fields.Length > 2 ? fields[2].ParseNumber<float>() : 0;
            float longitude = fields.Length > 3 ? fields[3].ParseNumber<float>() : 0;
            int locationType = fields.Length > 4 ? fields[4].ParseNumber<int>() : 0;
            string parentStation = fields.Length > 5 ? fields[5] : string.Empty;
            string platformCode = fields.Length > 6 ? fields[6] : string.Empty;

            yield return new GtfsStop(stop_id, stop_name, new Coordinate(longitude, latitude), locationType, parentStation, platformCode);

        }
    }

    public static SvgGroup ToSvgGroup(this IEnumerable<GtfsStop> stops, PointConverter converter)
    {
        IEnumerable<SvgVisual> svgShapeElements = stops.Select(s => s.ToSvgShape(converter));
        SvgGroup svgGroup = new SvgGroup();
        svgGroup.ID = "stops";
        svgGroup.StrokeColor = SKColors.Blue;
        svgGroup.AddAll(svgShapeElements);
        return svgGroup;

    }

    //public static SvgVisual ToSvgShape(this GtfsStop gtfsStop, PointConverter converter)
    //{
    //    SvgCircle svgCircle = new SvgCircle();
    //    svgCircle.ID = gtfsStop.StopID;
    //    svgCircle.Center = converter.ToPoint(gtfsStop.Coordinate);
    //    svgCircle.Radius = 0.2f;
    //    return svgCircle;
    //}
}
