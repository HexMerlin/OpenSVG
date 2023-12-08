
using OpenSvg.SvgNodes;
using System.Collections.Immutable;

namespace OpenSvg.Optimization;

/// <summary>
/// Represents a fast polyline.
/// </summary>
public partial class FastPolyline : IEquatable<FastPolyline>
{
   
    /// <summary>
    /// The points of the polyline.
    /// </summary>
    public readonly ImmutableArray<Point> Points;

    /// <summary>
    /// The number of points in the polyline.
    /// </summary>
    public int Length => Points.Length;

    /// <summary>
    /// Initializes a new instance of the <see cref="FastPolyline"/> class.
    /// </summary>
    /// <param name="points">The points of the polyline.</param>
    /// <exception cref="ArgumentException">Thrown when the polyline has less than two points.</exception>
    public FastPolyline(ImmutableArray<Point> points)
    {
        if (points.Length < 2) throw new ArgumentException("A polyline must have at least two points");
        this.Points = points[0].CompareTo(points[^1]) <= 0 ? points : points.Reverse().ToImmutableArray();
      
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FastPolyline"/> class.
    /// </summary>
    /// <param name="points">The points of the polyline.</param>
    /// <exception cref="ArgumentException">Thrown when the polyline has less than two points.</exception>
    public FastPolyline(IEnumerable<Point> points) : this(points.ToImmutableArray()) { }    

    /// <summary>
    ///     Gets the point at the specified index.
    /// </summary>
    /// <param name="index">The index of the point.</param>
    /// <returns>The point at the specified index.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the index is out of range.</exception>
    public Point this[int index] => Points[index];

    /// <summary>
    /// Determines whether the current polyline is equal to another polyline.
    /// </summary>
    /// <param name="other">The polyline to compare with the current polyline.</param>
    /// <returns><c>true</c> if the current polyline is equal to the other polyline; otherwise, <c>false</c>.</returns>
    public bool Equals(FastPolyline? other) =>
        ReferenceEquals(this, other) || (other != null && this.Points.SequenceEqual(other.Points));

    /// <summary>
    ///     Determines whether the current polyline is equal to another object.
    /// </summary>
    /// <param name="obj">The object to compare with the current polyline.</param>
    /// <returns><c>true</c> if the current polyline is equal to the other object; otherwise, <c>false</c>.</returns>  
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((FastPolyline)obj);
    }


    /// <summary>
    /// Gets the hash code for the current polyline.
    /// </summary>
    /// <returns>The hash code for the current polyline.</returns>
    public override int GetHashCode() 
    {
        return Points.GetHashCode();
    }
    

    /// <summary>
    /// Determines whether two polylines are equal.
    /// </summary>
    /// <param name="left">The first polyline to compare.</param>
    /// <param name="right">The second polyline to compare.</param>
    /// <returns><c>true</c> if the two polylines are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(FastPolyline? left, FastPolyline? right) => left is null ? right is null : left.Equals(right);

    /// <summary>
    /// Determines whether two polylines are not equal.
    /// </summary>
    /// <param name="left">The first polyline to compare.</param>
    /// <param name="right">The second polyline to compare.</param>
    /// <returns><c>true</c> if the two polylines are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(FastPolyline? left, FastPolyline? right) => !(left == right);

    /// <summary>
    /// Converts the current polyline to an SVG polyline.
    /// </summary>
    /// <returns>The SVG polyline representation of the current polyline.</returns>
    public SvgPolyline ToSvgPolyline()
    {
        SvgPolyline svgPolyline = new SvgPolyline();
        svgPolyline.Polyline = new Polyline(Points);
        return svgPolyline;
    }
    public bool HasDuplicatedPoints()
    {
        HashSet<Point> hashedPoints = new HashSet<Point>(Points);
        return Length != hashedPoints.Count;
    }

    /// <summary>
    /// Removes adjacent points that are identical, or equivalent (i.e. closer than a given threshold).
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>
    /// <description>The method always retains the first and last points of the polyline.</description>
    /// </item>
    /// <item>
    /// <description>For polylines of lengths 2, the input polyline is returned.</description>
    /// </item>
    /// <item>
    /// <description>For adjacent equivalent points, the first point is retained, except if one of the points is the last point of the polyline.
    /// In that case the last point is retained instead.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public FastPolyline RemoveEquivalentAdjacentPoints(float minDistanceSquaredThreshold = 0.00001f)
    {
        if (Length == 2) return this; //do not optimize polylines with only two points

        List<Point> result = new List<Point>(Length);
        result.Add(this[0]);

        for (int i = 1; i < Points.Length; i++)
        {
            if (Point.DistanceSquared(Points[i], result[^1]) >= minDistanceSquaredThreshold)
                result.Add(Points[i]);
        }
        //assert that the last point is the same as the last point of the original polyline
        if (result[^1] != Points[^1])
            result[^1] = Points[^1];
        return new FastPolyline(result);
    }

    /// <summary>
    /// Finds the longest common substring between two polylines.
    /// </summary>
    /// <param name="polyline1">The first polyline.</param>
    /// <param name="polyline2">The second polyline.</param>
    /// <param name="matrix">The matrix used for dynamic programming.</param>
    /// <returns>The result of finding the longest common substring.</returns>
    public static SubstringResult FindLongestCommonSubstring(FastPolyline polyline1, FastPolyline polyline2, int[,] matrix)
    {
        ImmutableArray<Point> arr1 = polyline1.Points;
        ImmutableArray<Point> arr2 = polyline2.Points;
        int m = arr1.Length;
        int n = arr2.Length;
        if (m >= matrix.GetLength(0) || n >= matrix.GetLength(1))
            throw new ArgumentException("Pre-allocated matrix is too small.");

        Array.Clear(matrix);

        int maxLength = 0, endIndexInArr1 = 0, endIndexInArr2 = 0;

        for (int i = 1; i <= m; i++)
            for (int j = 1; j <= n; j++)
                if (arr1[i - 1] == arr2[j - 1])
                {
                    matrix[i, j] = matrix[i - 1, j - 1] + 1;
                    if (matrix[i, j] > maxLength)
                    {
                        maxLength = matrix[i, j];
                        endIndexInArr1 = i - 1;
                        endIndexInArr2 = j - 1;
                    }
                }
                else
                    matrix[i, j] = 0;


        int startIndexInArr1 = endIndexInArr1 - maxLength + 1;
        int startIndexInArr2 = endIndexInArr2 - maxLength + 1;

        return new SubstringResult(startIndexInArr1, startIndexInArr2, maxLength);
    }

    public override string ToString() => string.Join(", ", Points);
}

/// <summary>
/// Represents the result of finding the longest common substring between two polylines.
/// </summary>
public readonly record struct SubstringResult(int StartIndex1, int StartIndex2, int SubstringLength)
{ }
