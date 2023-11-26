using OpenSvg.Config;
using OpenSvg.SvgNodes;
using SkiaSharp;
using System.Collections;

namespace OpenSvg;


/// <summary>
///     Represents a collection of enclosed polygon groups.
/// </summary>
public class MultiPolygon : IReadOnlyList<EnclosedPolygonGroup>  
{
    private readonly List<EnclosedPolygonGroup> enclosedPolygonGroups;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MultiPolygon" /> class.
    /// </summary>
    /// <seealso cref="EnclosedPolygonGroup" />
    public MultiPolygon() : this(Enumerable.Empty<Polygon>())
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MultiPolygon" /> class with the specified collection of
    ///     <see cref="Polygon" /> objects.
    ///     The input polygons will be added into to proper groups of type <see cref="EnclosedPolygonGroup"/>  
    /// </summary>
    /// <param name="polygons">The collection of <see cref="Polygon" /> objects to add.</param>
    public MultiPolygon(IEnumerable<Polygon> polygons) 
    {
        enclosedPolygonGroups = new List<EnclosedPolygonGroup>();
        AddAll(enclosedPolygonGroups, polygons);
        ConvexHull = ComputeConvexHull();
        BoundingBox = ConvexHull.BoundingBox;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MultiPolygon" /> class with the specified collection of
    ///     <see cref="EnclosedPolygonGroup" /> objects. 
    /// </summary>
    /// <param name="polygonGroups">The collection of <see cref="EnclosedPolygonGroup" /> objects to add.</param>
    public MultiPolygon(IEnumerable<EnclosedPolygonGroup> polygonGroups)
    {
        enclosedPolygonGroups = polygonGroups.ToList();
        ConvexHull = ComputeConvexHull();
        BoundingBox = ConvexHull.BoundingBox;
    }


    public EnclosedPolygonGroup this[int index] => enclosedPolygonGroups[index];


    /// <summary>
    ///     Gets the bounding box that encloses all polygons in the <see cref="MultiPolygon" />.
    /// </summary>
    /// <returns>The bounding box enclosing all polygons.</returns>
    public BoundingBox BoundingBox { get;}

    /// <returns>The convex hull of all polygons.</returns>
    public ConvexHull ConvexHull { get; }

    public int Count => enclosedPolygonGroups.Count;

    /// <summary>
    ///     Adds the specified <see cref="Polygon" /> object to the <see cref="MultiPolygon" />.
    ///     The input polygon will be added to to proper group of type <see cref="EnclosedPolygonGroup"/>  
    /// </summary>
    /// <param name="polygonGroups"></param>
    /// <param name="polygon">The <see cref="Polygon" /> object to add.</param>
    private static void Add(List<EnclosedPolygonGroup> polygonGroups, Polygon polygon)
    {
        bool added = false;
        foreach (EnclosedPolygonGroup polygonGroup in polygonGroups)
        {
            PolygonRelation relation = polygon.RelationTo(polygonGroup.ExteriorPolygon);
            switch (relation)
            {
                case PolygonRelation.Inside:
                    polygonGroup.InteriorPolygons.Add(polygon);
                    added = true;
                    break;

                case PolygonRelation.Cover:
                    if (polygonGroup.InteriorPolygons.Any())
                        throw new ArgumentException(
                            "Cannot add polygon to MultiPolygon, since it would create more than 1 level of containment");
                    polygonGroup.InteriorPolygons = new List<Polygon> { polygonGroup.ExteriorPolygon };
                    polygonGroup.ExteriorPolygon = polygon;
                    added = true;
                    break;

                case PolygonRelation.Disjoint:
                    break;

                case PolygonRelation.Intersect:
                    break;

                case PolygonRelation.Equal:
                    throw new ArgumentException(
                        "Cannot add polygon to MultiPolygon, since it already contains an identical polygon");
            }
        }

        if (!added)
            polygonGroups.Add(new EnclosedPolygonGroup(polygon));
    }

    /// <summary>
    ///     Adds the specified collection of <see cref="Polygon" /> objects to the <see cref="MultiPolygon" />.
    /// </summary>
    /// <param name="polygonGroups">A list of <see cref="EnclosedPolygonGroup"/> </param>
    /// <param name="polygons">The collection of <see cref="Polygon" /> objects to add.</param>
    /// <seealso cref="Add(Polygon)"/>
    private static void AddAll(List<EnclosedPolygonGroup> polygonGroups, IEnumerable<Polygon> polygons)
    {
        foreach (Polygon polygon in polygons)
            Add(polygonGroups, polygon);
    }

    /// <summary>
    /// Converts the MultiPolygon to a Path.
    /// </summary>
    /// <returns>An Path representing the MultiPolygon.</returns>
    public Path ToPath()
    {
        var skPath = new SKPath();
        foreach (EnclosedPolygonGroup epg in enclosedPolygonGroups)
        {
            skPath.AddPoly(epg.ExteriorPolygon.Select(p => new SKPoint((float)p.X, (float)p.Y)).ToArray(), close: true);

            foreach (Polygon interiorPolygon in epg.InteriorPolygons)
                 skPath.AddPoly(interiorPolygon.Select(p => new SKPoint((float)p.X, (float)p.Y)).ToArray(), close: true);
        }

        return new Path(skPath);
    }

    public IEnumerator<EnclosedPolygonGroup> GetEnumerator() => enclosedPolygonGroups.GetEnumerator();

    /// <summary>
    ///     Computes the convex hull of all polygons in the <see cref="MultiPolygon" />.
    /// </summary>
    /// <returns>The convex hull of all polygons.</returns>
    private ConvexHull ComputeConvexHull() => new(enclosedPolygonGroups.SelectMany(polygonGroup => polygonGroup.ExteriorPolygon));

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
