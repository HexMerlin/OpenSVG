using SkiaSharp;

namespace OpenSvg.Attributes;

/// <summary>
/// Represents an attribute for path data, represented as a <see cref="Path"/> class.
/// </summary>
public class PathAttr : Attr<Path>
{
    public PathAttr() : base(SvgNames.D, new Path(), false)
    {
    }

    protected override Path Deserialize(string xmlString) => Path.FromXmlString(xmlString);

    protected override string Serialize(Path value) => value.ToXmlString();
}