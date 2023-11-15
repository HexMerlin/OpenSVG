
using OpenSvg.SvgNodes;

namespace OpenSvg;


/// <summary>
///     Specifies the horizontal alignment of a <see cref="SvgVisual" /> element relative to a reference element.
///     Used by <see cref="SvgVisual.AlignRelativeTo" />.
/// </summary>
/// <seealso cref="SvgVisual"/>
/// <seealso cref="VerticalAlignment"/>

public enum HorizontalAlignment
{
    /// <summary>
    ///     Align the <c>left edge</c> of the element with the <c>left edge</c> of the reference.
    /// </summary>
    LeftWithLeft,
    /// <summary>
    ///     Align the <c>left edge</c> of the element with the <c>horizontal center</c> of the reference.
    /// </summary>
    LeftWithCenter,
    /// <summary>
    ///     Align the <c>left edge</c> of the element with the <c>right edge</c> of the reference.
    /// </summary>
    LeftWithRight,
    /// <summary>
    ///     Align the <c>horizontal center</c> of the element with the <c>left edge</c> of the reference.
    /// </summary>
    CenterWithLeft,
    /// <summary>
    ///     Align the <c>horizontal center</c> of the element with the <c>horizontal center</c> of the reference.
    /// </summary>
    CenterWithCenter,
    /// <summary>
    ///     Align the <c>horizontal center</c> of the element with the <c>right edge</c> of the reference.
    /// </summary>
    CenterWithRight,
    /// <summary>
    ///     Align the <c>right edge</c> of the element with the <c>left edge</c> of the reference.
    /// </summary>
    RightWithLeft,
    /// <summary>
    ///     Align the <c>right edge</c> of the element with the <c>horizontal center</c> of the reference.
    /// </summary>
    RightWithCenter,
    /// <summary>
    ///     Align the <c>right edge</c> of the element with the <c>right edge</c> of the reference.
    /// </summary>
    RightWithRight,
}