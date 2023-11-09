using OpenSvg.Attributes;
using OpenSvg.Config;

namespace OpenSvg.SvgNodes;

public sealed partial class SvgPath : SvgVisual, IDisposable
{
    public readonly PathAttr Path = new();

    private const int SegmentCountForCurveApproximation = 10;

    public SvgPath() => this.Path.Set(new Path());

    /// <summary>
    ///     Initializes a new instance of the <see cref="SvgPath" /> class with the specified <see cref="TextConfig" />.
    /// </summary>
    /// <param name="textConfig">The styling information for the text.</param>
    public SvgPath(TextConfig textConfig)
    {
        DrawConfig = textConfig.DrawConfig;
        this.Path.Set(ConvertTextToSkPath(textConfig));
    }

    public override string SvgName => SvgNames.Path;

    public void Dispose() => this.Path.Get().Dispose();

    public MultiPolygon ApproximateToMultiPolygon(int segments) => Path.Get().ApproximateToMultiPolygon(segments);

    protected override ConvexHull ComputeConvexHull() => ApproximateToMultiPolygon(SegmentCountForCurveApproximation).ComputeConvexHull();

    //public override (bool Equal, string Message) CompareSelfAndDescendants(SvgElement other, double doublePrecision = Constants.DoublePrecision)
    //{
    //    if (ReferenceEquals(this, other)) return (true, "Same reference");
    //    (bool equal, string message) = base.CompareSelfAndDescendants(other);
    //    if (!equal)
    //        return (equal, message);
    //    var sameType = (SvgPath)other;

    //    if (this.Path != sameType.Path)
    //        return (false, $"Path: {this.Path} != {sameType.Path}");

    //    return (true, "Equal");
    //}
}