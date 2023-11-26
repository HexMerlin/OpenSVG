using OpenSvg.Config;
using OpenSvg.SvgNodes;
using SkiaSharp;

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
    public EnclosedPolygonGroup(Polygon exteriorPolygon) : this(exteriorPolygon, new List<Polygon>()) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="EnclosedPolygonGroup" /> class.
    /// </summary>
    /// <param name="exteriorPolygon">The polygon that encloses all interior polygons.</param>
    /// <param name="interiorPolygons">The list of polygons inside the exterior polygon.</param>
    public EnclosedPolygonGroup(Polygon exteriorPolygon, List<Polygon> interiorPolygons)
    {
        ExteriorPolygon = exteriorPolygon;
        InteriorPolygons = interiorPolygons;
    }

    /// <summary>
    ///     Gets or sets the polygon that encloses all interior polygons.
    /// </summary>
    public Polygon ExteriorPolygon { get; set; }

    /// <summary>
    ///     Gets the list of polygons inside the exterior polygon.
    /// </summary>
    public List<Polygon> InteriorPolygons { get; set; }

    /// <summary>
    /// Converts the EnclosedPolygonGroup to a Path.
    /// </summary>
    /// <returns>An Path representing the EnclosedPolygonGroup.</returns>
    public Path ToPath()
    {
        var skPath = new SKPath();
        skPath.AddPoly(ExteriorPolygon.Select(p => new SKPoint((float)p.X, (float)p.Y)).ToArray(), close: true);
      
       foreach (var interiorPolygon in InteriorPolygons)
       {
          skPath.AddPoly(interiorPolygon.Select(p => new SKPoint((float)p.X, (float)p.Y)).ToArray(), close: true);
       }
          
       return new Path(skPath);
    }

}

