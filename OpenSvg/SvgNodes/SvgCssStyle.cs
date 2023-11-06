using OpenSvg.Attributes;

namespace OpenSvg.SvgNodes;

/// <summary>
///     Represents an SVG style element with CSS text content.
/// </summary>
/// <remarks>
///     The style element is used inside SVG documents to embed stylings. CSS can be used as a standalone stylesheet or can
///     be included directly into SVG elements as attributes.
/// </remarks>
public class SvgCssStyle : SvgStyle, IHasElementContent
{
    public readonly StringAttr Type = new(SvgNames.Type, SvgNames.TextCss, true);


    private SvgFont? singleFont;

    public override string SvgName => SvgNames.Style;


    public IReadOnlyList<SvgFont> Fonts => this.singleFont is null ? Array.Empty<SvgFont>() : new[] { this.singleFont };

    public string Content
    {
        get => this.singleFont is null ? "" : this.singleFont.XText;
        set
        {
            this.singleFont = null;
            Add(new SvgFont(value));
        }
    }


    /// <summary>
    ///     Adds an <see cref="SvgFont" /> element into the current SVG style.
    /// </summary>
    /// <param name="svgFont">The <see cref="SvgFont" /> element to add.</param>
    public void Add(SvgFont svgFont)
    {
        if (this.singleFont != null)
            throw new NotSupportedException("Adding more than one font to a CSS style is not currently supported");
        this.singleFont = svgFont;
        svgFont.Parent = this;
    }
}