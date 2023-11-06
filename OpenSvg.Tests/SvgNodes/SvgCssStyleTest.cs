using JetBrains.Annotations;
using OpenSvg.SvgNodes;
using SkiaSharp;

namespace OpenSvg.Tests.SvgNodes;

[TestSubject(typeof(SvgCssStyle))]
public class SvgCssStyleTest
{
    [Fact]
    public void Deserialize_FromFileWithEmbeddedFont_FileIsExpected()
    {
        // Arrange

        string sourceSvgFilePath =
            TestPaths.GetTestFilePath(nameof(Deserialize_FromFileWithEmbeddedFont_FileIsExpected), "svg",
                FileCategory.Expected);

        var sourceSvgDocument = SvgDocument.Load(sourceSvgFilePath);
        string expectedFontFilePath = TestPaths.GetFontPath(Resources.FontFileNameDsDigital);
        string actualFontFilePath = TestPaths.GetTestFilePath(nameof(Deserialize_FromFileWithEmbeddedFont_FileIsExpected),
            "ttf", FileCategory.Actual);

        // Act
        SvgFont actualFont = sourceSvgDocument.EmbeddedFonts().First();
        actualFont.SaveFontToFile(actualFontFilePath);

        // Assert
        Assert.Equal("DS-Digital", actualFont.FontName);
        Assert.Equal(SKFontStyleSlant.Upright, actualFont.FontSlant);
        Assert.Equal(400, actualFont.FontWeight);
        Assert.Equal(5, actualFont.FontWidth);
        Assert.NotNull(actualFont.Parent);
        Assert.IsType<SvgCssStyle>(actualFont.Parent);
        (bool isEqual, string? errorMessage) = FileIO.BinaryFileCompare(expectedFontFilePath, actualFontFilePath);
        Assert.True(isEqual, errorMessage);
    }
}