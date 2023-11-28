using OpenSvg.Attributes;
using OpenSvg.Config;
using SkiaSharp;

namespace OpenSvg.SvgNodes;

/// <summary>
///     Represents a SVG 'text' element
/// </summary>
public class SvgText : SvgVisual, IHasElementContent
{
    protected readonly StringAttr fontName = new(SvgNames.FontName, SvgNames.DefaultFontName, false);
    protected readonly FloatAttr fontSize = new(SvgNames.FontSize, SvgNames.DefaultFontSize);
    protected readonly FloatAttr x = new(SvgNames.X);
    protected readonly FloatAttr y = new(SvgNames.Y);

    /// <summary>
    ///     Gets or sets the font name for this <see cref="SvgVisual" /> element.
    /// </summary>
    /// <remarks>
    ///     The default font is not defined according to the SVG 1.1 specification.
    ///     The default font is instead set in in accordance to the major browsers (Chrome, Firefox and Edge) to
    ///     <c>Times New Roman</c>
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/SVG11/text.html#FontPropertiesUsedBySVG">SVG 1.1 Font selection properties</seealso>
    public string FontName { get => this.fontName.Get(); set => this.fontName.Set(value); }

    /// <summary>
    ///     Gets or sets the font size for this <see cref="SvgVisual" /> element.
    /// </summary>
    /// <remarks>
    ///     The default font size is not defined according to the SVG 1.1 specification.
    ///     Instead a de facto standard based on browser implementation and CSS specifications is adopted.
    ///     The CSS2 specification suggests a "medium" font size that equates to 16px in many desktop browsers, which is adopted here.
    /// </remarks>
    /// <seealso href="https://www.w3.org/TR/CSS2/fonts.html#font-size-props">SVG 1.1 Font size</seealso>
    public float FontSize { get => this.fontSize.Get(); set => this.fontSize.Set(value); }

    /// <summary>
    /// Attribute for getting or setting X position of this element
    /// </summary>
    public float X { get => this.x.Get(); set => this.x.Set(value); }

    /// <summary>
    /// Attribute for getting or setting Y position of this element
    /// </summary>
    public float Y { get => this.y.Get(); set => this.y.Set(value); }

    private Point offset = Point.Origin;

    private Size size = new(0, 0);

    /// <summary>
    ///     Constructs a new empty instance of a <see cref="SvgText" />
    /// </summary>
    public SvgText() {}

    /// <summary>
    /// Gets or sets the position of the <see cref="SvgText"/> element.
    /// </summary>
    public Point Point
    {
        get => new(X, Y);
        set { X = value.X; Y = value.Y; }
    }

    /// <summary>
    ///     Constructs a new instance of the <see cref="SvgText" /> using the provided <see cref="TextConfig" />
    /// </summary>
    /// <remarks>
    ///     The <see cref="SvgFont" /> provided in the <see cref="TextConfig" /> will be embedded in the provided SVG document
    ///     if it does not already exist.
    /// </remarks>
    /// <param name="textConfig">A <see cref="TextConfig" /> object providing properties for the text.</param>
    public SvgText(TextConfig textConfig) => TextConfig = textConfig;

    ///<inheritdoc/>
    public override string SvgName => SvgNames.Text;

    /// <summary>
    /// Setting or getting the text configuration for this <see cref="SvgText"/> element.
    /// </summary>
    public TextConfig TextConfig
    {
        get
        {
            string fontName = this.FontName;
            SvgFont svgFont = RootDocument.EmbeddedFont(fontName) ?? throw new InvalidOperationException(
                $"An embedded font with name {fontName} could not be found in the current {nameof(SvgDocument)}");
            DrawConfig drawConfig = DrawConfig;
            string textContent = Content;
            float svgFontSize = this.FontSize;
            return new TextConfig(textContent, svgFont, svgFontSize, drawConfig);
        }
        set
        {
            (this.size, this.offset) = GetSizeAndOffset(value.Text, value.SvgFont.Font, value.FontSize);
            X = this.offset.X;
            Y = this.offset.Y;
            DrawConfig = value.DrawConfig;
            Content = value.Text;
            FontName = value.FontName;
            FontSize = value.FontSize;
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
    private static (Size Size, Point Offset) GetSizeAndOffset(string text, SKTypeface font, float fontSize)
    {
        using var textPaint = new SKPaint
        {
            Typeface = font,
            TextSize = fontSize
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
        Point ActualXY(float x, float y) => new(this.X - this.offset.X + x, this.Y - this.offset.Y + y);

        return new ConvexHull(new[]
            { ActualXY(0, 0), ActualXY(this.size.Width, 0), ActualXY(this.size.Width, this.size.Height), ActualXY(0, this.size.Height) });
    }
}