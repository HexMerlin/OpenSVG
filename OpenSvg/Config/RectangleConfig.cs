//File RectangleConfig.cs

using SkiaSharp;
using OpenSvg.SvgNodes;

namespace OpenSvg.Config;

/// <summary>
/// Defining attributes of a rectangle in SVG
/// The rectangle can be rendered either as a 'polygon' or a high-level 'rect' in SVG.
/// </summary>
///
/// When a rectangle is drawn with rounded corners as a polygon <see cref="SvgPolygon"/>, the number of points used to approximate a corner must be given (NumberOfCornerPoints)
/// NumberOfCornerPoints is ignored when the rectangle is drawn as a 'rect' using <see cref="SvgRectangleAsRect"/>
///
/// <remarks>
/// When converting to GeoJSON rectangles must be drwawn as <see cref="SvgPolygon"/>
/// </remarks>
///<param name="Size"> Size of the rectangle </param>
///<param name="DrawConfig">Styling options for the rectangle</param>
///<param name="NumberOfCornerPoints">Number of points used to approximate curved corners, when the rectangle is drawn as a polygon</param>
///<param name="CornerRadius">Defines the circle radius of the corners.
/// If CornerRadius=0 there will be no rounding corners, but just normal square corners</param>
public record RectangleConfig(Size Size, DrawConfig DrawConfig, int NumberOfCornerPoints = 10, double CornerRadius = 0)
{
    /// <summary>
    /// Gets a transparent RectangleConfig instance.
    /// This is useful for creating invisible spaces between shapes.
    /// </summary>
    /// <param name="size">Size of the rectangle.</param>
    /// <returns>A new instance of the <see cref="RectangleConfig"/> with transparent <see cref="DrawConfig"/>.
    /// </returns>
    public static RectangleConfig Transparent(Size size) => new(size, DrawConfig.Transparent);

    /// <summary>
    /// Creates a <see cref="SvgPolygon"/> element from this <see cref="RectangleConfig"/>"/>
    /// </summary>
    public SvgPolygon ToSvgPolygon()
    {
        SvgPolygon svgPolygon = new();
        svgPolygon.Polygon.Set(ToPolygon());
        svgPolygon.DrawConfig = DrawConfig;
        return svgPolygon;
    }

    /// <summary>
    /// Convert to SVG Rectangle object
    /// </summary>
    public SvgRectangleAsRect ToSvgRect() => new() { RectangleConfig = this };

    /// <summary>
    /// Returns a new copy of the RectangleConfig record with the specified fill color.
    /// </summary>
    /// <param name="fillColor">The new fill color to set.</param>
    /// <returns>A new RectangleConfig with the specified fill color.</returns>
    public RectangleConfig WithFillColor(SKColor fillColor) => this with { DrawConfig = DrawConfig.WithFillColor(fillColor) };

    /// <summary>
    /// Returns a new copy of the RectangleConfig record with the specified stroke color.
    /// </summary>
    /// <param name="strokeColor">The new stroke color to set.</param>
    /// <returns>A new RectangleConfig with the specified stroke color.</returns>
    public RectangleConfig WithStrokeColor(SKColor strokeColor) => this with { DrawConfig = DrawConfig.WithStrokeColor(strokeColor) };

    public Polygon ToPolygon()
    {
        bool roundedCorners = CornerRadius > 0;

        if (CornerRadius < 0)
            throw new ArgumentException("CornerRadius cannot be negative");

        if (CornerRadius > Size.Width / 2 || CornerRadius > Size.Height / 2)
            throw new ArgumentException("CornerRadius cannot be larger than half of the width or height of the rectangle");

        if (roundedCorners && NumberOfCornerPoints < 2)
            throw new ArgumentException("numberOfCornerPoints must be at least 2");

        double cr = CornerRadius;
        double height = Size.Height;
        double width = Size.Width;
        List<Point> points = new ();
        const float arcLength = -90; //draw the corners arcs clockwise, since we draw the polygon clockwise (from the top left corner)
        if (roundedCorners)
        {
            points.Add(new Point(cr, 0));
            points.Add(new Point(width - cr, 0));

            points.AddRange(CircularArc.CreateArcPoints(new Point(width - cr, cr), cr, 90, arcLength, NumberOfCornerPoints));

            points.Add(new Point(width, cr));
            points.Add(new Point(width, height - cr));

            points.AddRange(CircularArc.CreateArcPoints(new Point(width - cr, height - cr), cr, 0, arcLength, NumberOfCornerPoints));

            points.Add(new Point(width - cr, height));
            points.Add(new Point(cr, height));

            points.AddRange(CircularArc.CreateArcPoints(new Point(cr, height - cr), cr, 270, arcLength, NumberOfCornerPoints));

            points.Add(new Point(0, height - cr));
            points.Add(new Point(0, cr));

            points.AddRange(CircularArc.CreateArcPoints(new Point(cr, cr), cr, 180, arcLength, NumberOfCornerPoints));
        }
        else
        {
            points.Add(Point.Origin);
            points.Add(new Point(width, 0));
            points.Add(new Point(width, height));
            points.Add(new Point(0, height));
        }

        return new Polygon(points);
    }
}