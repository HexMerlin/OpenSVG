using Microsoft.VisualBasic.FileIO;
using OpenSvg.Geographics;
using OpenSvg.SvgNodes;
using SkiaSharp;
using System.IO.Compression;

namespace OpenSvg.Gtfs;

public static class TripsParser
{


    public static IEnumerable<GtfsTrip> Read(ZipArchiveEntry tripsEntry)
    {
     
        using Stream tripStream = tripsEntry.Open();
        using var parser = new TextFieldParser(tripStream);
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;

        if (!parser.EndOfData) parser.ReadLine();

        while (!parser.EndOfData)
        {
            string[]? fields = parser.ReadFields();
            if (fields == null) continue;

            string route_id = fields.Length > 0 ? fields[0] : string.Empty;
            string service_id = fields.Length > 1 ? fields[1] : string.Empty;
            string trip_id = fields.Length > 2 ? fields[2] : string.Empty;
            string trip_headsign = fields.Length > 3 ? fields[3] : string.Empty;
            int direction_id = fields.Length > 4 ? fields[4].ParseNumber<int>() : 0;
            string shape_id = fields.Length > 5 ? fields[5] : string.Empty;
       

            yield return new GtfsTrip(route_id, service_id, trip_id, trip_headsign, direction_id, shape_id);

        }
    }


   

}
