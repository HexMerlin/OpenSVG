
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenSvg.GeoJson.Converters;
using OpenSvg.SvgNodes;

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

    /// <summary>
    /// Loads a GeoJSON file and creates a GeoJsonDocument instance.
    /// </summary>
    /// <param name="geoJsonFilePath">Path to the GeoJSON file.</param>
    /// <returns>A new instance of GeoJsonDocument.</returns>
    public static GeoJsonDocument Load(string geoJsonFilePath)
    {
        throw new NotImplementedException(); 
        //using var reader = new StreamReader(geoJsonFilePath);
        //using var jsonReader = new JsonTextReader(reader);
        //var geoJsonReader = new GeoJsonReader();
        //var featureCollection = geoJsonReader.Read<FeatureCollection>(jsonReader);
        //return new GeoJsonDocument(featureCollection);
    }


    /// <summary>
    ///     Writes the GeoJSON representation of the feature collection to a file.
    /// </summary>
    /// <param name="geoJsonFilePath">The file path and name of the GeoJSON file write.</param>
    /// <param name="indentedFormatting">If set to <c>true</c>, the JSON output will be indented.</param>

    public void Save(string geoJsonFilePath, bool indentedFormatting = true)
    {
        string geoJsonString = JsonConvert.SerializeObject(featureCollection, indentedFormatting ? Formatting.Indented : Formatting.None);
        File.WriteAllText(geoJsonFilePath, geoJsonString);

    }

    ///// <summary>
    ///// Calculates the minimum and maximum coordinates of all geometries in the feature collection.
    ///// </summary>
    ///// <returns>A tuple containing the minimum and maximum coordinates.</returns>
    //public (Coordinate min, Coordinate max) GetBounds()
    //{
    //    double minLong = double.MaxValue;
    //    double maxLong = double.MinValue;
    //    double minLat = double.MaxValue;
    //    double maxLat = double.MinValue;

    //    foreach (Feature feature in featureCollection.Features)
    //    {
    //        var xxx = feature.Geometry.
    //        foreach (var coordinate in feature)
    //        {
    //            minLong = Math.Min(minLong, coordinate.X);
    //            maxLong = Math.Max(maxLong, coordinate.X);
    //            minLat = Math.Min(minLat, coordinate.Y);
    //            maxLat = Math.Max(maxLat, coordinate.Y);
    //        }
    //    }

    //    return (new Coordinate(minLong, minLat), new Coordinate(maxLong, maxLat));
    //}
}

