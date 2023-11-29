using System.Numerics;
using System.Runtime.CompilerServices;

namespace OpenSvg;

/// <summary>
/// Provides a default comparer for <see cref="Point"/> objects.
/// </summary>
public class PointComparer : IComparer<Vector2>
{
    /// <summary>
    /// Returns a default PointComparer.
    /// </summary>
    public static PointComparer Default { get; } = new PointComparer();

    /// <summary>
    ///     Compares two points using Default CompareTo.
    ///     Ordering is similar to how text is read in an English document: left to right, then top to bottom
    /// </summary>
    /// <example>
    /// <para>
    ///     The following example demonstrates the use of the default <see cref="PointComparer"/> method for sorting points.
    /// </para>
    /// <code><![CDATA[
    /// Point[] points = new Point[] { new Point(0, 10), new Point(20, 5) };
    /// Array.Sort(points, PointComparer.Default);
    /// Console.WriteLine(String.Join(", ", points));
    /// // Output: (20, 5), (0, 10)
    /// ]]></code>
    /// </example>
    /// <param name="p1">The first point to compare.</param>
    /// <param name="p2">The second point to compare.</param>
    /// <seealso cref="OpenSvg.PointExtensions.CompareTo(Vector2, Vector2)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(Point p1, Point p2) => p1.CompareTo(p2);
}