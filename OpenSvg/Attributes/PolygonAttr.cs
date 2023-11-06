using System.Globalization;

namespace OpenSvg.Attributes;

public class PolygonAttr : Attr<Polygon>
{
    public PolygonAttr() : base(SvgNames.Points, Polygon.Empty, false)
    {
    }

    protected override string Serialize(Polygon value) => string.Join(" ", value.Select(p => $"{p.X.ToXmlString()},{p.Y.ToXmlString()}"));

    protected override Polygon Deserialize(string xmlString) => new(xmlString.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(pointStr =>
                                                                     {
                                                                         string[] coordinates = pointStr.Split(',', StringSplitOptions.TrimEntries);
                                                                         if (coordinates.Length != 2)
                                                                             throw new ArgumentException("Invalid point in SVG polygon data.");

                                                                         if (!double.TryParse(coordinates[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double x) ||
                                                                             !double.TryParse(coordinates[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double y))
                                                                             throw new FormatException("Invalid coordinate in SVG polygon data.");

                                                                         return new Point(x, y);
                                                                     }));
}