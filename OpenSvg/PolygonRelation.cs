namespace OpenSvg;

/// <summary>
///     Enumerates the types of spatial relationships between two polygons, A and B, based on the positions of their
///     vertices.
/// </summary>
/// <remarks>
///     <para>
///         Note: Border-touching vertices are treated as "neutral" and do not by themselves determine the relationship.
///         The relationship is assessed based on the positions of the other vertices.
///     </para>
///     <para>
///         The relationships are symmetric, meaning:
///         <list type="bullet">
///             <item>If A is <see cref="Inside" /> B, then B <see cref="Cover" /> A.</item>
///             <item>If A is <see cref="Disjoint" /> from B, then B is <see cref="Disjoint" /> from A.</item>
///             <item>If A <see cref="Intersect" /> B, then B <see cref="Intersect" /> A.</item>
///             <item>If A is <see cref="Equal" /> to B, then B is <see cref="Equal" /> to A.</item>
///         </list>
///     </para>
/// </remarks>
public enum PolygonRelation
{
    /// <summary>
    ///     No part of A overlaps with B. Any border-touching vertices are not considered.
    /// </summary>
    Disjoint,

    /// <summary>
    ///     At least one vertex of A is inside the interior of B, or vice versa. Any border-touching vertices are not
    ///     considered.
    /// </summary>
    Intersect,

    /// <summary>
    ///     All vertices of A are within the interior of B. Any border-touching vertices are not considered.
    /// </summary>
    Inside,

    /// <summary>
    ///     All vertices of B are within the interior of A. Any border-touching vertices are not considered.
    /// </summary>
    Cover,

    /// <summary>
    ///     Both polygons have the same vertices and are identical in shape and position.
    /// </summary>
    Equal
}