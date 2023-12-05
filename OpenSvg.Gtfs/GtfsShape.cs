using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace OpenSvg.Gtfs;
public class GtfsShape
{
    public string ID { get; }
    public ImmutableArray<GtfsShapePoint> ShapePoints { get; }


    public GtfsShape(string id, IEnumerable<GtfsShapePoint> shapePoints)
    {
        ID = id;
        ShapePoints = shapePoints.ToImmutableArray();
    }

    public Coordinate? CoordinateUsingGtfsDistance(double distanceTraveled)
    {
        (GtfsShapePoint? pointA, GtfsShapePoint? pointB, double distanceFractionFromAtoB) = GetPointPairUsingGtfsDistance(distanceTraveled);
        if (pointA != null && distanceFractionFromAtoB == 0)
            return pointA.Value.Coordinate; //exact match
        if (distanceTraveled <= 0)
            return null;
        if (pointA == null || pointB == null)
            return null;

        return Coordinate.Interpolate(pointA.Value.Coordinate, pointB.Value.Coordinate, distanceFractionFromAtoB);
      
    }

    public Coordinate FindClosestCoordinateOnShape(Coordinate coordinate)
    {
        Coordinate closestCoordinate = new Coordinate(0, 0);   
        double closestDistance = double.MaxValue;

        foreach (GtfsShapePoint shapePoint in ShapePoints)
        {
            double distance = shapePoint.Coordinate.DistanceTo(coordinate);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCoordinate = shapePoint.Coordinate;
            }
        }
        return closestCoordinate;
    }


    public (GtfsShapePoint? pointA, GtfsShapePoint? pointB, double distanceFractionFromAtoB) GetPointPairUsingGtfsDistance(double distanceTraveled)
    {
     
        for (int i = 0; i < ShapePoints.Length - 1; i++)
        {
            GtfsShapePoint pointA = ShapePoints[i];
            GtfsShapePoint pointB = ShapePoints[i + 1];
            if (pointA.ShapeDistTraveled < 0 || pointB.ShapeDistTraveled < 0)
                continue;
            if (distanceTraveled >= pointA.ShapeDistTraveled && distanceTraveled <= pointB.ShapeDistTraveled)
            {
                if (distanceTraveled == pointA.ShapeDistTraveled)                   
                    return (pointA, null, 0); //pointA is an exact match

                if (distanceTraveled == pointB.ShapeDistTraveled)
                    return (pointB, null, 0); //pointB is an exact match

                return (pointA, pointB, GetDistanceFraction(distanceTraveled, pointA, pointB));
            }
        }
        return (null, null, -1); //could not resolve location
    }


    private static double GetDistanceFraction(double distanceTraveled, GtfsShapePoint pointA, GtfsShapePoint pointB)
    {
        double gtfsDistanceTraveledFromA = distanceTraveled - pointA.ShapeDistTraveled;
        var gtfsDistanceFromAtoB = pointB.ShapeDistTraveled - pointA.ShapeDistTraveled;
       

        var distanceFraction = gtfsDistanceTraveledFromA / gtfsDistanceFromAtoB;
        if (distanceFraction < 0 || distanceFraction > 1)
            Console.WriteLine("Bad DistanceFraction: " + distanceFraction);
        return distanceFraction;
    }

    public Polyline ToPolyline(PointConverter converter)
    {
        IEnumerable<Point> points = ShapePoints.Select(sp => converter.ToPoint(sp.Coordinate));
        return new Polyline(points);
    }

    public SvgVisual ToSvgShape(PointConverter converter)
    {
        IEnumerable<Point> points = ShapePoints.Select(sp => converter.ToPoint(sp.Coordinate));
        Polyline polyline = new Polyline(points);
    
        SvgPolyline svgVisual = polyline.ToSvgPolyline();
        svgVisual.ID = this.ID;
        return svgVisual;
    }

}
