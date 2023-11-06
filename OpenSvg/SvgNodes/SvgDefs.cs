using System.Xml.Serialization;

namespace OpenSvg.SvgNodes;

public class SvgDefs : SvgElement, ISvgElementContainer
{
    [XmlElement(SvgNames.Style, typeof(SvgCssStyle))]
    public List<SvgStyle> ChildElements { get; set; } = new();

    public override string SvgName => SvgNames.Defs;

    public void Add(SvgElement child)
    {
        Add((SvgStyle)child);
    }

    public IEnumerable<SvgElement> Children()
    {
        return ChildElements;
    }

    public IEnumerable<SvgElement> Descendants()
    {
        return ChildElements;
    }

    public void Add(SvgStyle svgStyle)
    {
        ChildElements.Add(svgStyle);
        svgStyle.Parent = this;
    }

    /// <summary>
    ///     Adds an embedded font to the parent element.
    /// </summary>
    /// <param name="svgFont">The font to add.</param>
    public void AddEmbeddedFont(SvgFont svgFont)
    {
        var svgCssStyle = Descendants().OfType<SvgCssStyle>().FirstOrDefault();
        if (svgCssStyle is null)
        {
            svgCssStyle = new SvgCssStyle();
            Add(svgCssStyle);
        }

        svgCssStyle.Add(svgFont);
    }
}