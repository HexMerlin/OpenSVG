using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System.IO.Compression;
using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;
using SkiaSharp;

namespace OpenSvg.Gtfs;

public static class ShapesParser
{

    public static IEnumerable<GtfsShape> Read(ZipArchiveEntry shapeEntry) 
        => ParseShapePoints(shapeEntry)
               .GroupBy(sp => sp.ID)
               .Select(group => new GtfsShape(group.Key, group.OrderBy(sp => sp.Sequence)));


    private static IEnumerable<GtfsShapePoint> ParseShapePoints(ZipArchiveEntry shapeEntry)
    {
        using Stream shapeStream = shapeEntry.Open();
        using var parser = new TextFieldParser(shapeStream);
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;

        if (!parser.EndOfData) parser.ReadLine();

      
        while (!parser.EndOfData)
        {
            string[]? fields = parser.ReadFields();
            if (fields == null) continue;

            string shapeId = fields.Length > 0 ? fields[0] : string.Empty;
            float latitude = fields.Length > 1 ? fields[1].ParseNumber<float>() : 0;
            float longitude = fields.Length > 2 ? fields[2].ParseNumber<float>() : 0;
            int shape_pt_sequence = fields.Length > 3 ? fields[3].ParseNumber<int>() : 0;
            float shape_dist_traveled = fields.Length > 4 ? fields[4].ParseNumber<float>() : -1;
            yield return new GtfsShapePoint(shapeId, new Coordinate(longitude, latitude), shape_pt_sequence, shape_dist_traveled);

        }
    }

    public static SvgGroup ToSvgGroup(this IEnumerable<GtfsShape> shapes, PointConverter converter)
    {
        IEnumerable<SvgVisual> svgShapeElements = shapes.Select(s => s.ToSvgShape(converter));

        SvgGroup svgGroup = new SvgGroup();
        svgGroup.ID = "shapes";
        svgGroup.StrokeColor = SKColors.DarkRed;
        svgGroup.StrokeWidth = 0.1f;
        svgGroup.FillColor = SKColors.DarkRed;
        svgGroup.AddAll(svgShapeElements);
        return svgGroup;

    }



}
