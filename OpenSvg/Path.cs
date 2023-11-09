using SkiaSharp;

namespace OpenSvg;
public class Path : IEquatable<Path>, IDisposable
{
    private readonly SKPath path;

    private readonly string xmlString;

    public Path() : this(new SKPath())
    {
    }

    public Path(SKPath path)
    {
        this.path = path; // path.Simplify() ?? path;
        this.xmlString = this.path.ToSvgPathData();

    }

  
    public static Path FromXmlString(string xmlString) => new(SKPath.ParseSvgPathData(xmlString));

    public string ToXmlString() => xmlString;

    /// <summary>
    ///     Approximates the path to multi polygon.
    /// </summary>
    /// <param name="segments">The number of line segments to use to approximate every single Bezier curve.</param>
    /// <returns>A multi polygon object.</returns>
    public MultiPolygon ApproximateToMultiPolygon(int segments) => new(ApproximatePathToPolygons(segments));

    public IEnumerable<Polygon> ApproximatePathToPolygons(int segments)
    {
        var currentPoints = new List<Point>();

        foreach (var c in Commands())
        {
            switch (c.verb)
            {
                case SKPathVerb.Move:
                    if (currentPoints.Count > 0)
                    {
                        yield return new Polygon(currentPoints);
                        currentPoints = new List<Point>();
                    }
                    currentPoints.Add(c.p0);
                    break;

                case SKPathVerb.Line:
                    currentPoints.Add(c.p1);
                    break;

                case SKPathVerb.Quad:
                    currentPoints.AddRange(ApproximateQuadBezier(c.p0, c.p1, c.p2, segments));
                    break;

                case SKPathVerb.Cubic:
                    currentPoints.AddRange(ApproximateCubicBezier(c.p0, c.p1, c.p2, c.p3, segments));
                    break;

                case SKPathVerb.Close:
                    if (currentPoints.Count > 0)
                    {
                        currentPoints.Add(currentPoints[0]); // Close the polygon
                        yield return new Polygon(currentPoints);
                        currentPoints = new List<Point>();
                    }
                    break;
                default:  throw new NotSupportedException($"Unsupported path command found: {c.verb}");
            }
        }
        if (currentPoints.Count > 0) yield return new Polygon(currentPoints);
    }


    /// <summary>
    ///     Approximates a quadratic Bezier curve using the De Casteljau's algorithm.
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
            var t = i / (float)segments;
            var u = 1.0f - t;
            var x = u * u * start.X + 2.0f * u * t * control.X + t * t * end.X;
            var y = u * u * start.Y + 2.0f * u * t * control.Y + t * t * end.Y;
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
            var t = i / (float)segments;
            var u = 1.0f - t;
            var x = u * u * u * start.X + 3 * u * u * t * control1.X + 3 * u * t * t * control2.X + t * t * t * end.X;
            var y = u * u * u * start.Y + 3 * u * u * t * control1.Y + 3 * u * t * t * control2.Y + t * t * t * end.Y;
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
    private static IEnumerable<Point> ApproximateConicBezier(Point start, Point control, Point end, float weight, int segments)
    {       
        for (int i = 0; i <= segments; i++)
        {
            var t = i / (float)segments;
            var u = 1.0f - t;
            var w = (u * weight + t) / (u + t * weight);
            var x = u * start.X + t * end.X + w * (control.X - u * start.X - t * end.X);
            var y = u * start.Y + t * end.Y + w * (control.Y - u * start.Y - t * end.Y);
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
        => other != null && this.ToXmlString().Equals(other.ToXmlString());
    
    //public bool Equals(Path? other)
    //{
    //    if (other == null)
    //        return false;
    //    if (ReferenceEquals(this, other))
    //        return true;
    //    //if (this.precomputedHashCode.Value != other.precomputedHashCode.Value)
    //    //    return false;

    //    var commands1 = this.Commands().ToArray();
    //    var commands2 = other.Commands().ToArray();

    //    if (commands1.Length != commands2.Length)
    //        return false;

    //    for (int i = 0; i < commands1.Length; i++)
    //    {
    //        var c1 = commands1[i];
    //        var c2 = commands2[i];
    //        if (c1.verb != c2.verb || c1.p0 != c2.p0 || c1.p1 != c2.p1 || c1.p2 != c2.p2 || c1.p3 != c2.p3)
    //            return false;
    //    }   

    //    return true;
    //}

    //private int ComputeHashCode()
    //{
    //    unchecked // Allow overflow
    //    {
    //        int hash = 17;
    //        foreach (var command in this.Commands())
    //        {
    //            hash = hash * 31 + (int) command.verb;
    //            hash = hash * 31 + command.p0.GetHashCode();
    //            hash = hash * 31 + command.p1.GetHashCode();
    //            hash = hash * 31 + command.p2.GetHashCode();
    //            hash = hash * 31 + command.p3.GetHashCode();
    //        }
    //        return hash;
    //    }
    //}

    public override bool Equals(object? obj) => Equals(obj as Path);
    public override int GetHashCode() => xmlString.GetHashCode();


    public static bool operator ==(Path? left, Path? right) => Equals(left, right);

    public static bool operator !=(Path? left, Path? right) => !Equals(left, right);

    public void Dispose() => path.Dispose();
}