using SkiaSharp;
using System.Xml.Serialization;
using OpenSvg.Attributes;
using OpenSvg.Config;


namespace OpenSvg.SvgNodes;

public abstract class SvgVisual : SvgElement
{

   
    public SvgVisual() : base()
    {
      
    }
      
    protected abstract ConvexHull ComputeConvexHull();

    public readonly TransformAttr Transform = new();

    /// <summary>
    /// Gets or sets the fill color of this <see cref="SvgVisual"/> element.
    /// </summary>
    /// <remarks>
    /// The default color is <c>Black</c> according to the SVG 1.1 specification.
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/SVG11/painting.html#FillProperty">SVG 1.1 Fill Property</seealso>
    public readonly ColorAttr FillColor = new(SvgNames.Fill, SKColors.Black);

    /// <summary>
    /// Gets or sets the stroke color of this <see cref="SvgVisual"/> element.
    /// </summary>
    /// <remarks>
    /// The default color is <c>None</c> according to the SVG 1.1 specification.
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/SVG11/painting.html#StrokeProperty">SVG 1.1 Stroke Property</seealso>
    public readonly ColorAttr StrokeColor = new(SvgNames.Stroke, SKColors.Transparent);


    /// <summary>
    /// Gets or sets the stroke-width of this <see cref="SvgVisual"/> element.
    /// </summary>
    /// <remarks>
    /// The default stroke-width is <c>1</c> according to the SVG 1.1 specification.
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/SVG11/painting.html#StrokeWidthProperty">SVG 1.1 Stroke Width</seealso>

    public readonly DoubleAttr StrokeWidth = new(SvgNames.StrokeWidth, 1);

    public DrawConfig DrawConfig
    {
        get => new(FillColor.Get(), StrokeColor.Get(), StrokeWidth.Get());
        set
        {
            FillColor.Set(value.FillColor);
            StrokeColor.Set(value.StrokeColor);
            StrokeWidth.Set(value.StrokeWidth);
        }
    }

    public ConvexHull ConvexHull => ComputeConvexHull().Transform(Transform.Get());

    public BoundingBox BoundingBox => ConvexHull.BoundingBox();

    /// <summary>
    /// Aligns this <see cref="SvgVisual"/> element relative to a specified reference element, using the given horizontal and vertical alignment parameters.
    /// </summary>
    /// <remarks>
    /// This method calculates the necessary translation of the <see cref="SvgVisual"/> element so it aligns with the reference element based on specified alignments.
    /// Horizontal alignment can be <see cref="HorizontalAlignment.InsideLeft"/>, <see cref="HorizontalAlignment.Center"/>, <see cref="HorizontalAlignment.InsideRight"/>, <see cref="HorizontalAlignment.OutsideLeft"/>, or <see cref="HorizontalAlignment.OutsideRight"/>.
    /// Vertical alignment can be <see cref="VerticalAlignment.InsideUp"/>, <see cref="VerticalAlignment.Center"/>, <see cref="VerticalAlignment.InsideDown"/>, <see cref="VerticalAlignment.OutsideUp"/>, or <see cref="VerticalAlignment.OutsideDown"/>.
    /// See individual enum values for detailed explanations.
    /// </remarks>
    /// <param name="referenceElement">The reference <see cref="SvgVisual"/> element to which this element will be aligned. It provides the frame of reference for alignment.</param>
    /// <param name="horizontalAlignment">The horizontal alignment strategy. If null, no horizontal alignment is applied. See <see cref="HorizontalAlignment"/> for detailed options.</param>
    /// <param name="verticalAlignment">The vertical alignment strategy. If null, no vertical alignment is applied. See <see cref="VerticalAlignment"/> for detailed options.</param>
    /// <exception cref="NotSupportedException">Thrown when an unsupported alignment is provided.</exception>
    public void AlignRelativeTo(SvgVisual referenceElement, HorizontalAlignment? horizontalAlignment = null, VerticalAlignment? verticalAlignment = null)
    {
        var thisBox = BoundingBox;
        var refBox = referenceElement.BoundingBox;

        double dx = horizontalAlignment switch
        {
            null => 0,
            HorizontalAlignment.InsideLeft => refBox.MinX - thisBox.MinX,
            HorizontalAlignment.Center => refBox.MidX - thisBox.MidX,
            HorizontalAlignment.InsideRight => refBox.MaxX - thisBox.MaxX,
            HorizontalAlignment.OutsideLeft => refBox.MinX - thisBox.MaxX,
            HorizontalAlignment.OutsideRight => refBox.MaxX - thisBox.MinX,
            _ => throw new NotSupportedException($"Unknown HorizontalAlignment: {horizontalAlignment}")
        };

        double dy = verticalAlignment switch
        {
            null => 0,
            VerticalAlignment.InsideUp => refBox.MinY - thisBox.MinY,
            VerticalAlignment.Center => refBox.MidY - thisBox.MidY,
            VerticalAlignment.InsideDown => refBox.MaxY - thisBox.MaxY,
            VerticalAlignment.OutsideUp => refBox.MinY - thisBox.MaxY,
            VerticalAlignment.OutsideDown => refBox.MaxY - thisBox.MinY,
            _ => throw new NotSupportedException($"Unknown VerticalAlignment: {verticalAlignment}")
        };

        Transform.ComposeWith(OpenSvg.Transform.CreateTranslation(dx, dy));

    }

    public override (bool Equal, string Message) CompareSelfAndDescendants(SvgElement other, double doublePrecision = Constants.DoublePrecision)
    {
        if (ReferenceEquals(this, other)) return (true, "Same reference");
        var (equal, message) = base.CompareSelfAndDescendants(other);
        if (!equal)
            return (equal, message);
        SvgVisual sameType = (SvgVisual)other;
        if (Transform != sameType.Transform)
            return (false, $"Transform: {Transform} != {sameType.Transform}");
        if (FillColor != sameType.FillColor)
            return (false, $"FillColor: {FillColor} != {sameType.FillColor}");
        if (StrokeColor != sameType.StrokeColor)
            return (false, $"StrokeColor: {StrokeColor} != {sameType.StrokeColor}");
        if (StrokeWidth != sameType.StrokeWidth)
            return (false, $"StrokeWidth: {StrokeWidth} != {sameType.StrokeWidth}");
        return (true, "Equal");
    }
}

