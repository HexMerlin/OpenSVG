using HarfBuzzSharp;
using System.Collections;
using System.Globalization;
using System.Collections.Immutable;

namespace OpenSvg;

/// <summary>
///     Represents a sequence of points.
///     Provides methods for generating bounding boxes and translating polylines using a specified transform.
/// </summary>
public abstract class PointList : IReadOnlyList<Point>, IEquatable<PointList>
{

    protected readonly ImmutableArray<Point> Points;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PointList" /> class with the specified collection of points.
    /// </summary>
    /// <param name="points">The collection of points.</param>
    public PointList(ImmutableArray<Point> points)
    {
        this.Points = points;
    }

    public int Count => Points.Length;

    public Point this[int index] => Points[index];


    public bool Contains(Point point) => Points.Contains(point);

    /// <summary>
    /// Creates a point sequence from an XML string representation.
    /// </summary>
    /// <param name="xmlString">The XML string representing the polyline points.</param>
    /// <returns>An array of points.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid point is encountered in the SVG polyline data.</exception>
    /// <exception cref="FormatException">Thrown when a coordinate in the SVG polyline data is invalid.</exception>

    protected static Point[] FromXmlString(string xmlString) =>
        xmlString.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(pointStr =>
        {
            string[] coordinates = pointStr.Split(',', StringSplitOptions.TrimEntries);
            if (coordinates.Length != 2)
                throw new ArgumentException("Invalid point in SVG points data.");

            if (!double.TryParse(coordinates[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double x) ||
                !double.TryParse(coordinates[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double y))
                throw new FormatException("Invalid point in SVG points data.");

            return new Point(x, y);
        }).ToArray();


    /// <summary>
    ///     Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="other">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>

    public bool Equals(PointList? other)
    {
        if (other == null || Count != other.Count)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        if (this.GetType() != other.GetType())
            return false;

        for (int i = 0; i < Count; i++)
            if (!this[i].Equals(other[i]))
                return false;

        return true;
    }

    ///<inheritdoc/>
    public override bool Equals(object? obj) => base.Equals(obj);

    ///<inheritdoc/>
    public override int GetHashCode() => Points.GetHashCode();

    /// <summary>
    /// Converts the polyline to its XML string representation.
    /// </summary>
    /// <returns>A string representing the polyline in XML format.</returns>
    public string ToXmlString()
        => string.Join(" ", this.Select(p => $"{p.X.ToXmlString()},{p.Y.ToXmlString()}"));

    /// <summary>
    ///     Returns a string representing the current polyline object in a readable format.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => ToXmlString();


    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<Point> GetEnumerator() => ((IEnumerable<Point>)this.Points).GetEnumerator();
}