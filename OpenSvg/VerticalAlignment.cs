//File CommonTypes.cs

using OpenSvg.SvgNodes;

namespace OpenSvg;


/// <summary>
///     Specifies the vertical alignment of a <see cref="SvgVisual" /> element relative to a reference element.
/// </summary>
/// <remarks>
///     The alignment determines the vertical position of the element in relation to a reference bounding box.
///     <list type="bullet">
///         <item>
///             <term>
///                 <see cref="VerticalAlignment.InsideUp" />
///             </term>
///             <description>Align the top edge of the element with the top edge of the reference.</description>
///         </item>
///         <item>
///             <term>
///                 <see cref="VerticalAlignment.Center" />
///             </term>
///             <description>Align the vertical center of the element with the vertical center of the reference.</description>
///         </item>
///         <item>
///             <term>
///                 <see cref="VerticalAlignment.InsideDown" />
///             </term>
///             <description>Align the bottom edge of the element with the bottom edge of the reference.</description>
///         </item>
///         <item>
///             <term>
///                 <see cref="VerticalAlignment.OutsideUp" />
///             </term>
///             <description>Place the element above the reference, aligning its bottom edge with the reference's top edge.</description>
///         </item>
///         <item>
///             <term>
///                 <see cref="VerticalAlignment.OutsideDown" />
///             </term>
///             <description>Place the element below the reference, aligning its top edge with the reference's bottom edge.</description>
///         </item>
///     </list>
/// </remarks>
/// <seealso cref="SvgVisual"/>
public enum VerticalAlignment
{
    /// <summary>
    /// Align the top edge of the element with the top edge of the reference.
    /// </summary>
    InsideUp,
    /// <summary>
    /// Align the vertical center of the element with the vertical center of the reference.
    /// </summary>
    Center,
    /// <summary>
    /// Align the bottom edge of the element with the bottom edge of the reference.
    /// </summary>
    InsideDown,
    /// <summary>
    /// Place the element above the reference, aligning its bottom edge with the reference's top edge.
    /// </summary>
    OutsideUp,
    /// <summary>
    /// Place the element below the reference, aligning its top edge with the reference's bottom edge.
    /// </summary>
    OutsideDown
}
