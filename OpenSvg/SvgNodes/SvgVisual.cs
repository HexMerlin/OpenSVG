using OpenSvg.Attributes;
using OpenSvg.Config;
using SkiaSharp;

namespace OpenSvg.SvgNodes;


/// <summary>
///     Represents an SVG element that can be rendered.
///     It has a convex hull, a bounding box and a size
/// </summary>
public abstract class SvgVisual : SvgElement
{
    /// <summary>
    ///     Gets or sets the fill color of this <see cref="SvgVisual" /> element.
    /// </summary>
    /// <remarks>
    ///     The default color is <c>Black</c> according to the SVG 1.1 specification.
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/SVG11/painting.html#FillProperty">SVG 1.1 Fill Property</seealso>
    public readonly ColorAttr FillColor = new(SvgNames.Fill, DrawConfig.DefaultFillColor);

    /// <summary>
    ///     Gets or sets the stroke color of this <see cref="SvgVisual" /> element.
    /// </summary>
    /// <remarks>
    ///     The default color is <c>None</c> according to the SVG 1.1 specification.
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/SVG11/painting.html#StrokeProperty">SVG 1.1 Stroke Property</seealso>
    public readonly ColorAttr StrokeColor = new(SvgNames.Stroke, DrawConfig.DefaultStrokeColor);


    /// <summary>
    ///     Gets or sets the stroke-width of this <see cref="SvgVisual" /> element.
    /// </summary>
    /// <remarks>
    ///     The default stroke-width is <c>1</c> according to the SVG 1.1 specification.
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/SVG11/painting.html#StrokeWidthProperty">SVG 1.1 Stroke Width</seealso>
    public readonly DoubleAttr StrokeWidth = new(SvgNames.StrokeWidth, DrawConfig.DefaultStrokeWidth);

    /// <summary>
    /// Gets the transformation attribute of this <see cref="SvgVisual"/> element.
    /// </summary>
    /// <remarks>
    /// This attribute defines the transformation to be applied to the element.
    /// </remarks>

    public readonly TransformAttr Transform = new();

    /// <summary>
    /// Gets or sets the drawing configuration for this <see cref="SvgVisual"/> element.
    /// </summary>
    /// <remarks>
    /// This configuration includes fill color, stroke color, and stroke width.
    /// </remarks>
    public DrawConfig DrawConfig
    {
        get => new(this.FillColor.Get(), this.StrokeColor.Get(), this.StrokeWidth.Get());
        set
        {
            this.FillColor.Set(value.FillColor);
            this.StrokeColor.Set(value.StrokeColor);
            this.StrokeWidth.Set(value.StrokeWidth);
        }
    }

    /// <summary>
    /// Gets the convex hull of this <see cref="SvgVisual"/> element.
    /// </summary>
    /// <remarks>
    /// The convex hull is calculated based on the current transformation of the element.
    /// </remarks>
    public ConvexHull ConvexHull => ComputeConvexHull().Transform(this.Transform.Get());

    /// <summary>
    /// Gets the rectangular bounding box of this <see cref="SvgVisual"/> element.
    /// </summary>
    /// <remarks>
    /// The bounding box is derived from the convex hull of the element.
    /// </remarks>
    public BoundingBox BoundingBox => ConvexHull.BoundingBox;

    /// <summary>
    /// Protected abstract method to compute the raw convex hull of this <see cref="SvgVisual"/> element.
    /// By raw meaning that the resulting convex hull of this method does not apply any apply the current transformation of the element.
    /// </summary>
    protected abstract ConvexHull ComputeConvexHull();

    /// <summary>
    ///     Aligns this <see cref="SvgVisual" /> element relative to a specified reference element, using the given horizontal
    ///     and vertical alignment parameters.
    /// </summary>
    /// <remarks>
    ///     This operations moves this element. The reference element is not moved.
    /// </remarks>
    /// <param name="referenceElement">
    ///     The reference <see cref="SvgVisual" /> element to which this element will be aligned. It
    ///     provides the frame of reference for alignment.
    /// </param>
    /// <param name="horizontalAlignment">
    ///     The horizontal alignment strategy. If null, no horizontal alignment is applied. See
    ///     <see cref="HorizontalAlignment" /> for detailed options.
    /// </param>
    /// <param name="verticalAlignment">
    ///     The vertical alignment strategy. If null, no vertical alignment is applied. See
    ///     <see cref="VerticalAlignment" /> for detailed options.
    /// </param>
    /// <exception cref="NotSupportedException">
    ///     Thrown if an unsupported alignment is provided.
    /// </exception>
    /// <seealso cref="HorizontalAlignment"/>
    /// <seealso cref="VerticalAlignment"/>"/>
    public void AlignRelativeTo(SvgVisual referenceElement, HorizontalAlignment? horizontalAlignment = null,
        VerticalAlignment? verticalAlignment = null)
    {
        BoundingBox thisBox = BoundingBox;
        BoundingBox refBox = referenceElement.BoundingBox;

        double xThis = horizontalAlignment switch
        {
            null => 0,
            HorizontalAlignment.LeftWithLeft or HorizontalAlignment.LeftWithCenter or HorizontalAlignment.LeftWithRight => thisBox.MinX,
            HorizontalAlignment.CenterWithLeft or HorizontalAlignment.CenterWithCenter or HorizontalAlignment.CenterWithRight => thisBox.MidX,
            HorizontalAlignment.RightWithLeft or HorizontalAlignment.RightWithCenter or HorizontalAlignment.RightWithRight => thisBox.MaxX,
            _ => throw new NotSupportedException($"Unknown HorizontalAlignment: {horizontalAlignment}")
        };
        double xRef = horizontalAlignment switch
        {
            null => 0,
            HorizontalAlignment.LeftWithLeft or HorizontalAlignment.CenterWithLeft or HorizontalAlignment.RightWithLeft => refBox.MinX,
            HorizontalAlignment.LeftWithCenter or HorizontalAlignment.CenterWithCenter or HorizontalAlignment.RightWithCenter => refBox.MidX,
            HorizontalAlignment.LeftWithRight or HorizontalAlignment.CenterWithRight or HorizontalAlignment.RightWithRight => refBox.MaxX,
            _ => throw new NotSupportedException($"Unknown HorizontalAlignment: {horizontalAlignment}")
        };

        double yThis = verticalAlignment switch
        {
            null => 0,
            VerticalAlignment.TopWithTop or VerticalAlignment.TopWithCenter or VerticalAlignment.TopWithBottom => thisBox.MinY,
            VerticalAlignment.CenterWithTop or VerticalAlignment.CenterWithCenter or VerticalAlignment.CenterWithBottom => thisBox.MidY,
            VerticalAlignment.BottomWithTop or VerticalAlignment.BottomWithCenter or VerticalAlignment.BottomWithBottom => thisBox.MaxY,
            _ => throw new NotSupportedException($"Unknown VerticalAlignment: {verticalAlignment}")
        };
        double yRef = verticalAlignment switch
        {
            null => 0,
            VerticalAlignment.TopWithTop or VerticalAlignment.CenterWithTop or VerticalAlignment.BottomWithTop => refBox.MinY,
            VerticalAlignment.TopWithCenter or VerticalAlignment.CenterWithCenter or VerticalAlignment.BottomWithCenter => refBox.MidY,
            VerticalAlignment.TopWithBottom or VerticalAlignment.CenterWithBottom or VerticalAlignment.BottomWithBottom => refBox.MaxY,
            _ => throw new NotSupportedException($"Unknown VerticalAlignment: {verticalAlignment}")
        };

        double dx = xRef - xThis;
        double dy = yRef - yThis;
        this.Transform.ComposeWith(OpenSvg.Transform.CreateTranslation(dx, dy));
    }

}