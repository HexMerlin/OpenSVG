using SkiaSharp;

namespace OpenSvg.Attributes;

public class ColorAttr : Attr<SKColor>
{
    public ColorAttr(string name, SKColor defaultValue) : base(name, defaultValue, isConstant: false) { }
    protected override SKColor Deserialize(string xmlString) => xmlString.ToColor();

    protected override string Serialize(SKColor value) => value.ToHexColorString();
}



