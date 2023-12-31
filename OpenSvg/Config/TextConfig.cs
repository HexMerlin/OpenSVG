﻿using OpenSvg.SvgNodes;
using SkiaSharp;

namespace OpenSvg.Config;

/// <summary>
///     Configuration options for rendering text.
/// </summary>
/// <param name="Text">The text to be displayed.</param>
/// <param name="SvgFont">The font to be used for rendering the text.</param>
/// <param name="FontSize">The font size of the text.</param>
/// <param name="DrawConfig">
///     The configuration of the pen for drawing text, including StrokeWidth, FillColor, and
///     StrokeColor
/// </param>
public record TextConfig(string Text, SvgFont SvgFont, float FontSize, DrawConfig DrawConfig)
{

    /// <summary>
    /// Creates a new TextConfig with default values.
    /// </summary>
    public TextConfig() : this(string.Empty, SvgFont.DefaultFont, DefaultFontSize, new DrawConfig().WithStrokeWidth(0)) {}


    /// <summary>
    ///     The default font size used for rendering text.
    /// </summary>
    public static float DefaultFontSize = SvgNames.DefaultFontSize;

    /// <summary>
    ///     Gets the name of the font.
    /// </summary>
    public string FontName => SvgFont.FontName;

    /// <summary>
    ///     Creates a new TextConfig object with the specified text.
    /// </summary>
    /// <param name="text">The text for the new TextConfig.</param>
    /// <returns>A new TextConfig object with the specified text.</returns>
    public TextConfig WithText(string text) => this with { Text = text };

    /// <summary>
    ///     Creates a new TextConfig object with the specified color.
    /// </summary>
    /// <param name="textColor">The color for the new TextConfig.</param>
    /// <returns>A new TextConfig object with the specified color.</returns>
    public TextConfig WithTextColor(SKColor textColor) => this with { DrawConfig = DrawConfig.WithStrokeColor(textColor).WithFillColor(textColor) };

    /// <summary>
    ///     Converts the TextConfig object to a SvgPath object.
    /// </summary>
    /// <returns>A new SvgPath object.</returns>
    public SvgPath ToSvgPath() => new(this);

    /// <summary>
    ///     Converts the TextConfig object to a SvgText object.
    /// </summary>
    /// <returns>A new SvgText object.</returns>
    public SvgText ToSvgText() => new(this);
}