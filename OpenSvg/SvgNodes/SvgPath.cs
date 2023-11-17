using OpenSvg.Attributes;
using OpenSvg.Config;

namespace OpenSvg.SvgNodes;

public sealed partial class SvgPath : SvgVisual, IDisposable
{
    public readonly PathAttr Path = new();

  

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

    /// <inheritdoc/>
    public override string SvgName => SvgNames.Path;

    public void Dispose() => this.Path.Get().Dispose();

    public MultiPolygon ApproximateToMultiPolygon(int segments) => Path.Get().ApproximateToMultiPolygon(segments);

    protected override ConvexHull ComputeConvexHull() => Path.Get().ConvexHull;

}