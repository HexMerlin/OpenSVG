global using Point = System.Numerics.Vector2;
using System;
using System.Collections.Immutable;
using SkiaSharp;
using OpenSvg.Gtfs;
using OpenSvg.Config;
using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;
using Microsoft.VisualBasic.FileIO;
using OpenSvg.Gtfs.Optimized;
using System.Numerics;
using System.Data.SqlTypes;

namespace OpenSvg.ConsoleTest;

internal class Program
{

    public static void Main()
    {

        Point[] points = new Point[] { new Point(0, 10), new Point(20, 5) };
        Array.Sort(points, PointComparer.Default);
        Console.WriteLine(String.Join(", ", points));
        /// 

        GtfsFeed gtfsFeed = GtfsFeed.Load(@"D:\Downloads\Test\GTFS\skane.zip");
        gtfsFeed.JoinDataSources();

       // AllPoints allPoints = new AllPoints(gtfsFeed);

        SvgDocument svgDocument = gtfsFeed.ToSvgDocument();
        svgDocument.Save(@"D:\Downloads\Test\GTFS\skane.svg");

        
       

        //GeoJsonDocument geoJsonDocument1 = GeoJsonDocument.Load(@"D:\Downloads\Test\legend.geojson");
        //GeoJsonDocument geoJsonDocument2 = GeoJsonDocument.Load(@"D:\Downloads\Test\otraf.geojson");
        //SvgDocument svgDocument1 = geoJsonDocument1.ToSvgDocument();
        //SvgDocument svgDocument2 = geoJsonDocument2.ToSvgDocument();

        //svgDocument1.Save(@"D:\Downloads\Test\legend.svg");
        //svgDocument2.Save(@"D:\Downloads\Test\otraf.svg");



    }
}