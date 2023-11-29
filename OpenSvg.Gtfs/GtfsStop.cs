using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;
using SkiaSharp;
using System.Security.Permissions;

namespace OpenSvg.Gtfs;
public record GtfsStop(string StopID, string Name, Coordinate Coordinate, int LocationType = 0, string ParentStation = "", string PlatformCode = "")
{
    public SortedDictionary<string, (GtfsShape gtfsShape, Coordinate? coordByDistTraveled, Coordinate coordByCloseness)> Shapes { get; } = new();

    public void AddShape(GtfsShape shape, Coordinate? coordByDistTraveled, Coordinate coordByCloseness)
    {
        Shapes[shape.ID] = (shape, coordByDistTraveled, coordByCloseness);
    }


    public SvgVisual ToSvgShape(PointConverter converter)
    {
        var rectDim = 0.1f;
        var center = converter.ToPoint(this.Coordinate);
        var up = new Point(center.X, center.Y - rectDim);
        var right = new Point(center.X + rectDim, center.Y);
        var down = new Point(center.X, center.Y + rectDim);
        var left = new Point(center.X - rectDim, center.Y);
        Polygon polygon = new Polygon(new Point[] { up, right, down, left });
        SvgPolygon svgPolygon = new SvgPolygon();
        svgPolygon.Polygon = polygon;

        svgPolygon.StrokeColor = SKColors.Blue;

        SvgGroup svgGroup = new SvgGroup();
        svgGroup.ID = this.StopID;
        svgGroup.Add(svgPolygon);

        IEnumerable<Point> coordsByDistTraveled = Shapes.Values.Where(s => s.coordByDistTraveled != null).Select(c => converter.ToPoint((Coordinate)c.coordByDistTraveled.Value));
        IEnumerable<Point> coordByCloseness = Shapes.Values.Select(c => converter.ToPoint(c.coordByCloseness));
        
        foreach (var stopPoint in coordsByDistTraveled)
        {
            SvgLine svgLine = new SvgLine();
            svgLine.P1 = center;
            svgLine.P2 = stopPoint;
            svgLine.StrokeColor = SKColors.DarkGreen;
            svgLine.StrokeWidth = 0.2f;
            svgLine.FillColor = SKColors.DarkMagenta;
            svgGroup.Add(svgLine);
        }

        foreach (var stopPoint in coordByCloseness)
        {
            SvgLine svgLine = new SvgLine();
            svgLine.P1 = center;
            svgLine.P2 = stopPoint;
            svgLine.StrokeColor = SKColors.DarkGreen;
            svgLine.StrokeWidth = 0.2f;
            svgLine.FillColor = SKColors.DarkRed;
            svgGroup.Add(svgLine);
        }

        return svgGroup;
    }

}
