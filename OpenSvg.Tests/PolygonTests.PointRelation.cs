namespace OpenSvg.Tests;

public partial class PolygonTests
{
    [Fact]
    public void Test_PointInsidePolygon_ReturnsInside()
    {
        var polygon = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 10),
            new Point(0, 10)
        });

        var point = new Point(5, 5);
        Assert.Equal(PointRelation.Inside, polygon.RelationTo(point));
    }

    [Fact]
    public void Test_PointOutsidePolygon_ReturnsDisjoint()
    {
        var polygon = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 10),
            new Point(0, 10)
        });

        var point = new Point(15, 15);
        Assert.Equal(PointRelation.Disjoint, polygon.RelationTo(point));
    }

    [Fact]
    public void Test_PointOnPolygonEdge_ReturnsInside()
    {
        var polygon = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 10),
            new Point(0, 10)
        });

        var point = new Point(10, 5);
        Assert.Equal(PointRelation.Inside, polygon.RelationTo(point));
    }

    [Fact]
    public void Test_PointOnPolygonVertex_ReturnsInside()
    {
        var polygon = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 10),
            new Point(0, 10)
        });

        var point = new Point(0, 0); // Vertex of the polygon
        Assert.Equal(PointRelation.Inside, polygon.RelationTo(point));
    }

    [Fact]
    public void Test_PointOnNonRectangularPolygonEdge_ReturnsInside()
    {
        var polygon = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 5),
            new Point(5, 5),
            new Point(5, 10),
            new Point(0, 10)
        });

        var point = new Point(5, 7.5); // Lies on the vertical edge
        Assert.Equal(PointRelation.Inside, polygon.RelationTo(point));
    }

    [Fact]
    public void Test_PointInsideConcavePolygon_ReturnsInside()
    {
        var polygon = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 10),
            new Point(5, 5), // This makes the polygon concave
            new Point(0, 10)
        });

        var point = new Point(6, 6); // Inside the concave part
        Assert.Equal(PointRelation.Inside, polygon.RelationTo(point));
    }

    [Fact]
    public void Test_PointOutsideConcavePolygon_ReturnsDisjoint()
    {
        var polygon = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 10),
            new Point(5, 5), // This makes the polygon concave
            new Point(0, 10)
        });

        var point = new Point(12, 12); // Outside the polygon
        Assert.Equal(PointRelation.Disjoint, polygon.RelationTo(point));
    }

    [Fact]
    public void Test_PointInRelationToComplexPolygon_ReturnsExpectedResult()
    {
        var polygon = new Polygon(Enumerable.Range(0, 1000).Select(i => new Point(Math.Cos(i), Math.Sin(i))));

        var pointInside = new Point(0, 0); // Center of the complex polygon
        Assert.Equal(PointRelation.Inside, polygon.RelationTo(pointInside));

        var pointOutside = new Point(2, 2); // Outside the complex polygon
        Assert.Equal(PointRelation.Disjoint, polygon.RelationTo(pointOutside));
    }
}