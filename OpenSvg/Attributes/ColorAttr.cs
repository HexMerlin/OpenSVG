using SkiaSharp;

namespace OpenSvg.Attributes;

/// <summary>
/// Represents an attribute for color values.
/// </summary>
public class ColorAttr : Attr<SKColor>
{
    public ColorAttr(string name, SKColor defaultValue) : base(name, defaultValue, false)
    {
    }

    protected override SKColor Deserialize(string xmlString) => xmlString.ToColor();

    protected override string Serialize(SKColor value) => value.ToHexColorString();
}