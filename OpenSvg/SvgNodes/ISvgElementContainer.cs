namespace OpenSvg.SvgNodes;

public interface ISvgElementContainer
{

    public IEnumerable<SvgElement> Children();

    public void Add(SvgElement child);

    /// <returns>
    /// Returns an <see cref="IEnumerable"/> of <see cref="SvgElement"/> containing the descendants (children
    /// and their children down to the leaf level) of the specified type
    /// </returns>
    public IEnumerable<SvgElement> Descendants();


}

