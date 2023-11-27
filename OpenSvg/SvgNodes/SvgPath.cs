using OpenSvg.Attributes;
using OpenSvg.Config;

namespace OpenSvg.SvgNodes;

public partial class SvgPath : SvgVisual, IDisposable
{
    protected readonly PathAttr path = new();

    public Path Path { get => this.path.Get(); set => this.path.Set(value); }

    public SvgPath() => this.Path = new Path();

    /// <summary>
    ///     Initializes a new instance of the <see cref="SvgPath" /> class with the specified <see cref="TextConfig" />.
    /// </summary>
    /// <param name="textConfig">The styling information for the text.</param>
    public SvgPath(TextConfig textConfig)
    {
        DrawConfig = textConfig.DrawConfig;
        this.Path = ConvertTextToSkPath(textConfig);
    }

    /// <inheritdoc/>
    public override string SvgName => SvgNames.Path;

    public void Dispose() => this.Path.Dispose();

    public MultiPolygon ApproximateToMultiPolygon(int segments) => Path.ApproximateToMultiPolygon(segments);

    protected override ConvexHull ComputeConvexHull() => Path.ConvexHull;

}