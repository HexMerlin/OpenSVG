namespace OpenSvg;

/// <summary>
/// Provides extension methods for working with collections of <see cref="Point"/> objects.
/// </summary>
public static class PointCollectionExtensions
{
    /// <summary>
    /// Returns the minimum <see cref="Point"/> in the collection using the default Point comparer.
    /// </summary>
    /// <param name="points">The collection of <see cref="Point"/> objects.</param>
    /// <returns>The minimum <see cref="Point"/> in the collection.</returns>
    public static Point Min(this IEnumerable<Point> points) => points.Min(PointComparer.Default);

    /// <summary>
    /// Returns the maximum <see cref="Point"/> in the collection using the default Point comparer.
    /// </summary>
    /// <param name="points">The collection of <see cref="Point"/> objects.</param>
    /// <returns>The maximum <see cref="Point"/> in the collection.</returns>
    public static Point Max(this IEnumerable<Point> points) => points.Max(PointComparer.Default);
}
