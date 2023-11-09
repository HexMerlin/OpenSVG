using System.Globalization;

namespace OpenSvg.Attributes;

public class PolygonAttr : Attr<Polygon>
{
    public PolygonAttr() : base(SvgNames.Points, Polygon.Empty, false)
    {
    }

    protected override string Serialize(Polygon value) => value.ToXmlString();

    protected override Polygon Deserialize(string xmlString) => Polygon.FromXmlString(xmlString);
}