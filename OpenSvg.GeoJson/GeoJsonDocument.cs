//file GeoJsonDocument.cs

using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenSvg;
using OpenSvg.Config;
using OpenSvg.SvgNodes;
using System;

namespace OpenSvg.GeoJson;

/// <summary>
/// Represents a GeoJSON document, containing features representing SVG elements.
/// </summary>
public class GeoJsonDocument
{
    /// <summary>
    /// Gets or sets a value indicating whether to throw an exception when an unsupported SVG element is encountered.
    /// </summary>
    public const bool ErrorOnUnsupportedElement = true;

    private readonly FeatureCollection featureCollection;


    public GeoJsonDocument(FeatureCollection featureCollection)
    {
        this.featureCollection = featureCollection;
    }

    /// <summary>
    /// Creates a GeoJSON document of type <see cref="GeoJsonDocument"/> from an <see cref="SvgDocument"/>.
    /// </summary>
    /// <param name="svgDocument">A SVG document.</param>
    /// <param name="startLocation">The world coordinate which corresponds to the upper-left corner of the SVG.</param>
    /// <param name="metersPerPixel">The scaling factor in meters per pixel.</param>
    /// <param name="coordinateRoundToDecimals">An optional value for the number of decimal places to round to when converting pixel coordinates to world coordinates.
    /// The default value of -1 means no rounding will occur.
    /// Rounding is typically only useful either
    /// <list type="bullet">
    /// <item>to simplify examples</item>
    /// <item>for diagnostic purposes, to simplify human reading</item>
    /// <item>when high precision is not really required</item>
    /// <item>to reduce memory size of resulting file size</item>
    /// </list>
    /// In all other cases, you can probably safely skip rounding.
    /// </param>

    public GeoJsonDocument(SvgDocument svgDocument, Coordinate startLocation, double metersPerPixel = 1, int segmentCountForCurveApproximation = 10, int coordinateRoundToDecimals = -1)
    {
        Size svgSize = svgDocument.BoundingBox.Size;

        PointToCoordinateConverter pointToCoordinateConverter = new PointToCoordinateConverter(startLocation, metersPerPixel, svgSize.Height, coordinateRoundToDecimals);

        this.featureCollection = new FeatureCollection();

        ParseSvgShape(svgDocument, Transform.Identity, segmentCountForCurveApproximation, pointToCoordinateConverter);
    }

    /// <summary>
    /// Loads a GeoJSON file and creates a GeoJsonDocument instance.
    /// </summary>
    /// <param name="geoJsonFilePath">Path to the GeoJSON file.</param>
    /// <returns>A new instance of GeoJsonDocument.</returns>
    public static GeoJsonDocument Load(string geoJsonFilePath)
    {
        using var reader = new StreamReader(geoJsonFilePath);
        using var jsonReader = new JsonTextReader(reader);
        var geoJsonReader = new GeoJsonReader();
        var featureCollection = geoJsonReader.Read<FeatureCollection>(jsonReader);
        return new GeoJsonDocument(featureCollection);
    }

    public SvgDocument ConvertToSvgDocument()
    {
        SvgDocument svgDocument = new SvgDocument();
        foreach (var feature in featureCollection)
        {
            switch (feature.Geometry)
            {
                case NetTopologySuite.Geometries.Point _:
                    throw new NotImplementedException("Conversion for Point not implemented.");

                case NetTopologySuite.Geometries.LineString _:
                    throw new NotImplementedException("Conversion for LineString not implemented.");

                case NetTopologySuite.Geometries.Polygon _:
                    throw new NotImplementedException("Conversion for Polygon not implemented.");

                case NetTopologySuite.Geometries.MultiPoint _:
                    throw new NotImplementedException("Conversion for MultiPoint not implemented.");

                case NetTopologySuite.Geometries.MultiLineString _:
                    throw new NotImplementedException("Conversion for MultiLineString not implemented.");

                case NetTopologySuite.Geometries.MultiPolygon _:
                    throw new NotImplementedException("Conversion for MultiPolygon not implemented.");

                case NetTopologySuite.Geometries.GeometryCollection _:
                    throw new NotImplementedException("Conversion for GeometryCollection not implemented.");

                default:
                    throw new NotSupportedException($"Unsupported geometry type: {feature.Geometry.GetType()}");
            }
        }

        return svgDocument;
    }


    private static AttributesTable GetDrawConfigAttributes(SvgVisual svgVisual)
    {
        static string GeoJsonColorString(SkiaSharp.SKColor color) =>
            color.IsTransparent() ? "rgba(0, 0, 0, 0)" : color.ToHexColorString();

        AttributesTable attributesTable = new();

        DrawConfig drawConfig = svgVisual.DrawConfig;

        attributesTable.Add(GeoJsonNames.Stroke, GeoJsonColorString(drawConfig.StrokeColor));
        attributesTable.Add(GeoJsonNames.StrokeWidth, drawConfig.StrokeWidth.ToXmlString());

        attributesTable.Add(GeoJsonNames.Fill, GeoJsonColorString(drawConfig.FillColor));
        attributesTable.Add(GeoJsonNames.FillOpacity, 1);
        return attributesTable;
    }

    private LinearRing GetLinearRing(PointToCoordinateConverter converter, Polygon polygon, Transform transform)
    {
        var coordinateList = polygon.Select(svgPoint => ToNativeCoordinate(converter, svgPoint, transform)).ToList();
        coordinateList.Add(coordinateList[0]); //close the ring
        return new LinearRing(coordinateList.ToArray());
    }

    private NetTopologySuite.Geometries.Coordinate ToNativeCoordinate(PointToCoordinateConverter converter, Point point, Transform transform)
    {
        Coordinate coord = converter.ConvertToCoordinate(point.Transform(transform));
        return new NetTopologySuite.Geometries.Coordinate(coord.Long, coord.Lat);
    }

    private void ParseSvgShape(SvgElement svgElement, Transform parentTransform, int segmentCountForCurveApproximation, PointToCoordinateConverter converter)
    {
        Transform composedTransform = svgElement is SvgVisual svgVisual ? parentTransform.ComposeWith(svgVisual.Transform.Get()) : parentTransform;

        switch (svgElement)
        {
            case SvgVisualContainer parent:
                foreach (SvgElement child in parent.ChildElements)
                    ParseSvgShape(child, composedTransform, segmentCountForCurveApproximation, converter);
                break;

            case SvgPolygon svgPolygon:

                Polygon polygon = svgPolygon.Polygon.Get();
                NetTopologySuite.Geometries.Polygon nativePolygon = new(GetLinearRing(converter, polygon, composedTransform));
                featureCollection.Add(new Feature(nativePolygon, GetDrawConfigAttributes(svgPolygon)));
                break;

            case SvgPath svgPath:
                {
                    featureCollection.Add(ConvertSvgPathToFeature(svgPath, composedTransform, segmentCountForCurveApproximation, converter));
                    break;
                }
            case SvgLine svgLine:
                {

                    var nativeLine = new LineString(new[] { ToNativeCoordinate(converter, svgLine.P1, composedTransform), ToNativeCoordinate(converter, svgLine.P2, composedTransform) });
                    featureCollection.Add(new Feature(nativeLine, GetDrawConfigAttributes(svgLine)));
                    break;
                }
            case SvgText svgText:
                {
                    // Extract position from the SVG text element

                    var textPoint = new Point(svgText.X.Get(), svgText.Y.Get());
                    NetTopologySuite.Geometries.Coordinate geoJsonTextCoordinate = ToNativeCoordinate(converter, textPoint, composedTransform);

                    // Create a GeoJSON Point for the text position
                    NetTopologySuite.Geometries.Point textPointNative = new(geoJsonTextCoordinate);

                    // Extract text content and other attributes

                    TextConfig textConfig = svgText.TextConfig;

                    double fontSizeScaled = textConfig.FontSize * converter.MetersPerPixel;

                    // Create a GeoJSON feature for the text
                    Feature textFeature = new(textPointNative, new AttributesTable
                    {
                        { GeoJsonNames.Text, textConfig.Text },
                        { GeoJsonNames.FontName, textConfig.SvgFont.FontName },
                        { GeoJsonNames.FontSize, fontSizeScaled.ToXmlString()  },
                        { GeoJsonNames.Fill, textConfig.DrawConfig.FillColor.ToHexColorString() },  //using fill color for text color in this element
                        { GeoJsonNames.FillOpacity, 1 }
                    });

                    featureCollection.Add(textFeature);
                    break;
                }
            default:
                if (ErrorOnUnsupportedElement) throw new NotSupportedException("Unsupported SVG element: " + svgElement.GetType());
        }
    }

    /// <summary>
    /// Converts an SvgPath to a GeoJSON feature.
    /// </summary>
    /// <param name="svgPath">The SvgPath to convert.</param>
    /// <param name="transform">The transformation object for the SvgPath.</param>
    /// <returns>The resulting GeoJSON feature.</returns>
    private Feature ConvertSvgPathToFeature(SvgPath svgPath, Transform transform, int segmentCountForCurveApproximation, PointToCoordinateConverter converter)
    {
        MultiPolygon multiPolygon = svgPath.ApproximateToMultiPolygon(segmentCountForCurveApproximation);

        List<NetTopologySuite.Geometries.Polygon> nativePolygons = new();

        foreach (EnclosedPolygonGroup enclosedPolygonGroup in multiPolygon)
        {
            LinearRing exteriorRing = GetLinearRing(converter, enclosedPolygonGroup.ExteriorPolygon, transform);

            List<LinearRing> interiorRings = enclosedPolygonGroup.InteriorPolygons.Select(p => GetLinearRing(converter, p, transform)).ToList();

            NetTopologySuite.Geometries.Polygon nativePolygon = new(exteriorRing, interiorRings.ToArray());

            nativePolygons.Add(nativePolygon);
        }

        // Create a GeoJSON MultiPolygon feature using the polygons
        NetTopologySuite.Geometries.MultiPolygon geoJsonMultiPolygon = new(nativePolygons.ToArray());

        Feature feature = new(geoJsonMultiPolygon, GetDrawConfigAttributes(svgPath));
        return feature;
    }

    /// <summary>
    /// Writes the GeoJSON representation of the feature collection to a file.
    /// </summary>
    /// <param name="filePath">The file path and name to write.</param>
    /// <param name="indentedFormatting">If set to <c>true</c>, the JSON output will be indented.</param>
    public void SaveToGeoJsonFile(string filePath, bool indentedFormatting = true)
    {
        GeoJsonWriter writer = new();
        string geoJsonString = writer.Write(featureCollection);

        if (indentedFormatting)
        {
            // Parse the JSON string to a JObject
            JObject jsonObject = JObject.Parse(geoJsonString);

            // Serialize the JObject with indented formatting
            geoJsonString = jsonObject.ToString(Formatting.Indented);
        }
        File.WriteAllText(filePath, geoJsonString);
    }

    /// <summary>
    /// Calculates the minimum and maximum coordinates of all geometries in the feature collection.
    /// </summary>
    /// <returns>A tuple containing the minimum and maximum coordinates.</returns>
    public (Coordinate min, Coordinate max) GetBounds()
    {
        double minLong = double.MaxValue;
        double maxLong = double.MinValue;
        double minLat = double.MaxValue;
        double maxLat = double.MinValue;

        foreach (IFeature? feature in featureCollection)
        {
            foreach (NetTopologySuite.Geometries.Coordinate? coordinate in feature.Geometry.Coordinates)
            {
                minLong = Math.Min(minLong, coordinate.X);
                maxLong = Math.Max(maxLong, coordinate.X);
                minLat = Math.Min(minLat, coordinate.Y);
                maxLat = Math.Max(maxLat, coordinate.Y);
            }
        }

        return (new Coordinate(minLong, minLat), new Coordinate(maxLong, maxLat));
    }
}

