using SkiaSharp;

namespace OpenSvg.Attributes;

public class PathAttr : Attr<SKPath>
{
    public PathAttr() : base(SvgNames.D, new SKPath(), isConstant: false) {}

    protected override SKPath Deserialize(string xmlString) => SKPath.ParseSvgPathData(xmlString);

    protected override string Serialize(SKPath value) => value.ToSvgPathData();

}
