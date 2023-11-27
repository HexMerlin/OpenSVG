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

        MathF.PI.ToString();


        GtfsFeed gtfsFeed = GtfsFeed.Load(@"D:\Downloads\Test\GTFS\skane.zip");

        AllPoints allPoints = new AllPoints(gtfsFeed);

        //SvgDocument svgDocument = gtfsFeed.ToSvgDocument();
        //svgDocument.Save(@"D:\Downloads\Test\GTFS\skane.svgz");



        //GeoJsonDocument geoJsonDocument1 = GeoJsonDocument.Load(@"D:\Downloads\Test\legend.geojson");
        //GeoJsonDocument geoJsonDocument2 = GeoJsonDocument.Load(@"D:\Downloads\Test\otraf.geojson");
        //SvgDocument svgDocument1 = geoJsonDocument1.ToSvgDocument();
        //SvgDocument svgDocument2 = geoJsonDocument2.ToSvgDocument();

        //svgDocument1.Save(@"D:\Downloads\Test\legend.svg");
        //svgDocument2.Save(@"D:\Downloads\Test\otraf.svg");



    }
}