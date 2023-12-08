using System.Diagnostics;

namespace OpenSvg.Optimization;

public class LineSet
{ 
    public HashSet<Point> OriginalEndPoints = new HashSet<Point>();
    public HashSet<Point> AddedEndPoints = new HashSet<Point>();

    public readonly SortedSet<Line> ByMinPoint = new SortedSet<Line>(new LineComparerMinPoint());
    public readonly SortedSet<Line> ByMaxPoint = new SortedSet<Line>(new LineComparerMaxPoint());

    private readonly Point minPoint = new (float.MinValue, float.MinValue);
    private readonly Point maxPoint = new (float.MaxValue, float.MaxValue);

    public List<FastPolyline> NonOptimizedPolylines;
    public List<FastPolyline> OptimizedPolylines = new List<FastPolyline>();
  


    public int LineCount => ByMinPoint.Count;

    public LineSet(IEnumerable<FastPolyline> polylines)
    {
        NonOptimizedPolylines = polylines.ToList();
      
    }

    public void Optimize()
    {
        SplitPolylines();
        while (LineCount > 0)
        {
            Line line = ByMinPoint.Last();
            FastPolyline fastPolyline = ExtendBothDirections(line);
            OptimizedPolylines.Add(fastPolyline);

            //if (LineCount % 10 == 0)
            //    Console.WriteLine("Remaining lines: " + LineCount);

        }

    }

    public int TotalPointCount()
    {
        HashSet<Point> points = new HashSet<Point>();
        points.UnionWith(NonOptimizedPolylines.SelectMany(p => p.Points));
        points.UnionWith(OptimizedPolylines.SelectMany(p => p.Points));
        points.UnionWith(ByMinPoint.Select(l => l.MinPoint));
        points.UnionWith(ByMinPoint.Select(l => l.MaxPoint));
        return points.Count;
    }

    private void SplitPolylines()
    {
        var queue = this.NonOptimizedPolylines;
        this.NonOptimizedPolylines = new List<FastPolyline>();
        

        foreach (var polyline in queue)
        {
            if (polyline.Length < 2)
                continue; // Ignore malformed polylines with less than 2 points

          

            //Skip optimization for polylines with cycles (i.e. with duplicated points) 
            if (polyline.HasDuplicatedPoints())
            { 
                NonOptimizedPolylines.Add(polyline);
         

                continue;
            }

            OriginalEndPoints.Add(polyline[0]);
            OriginalEndPoints.Add(polyline[^1]);
            
            for (int i = 0; i < polyline.Length - 1; i++)
            {
                Line line = new Line(polyline[i], polyline[i + 1]);
                AddLine(line);
            }
        }
    }



    private FastPolyline ExtendBothDirections(Line line)
    {
        RemoveLine(line);
        var polyline = new FastPolyline(ExtendMany(line.MinPoint).Reverse().Concat(ExtendMany(line.MaxPoint)));
        AddEndPoint(polyline[0]);
        AddEndPoint(polyline[^1]);
        return polyline;
    }

    private IEnumerable<Point> ExtendMany(Point point)
    {
        Point current = point;
        yield return current;

        while (true)
        {
            (bool success, Point newPoint) = TryExtendOne(current);
            if (!success)
                yield break;

            current = newPoint;
            yield return current;
            if (IsEndPoint(current))
                yield break;
        }
    }

    private (bool success, Point point) TryExtendOne(Point point)
    {
        SortedSet<Line> withMinPoint = GetLinesWithMinPoint(point);
 
        if (withMinPoint.Count > 1)
            return (false, Point.Zero);
        SortedSet<Line> withMaxPoint = GetLinesWithMaxPoint(point);
      

        if (withMinPoint.Count + withMaxPoint.Count != 1)
            return (false, Point.Zero);
        
        (Line line, Point newPoint) = withMinPoint.Count == 1 ? 
            (withMinPoint.First(), withMinPoint.First().MaxPoint) 
            : (withMaxPoint.First(), withMaxPoint.First().MinPoint);
        RemoveLine(line);
        return (true, newPoint);
    }


    public bool IsEndPoint(Point point) => OriginalEndPoints.Contains(point) || AddedEndPoints.Contains(point);

    public void AddEndPoint(Point point)
    {
        if (OriginalEndPoints.Contains(point))
            return;
        AddedEndPoints.Add(point);
    }

    public void AddLine(Line line)
    {
        ByMinPoint.Add(line);
        ByMaxPoint.Add(line);
    }

    public void RemoveLine(Line line)
    {
        ByMinPoint.Remove(line);
        ByMaxPoint.Remove(line);
    }



    public SortedSet<Line> GetLinesWithMinPoint(Point point)
        => ByMinPoint.GetViewBetween(new Line(point, point, Line.SkipSort.Yes), new Line(point, maxPoint, Line.SkipSort.Yes));

    public SortedSet<Line> GetLinesWithMaxPoint(Point point)
        => ByMaxPoint.GetViewBetween(new Line(minPoint, point, Line.SkipSort.Yes), new Line(point, point, Line.SkipSort.Yes));

}
