namespace OpenSvg;

/// <summary>
/// This class contains string constants for all the names for SVG elements and attributes
/// </summary>
public static class SvgNames
{
    /// <summary>
    /// Namespace for SVG
    /// </summary>
    public const string SvgNamespace = "http://www.w3.org/2000/svg";



    #region SVG Elements

    public const string Unsupported = "unsupported";
    /// <summary>
    /// Defs element
    /// </summary>
    public const string Defs = "defs";

    /// <summary>
    /// Group element
    /// </summary>
    public const string Group = "g";

    /// <summary>
    /// Line element
    /// </summary>
    public const string Line = "line";

    /// <summary>
    /// Path element
    /// </summary>
    public const string Path = "path";

    /// <summary>
    /// Polygon element
    /// </summary>
    public const string Polygon = "polygon";

    /// <summary>
    /// Rect element
    /// </summary>
    public const string Rect = "rect";

    /// <summary>
    /// Style element
    /// </summary>
    public const string Style = "style";

    /// <summary>
    /// SVG element
    /// </summary>
    public const string Svg = "svg";
    /// <summary>
    /// Text element
    /// </summary>
    public const string Text = "text";
    #endregion SVG Elements

    #region SVG Attributes

    /// <summary>
    /// ID attribute
    /// </summary>
    public const string ID = "id";

    /// <summary>
    /// D attribute
    /// </summary>
    public const string D = "d";

    /// <summary>
    /// Dominant baseline attribute
    /// </summary>
    public const string DominantBaseline = "dominant-baseline";

    /// <summary>
    /// Fill attribute
    /// </summary>
    public const string Fill = "fill";

    /// <summary>
    /// Font family attribute
    /// </summary>
    public const string FontName = "font-family";

    /// <summary>
    /// Font size attribute
    /// </summary>
    public const string FontSize = "font-size";

    /// <summary>
    /// Height attribute
    /// </summary>
    public const string Height = "height";

    /// <summary>
    /// Points attribute
    /// </summary>
    public const string Points = "points";

    /// <summary>
    /// Rx attribute
    /// </summary>
    public const string Rx = "rx";

    /// <summary>
    /// Ry attribute
    /// </summary>
    public const string Ry = "ry";

    /// <summary>
    /// Stroke attribute
    /// </summary>
    public const string Stroke = "stroke";

    /// <summary>
    /// Stroke width attribute
    /// </summary>
    public const string StrokeWidth = "stroke-width";

    /// <summary>
    /// Text anchor attribute
    /// </summary>
    public const string TextAnchor = "text-anchor";

    /// <summary>
    /// Transformation attribute
    /// </summary>
    public const string Transform = "transform";

    /// <summary>
    /// Type attribute for Style element
    /// </summary>
    public const string Type = "type";

    /// <summary>
    /// Width attribute
    /// </summary>
    public const string Width = "width";
    /// <summary>
    /// X attribute
    /// </summary>
    public const string X = "x";

    /// <summary>
    /// X1 attribute
    /// </summary>
    public const string X1 = "x1";

    /// <summary>
    /// X2 attribute
    /// </summary>
    public const string X2 = "x2";

    /// <summary>
    /// Y attribute
    /// </summary>
    public const string Y = "y";
    /// <summary>
    /// Y1 attribute
    /// </summary>
    public const string Y1 = "y1";
    /// <summary>
    /// Y2 attribute
    /// </summary>
    public const string Y2 = "y2";
    #endregion SVG Attributes

    #region Attribute values

    /// <summary>
    /// CSS text attribute value
    /// </summary>
    public const string TextCss = "text/css";

    /// <summary>
    /// Translation attribute value
    /// </summary>
    public const string Translate = "translate";

    public const string Scale = "scale";

    public const string Rotate = "rotate";

    public const string SkewX = "skewX";

    public const string SkewY = "skewY";

    public const string Matrix = "matrix";

    #endregion Attribute values

    #region Special constants

    /// <summary>
    /// String for CSS font-face rule
    /// </summary>
    public const string FontFaceRule = "@font-face";

    /// <summary>
    /// Attribute for CSS font-face rule for font format
    /// </summary>
    public const string Format = "format";

    /// <summary> Value for <see cref="Format"/> for OTF fonts</summary>
    public const string OpenType = "opentype";

    /// <summary>
    /// String for transparent color
    /// </summary>
    public const string Transparent = "none";
    /// <summary> Value for <see cref="Format"/> for TTF fonts</summary>
    public const string TrueType = "truetype";

    #endregion Special constants
}