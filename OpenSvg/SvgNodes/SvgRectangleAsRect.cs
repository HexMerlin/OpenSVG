using OpenSvg.Attributes;
using OpenSvg.Config;

namespace OpenSvg.SvgNodes;

public class SvgRectangleAsRect : SvgVisual
{
    protected readonly DoubleAttr cornerRadiusX = new(SvgNames.Rx);
    protected readonly DoubleAttr cornerRadiusY = new(SvgNames.Ry);
    protected readonly AbsoluteOrRatioAttr definedHeight = new(SvgNames.Height);
    protected readonly AbsoluteOrRatioAttr definedWidth = new(SvgNames.Width);
    protected readonly DoubleAttr x = new(SvgNames.X);
    protected readonly DoubleAttr y = new(SvgNames.Y);

    public double CornerRadiusX { get => cornerRadiusX.Get(); set => cornerRadiusX.Set(value); }

    public double CornerRadiusY { get => cornerRadiusY.Get(); set => cornerRadiusY.Set(value); }

    public AbsoluteOrRatio DefinedHeight { get => definedHeight.Get(); set => definedHeight.Set(value); }

    public AbsoluteOrRatio DefinedWidth { get => definedWidth.Get(); set => definedWidth.Set(value); }
  
    public double X { get => x.Get(); set => x.Set(value); }

    public double Y { get => y.Get(); set => y.Set(value); }
    /// <inheritdoc/>
    public override string SvgName => SvgNames.Rect;

    public RectangleConfig RectangleConfig
    {
        get => new(BoundingBox.Size, DrawConfig,
            CornerRadius: this.CornerRadiusX); //TODO: Add support for different X and Y CornerRadius
        set
        {
            this.DefinedWidth = value.Size.Width;
            this.DefinedHeight = value.Size.Height;
            DrawConfig = value.DrawConfig;
            this.CornerRadiusX = value.CornerRadius;
            this.CornerRadiusY = value.CornerRadius;
        }
    }

    public double DefinedWidthAbsolute => this.DefinedWidth.Resolve(() => ViewPortWidth);

    public double DefinedHeightAbsolute => this.DefinedHeight.Resolve(() => ViewPortHeight);

    public Point TopLeft => new(X, Y);

    public Point BottomRight => new(X + DefinedWidthAbsolute, Y + DefinedHeightAbsolute);

    protected override ConvexHull ComputeConvexHull() => new(new Point[]
            { TopLeft, (BottomRight.X, TopLeft.Y), BottomRight, (TopLeft.X, BottomRight.Y) });
}