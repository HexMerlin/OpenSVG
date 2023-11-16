using System.Globalization;


namespace OpenSvg;

/// <summary>
///     Represents a polygon of points.
///     Provides methods for generating bounding boxes and translating polygons using a specified transform.
/// </summary>
public partial class Polygon : PolyLine, IEquatable<Polygon>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Polygon" /> class with the specified collection of points.
    /// </summary>
    /// <param name="points">The collection of points.</param>
    public Polygon(IEnumerable<Point> points) : base(points)
    {
    }
 
    /// <summary>
    /// Returns an empty polygon.
    /// </summary>
    public static new Polygon Empty => new(Enumerable.Empty<Point>());


    /// <summary>
    /// Creates a polygon from an XML string representation.
    /// </summary>
    /// <param name="xmlString">The XML string representing the polygon points.</param>
    /// <returns>A new Polygon instance.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid point is encountered in the SVG polygon data.</exception>
    /// <exception cref="FormatException">Thrown when a coordinate in the SVG polygon data is invalid.</exception>

    public static new Polygon FromXmlString(string xmlString) => new(PolyLine.FromXmlString(xmlString));


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

    ///<inheritdoc/>
    public override bool Equals(object? obj) => base.Equals(obj);

    ///<inheritdoc/>
    public override int GetHashCode() => base.GetHashCode();
}