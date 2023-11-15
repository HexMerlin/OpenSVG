//File CommonTypes.cs

using OpenSvg.SvgNodes;

namespace OpenSvg;


/// <summary>
///     Specifies the vertical alignment of a <see cref="SvgVisual" /> element relative to a reference element.
///     Used by <see cref="SvgVisual.AlignRelativeTo" />.
/// </summary>
/// <seealso cref="SvgVisual"/>
/// <seealso cref="HorizontalAlignment"/>
public enum VerticalAlignment
{
    /// <summary>
    ///     Align the <c>top edge</c> of the element with the <c>top edge</c> of the reference.
    /// </summary>
    TopWithTop,
    /// <summary>
    ///     Align the <c>top edge</c> of the element with the <c>vertical center</c> of the reference.
    /// </summary>
    TopWithCenter,
    /// <summary>
    ///     Align the <c>top edge</c> of the element with the <c>bottom edge</c> of the reference.
    /// </summary>
    TopWithBottom,
    /// <summary>
    ///     Align the <c>vertical center</c> of the element with the <c>top edge</c> of the reference.
    /// </summary>
    CenterWithTop,
    /// <summary>
    ///     Align the <c>vertical center</c> of the element with the <c>vertical center</c> of the reference.
    /// </summary>
    CenterWithCenter,
    /// <summary>
    ///     Align the <c>vertical center</c> of the element with the <c>bottom edge</c> of the reference.
    /// </summary>
    CenterWithBottom,
    /// <summary>
    ///     Align the <c>bottom edge</c> of the element with the <c>top edge</c> of the reference.
    /// </summary>
    BottomWithTop,
    /// <summary>
    ///     Align the <c>bottom edge</c> of the element with the <c>vertical center</c> of the reference.
    /// </summary>
    BottomWithCenter,
    /// <summary>
    ///     Align the <c>bottom edge</c> of the element with the <c>bottom edge</c> of the reference.
    /// </summary>
    BottomWithBottom,
}