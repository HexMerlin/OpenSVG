using OpenSvg.Attributes;
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
    protected readonly ViewBoxAttr viewBox = new();
    protected readonly PreserveAspectRatioAttr preserveAspectRatio = new();

    /// <summary>
    /// Gets or sets the view box of this SvgDocument.
    /// </summary>
    public BoundingBox ViewBox { get => viewBox.Get(); set => viewBox.Set(value); }

    /// <summary>
    /// Gets or sets the 'preserveAspectRatio' of this SvgDocument.
    /// </summary>
    public AspectRatio PreserveAspectRatio { get => preserveAspectRatio.Get(); set => preserveAspectRatio.Set(value); }

    /// <summary>
    /// Gets or sets the defined view port width of this SvgDocument.
    /// </summary>
    public AbsoluteOrRatio DefinedViewPortWidth { get => definedViewPortWidth.Get(); set => definedViewPortWidth.Set(value); }

    /// <summary>
    /// Gets or sets the defined view port height of this SvgDocument.
    /// </summary>
    public AbsoluteOrRatio DefinedViewPortHeight { get => definedViewPortHeight.Get(); set => definedViewPortHeight.Set(value); }

    /// <inheritdoc/>
    public override string SvgName => SvgNames.Svg;

    /// <inheritdoc/>
    public override float ViewPortWidth => DefinedViewPortWidth.Resolve(() => Parent?.ViewPortWidth ?? BoundingBox.Width);

    /// <inheritdoc/>
    public override float ViewPortHeight => DefinedViewPortHeight.Resolve(() => Parent?.ViewPortHeight ?? BoundingBox.Height);

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
    /// Sets the viewBox of the SVG document to the actual size of its bounding box and 
    /// defines a default viewport size. This method aligns the SVG content to the upper-left corner 
    /// of the viewport and scales it to fit within the default viewport dimensions, maintaining the aspect ratio.
    /// </summary>
    /// <remarks>
    /// This method sets the <see cref="ViewBox"/> property to the bounding box of the SVG document.
    /// It also sets the <see cref="PreserveAspectRatio"/> to align the content to the upper-left corner 
    /// ('XMinYMin') and scales it to fit ('Meet') within the default viewport.
    /// The default viewport dimensions are defined by <see cref="Constants.DefaultContainerWidth"/> 
    /// and <see cref="Constants.DefaultContainerHeight"/>, which are set to 1024 and 768 pixels respectively.
    /// </remarks>
    /// <seealso cref="AspectRatio"/>
    /// <seealso cref="AspectRatioAlign"/>
    /// <seealso cref="AspectRatioMeetOrSlice"/>

    public void SetViewBoxToActualSizeAndDefaultViewPort()
    {
        ViewBox = this.BoundingBox;
        PreserveAspectRatio = new AspectRatio(AspectRatioAlign.XMinYMin, AspectRatioMeetOrSlice.Meet);

        DefinedViewPortWidth = Constants.DefaultContainerWidth; //= 1024
        DefinedViewPortHeight = Constants.DefaultContainerHeight; //= 768
    }
}
