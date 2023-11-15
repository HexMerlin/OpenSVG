namespace OpenSvg.SvgNodes;

/// <summary>
/// Provides extension methods for collections of <see cref="SvgElement"/>.
/// </summary>
public static class SvgElementCollectionExtensions
{
    /// <summary>
    ///     Returns a new SvgGroup element from a collection of SvgElement.
    /// </summary>
    /// <param name="svgElements">The collection of SvgElement.</param>
    /// <returns>A new SvgGroup element.</returns>
    public static SvgGroup Group(this IEnumerable<SvgElement> svgElements)
    {
        var group = new SvgGroup();
        group.AddAll(svgElements);
        return group;
    }

    /// <summary>
    ///     Creates a new SvgGroup element from a SvgElement collection
    ///     and stacking the elements in the specified orientation.
    ///     If the orientation is <see cref="Orientation.Horizontal"/>, the elements are stacked horizontally, from left to right.
    ///     If the orientation is <see cref="Orientation.Vertical"/>, the elements are stacked vertically, from top to bottom."/>
    /// </summary>
    /// <param name="orientation">The orientation of the elements in the collection.</param>
    /// <param name="svgVisuals">The collection of type SvgElement.</param>
    /// <returns>A new SvgGroup element.</returns>
    public static SvgGroup Stack(this IEnumerable<SvgVisual> svgVisuals, Orientation orientation)
    {
        HorizontalAlignment? horizontalAlignment =
            orientation == Orientation.Horizontal ? HorizontalAlignment.LeftWithRight : null;
        VerticalAlignment? verticalAlignment =
            orientation == Orientation.Vertical ? VerticalAlignment.TopWithBottom : null;
        var svgGroup = new SvgGroup();

        foreach (SvgVisual svgVisual in svgVisuals)
        {
            svgVisual.AlignRelativeTo(svgGroup, horizontalAlignment, verticalAlignment);
            svgGroup.Add(svgVisual);
        }

        return svgGroup;
    }
}