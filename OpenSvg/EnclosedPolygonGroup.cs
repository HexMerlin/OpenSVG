namespace OpenSvg;

/// <summary>
///     Represents a group of polygons where the exterior polygon encloses all interior polygons.
///     All the polygons in the group must be disjoint.
///     A typical use case of this class is to represent a polygon with holes inside it,
///     where the exterior polygon is the outer boundary and the interior polygons are the holes.
/// </summary>
/// <seealso cref="MultiPolygon" />
public class EnclosedPolygonGroup
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="EnclosedPolygonGroup" /> class.
    /// </summary>
    /// <param name="exteriorPolygon">The polygon that encloses all interior polygons.</param>
    public EnclosedPolygonGroup(Polygon exteriorPolygon)
    {
        ExteriorPolygon = exteriorPolygon;
        InteriorPolygons = new List<Polygon>();
    }

    /// <summary>
    ///     Gets or sets the polygon that encloses all interior polygons.
    /// </summary>
    public Polygon ExteriorPolygon { get; set; }

    /// <summary>
    ///     Gets or sets the list of polygons inside the exterior polygon.
    /// </summary>
    public List<Polygon> InteriorPolygons { get; set; }
}