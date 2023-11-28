using Microsoft.VisualBasic.FileIO;
using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;
using SkiaSharp;
using System.IO.Compression;

namespace OpenSvg.Gtfs;



public static class StopTimesParser
{

    public static IEnumerable<GtfsStopTime> Read(ZipArchiveEntry stopEntry)
    {

        using Stream stopStream = stopEntry.Open();
        using var parser = new TextFieldParser(stopStream);
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;

        if (!parser.EndOfData) parser.ReadLine();

        while (!parser.EndOfData)
        {
            string[]? fields = parser.ReadFields();
            if (fields == null) continue;

            string trip_id = fields.Length > 0 ? fields[0] : string.Empty;
            string arrival_time = fields.Length > 1 ? fields[1] : string.Empty;
            string departure_time = fields.Length > 2 ? fields[2] : string.Empty;
            string stop_id = fields.Length > 3 ? fields[3] : string.Empty;
            int stop_sequence = fields.Length > 4 ? fields[4].ParseNumber<int>() : 0;
            string stop_headsign = fields.Length > 5 ? fields[5] : string.Empty;
            string pickup_type = fields.Length > 6 ? fields[6] : string.Empty;


            string drop_off_type = fields.Length > 7 ? fields[7] : string.Empty;
            float shape_dist_traveled = fields.Length > 8 ? fields[8].ParseNumber<float>() : 0;
            string timepoint = fields.Length > 9 ? fields[9] : string.Empty;
            yield return new GtfsStopTime(trip_id, arrival_time, departure_time, stop_id, stop_sequence, stop_headsign, pickup_type, drop_off_type, shape_dist_traveled, timepoint);

        }

    }



}
