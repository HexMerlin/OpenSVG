using System.Globalization;

namespace OpenSvg.Attributes;


/// <summary>
/// Represents an attribute for setting a point list as a <see cref="Polygon"/> for an SVG element.
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
