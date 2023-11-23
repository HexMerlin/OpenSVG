using GTFS;
using OpenSvg;
using OpenSvg.Attributes;
using OpenSvg.SvgNodes;


namespace OpenSvg.Gtfs;

public static class GtfsToSvg
{

    public static SvgDocument ToSvg(this GTFSFeed feed)
    {
        throw new NotImplementedException();
        //var svg = new SvgDocument();
        //var map = new SvgGroup();
        //svg.Add(map);

        //var stops = feed.Stops
        //    .Select(stop => new SvgCircle
        //    {
        //        Center = new(stop.Lon, stop.Lat),
        //        Radius = 0.0001,
        //        Fill = SvgColors.Red,
        //        Stroke = SvgColors.Black,
        //        StrokeWidth = 0.0001,
        //    });

        //map.Children.AddRange(stops);

        //var routes = feed.Routes
        //    .Select(route => new SvgGroup
        //    {
        //        Children =
        //        {
        //            new SvgPolygon
        //            {
        //                Points = route.Shape
        //                    .Select(shape => new SvgPoint(shape.Lon, shape.Lat))
        //                    .ToArray(),
        //                Fill = SvgColors.Transparent,
        //                Stroke = SvgColors.Black,
        //                StrokeWidth = 0.0001,
        //            },
        //            new SvgText
        //            {
        //                Position = new(route.Shape[0].Lon, route.Shape[0].Lat),
        //                Text = route.ShortName,
        //                FontSize = 0.0005,
        //                Fill = SvgColors.Black,
        //                Stroke = SvgColors.Black,
        //                StrokeWidth = 0.0001,
        //            },
        //        }
        //    });

        //map.Children.AddRange(routes);

        //return svg;
    }
}
