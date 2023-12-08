//using System.Collections.Immutable;
//using OpenSvg.Optimization;

//namespace OpenSvg.Netex;
//public class PolylineSet
//{
//    public HashSet<Point> EndPoints; 

//    public HashSet<FastPolyline> Polylines;

//    public PolylineSet(IEnumerable<FastPolyline> polylines)
//    {
//        this.Polylines = new HashSet<FastPolyline>(polylines);
//        this.EndPoints = GetEndPoints(this.Polylines);
//    }

//    public int Count => Polylines.Count;

//    public void Optimize()
//    {

//        int maxLen = Polylines.Max(p => p.Points.Length);

//        HashSet<FastPolyline> optimized = new HashSet<FastPolyline>();
//        int[,] matrix = new int[maxLen + 1, maxLen + 1];

//        void AddPolyline(FastPolyline p)
//        {
//            if (p.Length <= 4)
//                optimized.Add(p);
//            else
//                Polylines.Add(p);
//        }

//        while (Polylines.Any())
//        {   
//            FastPolyline polyline1 = Polylines.First();
//            Polylines.Remove(polyline1);
//            bool split = false;

//            foreach (FastPolyline polyline2 in Polylines)
//            {
//                SubstringResult result = FastPolyline.FindLongestCommonSubstring(polyline1, polyline2, matrix);  
//                if (result.SubstringLength > 30) //CHANGE THIS!!!! 
//                {
//                    int left1Len = result.StartIndex1;
//                    int left2Len = result.StartIndex2;
//                    int right1Len = polyline1.Points.Length - result.StartIndex1 - result.SubstringLength;
//                    int right2Len = polyline2.Points.Length - result.StartIndex2 - result.SubstringLength;
//                    if (left1Len == 1 || left2Len == 1 && right1Len == 1 && right2Len == 1)
//                        continue; //we do not want to split if any of the split parts becomes a single point

//                    split = true;
//                    Polylines.Remove(polyline2);
//                    //add the common substring (taken from the first polyline)
//                    AddPolyline(new FastPolyline(polyline1.Points.AsSpan(result.StartIndex1, result.SubstringLength).ToImmutableArray()));
//                    //add the other parts if length > 1
//                    if (left1Len > 1)
//                        AddPolyline(new FastPolyline(polyline1.Points.AsSpan(0, left1Len).ToImmutableArray()));
//                    if (left2Len > 1)
//                        AddPolyline(new FastPolyline(polyline2.Points.AsSpan(0, left2Len).ToImmutableArray()));
//                    if (right1Len > 1)
//                        AddPolyline(new FastPolyline(polyline1.Points.AsSpan(result.StartIndex1 + result.SubstringLength, right1Len).ToImmutableArray()));
//                    if (right2Len > 1)
//                        AddPolyline(new FastPolyline(polyline2.Points.AsSpan(result.StartIndex2 + result.SubstringLength, right2Len).ToImmutableArray()));
//                    break;
//                }
//            }   
//            if (!split)
//                optimized.Add(polyline1);

//            if (Polylines.Count % 10 == 0)
//                Console.WriteLine("Polylines remain: " + this.Polylines.Count);
//        }
//        this.Polylines = optimized;
       
//    }

//    private static HashSet<Point> GetEndPoints(IEnumerable<FastPolyline> polylines)
//    {
//        HashSet<Point> endPoints = new HashSet<Point>();
//        foreach (FastPolyline polyline in polylines)
//        {
//            endPoints.Add(polyline[0]);
//            endPoints.Add(polyline[^1]);
//        }
//        return endPoints;
//    }
//}
