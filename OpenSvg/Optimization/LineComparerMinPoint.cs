namespace OpenSvg.Optimization;

public class LineComparerMinPoint : IComparer<Line>
{
    public int Compare(Line a, Line b)
    {
        int c = a.MinPoint.CompareTo(b.MinPoint);
        if (c != 0) return c;
        c = a.MaxPoint.CompareTo(b.MaxPoint);
        return c;
    }
}
