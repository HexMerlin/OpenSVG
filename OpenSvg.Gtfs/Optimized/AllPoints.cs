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
        var geoBoundingBox = gtfsFeed.ComputeGeoBoundingBox();

        var topLeftCoordinate = geoBoundingBox.TopLeft;
       // double metersPerPixel = PointConverter.MetersPerPixels(1, geoBoundingBox);
        
        this.converter = new PointConverter(topLeftCoordinate, 1, 10);

        Console.WriteLine("metersPerPixel " + converter.MetersPerPixel);
        Console.WriteLine("pixels per meter: " + (1.0d / converter.MetersPerPixel));


        points = new SortedSet<Point>();

        AddAll(gtfsFeed.Shapes);
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
        var coordinates = shapes.SelectMany(s => s.ShapePoints).Select(sp => sp.Coordinate).ToArray();
        AddAll(coordinates.Select(c => converter.ToPoint(c)).Select(p => new Point((int) p.X, (int) p.Y)));

        Console.WriteLine("Total coordinates: " + coordinates.Length);
        Console.WriteLine("Total points: " + points.Count);
    }
}
