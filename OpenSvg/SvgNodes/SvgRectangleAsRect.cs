using OpenSvg;
using OpenSvg.Attributes;
using OpenSvg.Config;
using System.Xml.Serialization;

namespace OpenSvg.SvgNodes;

public class SvgRectangleAsRect : SvgVisual
{

    public override string SvgName => SvgNames.Rect;

    public RectangleConfig RectangleConfig
    {
        get => new (BoundingBox.Size, DrawConfig, CornerRadius: CornerRadiusX.Get()); //TODO: Add support for different X and Y CornerRadius
        set
        {
            DefinedWidth.Set(value.Size.Width);
            DefinedHeight.Set(value.Size.Height);
            DrawConfig = value.DrawConfig;
            CornerRadiusX.Set(value.CornerRadius);
            CornerRadiusY.Set(value.CornerRadius);
        }
    }

 
    public readonly DoubleAttr X = new(SvgNames.X);

    public readonly DoubleAttr Y = new(SvgNames.Y);

    public readonly AbsoluteOrRatioAttr DefinedWidth = new(SvgNames.Width);

    public readonly AbsoluteOrRatioAttr DefinedHeight = new(SvgNames.Height);

    public readonly DoubleAttr CornerRadiusX = new(SvgNames.Rx);

    public readonly DoubleAttr CornerRadiusY = new(SvgNames.Ry);

    public double DefinedWidthAbsolute => DefinedWidth.Get().Resolve(() => ViewPortWidth);

    public double DefinedHeightAbsolute => DefinedHeight.Get().Resolve(() => ViewPortHeight);

    public Point TopLeft => new(X.Get(), Y.Get());

    public Point BottomRight => new(X.Get() + DefinedWidthAbsolute, Y.Get() + DefinedHeightAbsolute);
    protected override ConvexHull ComputeConvexHull() =>
        new(new Point[]
            { TopLeft, (BottomRight.X, TopLeft.Y), BottomRight, (TopLeft.X, BottomRight.Y) });
}
