using OpenSvg.PathOptimize;
using System.Collections.Immutable;
using System.ComponentModel.Design.Serialization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OpenSvg.Optimization;

public class LineSet
{ 
    public readonly SortedSet<Line> ByMinPoint = new SortedSet<Line>(new LineComparerMinPoint());
    public readonly SortedSet<Line> ByMaxPoint = new SortedSet<Line>(new LineComparerMaxPoint());

    private readonly Point minPoint = new (float.MinValue, float.MinValue);
    private readonly Point maxPoint = new (float.MaxValue, float.MaxValue);
    
    public LineSet()
    {
        
    }

    public int Count => ByMinPoint.Count;


    public static List<Point> NoiseRemoval(Polyline polyline, float minDistanceThreshold)
    {
        var points = polyline.ToList();
        points = RDPA.RemoveClosePoints(points, minDistanceThreshold);
        if (points.Count <= 1)
            return new List<Point>(); // Ignore polylines with less than 2 points
        return points;
       
    }

    public void Add(Polyline polyline)
    {
        const int fractionalDigits = 2;
        const float minDistance = 1f;

        static Point Normalize(Point point)
        { // 
          //bool isP1Min = p1.CompareTo(p2) < 0;

           // return new Point(MathF.Round(point.X, fractionalDigits), MathF.Round(point.Y, fractionalDigits));
            return point;
        }
        var points = polyline.ToList();
        points = RDPA.RemoveClosePoints(points, 0.01f);
        if (points.Count < 2)
            return; // Ignore polylines with less than 2 points

        for (int i = 0; i < points.Count - 1; i++)
        {
            Point p1 = Normalize(points[i]);
            Point p2 = Normalize(points[i + 1]);

            bool isP1Min = p1.CompareTo(p2) < 0;
            var minPoint = isP1Min ? p1 : p2;
            var maxPoint = isP1Min ? p2 : p1;
            Line line = new Line(minPoint, maxPoint);
            Add(line);
        }
    }

   
    public IEnumerable<Polyline> Optimize()
    {
        Console.WriteLine("INITIAL POINT COUNT: " + Count);
        List<Point> pointBuffer = new List<Point>();
        List<Line> lineBuffer = new List<Line>();

        int debugPointCreatedCount = 0;
        while (Count > 0)
        {
            if (Count % 5000 == 0)
                Console.WriteLine("Remaining points: " + Count);
            Line line = ByMinPoint.ElementAt(0);
            Remove(line);

            lineBuffer.Clear();
            lineBuffer.AddRange(Traverse(line));

            if (lineBuffer.Count == 1)
            {
                yield return new Polyline(new Point[] { lineBuffer[0].MinPoint, lineBuffer[0].MaxPoint });
                continue;
            }

            Line l0 = lineBuffer[0];
            Line l1 = lineBuffer[1];

            Point point = l0.MinPoint == l1.MinPoint || l0.MinPoint == l1.MaxPoint ? l0.MaxPoint : l0.MinPoint;
            pointBuffer.Add(point);

            // Console.WriteLine("Selected point " + point);
            for (int i = 0; i < lineBuffer.Count; i++)
            {
                Line l = lineBuffer[i];
                //if (point.CompareTo(l.MinPoint) != 0 && point.CompareTo(l.MaxPoint) != 0)
                point = point == l.MinPoint ? l.MaxPoint : l.MinPoint;
                pointBuffer.Add(point);

            }

            //if (lineBuffer.Count < 4)
            //{
            //    for (int i = 0; i < lineBuffer.Count; i++)
            //    {
            //        Point p = pointBuffer[i];
            //        Line l = lineBuffer[i];

            //        if (p != l.MinPoint && p != l.MaxPoint)
            //        {
            //            Console.WriteLine("Index: " + i);
            //            Console.WriteLine("Line Buffer");
            //            Console.WriteLine(string.Join("\n", lineBuffer.Select(l => l.ToString())));
            //            Console.WriteLine("\nPoint Buffer");
            //            Console.WriteLine(string.Join("\n", pointBuffer.Select(p => p.ToString())));
            //            throw new Exception("Bad point 1");

            //        }
            //    }
            //}
        
        

            //if (pointBuffer[^1] != lineBuffer[^1].MinPoint && pointBuffer[^1] != lineBuffer[^1].MaxPoint)
            //    throw new Exception("Bad point 2");

            debugPointCreatedCount += pointBuffer.Count;
            yield return new Polyline(pointBuffer.ToImmutableArray());
            pointBuffer.Clear();
        }
        Console.WriteLine("OPTIMIZED POINT COUNT: " + debugPointCreatedCount);
    }

  

    public void Add(Line line)
    {
        ByMinPoint.Add(line);
        ByMaxPoint.Add(line);
    }

    public void Remove(Line line)
    {
        ByMinPoint.Remove(line);
        ByMaxPoint.Remove(line);
    }

    public int WithMinPointCount(Point point) => GetLinesWithMinPoint(point).Count;

    public int WithMaxPointCount(Point point) => GetLinesWithMaxPoint(point).Count;

    IEnumerable<Line> Traverse(Line line)
    {
        //foreach (var l in TraverseAll(line.MinPoint))
        //    yield return l;
        //yield return line;
        //foreach (var l in TraverseAll(line.MaxPoint))
        //    yield return l;
        var l1 = TraverseAll(line.MinPoint).ToList();
        AssertConsecutive(l1);
        var l2 = TraverseAll(line.MaxPoint).ToList();
        AssertConsecutive(l2);
        foreach (var l in l1)
            yield return l;
        yield return line;
        foreach (var l in l2)
            yield return l;
    }

    public IEnumerable<Line> TraverseAll(Point point)
    {
        while (true)
        {
            (bool success, Line line) = GetAndRemoveLineWithUniquePoint(point);
            if (success)
            {
                yield return line;
                point = point == line.MinPoint ? line.MaxPoint : line.MinPoint;
                continue;
            }
            yield break;
        }
    }

    public void AssertConsecutive(IEnumerable<Line> lines)
    {
        static bool IsConnected(Line l1, Line l2)
            => l1.MinPoint == l2.MinPoint || 
               l1.MaxPoint == l2.MinPoint || 
               l1.MinPoint == l2.MaxPoint || 
               l1.MaxPoint == l2.MaxPoint;
        foreach (var line in lines)
        {
            if (line.MinPoint == line.MaxPoint)
                throw new Exception("Line has zero length");
        }
        foreach (var (l1, l2) in lines.Zip(lines.Skip(1)))
        {
            if (!IsConnected(l1, l2))
                throw new Exception("Lines are not consecutive");
        }
    }

    public (bool success, Line line) GetAndRemoveLineWithUniquePoint(Point point)
    {
        
        SortedSet<Line> lines1 = GetLinesWithMinPoint(point);
        if (lines1.Count > 1)
            return (false, Line.Zero);
        SortedSet<Line> lines2 = GetLinesWithMaxPoint(point);
        if (lines2.Count > 1)
            return (false, Line.Zero);
        if (lines1.Count == 0 && lines2.Count == 0)
            return (false, Line.Zero);

        Line line = lines1.Count == 1 ? lines1.ElementAt(0) : lines2.ElementAt(0);   
        Remove(line);
        return (true, line);
    }


    public SortedSet<Line> GetLinesWithMinPoint(Point point)
    {
        var res = ByMinPoint.GetViewBetween(Line.CreateUnchecked(point, minPoint), Line.CreateUnchecked(point, maxPoint));
        return res;
    }

    public SortedSet<Line> GetLinesWithMaxPoint(Point point)
    {
        var res = ByMaxPoint.GetViewBetween(Line.CreateUnchecked(minPoint, point), Line.CreateUnchecked(maxPoint, point));
        return res;
    }





}
