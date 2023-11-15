using OpenSvg.Attributes;
using OpenSvg.Config;

namespace OpenSvg.SvgNodes;

public class SvgRectangleAsRect : SvgVisual
{
    public readonly DoubleAttr CornerRadiusX = new(SvgNames.Rx);

    public readonly DoubleAttr CornerRadiusY = new(SvgNames.Ry);

    public readonly AbsoluteOrRatioAttr DefinedHeight = new(SvgNames.Height);

    public readonly AbsoluteOrRatioAttr DefinedWidth = new(SvgNames.Width);


    public readonly DoubleAttr X = new(SvgNames.X);

    public readonly DoubleAttr Y = new(SvgNames.Y);

    /// <inheritdoc/>
    public override string SvgName => SvgNames.Rect;

    public RectangleConfig RectangleConfig
    {
        get => new(BoundingBox.Size, DrawConfig,
            CornerRadius: this.CornerRadiusX.Get()); //TODO: Add support for different X and Y CornerRadius
        set
        {
            this.DefinedWidth.Set(value.Size.Width);
            this.DefinedHeight.Set(value.Size.Height);
            DrawConfig = value.DrawConfig;
            this.CornerRadiusX.Set(value.CornerRadius);
            this.CornerRadiusY.Set(value.CornerRadius);
        }
    }

    public double DefinedWidthAbsolute => this.DefinedWidth.Get().Resolve(() => ViewPortWidth);

    public double DefinedHeightAbsolute => this.DefinedHeight.Get().Resolve(() => ViewPortHeight);

    public Point TopLeft => new(this.X.Get(), this.Y.Get());

    public Point BottomRight => new(this.X.Get() + DefinedWidthAbsolute, this.Y.Get() + DefinedHeightAbsolute);

    protected override ConvexHull ComputeConvexHull() => new(new Point[]
            { TopLeft, (BottomRight.X, TopLeft.Y), BottomRight, (TopLeft.X, BottomRight.Y) });
}