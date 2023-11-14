using SkiaSharp;

namespace OpenSvg.Attributes;


/// <summary>
/// Represents an attribute for path data, represented as a <see cref="Path"/> class.
/// </summary>
public class PathAttr : Attr<Path>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PathAttr"/> class.
    /// </summary>
    public PathAttr() : base(SvgNames.D, new Path(), false)
    {
    }

    /// <inheritdoc/>
    protected override Path Deserialize(string xmlString) => Path.FromXmlString(xmlString);

    /// <inheritdoc/>
    protected override string Serialize(Path value) => value.ToXmlString();
}
