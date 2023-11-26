﻿using OpenSvg.Attributes;
using System.IO.Compression;
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
    protected readonly AbsoluteOrRatioAttr definedViewPortWidth = new(SvgNames.Width);
    protected readonly AbsoluteOrRatioAttr definedViewPortHeight = new(SvgNames.Height);

    /// <summary>
    /// Gets or sets the defined view port width.
    /// </summary>
    public AbsoluteOrRatio DefinedViewPortWidth { get => definedViewPortWidth.Get(); set => definedViewPortWidth.Set(value); }

    /// <summary>
    /// Gets or sets the defined view port height.
    /// </summary>
    public AbsoluteOrRatio DefinedViewPortHeight { get => definedViewPortHeight.Get(); set => definedViewPortHeight.Set(value); }

    /// <inheritdoc/>
    public override string SvgName => SvgNames.Svg;

    /// <inheritdoc/>
    public override double ViewPortWidth => DefinedViewPortWidth.Resolve(() => Parent?.ViewPortWidth ?? BoundingBox.Size.Width);

    /// <inheritdoc/>
    public override double ViewPortHeight => DefinedViewPortHeight.Resolve(() => Parent?.ViewPortHeight ?? BoundingBox.Size.Height);

    /// <summary>
    /// Converts the SVG document to an XDocument.
    /// </summary>
    /// <returns>The XDocument representation of the SVG document.</returns>
    /// <remarks>
    /// This method serializes the SVG document using an XmlSerializer and returns the resulting XDocument.
    /// </remarks>
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

    /// <summary>
    /// Converts an XDocument to an SVG document.
    /// </summary>
    /// <param name="xDocument">The XDocument to convert.</param>
    /// <returns>The SVG document representation of the XDocument.</returns>
    /// <remarks>
    /// This method deserializes the XDocument using an XmlSerializer and returns the resulting SVG document.
    /// </remarks>
    public static SvgDocument FromXDocument(XDocument xDocument)
    {
        var serializer = new XmlSerializer(typeof(SvgDocument));

        using XmlReader xmlReader = xDocument.CreateReader();
        SvgDocument svgDocument = serializer.Deserialize(xmlReader) as SvgDocument
                          ?? throw new InvalidDataException("Source is not a valid SVG document");
        return svgDocument;
    }

    private static FileFormat GetFileFormat(string filePath)
    {
        string extension = System.IO.Path.GetExtension(filePath);
        return extension switch
        {
            ".svg" => FileFormat.Svg,
            ".svgz" => FileFormat.Svgz,
            _ => throw new ArgumentException($"Unknown SVG file extension: {extension}", nameof(filePath))
        };
    }

    /// <summary>
    ///     Loads an SVG file from the specified path.
    /// </summary>
    /// <param name="svgFilePath">The path to the SVG file to read from.</param>
    /// <remarks>
    /// This method supports both uncompressed (.svg) and compressed (.svgz) SVG files.
    /// </remarks>
    /// <returns>The XElement representation of the SVG file.</returns>
    public static SvgDocument Load(string svgFilePath)
    {
        FileFormat fileFormat = GetFileFormat(svgFilePath);
        if (fileFormat == FileFormat.Svgz)
        {
            using var fileStream = new FileStream(svgFilePath, FileMode.Open);
            using var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress);
            var xDocument = XDocument.Load(gzipStream);
            return FromXDocument(xDocument);
        }
        else
        {
            var xDocument = XDocument.Load(svgFilePath);
            return FromXDocument(xDocument);
        }
    }

    /// <summary>
    /// Saves the SVG document to a specified file path.
    /// </summary>
    /// <param name="svgFilePath">The path to save the SVG document.</param>
    /// <param name="fileFormat">The file format to save the SVG document in. Default is <see cref="FileFormat.Auto"/>.</param>
    /// <remarks>
    /// This method saves the SVG document to the specified file path using the specified file format
    /// </remarks>
    public void Save(string svgFilePath, FileFormat fileFormat = FileFormat.Auto)
    {
        if (fileFormat == FileFormat.Auto)
            fileFormat = GetFileFormat(svgFilePath);

        XDocument xDocument = ToXDocument();
        using var fileStream = new FileStream(svgFilePath, FileMode.Create);

        var settings = new XmlWriterSettings
        {
            Indent = fileFormat == FileFormat.Svg ? true : false,
            OmitXmlDeclaration = true,
        };

        if (fileFormat == FileFormat.Svgz)
        {
            using var gzipStream = new GZipStream(fileStream, CompressionLevel.Optimal);
            using var xmlWriter = XmlWriter.Create(gzipStream, settings);
            xDocument.Save(xmlWriter);
        }
        else
        {
            using var xmlWriter = XmlWriter.Create(fileStream, settings);
            xDocument.Save(xmlWriter);
        }
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

    /// <summary>
    /// Gets the collection of embedded fonts.
    /// </summary>
    /// <returns>The collection of embedded fonts.</returns>
    public IEnumerable<SvgFont> EmbeddedFonts() => Descendants().OfType<SvgCssStyle>().SelectMany(cssStyle => cssStyle.Fonts);

    /// <summary>
    /// Sets the view port to the actual size.
    /// </summary>
    public void SetViewPortToActualSize()
    {
        DefinedViewPortWidth = BoundingBox.Size.Width;
        DefinedViewPortHeight = BoundingBox.Size.Height;
    }
}
