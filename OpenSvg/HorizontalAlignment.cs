
using OpenSvg.SvgNodes;

namespace OpenSvg;


/// <summary>
///     Specifies the horizontal alignment of a <see cref="SvgVisual" /> element relative to a reference element.
/// </summary>
/// <remarks>
///     The alignment determines the horizontal position of the element in relation to a reference bounding box.
///     <list type="bullet">
///         <item>
///             <term>
///                 <see cref="HorizontalAlignment.InsideLeft" />
///             </term>
///             <description>Align the left edge of the element with the left edge of the reference.</description>
///         </item>
///         <item>
///             <term>
///                 <see cref="HorizontalAlignment.Center" />
///             </term>
///             <description>Align the horizontal center of the element with the horizontal center of the reference.</description>
///         </item>
///         <item>
///             <term>
///                 <see cref="HorizontalAlignment.InsideRight" />
///             </term>
///             <description>Align the right edge of the element with the right edge of the reference.</description>
///         </item>
///         <item>
///             <term>
///                 <see cref="HorizontalAlignment.OutsideLeft" />
///             </term>
///             <description>
///                 Place the element to the left of the reference, aligning its right edge with the reference's
///                 left edge.
///             </description>
///         </item>
///         <item>
///             <term>
///                 <see cref="HorizontalAlignment.OutsideRight" />
///             </term>
///             <description>
///                 Place the element to the right of the reference, aligning its left edge with the reference's
///                 right edge.
///             </description>
///         </item>
///     </list>
/// </remarks>
/// <seealso cref="SvgVisual"/>

public enum HorizontalAlignment
{
    /// <summary>
    /// Align the left edge of the element with the left edge of the reference.
    /// </summary>
    InsideLeft,
    /// <summary>
    /// Align the horizontal center of the element with the horizontal center of the reference.
    /// </summary>
    Center,
    /// <summary>
    /// Align the right edge of the element with the right edge of the reference.
    /// </summary>
    InsideRight,
    /// <summary>
    /// Place the element to the left of the reference, aligning its right edge with the reference's left edge.
    /// </summary>
    OutsideLeft,
    /// <summary>
    /// Place the element to the right of the reference, aligning its left edge with the reference's right edge.
    /// </summary>
    OutsideRight
}
