using System.Drawing;
using System.Runtime.CompilerServices;

namespace OpenSvg.Optimization;

public readonly struct Line //: IComparable<Line>
{
    public readonly Point MinPoint;
    public readonly Point MaxPoint;

    public Line(Point p1, Point p2) 
    {
        MinPoint = p1;
        MaxPoint = p2;
        if (p1.IsWithinDistance(p2, 0.0001f))
        {
            throw new ArgumentException("Line has near zero length");
        }
        //bool isP1Min = p1.CompareTo(p2) < 0;

        //MinPoint = isP1Min ? p1 : p2;
        //MaxPoint = isP1Min ? p2 : p1;
    }

    public static Line CreateUnchecked(Point minPoint, Point maxPoint)
        => new Line(minPoint, maxPoint, false);
    

    private Line(Point minPoint, Point maxPoint, bool _)
    {
        MinPoint = minPoint;
        MaxPoint = maxPoint;
    }

    public static Line Zero = CreateUnchecked(Point.Zero, Point.Zero); 

    //public int CompareTo(Line other)
    //{
    //    int c = MinPoint.CompareTo(other.MinPoint);
    //    if (c != 0) return c;
    //    c = MaxPoint.CompareTo(other.MaxPoint);
    //    return c;
    //}

    public override string ToString() => $"({MinPoint}, {MaxPoint})";
}
