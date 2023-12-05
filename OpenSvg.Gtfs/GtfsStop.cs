using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;
using SkiaSharp;

namespace OpenSvg.Gtfs;
public record GtfsStop(string StopID, string Name, Coordinate Coordinate, int LocationType = 0, string ParentStation = "", string PlatformCode = "")
{
    public bool HasTraffic { get; set; } = false;

    public bool HasDistTraveled { get; set; } = false;

    public SortedDictionary<string, (GtfsShape shape, GtfsStopTime stopTime)> Shapes { get; } = new();

    public void AssociateWith(GtfsShape shape, GtfsStopTime stopTime)
    {
        Shapes[shape.ID] = (shape, stopTime);

    }


    public SvgVisual ToSvgShape(PointConverter converter)
    {
        //var rectDim = 0.05f;
        //var center = converter.ToPoint(this.Coordinate);
        //var up = new Point(center.X, center.Y - rectDim);
        //var right = new Point(center.X + rectDim, center.Y);
        //var down = new Point(center.X, center.Y + rectDim);
        //var left = new Point(center.X - rectDim, center.Y);
        //Polygon polygon = new Polygon(new Point[] { up, right, down, left });
        //SvgPolygon svgPolygon = new SvgPolygon();
        //svgPolygon.Polygon = polygon;
        //return svgPolygon;
        SvgCircle svgCircle = new SvgCircle();
        svgCircle.Center = converter.ToPoint(this.Coordinate);
         svgCircle.Radius = 1.0f;
       // svgCircle.FillColor = SKColors.Black;
        return svgCircle;

        //IEnumerable<Point> coordsByDistTraveled = Shapes.Values.Where(s => s.coordByDistTraveled != null).Select(c => converter.ToPoint((Coordinate)c.coordByDistTraveled.Value));
        //IEnumerable<Point> coordByCloseness = Shapes.Values.Select(c => converter.ToPoint(c.coordByCloseness));
        
        //foreach (var stopPoint in coordsByDistTraveled)
        //{
        //    SvgLine svgLine = new SvgLine();
        //    svgLine.P1 = center;
        //    svgLine.P2 = stopPoint;
        //    svgLine.StrokeColor = SKColors.DarkGreen;
        //    svgLine.StrokeWidth = 0.2f;
        //    svgLine.FillColor = SKColors.DarkMagenta;
        //    svgGroup.Add(svgLine);
        //}

        //foreach (var stopPoint in coordByCloseness)
        //{
        //    SvgLine svgLine = new SvgLine();
        //    svgLine.P1 = center;
        //    svgLine.P2 = stopPoint;
        //    svgLine.StrokeColor = SKColors.DarkGreen;
        //    svgLine.StrokeWidth = 0.2f;
        //    svgLine.FillColor = SKColors.DarkRed;
        //    svgGroup.Add(svgLine);
        //}

    }

}
