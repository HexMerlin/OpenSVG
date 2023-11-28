using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenSvg;
using OpenSvg.GeoJson;

namespace OpenSvg.Gtfs.Optimized;
public class AllPoints
{

    SortedSet<Point> points;

    PointConverter converter;

    private AllPoints()
    {
        points = new SortedSet<Point>();
    }

    public AllPoints(GtfsFeed gtfsFeed) : this()
    {
       // var geoBoundingBox = gtfsFeed.ComputeGeoBoundingBox();

       // var topLeftCoordinate = geoBoundingBox.TopLeft;
       //// float metersPerPixel = PointConverter.MetersPerPixels(1, geoBoundingBox);
        
       // this.converter = new PointConverter(topLeftCoordinate, 1, 10);

       // Console.WriteLine("metersPerPixel " + converter.MetersPerPixel);
       // Console.WriteLine("pixels per meter: " + (1.0d / converter.MetersPerPixel));


       // points = new SortedSet<Point>();

       // AddAll(gtfsFeed.Shapes);
    }

    public void Add(Point point)
    {
        this.points.Add(point);
    }

    public void AddAll(IEnumerable<Point> points)
    {
        this.points.UnionWith(points);
    }

    public void AddAll(ImmutableArray<GtfsShape> shapes)
    {

        foreach (var shape in shapes)
        {
            Point[] shapePoints = shape.ShapePoints.Select(sp => sp.Coordinate).Select(sp => converter.ToPoint(sp)).ToArray();
           // Console.WriteLine(shapePoints.Length);

            int existCount = shapePoints.Count(sp => points.Contains(sp));
            Console.WriteLine(existCount + " / " + shapePoints.Length);
            if (existCount == 0)
            {
                AddAll(shapePoints);
            }
        }
        Console.WriteLine("Kept points: " + points.Count());
    }
}
