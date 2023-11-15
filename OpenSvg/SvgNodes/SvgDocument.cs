using OpenSvg.Attributes;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace OpenSvg.SvgNodes;

/// <summary>
/// Represents a SVG document.
/// </summary>
[XmlRoot(ElementName = SvgNames.Svg, Namespace = SvgNames.SvgNamespace)]
public class SvgDocument : SvgVisualContainer
{
    public readonly AbsoluteOrRatioAttr DefinedViewPortHeight = new(SvgNames.Height);

    public readonly AbsoluteOrRatioAttr DefinedViewPortWidth = new(SvgNames.Width);

    /// <inheritdoc/>
    public override string SvgName => SvgNames.Svg;

    /// <inheritdoc/>
    public override double ViewPortWidth => this.DefinedViewPortWidth.Get()
        .Resolve(() => Parent?.ViewPortWidth ?? BoundingBox.Size.Width);

    /// <inheritdoc/>
    public override double ViewPortHeight => this.DefinedViewPortHeight.Get()
        .Resolve(() => Parent?.ViewPortHeight ?? BoundingBox.Size.Height);

    public XDocument ToXDocument()
    {
        var serializer = new XmlSerializer(typeof(SvgDocument));

        var xDocument = new XDocument();
        using (XmlWriter xmlWriter = xDocument.CreateWriter())
        {
            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(xmlWriter, this, emptyNamespaces);
        }

        return xDocument;
    }

    public static SvgDocument FromXDocument(XDocument xDocument)
    {
        var serializer = new XmlSerializer(typeof(SvgDocument));

        using XmlReader xmlReader = xDocument.CreateReader();
        SvgDocument svgDocument = serializer.Deserialize(xmlReader) as SvgDocument
                          ?? throw new InvalidDataException("Source is not a valid SVG document");
        return svgDocument;
    }

    /// <summary>
    ///     Loads an SVG file from the specified path.
    /// </summary>
    /// <param name="svgFilePath">The path to the SVG file to read from.</param>
    /// <returns>The XElement representation of the SVG file.</returns>
    public static SvgDocument Load(string svgFilePath)
    {
        var xDocument = XDocument.Load(svgFilePath);
        return FromXDocument(xDocument);
    }

    /// <summary>
    ///     Saves an XElement to an SVG file with a specified path.
    /// </summary>
    /// <param name="svgFilePath">The path to the SVG file to write to.</param>
    public void Save(string svgFilePath)
    {
        XDocument xDocument = ToXDocument();

        var settings = new XmlWriterSettings
        {
            Indent = true,
            OmitXmlDeclaration = true
        };

        using var xmlWriter = XmlWriter.Create(svgFilePath, settings);
        xDocument.Save(xmlWriter);
    }


    /// <summary>
    /// Embeds a font into the SVG document.
    /// </summary>
    /// <param name="svgFont">The SVG font to embed.</param>
    public void EmbedFont(SvgFont svgFont)
    {
        SvgDefs? svgDefs = Descendants().OfType<SvgDefs>().FirstOrDefault();
        if (svgDefs is null)
        {
            svgDefs = new SvgDefs();
            Add(svgDefs);
        }

        svgDefs.AddEmbeddedFont(svgFont);
    }

    /// <summary>
    ///     Gets the embedded font with the specified font name.
    /// </summary>
    /// <param name="fontName">The name of the font to get.</param>
    /// <returns>The embedded font with the specified font name, or <c>null</c> if no such font was found.</returns>
    public SvgFont? EmbeddedFont(string fontName) => EmbeddedFonts().FirstOrDefault(svgFont => svgFont.FontName == fontName);


    public IEnumerable<SvgFont> EmbeddedFonts() => Descendants().OfType<SvgCssStyle>().SelectMany(cssStyle => cssStyle.Fonts);

    public void SetViewPortToActualSize()
    {
        this.DefinedViewPortWidth.Set(BoundingBox.Size.Width);
        this.DefinedViewPortHeight.Set(BoundingBox.Size.Height);
    }

    //public override (bool Equal, string Message) CompareSelfAndDescendants(SvgElement other,
    //    double precision = Constants.DoublePrecision)
    //{
    //    if (ReferenceEquals(this, other)) return (true, "Same reference");
    //    (bool equal, string message) = base.CompareSelfAndDescendants(other);
    //    if (!equal)
    //        return (equal, message);
    //    var sameType = (SvgDocument)other;
    //    if (this.DefinedViewPortWidth != sameType.DefinedViewPortWidth)
    //        return (false, $"DefinedViewPortWidth: {this.DefinedViewPortWidth} != {sameType.DefinedViewPortWidth}");

    //    if (this.DefinedViewPortHeight != sameType.DefinedViewPortHeight)
    //        return (false, $"DefinedViewPortHeight: {this.DefinedViewPortHeight} != {sameType.DefinedViewPortHeight}");
    //    return (true, "Equal");
    //}
}