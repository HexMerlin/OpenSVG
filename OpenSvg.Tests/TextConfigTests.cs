using SkiaSharp;
using OpenSvg.Config;
using OpenSvg.SvgNodes;
using Xunit.Abstractions;

namespace OpenSvg.Tests;

public class TextConfigTests
{
    private readonly ITestOutputHelper _output;

    public TextConfigTests(ITestOutputHelper output)
    {
        this._output = output;
    }

    [Theory]
    [InlineData(Resources.FontFileNameDsDigital, TextRenderMode.SvgPath, false)]
    [InlineData(Resources.FontFileNameDsDigital, TextRenderMode.SvgText, true)]
    [InlineData(Resources.FontFileNameSourceSans, TextRenderMode.SvgPath, false)]
    [InlineData(Resources.FontFileNameSourceSans, TextRenderMode.SvgText, true)]
    public void CreateTextShape_FromFontAndRenderMode_MatchesExpected(string fontFileName, TextRenderMode textRenderMode, bool embedFont)
    {
        // Arrange
        var svgFont = SvgFont.LoadFromFile(TestPaths.GetFontPath(fontFileName));
        var fontName = svgFont.FontName;
        var (expectedFilePath, actualFilePath) =
            TestPaths.GetReferenceAndActualTestFilePaths($"{nameof(CreateTextShape_FromFontAndRenderMode_MatchesExpected)}_{fontName}_{textRenderMode}", "svg");

        // Act
        TextConfig textConfig = new TextConfig("SAMPLE TEXT", svgFont, 100, new DrawConfig(SKColors.Red, SKColors.Red, 1));

        SvgVisual svgTextElement = textRenderMode switch
        {
            TextRenderMode.SvgPath => textConfig.ToSvgPath(),
            TextRenderMode.SvgText => textConfig.ToSvgText(),
            _ => throw new NotSupportedException($"{nameof(textRenderMode)} not supported")
        };

        var textSize = svgTextElement.BoundingBox.Size;
        var rectangle = new RectangleConfig(textSize, new DrawConfig(SKColors.Black, SKColors.Red, 1)).ToSvgPolygon();
        this._output.WriteLine(svgTextElement.BoundingBox.ToString());

        var svgDocument = new SvgDocument();
        svgDocument.Add(rectangle);
        svgDocument.Add(svgTextElement);
     
        if (embedFont)
            svgDocument.EmbedFont(textConfig.SvgFont);

        svgDocument.SetViewPortToActualSize();
        svgDocument.Save(actualFilePath);

        // Assert
        var (isEqual, errorMessage) = FileIO.BinaryFileCompare(expectedFilePath, actualFilePath);
        Assert.True(isEqual, errorMessage);
    }

    [Fact]
    public void CreateTextPath_FromSvgWithEmbeddedFont_MatchesExpected()
    {
        // Arrange
        var (expectedFilePath, actualFilePath) = TestPaths.GetReferenceAndActualTestFilePaths(nameof(CreateTextPath_FromSvgWithEmbeddedFont_MatchesExpected), "svg");
        SvgDocument expectedSvgDocument = SvgDocument.Load(expectedFilePath);

        SvgPath expectedSvgPath = expectedSvgDocument.Descendants().OfType<SvgPath>().First();
     
        // Act
        var embeddedFont = expectedSvgDocument.EmbeddedFonts().First();
        SvgPath actualSvgPath =
            new TextConfig("TIME 23:59", embeddedFont, 100, new DrawConfig(SKColors.Blue, SKColors.DarkBlue, 1))
                .ToSvgPath();
        SvgDocument actualSvgDocument = actualSvgPath.ToSvgDocument();
        actualSvgDocument.SetViewPortToActualSize();
        actualSvgDocument.EmbedFont(embeddedFont);
        actualSvgDocument.Save(actualFilePath);

        // Assert 
        Assert.Equal(SKColors.Blue, expectedSvgPath.FillColor.Get());
        Assert.Equal(SKColors.DarkBlue, expectedSvgPath.StrokeColor.Get());
        Assert.Equal(1, expectedSvgPath.StrokeWidth.Get());
        Assert.Equal(AbsoluteOrRatio.Absolute(403.9112243652344), expectedSvgDocument.DefinedViewPortWidth.Get());
        Assert.Equal(AbsoluteOrRatio.Absolute(63.59090805053711), expectedSvgDocument.DefinedViewPortHeight.Get());

        (bool isEqual, string errorMessage) = expectedSvgPath.CompareSelfAndDescendants(actualSvgPath);
        Assert.True(isEqual, errorMessage);

        (isEqual, errorMessage) = expectedSvgDocument.CompareSelfAndDescendants(actualSvgDocument);
        Assert.True(isEqual, errorMessage);

        (isEqual, errorMessage) = FileIO.BinaryFileCompare(expectedFilePath, actualFilePath);
        Assert.True(isEqual, errorMessage);
    }
    
}