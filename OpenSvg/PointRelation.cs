namespace OpenSvg;

/// <summary>
///     The PointRelation enumeration describes the spatial relationship between a point and a polygon.
///     The relationships are defined based on the position of the point in relation to the vertices and edges of the
///     polygon.
///     If a point coincides with a vertex of the polygon or lies directly on an edge, it is considered to be Inside the
///     polygon.
/// </summary>
public enum PointRelation
{
    /// <summary>
    ///     The point does not lie within the polygon or on its boundary.
    /// </summary>
    Disjoint,

    /// <summary>
    ///     The point lies within the polygon or on its boundary.
    /// </summary>
    Inside
}