﻿using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using OpenSvg.Attributes;

namespace OpenSvg.SvgNodes;

[XmlRoot(ElementName = SvgNames.Svg, Namespace = SvgNames.SvgNamespace)]
public class SvgDocument : SvgVisualContainer
{

    public override string SvgName => SvgNames.Svg;

    public readonly AbsoluteOrRatioAttr DefinedViewPortWidth = new(SvgNames.Width);

    public readonly AbsoluteOrRatioAttr DefinedViewPortHeight = new(SvgNames.Height);

    public override double ViewPortWidth => DefinedViewPortWidth.Get()
        .Resolve(() => Parent?.ViewPortWidth ?? BoundingBox.Size.Width);

    public override double ViewPortHeight => DefinedViewPortHeight.Get()
        .Resolve(() => Parent?.ViewPortHeight ?? BoundingBox.Size.Height);

    public XDocument ToXDocument()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SvgDocument));
   
        XDocument xDocument = new XDocument();
        using (XmlWriter xmlWriter = xDocument.CreateWriter())
        {
            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(xmlWriter, this, emptyNamespaces);
        }

        return xDocument;
    }

    public static SvgDocument FromXDocument(XDocument xDocument)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SvgDocument));

        using (XmlReader xmlReader = xDocument.CreateReader())
        {
            SvgDocument svgDocument = serializer.Deserialize(xmlReader) as SvgDocument
                                      ?? throw new InvalidDataException("Source is not a valid SVG document");
            return svgDocument;
        }
    }

    /// <summary>
    /// Loads an SVG file from the specified path.
    /// </summary>
    /// <param name="svgFilePath">The path to the SVG file to read from.</param>
    /// <returns>The XElement representation of the SVG file.</returns>
    public static SvgDocument Load(string svgFilePath)
    {
        XDocument xDocument = XDocument.Load(svgFilePath);
        return FromXDocument(xDocument);
    }

    /// <summary>
    /// Saves an XElement to an SVG file with a specified path.
    /// </summary>
    /// <param name="svgFilePath">The path to the SVG file to write to.</param>
    public void Save(string svgFilePath)
    {
        XDocument xDocument = ToXDocument();

        XmlWriterSettings settings = new XmlWriterSettings
        {
            Indent = true,
            OmitXmlDeclaration = true
        };

        using XmlWriter xmlWriter = XmlWriter.Create(svgFilePath, settings);
        xDocument.Save(xmlWriter);
    }



    /// <summary>
    /// Adds the specified font to the SVG document.
    /// </summary>
    /// <param name="svgFont">The font to add.</param>
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
    /// Gets the embedded font with the specified font name.
    /// </summary>
    /// <param name="fontName">The name of the font to get.</param>
    /// <returns>The embedded font with the specified font name, or <c>null</c> if no such font was found.</returns>
    public SvgFont? EmbeddedFont(string fontName)
        => EmbeddedFonts().FirstOrDefault(svgFont => svgFont.FontName == fontName);


    public IEnumerable<SvgFont> EmbeddedFonts()
        => Descendants().OfType<SvgCssStyle>().SelectMany(cssStyle => cssStyle.Fonts);

    public void SetViewPortToActualSize()
    {
        DefinedViewPortWidth.Set(BoundingBox.Size.Width);
        DefinedViewPortHeight.Set(BoundingBox.Size.Height);
    }
    
    public override (bool Equal, string Message) CompareSelfAndDescendants(SvgElement other, double precision = Constants.DoublePrecision)
    {
        if (ReferenceEquals(this, other)) return (true, "Same reference");
        var (equal, message) = base.CompareSelfAndDescendants(other);
        if (!equal)
            return (equal, message);
        SvgDocument sameType = (SvgDocument) other;
        if (DefinedViewPortWidth != sameType.DefinedViewPortWidth)
            return (false, $"DefinedViewPortWidth: {DefinedViewPortWidth} != {sameType.DefinedViewPortWidth}");

        if (DefinedViewPortHeight != sameType.DefinedViewPortHeight)
            return (false, $"DefinedViewPortHeight: {DefinedViewPortHeight} != {sameType.DefinedViewPortHeight}");
        return (true, "Equal");
    }
}



