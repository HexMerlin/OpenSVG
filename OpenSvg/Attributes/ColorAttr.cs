using SkiaSharp;

namespace OpenSvg.Attributes;

public class ColorAttr : Attr<SKColor>
{
    public ColorAttr(string name, SKColor defaultValue) : base(name, defaultValue, false)
    {
    }

    protected override SKColor Deserialize(string xmlString)
    {
        return xmlString.ToColor();
    }

    protected override string Serialize(SKColor value)
    {
        return value.ToHexColorString();
    }
}