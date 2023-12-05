namespace OpenSvg.Optimization;

public class LineComparerMaxPoint : IComparer<Line>
{
    public int Compare(Line a, Line b)
    {
        int c = a.MaxPoint.CompareTo(b.MaxPoint);
        if (c != 0) return c;
        c = a.MinPoint.CompareTo(b.MinPoint);
        return c;
    }
}
