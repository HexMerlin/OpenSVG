using System.Xml.Linq;
using System.Xml.Serialization;
using HarfBuzzSharp;
using OpenSvg.Attributes;


namespace OpenSvg.SvgNodes;

/// <summary>
/// Represents an SVG style element with CSS text content.
/// </summary>
/// <remarks>
/// The style element is used inside SVG documents to embed stylings. CSS can be used as a standalone stylesheet or can be included directly into SVG elements as attributes.
/// </remarks>

public class SvgCssStyle : SvgStyle, IHasElementContent
{


    public readonly StringAttr Type = new(SvgNames.Type, SvgNames.TextCss, isConstant: true);

    public override string SvgName => SvgNames.Style;

    public string Content 
    { 
        get => singleFont is null ? "" : singleFont.XText;
        set
        {
            singleFont = null; Add(new SvgFont(value));
        }
    }

  
    private SvgFont? singleFont = null;


    [XmlIgnore] 
    public IReadOnlyList<SvgFont> Fonts => singleFont is null ? Array.Empty<SvgFont>() :  new[] { singleFont };



    /// <summary>
    /// Adds an <see cref="SvgFont"/> element into the current SVG style.
    /// </summary>
    /// <param name="svgFont">The <see cref="SvgFont"/> element to add.</param>
    public void Add(SvgFont svgFont)
    {
        if (singleFont != null)
            throw new NotSupportedException("Adding more than one font to a CSS style is not currently supported");
        singleFont = svgFont;
        svgFont.Parent = this;
    }




}