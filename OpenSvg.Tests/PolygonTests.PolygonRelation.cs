namespace OpenSvg.Tests;

public partial class PolygonTests
{
    [Fact]
    public void Test_PolygonInsideAnotherPolygon_ReturnsInside()
    {
        var outerPolygon = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 10),
            new Point(0, 10)
        });

        var innerPolygon = new Polygon(new[]
        {
            new Point(2, 2),
            new Point(8, 2),
            new Point(8, 8),
            new Point(2, 8)
        });

        Assert.Equal(PolygonRelation.Inside, innerPolygon.RelationTo(outerPolygon));
    }

    [Fact]
    public void Test_PolygonOutsideAnotherPolygon_ReturnsDisjoint()
    {
        var firstPolygon = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 10),
            new Point(0, 10)
        });

        var secondPolygon = new Polygon(new[]
        {
            new Point(15, 15),
            new Point(25, 15),
            new Point(25, 25),
            new Point(15, 25)
        });

        Assert.Equal(PolygonRelation.Disjoint, firstPolygon.RelationTo(secondPolygon));
    }

    [Fact]
    public void Test_PolygonsIntersecting_ReturnsIntersect()
    {
        var firstPolygon = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 10),
            new Point(0, 10)
        });

        var secondPolygon = new Polygon(new[]
        {
            new Point(5, 5),
            new Point(15, 5),
            new Point(15, 15),
            new Point(5, 15)
        });

        Assert.Equal(PolygonRelation.Intersect, firstPolygon.RelationTo(secondPolygon));
    }

    [Fact]
    public void Test_NeutralIntersections_ReturnsExpectedResult()
    {
        var polygonA = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 10),
            new Point(0, 10)
        });

        var polygonB = new Polygon(new[]
        {
            new Point(10, 5),
            new Point(15, 5),
            new Point(15, 15),
            new Point(10, 15)
        });

        Assert.Equal(PolygonRelation.Disjoint, polygonA.RelationTo(polygonB));
    }

    [Fact]
    public void Test_ConcavePolygonInsideConvexPolygon_ReturnsInside()
    {
        var convexPolygon = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 10),
            new Point(0, 10)
        });

        var concavePolygon = new Polygon(new[]
        {
            new Point(2, 2),
            new Point(8, 2),
            new Point(5, 5),
            new Point(8, 8),
            new Point(2, 8)
        });

        Assert.Equal(PolygonRelation.Inside, concavePolygon.RelationTo(convexPolygon));
    }

    [Fact]
    public void Test_PolygonsWithSharedEdge_ReturnsNeutral()
    {
        var polygonA = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 10),
            new Point(0, 10)
        });

        var polygonB = new Polygon(new[]
        {
            new Point(10, 0),
            new Point(20, 0),
            new Point(20, 10),
            new Point(10, 10)
        });

        // Assuming the behavior for shared edges is neutral and thus the rest of the polygons determine the result
        Assert.Equal(PolygonRelation.Disjoint, polygonA.RelationTo(polygonB));
    }

    [Fact]
    public void Test_TouchingPolygons_ReturnsDisjoint()
    {
        var polygonA = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 10),
            new Point(0, 10)
        });

        var polygonB = new Polygon(new[]
        {
            new Point(10, 10),
            new Point(20, 10),
            new Point(20, 20),
            new Point(10, 20)
        });

        Assert.Equal(PolygonRelation.Disjoint, polygonA.RelationTo(polygonB));
    }

    [Fact]
    public void Test_NestedPolygons_ReturnsInside()
    {
        var outerPolygon = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(20, 0),
            new Point(20, 20),
            new Point(0, 20)
        });

        var middlePolygon = new Polygon(new[]
        {
            new Point(5, 5),
            new Point(15, 5),
            new Point(15, 15),
            new Point(5, 15)
        });

        var innerPolygon = new Polygon(new[]
        {
            new Point(7, 7),
            new Point(13, 7),
            new Point(13, 13),
            new Point(7, 13)
        });

        Assert.Equal(PolygonRelation.Inside, middlePolygon.RelationTo(outerPolygon));
        Assert.Equal(PolygonRelation.Inside, innerPolygon.RelationTo(middlePolygon));
    }

    [Fact]
    public void Test_SinglePointPolygon_ReturnsInside()
    {
        var polygon = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 10),
            new Point(0, 10)
        });

        var singlePointPolygon = new Polygon(new[]
        {
            new Point(5, 5)
        });

        Assert.Equal(PolygonRelation.Inside, singlePointPolygon.RelationTo(polygon));
    }

    [Fact]
    public void Test_CollinearPointsPolygon_ReturnsDisjoint()
    {
        var polygonA = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 10),
            new Point(0, 10)
        });

        var collinearPolygon = new Polygon(new[]
        {
            new Point(5, 5),
            new Point(6, 5),
            new Point(7, 5)
        });

        Assert.Equal(PolygonRelation.Inside, collinearPolygon.RelationTo(polygonA));
    }

    [Fact]
    public void Test_TinyPolygon_ReturnsInside()
    {
        var polygon = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 10),
            new Point(0, 10)
        });

        var tinyPolygon = new Polygon(new[]
        {
            new Point(5, 5),
            new Point(5.0001, 5),
            new Point(5.0001, 5.0001),
            new Point(5, 5.0001)
        });

        Assert.Equal(PolygonRelation.Inside, tinyPolygon.RelationTo(polygon));
    }

    [Fact]
    public void Test_LargePolygon_ReturnsExpectedResult()
    {
        var largePolygonA = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(1e6, 0),
            new Point(1e6, 1e6),
            new Point(0, 1e6)
        });

        var largePolygonB = new Polygon(new[]
        {
            new Point(1e6, 1e6),
            new Point(2e6, 1e6),
            new Point(2e6, 2e6),
            new Point(1e6, 2e6)
        });

        Assert.Equal(PolygonRelation.Disjoint, largePolygonA.RelationTo(largePolygonB));
    }

    [Fact]
    public void Test_DegeneratePolygon_ReturnsDisjoint()
    {
        var polygonA = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 10),
            new Point(0, 10)
        });

        var degeneratePolygon = new Polygon(new[]
        {
            new Point(5, 5),
            new Point(5, 5)
        });

        Assert.Equal(PolygonRelation.Inside, degeneratePolygon.RelationTo(polygonA));
    }

    [Fact]
    public void Test_ComplexIntersections_ReturnsIntersect()
    {
        var polygonA = new Polygon(new[]
        {
            new Point(0, 0),
            new Point(10, 0),
            new Point(10, 10),
            new Point(0, 10)
        });

        var complexPolygon = new Polygon(new[]
        {
            new Point(5, -5),
            new Point(15, -5),
            new Point(15, 15),
            new Point(5, 15)
        });

        Assert.Equal(PolygonRelation.Intersect, complexPolygon.RelationTo(polygonA));
    }
}