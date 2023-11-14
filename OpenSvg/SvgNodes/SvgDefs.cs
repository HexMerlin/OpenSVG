using System.Xml.Serialization;

namespace OpenSvg.SvgNodes;

/// <summary>
/// Represents a SVG 'defs' element that can contain definitions for reuse by other elements.
/// </summary>
public class SvgDefs : SvgElement, ISvgElementContainer
{
    [XmlElement(SvgNames.Style, typeof(SvgCssStyle))]
    public List<SvgStyle> ChildElements { get; set; } = new();

    public override string SvgName => SvgNames.Defs;

    public void Add(SvgElement child) => Add((SvgStyle)child);

    public IEnumerable<SvgElement> Children() => ChildElements;

    public IEnumerable<SvgElement> Descendants() => ChildElements;

    public void Add(SvgStyle svgStyle)
    {
        ChildElements.Add(svgStyle);
        svgStyle.Parent = this;
    }

    /// <summary>
    /// Adds an embedded font to the SVG definitions.
    /// </summary>
    /// <param name="svgFont">The font to embed.</param>
    public void AddEmbeddedFont(SvgFont svgFont)
    {
        SvgCssStyle? svgCssStyle = Descendants().OfType<SvgCssStyle>().FirstOrDefault();
        if (svgCssStyle is null)
        {
            svgCssStyle = new SvgCssStyle();
            Add(svgCssStyle);
        }

        svgCssStyle.Add(svgFont);
    }
}