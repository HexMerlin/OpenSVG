using OpenSvg.Config;
using OpenSvg.SvgNodes;
using SkiaSharp;

namespace OpenSvg.Tests;

public class TextConfigTests
{

    [Theory]
    [InlineData(Resources.FontFileNameDsDigital, TextRenderMode.SvgPath, false)]
    [InlineData(Resources.FontFileNameDsDigital, TextRenderMode.SvgText, true)]
    [InlineData(Resources.FontFileNameSourceSans, TextRenderMode.SvgPath, false)]
    [InlineData(Resources.FontFileNameSourceSans, TextRenderMode.SvgText, true)]
    public void CreateTextShape_FromFontAndRenderMode_MatchesExpected(string fontFileName,
        TextRenderMode textRenderMode, bool embedFont)
    {
        // Arrange
        var svgFont = SvgFont.LoadFromFile(TestPaths.GetFontPath(fontFileName));
        string fontName = svgFont.FontName;
        (string expectedFilePath, string actualFilePath) =
            TestPaths.GetReferenceAndActualTestFilePaths(
                $"{nameof(CreateTextShape_FromFontAndRenderMode_MatchesExpected)}_{fontName}_{textRenderMode}", "svg");

        // Act
        var textConfig = new TextConfig("SAMPLE TEXT", svgFont, 100, new DrawConfig(SKColors.Red, SKColors.Red, 1));

        SvgVisual svgTextElement = textRenderMode switch
        {
            TextRenderMode.SvgPath => textConfig.ToSvgPath(),
            TextRenderMode.SvgText => textConfig.ToSvgText(),
            _ => throw new NotSupportedException($"{nameof(textRenderMode)} not supported")
        };

        Size textSize = svgTextElement.BoundingBox.Size;
        var rectangle = new RectangleConfig(textSize, new DrawConfig(SKColors.Black, SKColors.Red, 1)).ToSvgPolygon();


        var svgDocument = new SvgDocument();
        svgDocument.Add(rectangle);
        svgDocument.Add(svgTextElement);

        if (embedFont)
            svgDocument.EmbedFont(textConfig.SvgFont);

        svgDocument.SetViewPortToActualSize();
        svgDocument.Save(actualFilePath);

        // Assert
        (bool isEqual, string errorMessage) = FileIO.BinaryFileCompare(expectedFilePath, actualFilePath);
        Assert.True(isEqual, errorMessage);
    }

    [Fact]
    public void CreateTextPath_FromSvgWithEmbeddedFont_MatchesExpected()
    {
        // Arrange
        (string expectedFilePath, string actualFilePath) =
            TestPaths.GetReferenceAndActualTestFilePaths(nameof(CreateTextPath_FromSvgWithEmbeddedFont_MatchesExpected),
                "svg");
        var expectedSvgDocument = SvgDocument.Load(expectedFilePath);

        SvgPath expectedSvgPath = expectedSvgDocument.Descendants().OfType<SvgPath>().First();

        // Act
        SvgFont embeddedFont = expectedSvgDocument.EmbeddedFonts().First();
        var actualSvgPath =
            new TextConfig("TIME 23:59", embeddedFont, 100, new DrawConfig(SKColors.Blue, SKColors.DarkBlue, 1))
                .ToSvgPath();
        var actualSvgDocument = actualSvgPath.ToSvgDocument();
        actualSvgDocument.SetViewPortToActualSize();
        actualSvgDocument.EmbedFont(embeddedFont);
        actualSvgDocument.Save(actualFilePath);

        // Assert 
        Assert.Equal(SKColors.Blue, expectedSvgPath.FillColor.Get());
        Assert.Equal(SKColors.DarkBlue, expectedSvgPath.StrokeColor.Get());
        Assert.Equal(1, expectedSvgPath.StrokeWidth.Get());
        Assert.Equal(AbsoluteOrRatio.Absolute(403.9112243652344), expectedSvgDocument.DefinedViewPortWidth.Get());
        Assert.Equal(AbsoluteOrRatio.Absolute(63.59090805053711), expectedSvgDocument.DefinedViewPortHeight.Get());

        (bool equal, string diffMessage) = expectedSvgDocument.InformedEquals(actualSvgDocument);
        Assert.True(equal, diffMessage);

        Assert.Equal(expectedSvgPath, actualSvgPath);
        Assert.Equal(expectedSvgDocument, actualSvgDocument);

        (var isEqual, var errorMessage) = FileIO.BinaryFileCompare(expectedFilePath, actualFilePath);
        Assert.True(isEqual, errorMessage);
    }
}