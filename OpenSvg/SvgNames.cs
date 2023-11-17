namespace OpenSvg;


/// <summary>
///     This class contains string constants for all the names for SVG elements and attributes
/// </summary>
public static class SvgNames
{
    /// <summary>
    ///     Namespace for SVG
    /// </summary>
    public const string SvgNamespace = "http://www.w3.org/2000/svg";


    #region SVG Elements

    /// <summary>
    ///     Element for unsupported SVG elements
    /// </summary>
    public const string Unsupported = "unsupported";

    /// <summary>
    ///     Defs element
    /// </summary>
    /// <remarks>
    ///     Defines a section in which definitions for SVG elements are stored.
    /// </remarks>
    public const string Defs = "defs";

    /// <summary>
    ///     Group element
    /// </summary>
    /// <remarks>
    ///     Defines a group of SVG elements.
    /// </remarks>
    public const string Group = "g";

    /// <summary>
    ///     Line element
    /// </summary>
    /// <remarks>
    ///     Defines a line in SVG.
    /// </remarks>
    public const string Line = "line";

    /// <summary>
    ///     Path element
    /// </summary>
    /// <remarks>
    ///     Defines a path in SVG.
    /// </remarks>
    public const string Path = "path";

    /// <summary>
    ///     Polygon element
    /// </summary>
    /// <remarks>
    ///     Defines a polygon in SVG.
    /// </remarks>
    public const string Polygon = "polygon";

    /// <summary>
    ///     Polyline element
    /// </summary>
    /// <remarks>
    ///     Defines a polyline in SVG.
    /// </remarks>
    public const string Polyline = "polyline";

    /// <summary>
    ///     Rect element
    /// </summary>
    /// <remarks>
    ///     Defines a rectangle in SVG.
    /// </remarks>
    public const string Rect = "rect";

    /// <summary>
    ///     Style element
    /// </summary>
    /// <remarks>
    ///     Defines a style for SVG elements.
    /// </remarks>
    public const string Style = "style";

    /// <summary>
    ///     SVG element
    /// </summary>
    /// <remarks>
    ///     Defines an SVG document.
    /// </remarks>
    public const string Svg = "svg";

    /// <summary>
    ///     Text element
    /// </summary>
    /// <remarks>
    ///     Defines a text in SVG.
    /// </remarks>
    public const string Text = "text";

    #endregion SVG Elements

    #region SVG Attributes

    /// <summary>
    ///     ID attribute
    /// </summary>
    /// <remarks>
    ///     Defines an identifier for SVG elements.
    /// </remarks>
    public const string ID = "id";

    /// <summary>
    ///     D attribute
    /// </summary>
    /// <remarks>
    ///     Defines a path data for SVG elements.
    /// </remarks>
    public const string D = "d";

    /// <summary>
    ///     Dominant baseline attribute
    /// </summary>
    /// <remarks>
    ///     Defines a dominant baseline for SVG text elements.
    /// </remarks>
    public const string DominantBaseline = "dominant-baseline";

    /// <summary>
    ///     Fill attribute
    /// </summary>
    /// <remarks>
    ///     Defines a fill color for SVG elements.
    /// </remarks>
    public const string Fill = "fill";

    /// <summary>
    ///     Font family attribute
    /// </summary>
    /// <remarks>
    ///     Defines a font family for SVG text elements.
    /// </remarks>
    public const string FontName = "font-family";

    /// <summary>
    ///     Font size attribute
    /// </summary>
    /// <remarks>
    ///     Defines a font size for SVG text elements.
    /// </remarks>
    public const string FontSize = "font-size";

    /// <summary>
    ///     Height attribute
    /// </summary>
    /// <remarks>
    ///     Defines a height for SVG elements.
    /// </remarks>
    public const string Height = "height";

    /// <summary>
    ///     Points attribute
    /// </summary>
    /// <remarks>
    ///     Defines a set of points for SVG polygon elements.
    /// </remarks>
    public const string Points = "points";

    /// <summary>
    ///     Rx attribute
    /// </summary>
    /// <remarks>
    ///     Defines a horizontal radius for SVG ellipse elements.
    /// </remarks>
    public const string Rx = "rx";

    /// <summary>
    ///     Ry attribute
    /// </summary>
    /// <remarks>
    ///     Defines a vertical radius for SVG ellipse elements.
    /// </remarks>
    public const string Ry = "ry";

    /// <summary>
    ///     Stroke attribute
    /// </summary>
    /// <remarks>
    ///     Defines a stroke color for SVG elements.
    /// </remarks>
    public const string Stroke = "stroke";

    /// <summary>
    ///     Stroke width attribute
    /// </summary>
    /// <remarks>
    ///     Defines a stroke width for SVG elements.
    /// </remarks>
    public const string StrokeWidth = "stroke-width";

    /// <summary>
    ///     Text anchor attribute
    /// </summary>
    /// <remarks>
    ///     Defines a text anchor for SVG text elements.
    /// </remarks>
    public const string TextAnchor = "text-anchor";

    /// <summary>
    ///     Transformation attribute
    /// </summary>
    /// <remarks>
    ///     Defines a transformation for SVG elements.
    /// </remarks>
    public const string Transform = "transform";

    /// <summary>
    ///     Type attribute for Style element
    /// </summary>
    /// <remarks>
    ///     Defines a type for SVG style elements.
    /// </remarks>
    public const string Type = "type";

    /// <summary>
    ///     Width attribute
    /// </summary>
    /// <remarks>
    ///     Defines a width for SVG elements.
    /// </remarks>
    public const string Width = "width";

    /// <summary>
    ///     X attribute
    /// </summary>
    /// <remarks>
    ///     Defines a horizontal position for SVG elements.
    /// </remarks>
    public const string X = "x";

    /// <summary>
    ///     X1 attribute
    /// </summary>
    /// <remarks>
    ///     Defines a horizontal position for SVG line elements.
    /// </remarks>
    public const string X1 = "x1";

    /// <summary>
    ///     X2 attribute
    /// </summary>
    /// <remarks>
    ///     Defines a horizontal position for SVG line elements.
    /// </remarks>
    public const string X2 = "x2";

    /// <summary>
    ///     Y attribute
    /// </summary>
    /// <remarks>
    ///     Defines a vertical position for SVG elements.
    /// </remarks>
    public const string Y = "y";

    /// <summary>
    ///     Y1 attribute
    /// </summary>
    /// <remarks>
    ///     Defines a vertical position for SVG line elements.
    /// </remarks>
    public const string Y1 = "y1";

    /// <summary>
    ///     Y2 attribute
    /// </summary>
    /// <remarks>
    ///     Defines a vertical position for SVG line elements.
    /// </remarks>
    public const string Y2 = "y2";

    #endregion SVG Attributes

    #region Attribute values

    /// <summary>
    ///     CSS text attribute value
    /// </summary>
    public const string TextCss = "text/css";

    /// <summary>
    ///     Translation attribute value
    /// </summary>
    public const string Translate = "translate";

    /// <summary>
    ///     Scale attribute value
    /// </summary>
    public const string Scale = "scale";

    /// <summary>
    ///     Rotate attribute value
    /// </summary>
    public const string Rotate = "rotate";

    /// <summary>
    ///     SkewX attribute value
    /// </summary>
    public const string SkewX = "skewX";

    /// <summary>
    ///     SkewY attribute value
    /// </summary>
    public const string SkewY = "skewY";

    /// <summary>
    ///     Matrix attribute value
    /// </summary>
    public const string Matrix = "matrix";

    #endregion Attribute values

    #region Special constants

    /// <summary>
    ///     String for CSS font-face rule
    /// </summary>
    public const string FontFaceRule = "@font-face";

    /// <summary>
    ///     Attribute for CSS font-face rule for font format
    /// </summary>
    public const string Format = "format";

    /// <summary> Value for <see cref="Format" /> for OTF fonts</summary>
    public const string OpenType = "opentype";

    /// <summary>
    ///     String for transparent color
    /// </summary>
    public const string Transparent = "none";

    /// <summary> Value for <see cref="Format" /> for TTF fonts</summary>
    public const string TrueType = "truetype";

    #endregion Special constants
}
