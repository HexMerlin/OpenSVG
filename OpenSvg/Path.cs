using SkiaSharp;
using System.Data.Common;

namespace OpenSvg;


/// <summary>
/// Represents a graphical path
/// </summary>
public class Path : IEquatable<Path>, IDisposable
{
    private readonly SKPath path;

    private readonly string xmlString;

    public ConvexHull ConvexHull { get; }

    private const int SegmentCountForCurveApproximation = 10;

    /// <summary>
    /// Represents a graphical path with various drawing commands.
    /// </summary>
    public Path() : this(new SKPath())
    {
    }

    /// <summary>
    /// Initializes a new instance of the Path class with the specified SKPath object.
    /// </summary>
    /// <param name="path">The SKPath object to initialize the Path with.</param>

    public Path(SKPath path)
    {
        this.path = path; 
        this.xmlString = this.path.ToSvgPathData();
        this.ConvexHull = ApproximateToMultiPolygon(SegmentCountForCurveApproximation).ConvexHull;

    }

    public BoundingBox BoundingBox => ConvexHull.BoundingBox;

    /// <summary>
    /// Initializes a new instance of the <see cref="Path"/> class from an XML string.
    /// </summary>
    /// <param name="xmlString">The XML string to initialize the instance with.</param>
    /// <remarks>
    /// This method initializes a new instance of the <see cref="Path"/> class from an XML string.
    /// </remarks>
    public static Path FromXmlString(string xmlString) => new(SKPath.ParseSvgPathData(xmlString));

    /// <summary>
    /// Converts the path to an XML string.
    /// </summary>
    /// <returns>The path as an XML string.</returns>
    /// <remarks>
    /// This method converts the path to an XML string.
    /// </remarks>
    public string ToXmlString() => xmlString;

    /// <summary>
    /// Approximates the path to a collection of polygons.
    /// </summary>
    /// <param name="segments">The number of segments for approximating curves.</param>
    /// <returns>An enumerable of polygons approximating the path.</returns>

    public MultiPolygon ApproximateToMultiPolygon(int segments) => new(ApproximatePathToPolygons(segments));

    /// <summary>
    /// Approximates the path to a collection of polygons.
    /// </summary>
    /// <param name="segments">The number of segments for approximating curves.</param>
    /// <returns>An enumerable of polygons approximating the path.</returns>
    /// <remarks>
    /// This method approximates the path to a collection of polygons.
    /// </remarks>
    public IEnumerable<Polygon> ApproximatePathToPolygons(int segments)
    {
        var currentPoints = new List<Point>();

        foreach ((SKPathVerb verb, Point p0, Point p1, Point p2, Point p3) in Commands())
        {
            switch (verb)
            {
                case SKPathVerb.Move:
                    if (currentPoints.Count > 0)
                    {
                        yield return new Polygon(currentPoints);
                        currentPoints = new List<Point>();
                    }
                    currentPoints.Add(p0);
                    break;

                case SKPathVerb.Line:
                    currentPoints.Add(p1);
                    break;

                case SKPathVerb.Quad:
                    currentPoints.AddRange(ApproximateQuadBezier(p0, p1, p2, segments));
                    break;

                case SKPathVerb.Cubic:
                    currentPoints.AddRange(ApproximateCubicBezier(p0, p1, p2, p3, segments));
                    break;

                case SKPathVerb.Close:
                    if (currentPoints.Count > 0)
                    {
                        currentPoints.Add(currentPoints[0]); // Close the polygon
                        yield return new Polygon(currentPoints);
                        currentPoints = new List<Point>();
                    }
                    break;
                default: throw new NotSupportedException($"Unsupported path command found: {verb}");
            }
        }
        if (currentPoints.Count > 0) yield return new Polygon(currentPoints);
    }


    /// <summary>
    /// Approximates a quadratic Bezier curve using De Casteljau's algorithm.
    /// </summary>
    /// <param name="start">The start point of the curve.</param>
    /// <param name="control">The control point of the curve.</param>
    /// <param name="end">The end point of the curve.</param>
    /// <param name="segments">The number of segments to divide the curve into.</param>
    /// <returns>A sequence of points approximating the curve.</returns>
    private static IEnumerable<Point> ApproximateQuadBezier(Point start, Point control, Point end, int segments)
    {
        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            float u = 1.0f - t;
            double x = u * u * start.X + 2.0f * u * t * control.X + t * t * end.X;
            double y = u * u * start.Y + 2.0f * u * t * control.Y + t * t * end.Y;
            yield return new Point(x, y);
        }

    }

    /// <summary>
    ///     Approximates a cubic Bezier curve using the De Casteljau's algorithm.
    /// </summary>
    /// <param name="start">The start point of the curve.</param>
    /// <param name="control1">The first control point of the curve.</param>
    /// <param name="control2">The second control point of the curve.</param>
    /// <param name="end">The end point of the curve.</param>
    /// <param name="segments">The number of segments to divide the curve into.</param>
    /// <returns>A sequence of points approximating the curve.</returns>
    private static IEnumerable<Point> ApproximateCubicBezier(Point start, Point control1, Point control2, Point end, int segments)
    {
        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            float u = 1.0f - t;
            double x = u * u * u * start.X + 3 * u * u * t * control1.X + 3 * u * t * t * control2.X + t * t * t * end.X;
            double y = u * u * u * start.Y + 3 * u * u * t * control1.Y + 3 * u * t * t * control2.Y + t * t * t * end.Y;
            yield return new Point(x, y);
        }

    }

    /// <summary>
    ///     Approximates a conic (weighted quadratic) Bezier curve using a version of the De Casteljau's algorithm
    /// </summary>
    /// <remarks>
    /// Added for completeness, but not currently used, since Conic Bezier curves are not supported in SVG 1.1.
    /// </remarks>
    /// <param name="start">The start point of the curve.</param>
    /// <param name="control">The control point of the curve.</param>
    /// <param name="end">The end point of the curve.</param>
    /// <param name="weight">The weight for the control point.</param>
    /// <param name="segments">The number of segments to divide the curve into.</param>
    /// <returns>A sequence of points approximating the curve.</returns>
#pragma warning disable IDE0051 //Allow unused private members
    private static IEnumerable<Point> ApproximateConicBezier(Point start, Point control, Point end, float weight, int segments)
#pragma warning restore IDE0051 // Restore disallow unused private members
    {
        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            float u = 1.0f - t;
            float w = (u * weight + t) / (u + t * weight);
            double x = u * start.X + t * end.X + w * (control.X - u * start.X - t * end.X);
            double y = u * start.Y + t * end.Y + w * (control.Y - u * start.Y - t * end.Y);
            yield return new Point(x, y);
        }

    }

    private IEnumerable<(SKPathVerb verb, Point p0, Point p1, Point p2, Point p3)> Commands()
    {
        static Point Point(SKPoint p) => new(p.X, p.Y);
        using SKPath.RawIterator iterator = path.CreateRawIterator();
        var points = new SKPoint[4];
        SKPathVerb verb;

        while ((verb = iterator.Next(points)) != SKPathVerb.Done)
        {
            yield return new(verb, Point(points[0]), Point(points[1]), Point(points[2]), Point(points[3]));
            points[0] = points[1] = points[2] = points[3] = SKPoint.Empty; // Reset points
        }

    }

    /// <summary>
    /// Compares two paths for equality.
    /// </summary>
    /// <remarks>
    /// Paths equality must not change from serialization and deserialization.
    /// SKPath can apply some optimizations (e.g. remove redundant commands) when converting to string representation, 
    /// so we want to avoid performing comparisons on the raw commands from the iterator. 
    /// Comparisons are instead performed by comparing the precomputed string representations of the paths.
    /// </remarks>
    public bool Equals(Path? other)
        => other != null && ToXmlString().Equals(other.ToXmlString());

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as Path);

    /// <inheritdoc/>
    public override int GetHashCode() => xmlString.GetHashCode();

    /// <summary>
    /// Implements the equality operator.
    /// </summary>
    /// <param name="left">The left path to compare.</param>
    /// <param name="right">The right path to compare.</param>
    /// <returns>True if the paths are equal, false otherwise.</returns>
    public static bool operator ==(Path? left, Path? right) => Equals(left, right);

    /// <summary>
    /// Implements the inequality operator.
    /// </summary>
    /// <param name="left">The left path to compare.</param>
    /// <param name="right">The right path to compare.</param>
    /// <returns>True if the paths are not equal, false otherwise.</returns>
    public static bool operator !=(Path? left, Path? right) => !Equals(left, right);

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);

    }

    /// <inheritdoc/>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            path.Dispose();
        }
    }
}
