using GeoJSON.Net.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSvg.GeoJson;
public class GeoBoundingBox
{
    public double MinLongitude { get; } = double.MaxValue;
    public double MaxLongitude { get; } = double.MinValue;
    public double MinLatitude { get; } = double.MaxValue;
    public double MaxLatitude { get; } = double.MinValue;

    /// <summary>
    /// Gets the coordinate of the top-left corner of the bounding box.
    /// In geographical terms, this represents the point with the minimum longitude 
    /// (westernmost) and the maximum latitude (northernmost).
    /// </summary>
    /// <value>The geographical coordinate of the top-left corner.</value>
    public Coordinate TopLeft => new Coordinate(MinLongitude, MaxLatitude);

    /// <summary>
    /// Gets the coordinate of the top-right corner of the bounding box.
    /// In geographical terms, this represents the point with the maximum longitude 
    /// (easternmost) and the maximum latitude (northernmost).
    /// </summary>
    /// <value>The geographical coordinate of the top-right corner.</value>
    public Coordinate TopRight => new Coordinate(MaxLongitude, MaxLatitude);

    /// <summary>
    /// Gets the coordinate of the bottom-left corner of the bounding box.
    /// In geographical terms, this represents the point with minimum longitude 
    /// (westernmost) and the minimum latitude (southernmost).
    /// </summary>
    /// <value>The geographical coordinate of the bottom-left corner.</value>
    public Coordinate BottomLeft => new Coordinate(MinLongitude, MinLatitude);

    /// <summary>
    /// Gets the coordinate of the bottom-right corner of the bounding box.
    /// In geographical terms, this represents the point with the maximum longitude 
    /// (easternmost) and the minimum latitude (southernmost).
    /// </summary>
    /// <value>The geographical coordinate of the bottom-right corner.</value>
    public Coordinate BottomRight => new Coordinate(MaxLongitude, MinLatitude);

    public GeoBoundingBox(IEnumerable<Coordinate> coordinates)
    {
        int count = 0;
        foreach (Coordinate coordinate in coordinates)
        {
           MinLongitude = Math.Min(MinLongitude, coordinate.Long);
           MaxLongitude = Math.Max(MaxLongitude, coordinate.Long);
           MinLatitude = Math.Min(MinLatitude, coordinate.Lat);
           MaxLatitude = Math.Max(MaxLatitude, coordinate.Lat);
            count++;
        }
        if (count < 2)
            throw new ArgumentException("At least 2 coordinates required for computing a GeoBoundingBox.");
    }

    public Coordinate[] Corners() => new Coordinate[] { TopLeft, TopRight, BottomRight, BottomLeft };

    public static GeoBoundingBox Union(GeoBoundingBox bounds1, GeoBoundingBox bounds2)
        => new GeoBoundingBox(bounds1.Corners().Concat(bounds2.Corners()));
       

}
