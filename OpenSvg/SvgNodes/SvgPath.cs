using System.Xml.Serialization;
using OpenSvg.Attributes;
using OpenSvg.Config;
using SkiaSharp;

namespace OpenSvg.SvgNodes;

public sealed partial class SvgPath : SvgVisual, IDisposable
{
   
    public readonly PathAttr Path = new();

 
    public override string SvgName => SvgNames.Path;

    public SvgPath() => Path.Set(new SKPath());

    /// <summary>
    /// Initializes a new instance of the <see cref="SvgPath"/> class with the specified <see cref="TextConfig"/>.
    /// </summary>
    /// <param name="textConfig">The styling information for the text.</param>
    public SvgPath(TextConfig textConfig)
    {
        DrawConfig = textConfig.DrawConfig;
        Path.Set(ConvertTextToSkPath(textConfig));
    }

    protected override ConvexHull ComputeConvexHull() => ApproximateToMultiPolygon().ComputeConvexHull();

    /// <summary>
    /// Approximates the path to multi polygon.
    /// </summary>
    /// <param name="segments">The number of line segments to use to approximate every single Bezier curve.</param>
    /// <returns>A multi polygon object.</returns>
    public MultiPolygon ApproximateToMultiPolygon(int segments = 10)
        => new(ApproximatePathToPolygons(segments));

    private IEnumerable<Polygon> ApproximatePathToPolygons(int segments = 10)
    {
        SKPath path = Path.Get();
      
        var currentPoints = new List<Point>();

        using var iterator = path.CreateRawIterator();
        SKPoint[] points = new SKPoint[4];
        SKPathVerb verb;

        while ((verb = iterator.Next(points)) != SKPathVerb.Done)
        {
            switch (verb)
            {
                case SKPathVerb.Move:
                    if (currentPoints.Count > 0)
                    {
                        yield return new Polygon(currentPoints);
                        currentPoints = new List<Point>();
                    }
                    currentPoints.Add(new Point(points[0].X, points[0].Y));
                    break;

                case SKPathVerb.Line:
                    currentPoints.Add(new Point(points[1].X, points[1].Y));
                    break;

                case SKPathVerb.Quad:
                    currentPoints.AddRange(ApproximateQuadBezier(points[0], points[1], points[2], segments));
                    break;

                case SKPathVerb.Cubic:
                    currentPoints.AddRange(ApproximateCubicBezier(points[0], points[1], points[2], points[3], segments));
                    break;

                case SKPathVerb.Conic: //Conic Bezier curves are not currently supported in SVGs, so this option will not be matched, but it is added for future compatibility
                    currentPoints.AddRange(ApproximateConicBezier(points[0], points[1], points[2], iterator.ConicWeight(), segments));
                    break;

                case SKPathVerb.Close:
                    if (currentPoints.Count > 0)
                    {
                        currentPoints.Add(currentPoints[0]); // Close the polygon
                        yield return new Polygon(currentPoints);
                        currentPoints = new List<Point>();
                    }
                    break;

                default:
                    break;
            }
        }

        if (currentPoints.Count > 0)
        {
            yield return new Polygon(currentPoints);
        }
    }



    /// <summary>
    /// Approximates a quadratic Bezier curve using the De Casteljau's algorithm.
    /// </summary>
    /// <param name="start">The start point of the curve.</param>
    /// <param name="control">The control point of the curve.</param>
    /// <param name="end">The end point of the curve.</param>
    /// <param name="segments">The number of segments to divide the curve into.</param>
    /// <returns>A sequence of points approximating the curve.</returns>
    private static IEnumerable<Point> ApproximateQuadBezier(SKPoint start, SKPoint control, SKPoint end, int segments)
    {
        var points = new List<Point>();

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            float u = 1.0f - t;
            var x = u * u * start.X + 2.0f * u * t * control.X + t * t * end.X;
            var y = u * u * start.Y + 2.0f * u * t * control.Y + t * t * end.Y;
            points.Add(new Point(x, y));
        }

        return points;
    }

    /// <summary>
    /// Approximates a cubic Bezier curve using the De Casteljau's algorithm.
    /// </summary>
    /// <param name="start">The start point of the curve.</param>
    /// <param name="control1">The first control point of the curve.</param>
    /// <param name="control2">The second control point of the curve.</param>
    /// <param name="end">The end point of the curve.</param>
    /// <param name="segments">The number of segments to divide the curve into.</param>
    /// <returns>A sequence of points approximating the curve.</returns>
    private static IEnumerable<Point> ApproximateCubicBezier(SKPoint start, SKPoint control1, SKPoint control2, SKPoint end, int segments)
    {
        var points = new List<Point>();

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            float u = 1.0f - t;
            var x = u * u * u * start.X + 3 * u * u * t * control1.X + 3 * u * t * t * control2.X + t * t * t * end.X;
            var y = u * u * u * start.Y + 3 * u * u * t * control1.Y + 3 * u * t * t * control2.Y + t * t * t * end.Y;
            points.Add(new Point(x, y));
        }

        return points;
    }

    /// <summary>
    /// Approximates a conic (weighted quadratic) Bezier curve using a version of the De Casteljau's algorithm
    /// </summary>
    /// <param name="start">The start point of the curve.</param>
    /// <param name="control">The control point of the curve.</param>
    /// <param name="end">The end point of the curve.</param>
    /// <param name="weight">The weight for the control point.</param>
    /// <param name="segments">The number of segments to divide the curve into.</param>
    /// <returns>A sequence of points approximating the curve.</returns>
    private static IEnumerable<Point> ApproximateConicBezier(SKPoint start, SKPoint control, SKPoint end, float weight, int segments)
    {
        var points = new List<Point>();

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            float u = 1.0f - t;
            float w = (u * weight + t) / (u + t * weight);
            var x = u * start.X + t * end.X + w * (control.X - u * start.X - t * end.X);
            var y = u * start.Y + t * end.Y + w * (control.Y - u * start.Y - t * end.Y);
            points.Add(new Point(x, y));
        }

        return points;
    }
    public override (bool Equal, string Message) CompareSelfAndDescendants(SvgElement other, double doublePrecision = Constants.DoublePrecision)
    {
        if (ReferenceEquals(this, other)) return (true, "Same reference");
        var (equal, message) = base.CompareSelfAndDescendants(other);
        if (!equal)
            return (equal, message);
        SvgPath sameType = (SvgPath)other;

        if (Path != sameType.Path)
            return (false, $"Path: {Path} != {sameType.Path}");

        return (true, "Equal");
    }

    public void Dispose()
    {
        Path.Get().Dispose();
    }
}
