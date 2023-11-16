using System.Globalization;


namespace OpenSvg;

/// <summary>
///     Represents a polyline of points.
///     Provides methods for generating bounding boxes and translating polylines using a specified transform.
/// </summary>
public class PolyLine : List<Point>, IEquatable<PolyLine>
{
    /// <returns>The convex hull as a ConvexHull object.</returns>
    public readonly ConvexHull ConvexHull;

    /// <returns>The bounding box as a BoundingBox object.</returns>
    public readonly BoundingBox BoundingBox;    

    /// <summary>
    ///     Initializes a new instance of the <see cref="PolyLine" /> class with the specified collection of points.
    /// </summary>
    /// <param name="points">The collection of points.</param>
    public PolyLine(IEnumerable<Point> points) : base(points)
    {
        ConvexHull = new(this);
        BoundingBox = ConvexHull.BoundingBox();
    }

    /// <summary>
    /// Returns an empty polyLine.
    /// </summary>
    public static PolyLine Empty => new(Enumerable.Empty<Point>());


    /// <summary>
    /// Creates a polyline from an XML string representation.
    /// </summary>
    /// <param name="xmlString">The XML string representing the polyline points.</param>
    /// <returns>A new Polyline instance.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid point is encountered in the SVG polyline data.</exception>
    /// <exception cref="FormatException">Thrown when a coordinate in the SVG polyline data is invalid.</exception>

    public static PolyLine FromXmlString(string xmlString) =>
        new(xmlString.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(pointStr =>
        {
            string[] coordinates = pointStr.Split(',', StringSplitOptions.TrimEntries);
            if (coordinates.Length != 2)
                throw new ArgumentException("Invalid point in SVG points data.");

            if (!double.TryParse(coordinates[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double x) ||
                !double.TryParse(coordinates[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double y))
                throw new FormatException("Invalid point in SVG points data.");

            return new Point(x, y);
        }));


    /// <summary>
    ///     Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="other">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>

    public bool Equals(PolyLine? other)
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

    ///<inheritdoc/>
    public override bool Equals(object? obj) => base.Equals(obj);

    ///<inheritdoc/>
    public override int GetHashCode() => base.GetHashCode();

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

}