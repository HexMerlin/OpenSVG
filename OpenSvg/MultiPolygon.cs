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
    /// </summary>
    /// <param name="polygons">The collection of <see cref="Polygon" /> objects to add.</param>
    public MultiPolygon(IEnumerable<Polygon> polygons) : this()
    {
        AddAll(polygons);
    }

    /// <summary>
    ///     Adds the specified <see cref="Polygon" /> object to the <see cref="MultiPolygon" />.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon" /> object to add.</param>
    public void Add(Polygon polygon)
    {
        var added = false;
        foreach (var polygonGroup in this)
        {
            var relation = polygon.RelationTo(polygonGroup.ExteriorPolygon);
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
    public void AddAll(IEnumerable<Polygon> polygons)
    {
        foreach (var polygon in polygons)
            Add(polygon);
    }

    /// <summary>
    ///     Gets the bounding box that encloses all polygons in the <see cref="MultiPolygon" />.
    /// </summary>
    /// <returns>The bounding box enclosing all polygons.</returns>
    public BoundingBox BoundingBox()
    {
        var boundingBox = new BoundingBox(Point.Origin, Point.Origin);
        foreach (var polygonGroup in this)
        {
            boundingBox = boundingBox.UnionWith(polygonGroup.ExteriorPolygon.BoundingBox());
            foreach (var interiorPolygon in polygonGroup.InteriorPolygons)
                boundingBox = boundingBox.UnionWith(interiorPolygon.BoundingBox());
        }

        return boundingBox;
    }

    public ConvexHull ComputeConvexHull()
    {
        return new ConvexHull(this.SelectMany(polygonGroup => polygonGroup.ExteriorPolygon));
    }
}