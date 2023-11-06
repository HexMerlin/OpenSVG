//File CommonTypes.cs

using OpenSvg.SvgNodes;

namespace OpenSvg;


/// <summary>
/// Specifies the horizontal alignment of a <see cref="SvgVisual"/> element relative to a reference element.
/// </summary>
/// <remarks>
/// The alignment determines the horizontal position of the element in relation to a reference bounding box.
/// <list type="bullet">
/// <item>
/// <term><see cref="HorizontalAlignment.InsideLeft"/></term>
/// <description>Align the left edge of the element with the left edge of the reference.</description>
/// </item>
/// <item>
/// <term><see cref="HorizontalAlignment.Center"/></term>
/// <description>Align the horizontal center of the element with the horizontal center of the reference.</description>
/// </item>
/// <item>
/// <term><see cref="HorizontalAlignment.InsideRight"/></term>
/// <description>Align the right edge of the element with the right edge of the reference.</description>
/// </item>
/// <item>
/// <term><see cref="HorizontalAlignment.OutsideLeft"/></term>
/// <description>Place the element to the left of the reference, aligning its right edge with the reference's left edge.</description>
/// </item>
/// <item>
/// <term><see cref="HorizontalAlignment.OutsideRight"/></term>
/// <description>Place the element to the right of the reference, aligning its left edge with the reference's right edge.</description>
/// </item>
/// </list>
/// </remarks>
/// <seealso cref="AlignRelativeTo"/>
public enum HorizontalAlignment
{
    InsideLeft,
    Center,
    InsideRight,
    OutsideLeft,
    OutsideRight
}

/// <summary>
/// Specifies the vertical alignment of a <see cref="SvgVisual"/> element relative to a reference element.
/// </summary>
/// <remarks>
/// The alignment determines the vertical position of the element in relation to a reference bounding box.
/// <list type="bullet">
/// <item>
/// <term><see cref="VerticalAlignment.InsideUp"/></term>
/// <description>Align the top edge of the element with the top edge of the reference.</description>
/// </item>
/// <item>
/// <term><see cref="VerticalAlignment.Center"/></term>
/// <description>Align the vertical center of the element with the vertical center of the reference.</description>
/// </item>
/// <item>
/// <term><see cref="VerticalAlignment.InsideDown"/></term>
/// <description>Align the bottom edge of the element with the bottom edge of the reference.</description>
/// </item>
/// <item>
/// <term><see cref="VerticalAlignment.OutsideUp"/></term>
/// <description>Place the element above the reference, aligning its bottom edge with the reference's top edge.</description>
/// </item>
/// <item>
/// <term><see cref="VerticalAlignment.OutsideDown"/></term>
/// <description>Place the element below the reference, aligning its top edge with the reference's bottom edge.</description>
/// </item>
/// </list>
/// </remarks>
/// <seealso cref="AlignRelativeTo"/>
public enum VerticalAlignment
{
    InsideUp,
    Center,
    InsideDown,
    OutsideUp,
    OutsideDown
}


/// <summary>
/// A type to define horizontal or vertical orientation
/// </summary>
public enum Orientation
{
    /// <summary>
    /// Orient horizontally
    /// </summary>
    Horizontal,

    /// <summary>
    /// Orient vertically
    /// </summary>
    Vertical
}

/// <summary>
///  Defines how rectangles should be rendered in SVG
///  Use SvgPolygon to be able to convert rectangle to GeoJson
/// </summary>
public enum RectangleRenderMode
{
    /// <summary>
    /// Renders the rectangle as a SVG 'polygon' element
    /// </summary>
    SvgPolygon,

    /// <summary>
    /// Renders the rectangle as a SVG 'rect' element
    /// </summary>
    SvgRect
}

/// <summary>
///  Defines how text should be rendered in SVG
/// </summary>
public enum TextRenderMode
{
    /// <summary>
    /// Renders text as a SVG 'path' element
    /// </summary>
    SvgPath,

    /// <summary>
    /// Renders the text as a SVG 'text' element
    /// </summary>
    SvgText
}
