namespace OpenSvg;


/// <summary>
///     Represents a collection of enclosed polygon groups.
/// </summary>
public class MultiPolygon : List<EnclosedPolygonGroup>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MultiPolygon" /> class.
    /// </summary>
    /// <seealso cref="EnclosedPolygonGroup" />
    public MultiPolygon()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MultiPolygon" /> class with the specified collection of
    ///     <see cref="Polygon" /> objects.
    ///     The input polygons will be added into to proper groups of type <see cref="EnclosedPolygonGroup"/>  
    /// </summary>
    /// <param name="polygons">The collection of <see cref="Polygon" /> objects to add.</param>
    public MultiPolygon(IEnumerable<Polygon> polygons) : this() => AddAll(polygons);

    /// <summary>
    ///     Adds the specified <see cref="Polygon" /> object to the <see cref="MultiPolygon" />.
    ///     The input polygon will be added to to proper group of type <see cref="EnclosedPolygonGroup"/>  
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon" /> object to add.</param>
    public void Add(Polygon polygon)
    {
        bool added = false;
        foreach (EnclosedPolygonGroup polygonGroup in this)
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
            Add(new EnclosedPolygonGroup(polygon));
    }

    /// <summary>
    ///     Adds the specified collection of <see cref="Polygon" /> objects to the <see cref="MultiPolygon" />.
    /// </summary>
    /// <param name="polygons">The collection of <see cref="Polygon" /> objects to add.</param>
    /// <seealso cref="Add(Polygon)"/>
    public void AddAll(IEnumerable<Polygon> polygons)
    {
        foreach (Polygon polygon in polygons)
            Add(polygon);
    }

    /// <summary>
    ///     Gets the bounding box that encloses all polygons in the <see cref="MultiPolygon" />.
    /// </summary>
    /// <returns>The bounding box enclosing all polygons.</returns>
    /// <remarks>
    ///     Computes the convex hull of all polygons in the <see cref="MultiPolygon" /> and returns its bounding box.
    /// </remarks>
    public BoundingBox BoundingBox() => ComputeConvexHull().BoundingBox();

    /// <summary>
    ///     Computes the convex hull of all polygons in the <see cref="MultiPolygon" />.
    /// </summary>
    /// <returns>The convex hull of all polygons.</returns>
    /// <remarks>
    ///     The convex hull is computed using the QuickHull algorithm.
    /// </remarks>
    public ConvexHull ComputeConvexHull() => new(this.SelectMany(polygonGroup => polygonGroup.ExteriorPolygon));
}
