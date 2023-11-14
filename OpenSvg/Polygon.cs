using SkiaSharp;
using System.Globalization;
using YamlDotNet.Core.Tokens;

namespace OpenSvg;

/// <summary>
///     Represents a polygon of points.
///     Provides methods for generating bounding boxes and translating polygons using a specified transform.
/// </summary>
public partial class Polygon : List<Point>, IEquatable<Polygon>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Polygon" /> class with the specified collection of points.
    /// </summary>
    /// <param name="points">The collection of points.</param>
    public Polygon(IEnumerable<Point> points) : base(points)
    {
    }

    public static Polygon Empty => new(Enumerable.Empty<Point>());

    /// <summary>
    /// Calculates the convex hull of the polygon.
    /// </summary>
    /// <returns>The convex hull as a ConvexHull object.</returns>

    public ConvexHull GetConvexHull() => new(this);


    /// <summary>
    ///     Gets the bounding box of the polygon.
    /// </summary>
    /// <returns>The bounding box as a <see cref="BoundingBox" /> object.</returns>
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
    /// Creates a polygon from an XML string representation.
    /// </summary>
    /// <param name="xmlString">The XML string representing the polygon points.</param>
    /// <returns>A new Polygon instance.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid point is encountered in the SVG polygon data.</exception>
    /// <exception cref="FormatException">Thrown when a coordinate in the SVG polygon data is invalid.</exception>

    public static Polygon FromXmlString(string xmlString) =>
        new(xmlString.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(pointStr =>
        {
            string[] coordinates = pointStr.Split(',', StringSplitOptions.TrimEntries);
            if (coordinates.Length != 2)
                throw new ArgumentException("Invalid point in SVG polygon data.");

            if (!double.TryParse(coordinates[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double x) ||
                !double.TryParse(coordinates[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double y))
                throw new FormatException("Invalid coordinate in SVG polygon data.");

            return new Point(x, y);
        }));


    /// <summary>
    ///     Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="other">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>

    public bool Equals(Polygon? other)
    {
        if (other == null || Count != other.Count)
            return false;
        if (ReferenceEquals(this, other))
            return true;

        for (int i = 0; i < Count; i++)
            if (!this[i].Equals(other[i]))
                return false;
        
        return true;
    }

    public override bool Equals(object? obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();

    /// <summary>
    /// Converts the polygon to its XML string representation.
    /// </summary>
    /// <returns>A string representing the polygon in XML format.</returns>

    public string ToXmlString() 
        => string.Join(" ", this.Select(p => $"{p.X.ToXmlString()},{p.Y.ToXmlString()}"));

    /// <summary>
    ///     Returns a string representing the current polygon object in a readable format.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => ToXmlString();

}