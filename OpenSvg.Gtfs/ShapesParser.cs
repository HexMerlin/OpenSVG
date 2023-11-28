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
            int sequence = fields.Length > 3 ? fields[3].ParseNumber<int>() : 0;
            float distance = fields.Length > 4 ? fields[4].ParseNumber<float>() : 0;
            yield return new GtfsShapePoint(shapeId, new Coordinate(longitude, latitude), sequence, distance);

        }
    }

    public static SvgGroup ToSvgGroup(this IEnumerable<GtfsShape> shapes, PointConverter converter)
    {
        IEnumerable<SvgVisual> svgShapeElements = shapes.Select(s => s.ToSvgShape(converter));

        SvgGroup svgGroup = new SvgGroup();
        svgGroup.ID = "shapes";
        svgGroup.StrokeColor = SKColors.DarkRed;
        svgGroup.StrokeWidth = 0.1f;
        svgGroup.FillColor = SKColors.Transparent;
        svgGroup.AddAll(svgShapeElements);
        return svgGroup;

    }

    public static SvgVisual ToSvgShape(this GtfsShape gtfsShape, PointConverter converter)
    {
        IEnumerable<Point> points = gtfsShape.ShapePoints.Select(sp => converter.ToPoint(sp.Coordinate));
        Polyline polyline = new Polyline(points);
        //SvgPath svgVisual = polyline.ToPath().ToSvgPath();

        SvgPolyline svgVisual = polyline.ToSvgPolyline();
        svgVisual.ID = gtfsShape.ID;
        return svgVisual;
    }


}
