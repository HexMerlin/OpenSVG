using OpenSvg.SvgNodes;
using SkiaSharp;
using System.Collections.Immutable;



namespace OpenSvg;

/// <summary>
///     Represents a polygon of points.
///     Provides methods for generating bounding boxes and translating polygons using a specified transform.
/// </summary>
public partial class Polygon : PointList, IEquatable<Polygon>
{

    private readonly ConvexHull convexHull;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Polygon" /> class with the specified collection of points.
    /// </summary>
    /// <param name="points">The collection of points.</param>
    public Polygon(IEnumerable<Point> points) : base(points.ToImmutableArray())
    {
        if (Points.Length >= 2 && Points[0] == Points[^1])
            throw new ArgumentException("The first and last points of a polygon cannot be the same. A polygon auto-closes the last point with the first.");
        convexHull = new ConvexHull(this);
    }

    public static new Polygon FromXmlString(string xmlString) => new Polygon(PointList.FromXmlString(xmlString));

    /// <summary>
    /// Returns an empty polygon.
    /// </summary>
    public static Polygon Empty => new(Enumerable.Empty<Point>());

    public ConvexHull ConvexHull => convexHull;

    public BoundingBox BoundingBox => ConvexHull.BoundingBox;

    /// <summary>
    /// Converts the <see cref="Polygon"/> to a <see cref="Path"/>.
    /// </summary>
    /// <returns>The <see cref="Polygon"/> represented as a <see cref="Path"/>.</returns>
    public Path ToPath()
    {
        SKPath skPath = new SKPath();
        skPath.AddPoly(this.Select(p => new SKPoint((float)p.X, (float)p.Y)).ToArray(), close: true);
        return new Path(skPath);
    }

    public SvgPolygon ToSvgPolygon()
    {
        SvgPolygon svgPolygon = new SvgPolygon();
        svgPolygon.Polygon = this;
        return svgPolygon;
    }

    /// <summary>
    ///     Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="other">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public bool Equals(Polygon? other) => base.Equals(other);
   

    ///<inheritdoc/>
    public override bool Equals(object? obj) => base.Equals(obj);

    ///<inheritdoc/>
    public override int GetHashCode() => base.GetHashCode();

  
}