using System.Globalization;

namespace OpenSvg.Attributes;


/// <summary>
/// Represents points attribute of an SVG element. Points are represented as a <see cref="Polygon"/> class.
/// </summary>
public class PolygonAttr : Attr<Polygon>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PolygonAttr"/> class.
    /// </summary>
    public PolygonAttr() : base(SvgNames.Points, Polygon.Empty, false)
    {
    }

    /// <inheritdoc/>
    protected override string Serialize(Polygon value) => value.ToXmlString();

    /// <inheritdoc/>
    protected override Polygon Deserialize(string xmlString) => Polygon.FromXmlString(xmlString);
}
