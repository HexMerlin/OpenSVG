using JetBrains.Annotations;
using OpenSvg.Config;
using OpenSvg.SvgNodes;
using SkiaSharp;
using OpenSvg.Tests;

namespace OpenSvg.Tests.SvgNodes;

[TestSubject(typeof(SvgCssStyle))]
public class SvgCssStyleTest
{
    [Fact]
    public void Deserialize_FromFileWithEmbeddedFont_FileIsExpected()
    {
        // Arrange

        var sourceSvgFilePath =
            TestPaths.GetTestFilePath(nameof(Deserialize_FromFileWithEmbeddedFont_FileIsExpected), "svg",
                FileCategory.Expected);
       
        SvgDocument sourceSvgDocument = SvgDocument.Load(sourceSvgFilePath);
        var expectedFontFilePath = TestPaths.GetFontPath(Resources.FontFileNameDsDigital);
        var actualFontFilePath = TestPaths.GetTestFilePath(nameof(Deserialize_FromFileWithEmbeddedFont_FileIsExpected), "ttf", FileCategory.Actual);

        // Act
        var actualFont = sourceSvgDocument.EmbeddedFonts().First();
        actualFont.SaveFontToFile(actualFontFilePath);

        // Assert
        Assert.Equal("DS-Digital", actualFont.FontName);
        Assert.Equal(SKFontStyleSlant.Upright, actualFont.FontSlant);
        Assert.Equal(400, actualFont.FontWeight);
        Assert.Equal(5, actualFont.FontWidth);
        Assert.NotNull(actualFont.Parent);
        Assert.IsType<SvgCssStyle>(actualFont.Parent);
        (bool isEqual, string errorMessage) = FileIO.BinaryFileCompare(expectedFontFilePath, actualFontFilePath);
        Assert.True(isEqual, errorMessage);


    }
}