using System.Globalization;

namespace OpenSvg.Attributes;


/// <summary>
/// Represents an attribute for setting a point list as a <see cref="Polyline"/> for an SVG element.
/// </summary>
public class PolylineAttr : Attr<Polyline>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PolygonAttr"/> class.
    /// </summary>
    public PolylineAttr() : base(SvgNames.Points, Polyline.Empty, false)
    {
    }

    /// <inheritdoc/>
    protected override string Serialize(Polyline value) => value.ToXmlString();

    /// <inheritdoc/>
    protected override Polyline Deserialize(string xmlString) => Polyline.FromXmlString(xmlString);
}
