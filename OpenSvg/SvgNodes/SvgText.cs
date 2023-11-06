﻿using OpenSvg.Attributes;
using OpenSvg.Config;
using SkiaSharp;

namespace OpenSvg.SvgNodes;

public class SvgText : SvgVisual, IHasElementContent
{
    /// <summary>
    ///     Gets or sets the font name for this <see cref="SvgVisual" /> element.
    /// </summary>
    /// <remarks>
    ///     The default font is not defined according to the SVG 1.1 specification.
    ///     The default font is instead set in in accordance to the major browsers (Chrome, Firefox and Edge) to
    ///     <c>Times New Roman</c>
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/SVG11/text.html#FontPropertiesUsedBySVG">SVG 1.1 Font selection properties</seealso>
    public readonly StringAttr FontName = new(SvgNames.FontName, "Times New Roman", false);

    /// <summary>
    ///     Gets or sets the font size for this <see cref="SvgVisual" /> element.
    /// </summary>
    /// <remarks>
    ///     The default font size is not defined according to the SVG 1.1 specification.
    ///     Instead a de facto standard based on browser implementation and CSS specifications is adopted.
    ///     The CSS2 specification suggests a "medium" font size that equates to 16px in many desktop browsers, which is adopted here.
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/CSS2/fonts.html#font-size-props">SVG 1.1 Font size</seealso>
    public readonly DoubleAttr FontSize = new(SvgNames.FontSize, 16);


    public readonly DoubleAttr X = new(SvgNames.X);

    public readonly DoubleAttr Y = new(SvgNames.Y);

    private Point offset = Point.Origin;

    private Size size = new(0, 0);

    public SvgText()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SvgText" /> using the provided <see cref="TextConfig" />
    /// </summary>
    /// <remarks>
    ///     The <see cref="SvgFont" /> provided in the <see cref="TextConfig" /> will be embedded in the provided SVG document
    ///     if it does not already exist.
    /// </remarks>
    /// <param name="textConfig">A <see cref="TextConfig" /> object providing properties for the text.</param>
    public SvgText(TextConfig textConfig) => TextConfig = textConfig;

    public override string SvgName => SvgNames.Text;

    public TextConfig TextConfig
    {
        get
        {
            string fontName = this.FontName.Get();
            SvgFont svgFont = RootDocument.EmbeddedFont(fontName) ?? throw new InvalidOperationException(
                $"An embedded font with name {fontName} could not be found in the current {nameof(SvgDocument)}");
            DrawConfig drawConfig = DrawConfig;
            string textContent = Content;
            double svgFontSize = this.FontSize.Get();
            return new TextConfig(textContent, svgFont, svgFontSize, drawConfig);
        }
        set
        {
            (this.size, this.offset) = GetSizeAndOffset(value.Text, value.SvgFont.Font, value.FontSize);
            this.X.Set(this.offset.X);
            this.Y.Set(this.offset.Y);
            DrawConfig = value.DrawConfig;
            Content = value.Text;
            this.FontName.Set(value.FontName);
            this.FontSize.Set(value.FontSize);
        }
    }


    public string Content { get; set; } = string.Empty;


    /// <summary>
    ///     Gets the local size and offset of the text.
    ///     Offset is defines as follows:
    ///     x: the x-distance from 0 where the text actually begins.
    ///     y: the y-distance from the top of the text down to the baseline of the text.
    ///     This is needed since the X and Y attributes of the SvgText does not denote the top left corner of the text,
    ///     but instead:
    ///     the X value denotes a value a little less the actual left-most point of the text.
    ///     the Y value denotes a value for the baseline of the text, close to the bottom of the text.
    /// </summary>
    private static (Size Size, Point Offset) GetSizeAndOffset(string text, SKTypeface font, double fontSize)
    {
        using var textPaint = new SKPaint
        {
            Typeface = font,
            TextSize = (float)fontSize
        };

        var textBounds = new SKRect();
        textPaint.MeasureText(text, ref textBounds);
        var size = new Size(textBounds.Width, textBounds.Height);

        using SKPath textPath = textPaint.GetTextPath(text, 0, 0);
        var offset = new Point(-textPath.Bounds.Left, -textPath.Bounds.Top);

        return (size, offset);
    }


    protected override ConvexHull ComputeConvexHull()
    {
        Point ActualXY(double x, double y) => new(this.X.Get() - this.offset.X + x, this.Y.Get() - this.offset.Y + y);

        return new ConvexHull(new[]
            { ActualXY(0, 0), ActualXY(this.size.Width, 0), ActualXY(this.size.Width, this.size.Height), ActualXY(0, this.size.Height) });
    }
}