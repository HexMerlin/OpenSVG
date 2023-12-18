using OpenSvg.SvgNodes;
using SkiaSharp;
using System.Net.Http.Headers;
using System.Numerics;

namespace OpenSvg.Optimization;
public partial class FastPolyline
{


   
    ///<summary>
    /// Reorders misplaced points in the polyline based on the angle threshold.
    /// </summary>
    /// <param name="angleThreshold">The angle threshold in degrees. Default is 120.</param>
    /// <returns>A new FastPolyline object with reordered points.
    /// </returns>
    public FastPolyline ReorderMisplacedPoints(float angleThreshold = 120)
    {
        if (Length <= 2)
            return this;
        if (angleThreshold < 0)
            throw new ArgumentException("Angle threshold must be positive", nameof(angleThreshold));
        if (!HasSharpTurns(angleThreshold)) //if there are no sharp turns, we leave the input as is
            return this;

        List<Point> pointList = new();

        for (int i = 0; i < Points.Length; i++)
        {
            if (pointList.Count > 2 && IsSharpTurn(pointList[^2], pointList[^1], Points[i], angleThreshold))
                InsertPointAtMinimumAngleIndex(pointList, Points[i], angleThreshold);
            else
                pointList.Add(Points[i]);
        }
        if (Point.DistanceSquared(Points[0], pointList[0]) > Point.DistanceSquared(Points[0], pointList[^1]))
            pointList.Reverse();

        var result = new FastPolyline(pointList);

        return result;
    }

    private static void InsertPointAtMinimumAngleIndex(List<Point> points, Point point, float angleThreshold)
    {
        if (points.Count < 2)
        {
            points.Add(point);
            return;
        }

        float minAngle = MathF.Abs(PointExtensions.GetAngle(point, points[0], points[1]));
        int insertIndex = 0;    
        
        for (int i = 1; i < points.Count; i++)
        {
            float newAngle1 = i >= 2 ? MathF.Abs(PointExtensions.GetAngle(points[i - 2], points[i-1], point)) : 0.0f;
            float newAngle2 = MathF.Abs(PointExtensions.GetAngle(points[i - 1], point, points[i]));
            float newAngle3 = i + 1 < points.Count ? MathF.Abs(PointExtensions.GetAngle(point, points[i], points[i + 1])) : 0.0f;
            float angle = MathF.Max(newAngle1, MathF.Max(newAngle2, newAngle3));

            if (angle < minAngle)
            {
                minAngle = angle;
                insertIndex = i;
            }
        }
        float lastAngle = MathF.Abs(PointExtensions.GetAngle(points[^2], points[^1], point));
        if (lastAngle < minAngle)
        {
            minAngle = lastAngle;
            insertIndex = points.Count;
        }
        if (minAngle < angleThreshold)
            points.Insert(insertIndex, point);

        //FastPolyline debugPolyline = new FastPolyline(points);
        //if (debugPolyline.ContainsSharpTurns())
        //{
        //    Console.WriteLine("Insert index = " + insertIndex);
        //    Console.WriteLine(debugPolyline.Points[0]);
        //    for (int i = 1; i < debugPolyline.Length - 1; i++)
        //    {
        //        Console.WriteLine(debugPolyline.Points[i] + "   Angle: " + FastPolyline.GetAngle(debugPolyline[i - 1], debugPolyline[i], debugPolyline[i + 1]) + " Dist: " + Vector2.DistanceSquared(debugPolyline.Points[i - 1], debugPolyline.Points[i]));
        //    }
        //    Console.WriteLine(debugPolyline.Points[^1]);


        //    SvgDocument svg = new SvgDocument();
        //    svg.StrokeColor = SKColors.Black;
        //    svg.StrokeWidth = 1f;
        //    svg.FillColor = SKColors.Transparent;

        //    foreach (var p in debugPolyline.Points)
        //    {
        //        SvgCircle circle = new SvgCircle();
        //        circle.Center = p;
        //        circle.Radius = 0.1f;
        //        svg.Add(circle);
        //    }

        //    svg.SetViewBoxToActualSizeAndDefaultViewPort();
        //    svg.Save($@"D:\Downloads\Test\Netex\error.svg");

        //    throw new Exception("Angle too large");
        //}

    }

    public void Dump_DEBUG_Svg()
    {
        SvgPath svgPath = this.ToSvgPath();
        svgPath.StrokeColor = SKColors.Black;
        svgPath.FillColor = SKColors.Transparent;
        svgPath.StrokeWidth = 5f;
        SvgDocument svg = svgPath.ToSvgDocument();
        svg.SetViewBoxToActualSizeAndDefaultViewPort();
        string filePath = $@"D:\Downloads\Test\Netex\error.svg";
        svg.Save(filePath);
        Console.WriteLine("WROTE FILE " + filePath);

    }

    public FastPolyline RemoveSharpTurns(float angleThreshold = 120)
    {
        if (Length <= 2)
            return this;

        List<Point> result = new();

        result.Add(this[0]);

        for (int i = 1; i < this.Length - 1; i++)
        {
            if (!IsSharpTurn(result[^1], this[i], this[i + 1], angleThreshold))
            {
                result.Add(this[i]);
            }
        }
        result.Add(this[^1]);

        return new FastPolyline(result);
    }

    public bool HasSharpTurns(float angleThreshold = 120)
    {
        if (Length <= 2)
            return false;
        for (int i = 1; i < Length - 1; i++)
            if (IsSharpTurn(Points[i-1], Points[i], Points[i+1], angleThreshold)) return true;
        return false;
    }




    public static bool IsSharpTurn(Point a, Point b, Point c, float angleThreshold = 120)
    {
        if (angleThreshold < 0)
            throw new ArgumentException("Angle threshold must be positive", nameof(angleThreshold));
        float angle = MathF.Abs(PointExtensions.GetAngle(a, b, c));
        return angle > angleThreshold; // Threshold angle
    }



    public static FastPolyline CreatePolyline(Point startingPoint, float[] turnAngles, float moveDistance = 10)
    {
        List<Point> points = new List<Point> { startingPoint };
        float currentAngle = 0; // Starting direction, adjust as needed

        foreach (float turnAngle in turnAngles)
        {
            currentAngle += turnAngle;

            Point lastPoint = points[^1];
            Point newPoint = lastPoint + Vector2FromAngle(currentAngle, moveDistance);
            points.Add(newPoint);
        }

        return new FastPolyline(points);
    }

    private static Vector2 Vector2FromAngle(float angleInDegrees, float length)
    {
        float angleInRadians = angleInDegrees * MathF.PI / 180f;

        return new Vector2(length * MathF.Cos(angleInRadians), length * MathF.Sin(angleInRadians));
    }

    //public float[] QualityScoresForPoints()
    //{
    //    var result = new float[Length];
    //    result[0] = 100f;
    //    result[^1] = 100f;
    //    for (int i = 1; i < Length - 2; i++)
    //    {
    //        result[i] = IsSharpTurn(Points[i - 1], Points[i], Points[i + 1]) ? 10f : 80f;
    //    }
    //    result = ApplyWeightedMovingAverage(result);
    //    return result;
    //}

    //public static float[] ApplyWeightedMovingAverage(float[] input)
    //{
    //    var result = new float[input.Length];

    //    for (int index = 0; index < input.Length; index++)
    //    {
    //        int before = Math.Min(2, index);
    //        int after = Math.Min(2, input.Length - index - 1);
    //        float centerWeight = (before, after) switch
    //        {
    //            (2, 2) => 4f / 10, //center weight when 2 points before and 2 points after
    //            (1, 2) => 4f / 9, //center weight when 1 point before and 2 points after
    //            (2, 1) => 4f / 9, //center weight when 2 points before and 1 point after
    //            (1, 1) => 1f / 2, //center weight when 1 point before and 1 point after
    //            (0, 2) => 4f / 7, //center weight when 0 points before and 2 points after
    //            (2, 0) => 4f / 7, //center weight when 2 points before and 0 points after
    //            (0, 1) => 2f / 3, //center weight when 0 points before and 1 point after
    //            (1, 0) => 2f / 3, //center weight when 1 point before and 0 points after
    //            _ => throw new ArgumentException("Invalid index")
    //        };
    //        if (before == 2)
    //            result[index - 2] += centerWeight / 4.0f * input[index];
    //        if (before >= 1)
    //            result[index - 1] += centerWeight / 2.0f * input[index];

    //        result[index] += centerWeight * input[index];

    //        if (after >= 1)
    //            result[index + 1] += centerWeight / 2.0f * input[index];
    //        if (after == 2)
    //            result[index + 2] += centerWeight / 4.0f * input[index];
    //    }
    //    return result;

    //}
}
