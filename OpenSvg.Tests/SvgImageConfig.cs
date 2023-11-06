using SkiaSharp;
using OpenSvg;
using OpenSvg.Config;
using OpenSvg.SvgNodes;
using Size = OpenSvg.Size;

namespace OpenSvg.Tests;

public record SvgImageConfig
{
    public static readonly SvgFont BadgeFont = SvgFont.LoadFromFile(TestPaths.GetFontPath(Resources.FontFileNameBitterProBold));
    public static readonly SvgFont SmallTextFont = SvgFont.LoadFromFile(TestPaths.GetFontPath(Resources.FontFileNameHelveticaNowDisplayCondensed));

    public SvgImageConfig() { }

    public RectangleConfig BadgeRectangleConfig { get; } = new(new Size(300, 170), new DrawConfig(FillColor: SKColors.DarkBlue, StrokeColor: SKColors.Transparent, 1), CornerRadius: 40);

    public TextConfig BadgeTextConfig { get; } = new("", BadgeFont, 130, new DrawConfig(FillColor: SKColors.LightGray, StrokeColor: SKColors.DarkGray, 1));

    public RectangleConfig SmallRectConfig { get; } = new(new Size(500, 80), new DrawConfig(FillColor: SKColors.Transparent, StrokeColor: SKColors.Transparent, 1));

    public TextConfig SmallTextConfig { get; } = new("", SmallTextFont, 80, new DrawConfig(FillColor: SKColors.White, StrokeColor: SKColors.Transparent, 1));

    private SvgGroup CreateSvgElement(string badgeText, string fromText, string toText, SKColor badgeTextColor, SKColor badgeColor)
    {
        SvgGroup badge = this.BadgeRectangleConfig.WithFillColor(badgeColor)
            .ToSvgPolygon()
            .ToSvgGroup();
        badge.AddChildWithAlignment(this.BadgeTextConfig.WithText(badgeText).WithTextColor(badgeTextColor).ToSvgPath(), HorizontalAlignment.Center, VerticalAlignment.Center);

        SvgGroup from = this.SmallRectConfig
           .ToSvgPolygon()
           .ToSvgGroup();
        from.AddChildWithAlignment(this.SmallTextConfig.WithText(fromText).ToSvgPath(), HorizontalAlignment.InsideLeft, VerticalAlignment.InsideUp);

        SvgGroup to = this.SmallRectConfig
            .ToSvgPolygon()
            .ToSvgGroup();
        to.AddChildWithAlignment(this.SmallTextConfig.WithText(toText).ToSvgPath(), HorizontalAlignment.InsideLeft, VerticalAlignment.InsideDown);

        var verticalGroup = (new[] {from, to}).Stack(Orientation.Vertical);
        verticalGroup.AlignRelativeTo(badge, verticalAlignment: VerticalAlignment.Center);
        var delimiter = RectangleConfig.Transparent(new Size(40, 10)).ToSvgPolygon();

        var horizontalGroup = (new SvgVisual[] { badge, delimiter, verticalGroup }).Stack(Orientation.Horizontal);

        return horizontalGroup;
    }

    public SvgDocument CreateSvgDocument()
    {
        static SvgGroup CreateLineDelimiter(double width, double height, double strokeWidth)
        {
            var rect = RectangleConfig.Transparent(new Size(width, height)).ToSvgPolygon();
            var line = new RectangleConfig(new Size(width, strokeWidth), new DrawConfig(SKColors.White, SKColors.Transparent, StrokeWidth: 0)).ToSvgPolygon();
            line.AlignRelativeTo(rect, HorizontalAlignment.Center, VerticalAlignment.Center);
            return new[] { rect, line }.Group();
        }
        var white = SKColors.White;
        var black = SKColors.Black;
        var data = new (string, string, string, SKColor, SKColor)[]
        {
            ("1", "Skäggetorp", "Vidingsjö", white, new(235, 29, 42)),
            ("2", "Resecentrum", "Lambohov", white, new (0, 145, 202)),
            ("3", "Resecentrum", "Ryd", black, new (158, 213, 30)),
            ("4", "Resecentrum", "Lambohov", black, new (122, 210, 191)),
            ("5", "Skäggetorp", "Ekholmen", black, new (255, 242, 0)),
            ("6", "Resecentrum", "Malmslätt", white, new (74, 188, 57)),
            ("10", "Ekängen", "Sturefors", black, new (249, 167, 43)),
            ("11", "Resecentrum", "Övre Johannelund", black, new (253, 224, 173)),
            ("12", "Resecentrum", "Lambohov", black, new (204, 233, 148)),
            ("13", "Tallboda", "Nedre Johannelund", black, new (244, 111, 24)),
            ("14", "Gamla Linköping", "Södra Ekholmen", black, new (248, 161, 212)),
            ("15", "Berga C", "Södra Ullstämma", white, new (241, 66, 170)),
            ("16", "Resecentrum", "Södra Ekholmen", white, new (177, 15, 142)),
            ("17", "Resecentrum", "Södra Ekholmen", white, new (105, 81, 173)),
            ("18", "Resecentrum", "Gamla Linköping", white, new (21, 137, 105)),
            ("20", "Resecentrum", "Mjärdevi", black, new (189, 188, 188))
        };

        var list = new List<SvgVisual>();

        for (int i = 0; i < data.Length; i++)
        {
            list.Add(this.CreateSvgElement(data[i].Item1, data[i].Item2, data[i].Item3, data[i].Item4, data[i].Item5));
            if (i < data.Length - 1) list.Add(CreateLineDelimiter(860, 80, strokeWidth: 6));
        }

        var svgDocument = new SvgDocument();
        var stackedItems = list.Stack(Orientation.Vertical);


        var background = new RectangleConfig(stackedItems.BoundingBox.Size,
            new DrawConfig(new SKColor(50, 53, 58), SKColors.Transparent, 0), 0).ToSvgPolygon();

        svgDocument.Add(background);

        svgDocument.Add(stackedItems);

        return svgDocument;
    }
}