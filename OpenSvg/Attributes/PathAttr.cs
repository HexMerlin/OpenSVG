using SkiaSharp;

namespace OpenSvg.Attributes;

public class PathAttr : Attr<SKPath>
{
    public PathAttr() : base(SvgNames.D, new SKPath(), false)
    {
    }

    protected override SKPath Deserialize(string xmlString)
    {
        return SKPath.ParseSvgPathData(xmlString);
    }

    protected override string Serialize(SKPath value)
    {
        return value.ToSvgPathData();
    }
}