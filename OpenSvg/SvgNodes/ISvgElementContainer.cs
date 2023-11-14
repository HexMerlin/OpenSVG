namespace OpenSvg.SvgNodes;


/// <summary>
/// Represents an SVG element container.
/// </summary>
public interface ISvgElementContainer
{
    /// <summary>
    /// Gets the children of the current element.
    /// </summary>
    /// <returns>An enumerable of <see cref="SvgElement"/> containing the children of the current element.</returns>
    public IEnumerable<SvgElement> Children();

    /// <summary>
    /// Adds a child element to the current element.
    /// </summary>
    /// <param name="child">The child element to add.</param>
    public void Add(SvgElement child);

    /// <summary>
    /// Returns an enumerable of <see cref="SvgElement"/> containing the descendants (children and their children down to the leaf level) of the specified type.
    /// </summary>
    /// <returns>An enumerable of <see cref="SvgElement"/> containing the descendants of the specified type.</returns>
    public IEnumerable<SvgElement> Descendants();
}
