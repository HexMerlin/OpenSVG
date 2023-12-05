using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using OpenSvg.GeoJson;
using GeoPoint = GeoJSON.Net.Geometry.Point;

public class GeoJsonBoundingBox
{
    public double MinLongitude { get; private set; } = double.MaxValue;
    public double MaxLongitude { get; private set; } = double.MinValue;
    public double MinLatitude { get; private set; } = double.MaxValue;
    public double MaxLatitude { get; private set; } = double.MinValue;

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

    public GeoJsonBoundingBox(FeatureCollection featureCollection)
    {
        foreach (var feature in featureCollection.Features)
            UpdateBoundingBox(feature.Geometry);
    }

    private void UpdateBoundingBox(IGeometryObject geometry)
    {
        switch (geometry)
        {
            case GeoPoint point:
                UpdateBounds(point.Coordinates);
                break;
            case MultiPoint multiPoint:
                UpdateBounds(multiPoint);
                break;
            case LineString lineString:
                UpdateBounds(lineString);
                break;
            case MultiLineString multiLineString:
                UpdateBounds(multiLineString);
                break;
            case Polygon polygon:
                UpdateBounds(polygon);
                break;
            case MultiPolygon multiPolygon:
                UpdateBounds(multiPolygon);
                break;
            case GeometryCollection geometryCollection:
                foreach (var geom in geometryCollection.Geometries)
                    UpdateBoundingBox(geom);
                break;
            default:
                throw new NotSupportedException($"Unsupported GeoJSON geometry type {geometry.GetType().Name}");
        }
    }


    private void UpdateBounds(MultiPoint multiPoint) => UpdateBounds(multiPoint.Coordinates.Select(p => p.Coordinates));
  
    private void UpdateBounds(LineString lineString) => UpdateBounds(lineString.Coordinates);


    private void UpdateBounds(Polygon polygon) => UpdateBounds(polygon.Coordinates.SelectMany(ring => ring.Coordinates));

    private void UpdateBounds(MultiLineString multiLineString) => UpdateBounds(multiLineString.Coordinates.SelectMany(line => line.Coordinates));

    private void UpdateBounds(MultiPolygon multiPolygon) => UpdateBounds(multiPolygon.Coordinates.SelectMany(poly => poly.Coordinates.SelectMany(ring => ring.Coordinates)));

    private void UpdateBounds(IEnumerable<IPosition> coords)
    {
        foreach (IPosition coord in coords) 
            UpdateBounds(coord);
    }

    
    private void UpdateBounds(IPosition coord)
    {
        MinLongitude = Math.Min(MinLongitude, coord.Longitude);
        MaxLongitude = Math.Max(MaxLongitude, coord.Longitude);
        MinLatitude = Math.Min(MinLatitude, coord.Latitude);
        MaxLatitude = Math.Max(MaxLatitude, coord.Latitude);
    }
}
