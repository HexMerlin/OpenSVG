using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OpenSvg.Optimization;



public readonly struct Line : IEquatable<Line> 
{
    public readonly Point MinPoint;
    public readonly Point MaxPoint;

    public Line(Point p1, Point p2)
    {
        bool isP1Min = p1.CompareTo(p2) <= 0;

        MinPoint = isP1Min ? p1 : p2;
        MaxPoint = isP1Min ? p2 : p1;
        if (p1.IsWithinDistance(p2, 0.00001f))
        {
            throw new ArgumentException("Line has near zero length: " + Point.Distance(p1, p2));
        }

    }

    public Line(Point minPoint, Point maxPoint, SkipSort _)
    {
        MinPoint = minPoint;
        MaxPoint = maxPoint;
    }

    public static Line Zero { get; } = new Line(Point.Zero, Point.Zero);

  
    public override string ToString() => $"({MinPoint}, {MaxPoint})";



    public bool Equals(Line other) => MinPoint.Equals(other.MinPoint) && MaxPoint.Equals(other.MaxPoint);

    public override bool Equals(object? obj) => obj is Line other && Equals(other);

    /// <summary>Returns a value that indicates whether each pair of Points in two specified Lines is equal.</summary>
    /// <param name="left">The first line to compare.</param>
    /// <param name="right">The second line to compare.</param>
    /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
    /// <remarks>Two <see cref="Line" /> objects are equal if each Point in <paramref name="left" /> is equal to the corresponding Point in <paramref name="right" />.</remarks>
    public static bool operator ==(Line left, Line right) => left.Equals(right);

    /// <summary>Returns a value that indicates whether two specified Lines are not equal.</summary>
    /// <param name="left">The first line to compare.</param>
    /// <param name="right">The second line to compare.</param>
    /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />.</returns>
    /// <remarks>Two <see cref="Line" /> objects are not equal if any Point in <paramref name="left" /> is not equal to the corresponding Point in <paramref name="right" />.</remarks>
    public static bool operator !=(Line left, Line right) => !left.Equals(right);
    
    public override int GetHashCode() => HashCode.Combine(MinPoint.X, MinPoint.Y, MaxPoint.X, MaxPoint.Y);

    public enum SkipSort
    {
        Yes,
    }
}
