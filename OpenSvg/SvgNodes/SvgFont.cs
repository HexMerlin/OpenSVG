﻿using SkiaSharp;

namespace OpenSvg.SvgNodes;

/// <summary>
///     Represents a scalar vector graphics font element defined through an XML schema.
/// </summary>
public sealed class SvgFont : SvgNode
{
    private readonly Lazy<SKTypeface> font;
    private readonly Lazy<string> xText;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SvgFont" /> class by boxing an existing <see cref="SKTypeface" />
    ///     object.
    /// </summary>
    /// <param name="font">An existing <see cref="SKTypeface" /> font-family descriptor instance.</param>
    public SvgFont(SKTypeface font)
    {
        this.font = new Lazy<SKTypeface>(font);
        this.xText = new Lazy<string>(FontToXText(Font));
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SvgFont" /> class by boxing an existing <see cref="XText" /> object.
    /// </summary>
    /// <param name="xText">An existing <see cref="XText" /> XML element that describes a font-family descriptor.</param>
    internal SvgFont(string xText)
    {
        this.xText = new Lazy<string>(xText);
        this.font = new Lazy<SKTypeface>(XTextToFont(XText));
    }

    /// <summary>
    ///     Gets the default font to use in cases where no font is specified.
    /// </summary>
    /// <remarks>
    ///     The default font is not defined according to the SVG 1.1 specification.
    ///     The default font is instead set in in accordance to the major browsers (Chrome, Firefox and Edge) to
    ///     <c>Times New Roman</c>
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/SVG11/text.html#FontPropertiesUsedBySVG">SVG 1.1 Font selection properties</seealso>
    public static SvgFont DefaultFont { get; } =  new SvgFont(SKTypeface.FromFamilyName(SvgNames.DefaultFontName));


    /// <summary>
    ///     Creates an instance of <see cref="SvgFont"/> based on the specified font name.
    /// </summary>
    /// <remarks>
    ///     This method tries to load a system font based on the specified font name.
    ///     It should be used with care as the font may not be available on all systems.
    ///     Consider using embedded fonts in SVG instead, which are supported by the OpenSvg library.
    ///     <seealso cref="OpenSvg.SvgNodes.SvgDocument.EmbedFont(SvgFont)"/>
    /// </remarks>
    /// <param name="fontName">The name of the font.</param>
    /// <returns>An instance of <see cref="SvgFont"/>.</returns>

    public static SvgFont GetSystemFont(string fontName) => new SvgFont(SKTypeface.FromFamilyName(fontName));

    /// <summary>
    ///     Gets the declared object that represents the value of the SVG 'font-face' rule : an XML element.
    /// </summary>
    public string XText => this.xText.Value;

    /// <summary>
    ///     Gets the actual value of the <see cref="SKTypeface" /> font instance that is used for rendering.
    /// </summary>
    public SKTypeface Font => this.font.Value;

    /// <summary>
    ///     Gets the name of the font-family for an SVG font element property.
    /// </summary>
    public string FontName => this.font.IsValueCreated ? Font.FamilyName : GetFontName(XText);

    /// <summary>
    ///     Gets a numerical representation of the font-weight for a font-family SVG property or a CSS property.
    /// </summary>
    public int FontWeight => Font.FontWeight;

    /// <summary>
    ///     Gets the style slant of a font-family SVG descriptor  or CSS property.
    /// </summary>
    public SKFontStyleSlant FontSlant => Font.FontSlant;

    /// <summary>
    ///     Gets the width of the font.
    /// </summary>
    public int FontWidth => Font.FontWidth;

    /// <summary>
    ///     Loads a new SvgFont from a file.
    /// </summary>
    /// <param name="fontFilePath">The file path of the SvgFont file to load.</param>
    /// <returns>The SvgFont instance that was loaded from the file.</returns>
    public static SvgFont LoadFromFile(string fontFilePath) => new(fontFilePath.LoadFromFile());

    /// <summary>
    ///     Saves the SVG font to a file (e.g. .TTF, .OTF).
    /// </summary>
    /// <param name="fontFilePath">The path and name of the file to save.</param>
    public void SaveFontToFile(string fontFilePath) => Font.SaveToFile(fontFilePath);

    /// <summary>
    ///     Returns the font name specified in the XText.
    /// </summary>
    /// <param name="xText">The XText.</param>
    /// <returns>The font name.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when an expected definition of '<see cref="SvgNames.FontName" />' is
    ///     missing in the XText.
    /// </exception>
    private static string GetFontName(string xText)
    {
        System.Text.RegularExpressions.Match match = RegexStore.GetFontNameFromXText().Match(xText);
        return match.Success
            ? match.Groups[1].Value
            : throw new InvalidOperationException("An expected definition of 'font-family' is missing in the XText.");
    }

    /// <summary>
    ///     Converts a <see cref="XText" /> XML element with embedded base64-encoded data font formatting to a
    ///     <see cref="SKTypeface" /> object.
    /// </summary>
    /// <param name="xText">The <see cref="XText" /> XML element that contains the font data and descriptors.</param>
    private static SKTypeface XTextToFont(string xText)
    {
        string fontFaceXText = xText;
        string base64FontData = fontFaceXText.Split("base64,", StringSplitOptions.RemoveEmptyEntries)[1];
        base64FontData = base64FontData.Split(')')[0];
        byte[] fontData = Convert.FromBase64String(base64FontData);

        using var stream = new MemoryStream(fontData);
        return SKTypeface.FromStream(stream);
    }

    /// <summary>
    ///     Converts a <see cref="SKTypeface" /> font to a new <see cref="XText" /> XML element with embedded base64-encoded
    ///     data font formatting.
    /// </summary>
    /// <param name="font">The <see cref="SKTypeface" /> font instance to convert.</param>
    /// <returns>A new <see cref="XText" /> XML element that describes this font-family and its embedded base64-encoded data.</returns>
    private static string FontToXText(SKTypeface font)
    {
        ArgumentNullException.ThrowIfNull(font, nameof(font));
        using SKStreamAsset stream = font.OpenStream(out _);
        byte[] fontData = new byte[stream.Length];
        int bytesRead = stream.Read(fontData, fontData.Length);
        if (bytesRead != fontData.Length)
            throw new InvalidOperationException("Failed to read the entire font data stream.");

        string format = GetFontFormatFromBase64(fontData);
        string base64Font = Convert.ToBase64String(fontData);
        return
            $"{SvgNames.FontFaceRule} {{ {SvgNames.FontName}: '{font.FamilyName}'; {SvgNames.Format}('{format}'); src: url(data:{format};base64,{base64Font}); }}";
    }


    /// <summary>
    ///     Gets the type of font based on its base64-encoded data.
    /// </summary>
    /// <param name="fontData">A byte array representing the font data in base64-encoded format.</param>
    /// <returns>The font format type.</returns>
    private static string GetFontFormatFromBase64(byte[] fontData)
    {
        if (fontData.Length < 4) throw new InvalidOperationException("Insufficient font data.");
        return
            fontData[0] == 0x00 && fontData[1] == 0x01 && fontData[2] == 0x00 && fontData[3] == 0x00
                ? SvgNames.TrueType
                : fontData[0] == 0x4F && fontData[1] == 0x54 && fontData[2] == 0x54 && fontData[3] == 0x4F
                    ? SvgNames.OpenType
                    : throw new InvalidOperationException("Cannot resolve a known font format from base64 code.");
    }
}