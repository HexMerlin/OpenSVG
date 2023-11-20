
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenSvg.GeoJson.Converters;
using OpenSvg.SvgNodes;
using System.Security.Cryptography.X509Certificates;

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

    public GeoJsonDocument(SvgDocument svgDocument, Coordinate startLocation, double metersPerPixel = 1, int segmentCountForCurveApproximation = 10)
    {
        PointConverter converter = new PointConverter(startLocation, metersPerPixel, segmentCountForCurveApproximation);
               
        var features = svgDocument.ToFeatures(Transform.Identity, converter).ToList();
        this.featureCollection = new FeatureCollection(features);    
        
    }
    public SvgDocument ToSvgDocument(double metersPerPixel = 10)
    {
        const int segmentCountForCurveApproximation = 10; //will not be used for conversion from GeoJson to Svg

        SvgDocument svgDocument = new SvgDocument();
        GeoJsonBoundingBox geoJsonBoundingBox = new GeoJsonBoundingBox(featureCollection);
        Coordinate startLocation = geoJsonBoundingBox.TopLeft;

        PointConverter converter = new PointConverter(startLocation, metersPerPixel, segmentCountForCurveApproximation);
        svgDocument.AddAll(featureCollection.Features.Select(f => ToSvgElement(f, converter)));
        return svgDocument;
    }

    private SvgElement ToSvgElement(Feature feature, PointConverter converter) => feature.Geometry switch
    {
        GeoJSON.Net.Geometry.LineString lineString when lineString.Coordinates.Count == 2 => SvgLineConverter.ToSvgLine(feature, converter),
        GeoJSON.Net.Geometry.LineString lineString when lineString.Coordinates.Count > 2 => SvgPolylineConverter.ToSvgPolyline(feature, converter),
        GeoJSON.Net.Geometry.Polygon polygon when polygon.Coordinates.Count == 1 => SvgPolygonConverter.ToSvgPolygon(feature, converter),
        GeoJSON.Net.Geometry.Polygon polygon when polygon.Coordinates.Count > 1 => EnclosedPolygonGroupConverter.ToEnclosedPolygonGroup(polygon, converter).ToSvgPolygonGroup(),
        GeoJSON.Net.Geometry.MultiPolygon multiPolygon => MultiPolygonConverter.ToMultiPolygon(multiPolygon, converter).ToSvgPolygonGroup(),
        GeoJSON.Net.Geometry.Point when feature.IsTextFeature() => SvgTextConverter.ToSvgText(feature, converter),
        GeoJSON.Net.Geometry.Point when !feature.IsTextFeature() => throw new NotSupportedException("Single points that are not text element are not supported by SVG"),
        _ => throw new NotSupportedException($"Unsupported GeoJSON Feature with geometry type {feature.Geometry.GetType().Name}"),
    };

    /// <summary>
    /// Loads a GeoJSON file and creates a GeoJsonDocument instance.
    /// </summary>
    /// <param name="geoJsonFilePath">Path to the GeoJSON file.</param>
    /// <returns>A new instance of GeoJsonDocument.</returns>
    public static GeoJsonDocument Load(string geoJsonFilePath)
    {
        using StreamReader file = File.OpenText(geoJsonFilePath);
        using JsonTextReader reader = new JsonTextReader(file);
        JsonSerializer serializer = new JsonSerializer();
        FeatureCollection? featureCollection = serializer.Deserialize<FeatureCollection>(reader);
        if (featureCollection != null)
            return new GeoJsonDocument(featureCollection);
        throw new Exception("Could not deserialize GeoJSON file.");
    }


    /// <summary>
    ///     Writes the GeoJSON representation of the feature collection to a file.
    /// </summary>
    /// <param name="geoJsonFilePath">The file path and name of the GeoJSON file write.</param>
    /// <param name="indentedFormatting">If set to <c>true</c>, the JSON output will be indented.</param>

    public void Save(string geoJsonFilePath, bool indentedFormatting = true)
    {
        using (StreamWriter file = File.CreateText(geoJsonFilePath))
        {
            JsonSerializer serializer = new JsonSerializer
            {
                Formatting = indentedFormatting ? Formatting.Indented : Formatting.None
            };
            serializer.Serialize(file, featureCollection);
        }
    }

}

