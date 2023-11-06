﻿namespace OpenSvg;

/// <summary>
/// Represents a polygon of points.
/// Provides methods for generating bounding boxes and translating polygons using a specified transform.
/// </summary>
public partial class Polygon : List<Point>
{

    /// <summary>
    /// Initializes a new instance of the <see cref="Polygon"/> class with the specified collection of points.
    /// </summary>
    /// <param name="points">The collection of points.</param>
    public Polygon(IEnumerable<Point> points) : base(points) { }

    public static Polygon Empty => new(Enumerable.Empty<Point>());


    public ConvexHull GetConvexHull() => new(this);


    /// <summary>
    /// Gets the bounding box of the polygon.
    /// </summary>
    /// <returns>The bounding box as a <see cref="BoundingBox"/> object.</returns>
    public BoundingBox BoundingBox()
    {
        if (Count == 0) return new BoundingBox(); // return empty bounding box

        const double min = double.MinValue;
        const double max = double.MaxValue;

        var bounds = this.Aggregate(
                           new { UpperLeft = new Point(max, max), LowerRight = new Point(min, min) },
                                 (acc, p) => new
                                 {
                                     UpperLeft = new Point(Math.Min(acc.UpperLeft.X, p.X), Math.Min(acc.UpperLeft.Y, p.Y)),
                                     LowerRight = new Point(Math.Max(acc.LowerRight.X, p.X), Math.Max(acc.LowerRight.Y, p.Y))
                                 });

        return new BoundingBox(bounds.UpperLeft, bounds.LowerRight);
    }


    /// <summary>
    /// Returns a string representing the current polygon object in a readable format.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => "{\n" + string.Join(",\n", this.Select(p => p.ToString())) + "\n}";
}