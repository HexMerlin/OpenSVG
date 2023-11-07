//File CommonTypes.cs

namespace OpenSvg;

/// <summary>
///     A type to define horizontal or vertical orientation
/// </summary>
public enum Orientation
{
    /// <summary>
    ///     Orient horizontally
    /// </summary>
    Horizontal,

    /// <summary>
    ///     Orient vertically
    /// </summary>
    Vertical
}

/// <summary>
///     Defines how rectangles should be rendered in SVG
///     Use SvgPolygon to be able to convert rectangle to GeoJson
/// </summary>
public enum RectangleRenderMode
{
    /// <summary>
    ///     Renders the rectangle as a SVG 'polygon' element
    /// </summary>
    SvgPolygon,

    /// <summary>
    ///     Renders the rectangle as a SVG 'rect' element
    /// </summary>
    SvgRect
}

/// <summary>
///     Defines how text should be rendered in SVG
/// </summary>
public enum TextRenderMode
{
    /// <summary>
    ///     Renders text as a SVG 'path' element
    /// </summary>
    SvgPath,

    /// <summary>
    ///     Renders the text as a SVG 'text' element
    /// </summary>
    SvgText
}