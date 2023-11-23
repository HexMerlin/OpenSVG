using OpenSvg.Config;
using OpenSvg.SvgNodes;
using SkiaSharp;

namespace OpenSvg.Tests;

public record SvgImageConfig
{
    public static readonly SvgFont BadgeFont =
        SvgFont.LoadFromFile(TestPaths.GetFontPath(Resources.FontFileNameBitterProBold));

    public static readonly SvgFont SmallTextFont =
        SvgFont.LoadFromFile(TestPaths.GetFontPath(Resources.FontFileNameHelveticaNowDisplayCondensed));

    public RectangleConfig BadgeRectangleConfig { get; } = new(new Size(300, 170),
        new DrawConfig(SKColors.DarkBlue, SKColors.Transparent, 1), CornerRadius: 40);

    public TextConfig BadgeTextConfig { get; } =
        new("", BadgeFont, 130, new DrawConfig(SKColors.LightGray, SKColors.DarkGray, 0));

    public RectangleConfig SmallRectConfig { get; } =
        new(new Size(500, 80), new DrawConfig(SKColors.Transparent, SKColors.Transparent, 0));

    public TextConfig SmallTextConfig { get; } =
        new("", SmallTextFont, 80, new DrawConfig(SKColors.White, SKColors.Transparent, 0));

    private SvgGroup CreateSvgElement(string badgeText, string fromText, string toText, SKColor badgeTextColor,
        SKColor badgeColor)
    {
        var badge = BadgeRectangleConfig.WithFillColor(badgeColor)
            .ToSvgPolygon()
            .ToSvgGroup();
        badge.AddChildWithAlignment(BadgeTextConfig.WithText(badgeText).WithTextColor(badgeTextColor).ToSvgPath(),
            HorizontalAlignment.CenterWithCenter, VerticalAlignment.CenterWithCenter);

        var from = SmallRectConfig
            .ToSvgPolygon()
            .ToSvgGroup();
        from.AddChildWithAlignment(SmallTextConfig.WithText(fromText).ToSvgPath(), HorizontalAlignment.LeftWithLeft,
            VerticalAlignment.TopWithTop);

        var to = SmallRectConfig
            .ToSvgPolygon()
            .ToSvgGroup();
        to.AddChildWithAlignment(SmallTextConfig.WithText(toText).ToSvgPath(), HorizontalAlignment.LeftWithLeft,
            VerticalAlignment.BottomWithBottom);

        SvgGroup verticalGroup = new[] { from, to }.Stack(Orientation.Vertical);
        verticalGroup.AlignRelativeTo(badge, verticalAlignment: VerticalAlignment.CenterWithCenter);
        var delimiter = RectangleConfig.Transparent(new Size(40, 10)).ToSvgPolygon();

        SvgGroup horizontalGroup = new SvgVisual[] { badge, delimiter, verticalGroup }.Stack(Orientation.Horizontal);

        return horizontalGroup;
    }

    public SvgDocument CreateSvgDocument()
    {
        static SvgGroup CreateLineDelimiter(double width, double height, double strokeWidth)
        {
            var rect = RectangleConfig.Transparent(new Size(width, height)).ToSvgPolygon();
            var line = new RectangleConfig(new Size(width, strokeWidth),
                new DrawConfig(SKColors.White, SKColors.Transparent, 0)).ToSvgPolygon();
            line.AlignRelativeTo(rect, HorizontalAlignment.CenterWithCenter, VerticalAlignment.CenterWithCenter);
            return new[] { rect, line }.Group();
        }

        SKColor white = SKColors.White;
        SKColor black = SKColors.Black;
        (string, string, string, SKColor, SKColor)[] data = new[]
        {
            ("1", "Skäggetorp", "Vidingsjö", white, new SKColor(235, 29, 42)),
            ("2", "Resecentrum", "Lambohov", white, new SKColor(0, 145, 202)),
            ("3", "Resecentrum", "Ryd", black, new SKColor(158, 213, 30)),
            ("4", "Resecentrum", "Lambohov", black, new SKColor(122, 210, 191)),
            ("5", "Skäggetorp", "Ekholmen", black, new SKColor(255, 242, 0)),
            ("6", "Resecentrum", "Malmslätt", white, new SKColor(74, 188, 57)),
            ("10", "Ekängen", "Sturefors", black, new SKColor(249, 167, 43)),
            ("11", "Resecentrum", "Övre Johannelund", black, new SKColor(253, 224, 173)),
            ("12", "Resecentrum", "Lambohov", black, new SKColor(204, 233, 148)),
            ("13", "Tallboda", "Nedre Johannelund", black, new SKColor(244, 111, 24)),
            ("14", "Gamla Linköping", "Södra Ekholmen", black, new SKColor(248, 161, 212)),
            ("15", "Berga C", "Södra Ullstämma", white, new SKColor(241, 66, 170)),
            ("16", "Resecentrum", "Södra Ekholmen", white, new SKColor(177, 15, 142)),
            ("17", "Resecentrum", "Södra Ekholmen", white, new SKColor(105, 81, 173)),
            ("18", "Resecentrum", "Gamla Linköping", white, new SKColor(21, 137, 105)),
            ("20", "Resecentrum", "Mjärdevi", black, new SKColor(189, 188, 188))
        };

        var list = new List<SvgVisual>();

        for (int i = 0; i < data.Length; i++)
        {
            list.Add(CreateSvgElement(data[i].Item1, data[i].Item2, data[i].Item3, data[i].Item4, data[i].Item5));
            if (i < data.Length - 1) list.Add(CreateLineDelimiter(860, 80, 6));
        }

        var svgDocument = new SvgDocument();
        SvgGroup stackedItems = list.Stack(Orientation.Vertical);


        var background = new RectangleConfig(stackedItems.BoundingBox.Size,
            new DrawConfig(new SKColor(50, 53, 58), SKColors.Transparent, 0), 0).ToSvgPolygon();

        svgDocument.Add(background);

        svgDocument.Add(stackedItems);

        return svgDocument;
    }
}