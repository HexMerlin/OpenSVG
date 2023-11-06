// File FontTests.cs

using OpenSvg.SvgNodes;

namespace OpenSvg.Tests;

public class FontTests
{
    [Fact]
    public void LoadAndSaveFont_SameFile_BinaryFilesMatch()
    {
        // Arrange
        string expectedFilePath = TestPaths.GetFontPath(Resources.FontFileNameDsDigital);
        SvgFont expectedFont = SvgFont.LoadFromFile(expectedFilePath);

        // Act
        string actualFilePath = TestPaths.GetTestFilePath(nameof(LoadAndSaveFont_SameFile_BinaryFilesMatch), "output", FileCategory.Actual);
        expectedFont.Font.SaveToFile(actualFilePath);

        // Assert
        var (isEqual, errorMessage) = FileIO.BinaryFileCompare(expectedFilePath, actualFilePath);
        Assert.True(isEqual, errorMessage);
    }

    [Fact]
    public void CreateSvgCssStyle_FromFont_SavedFontMatchesOriginal()
    {
        // Arrange
        string expectedFilePath = TestPaths.GetFontPath(Resources.FontFileNameSourceSans);

        SvgFont expectedFont = SvgFont.LoadFromFile(expectedFilePath);

        // Act
        string actualFilePath = TestPaths.GetTestFilePath(nameof(CreateSvgCssStyle_FromFont_SavedFontMatchesOriginal), "output", FileCategory.Actual);
        SvgCssStyle svgCssStyle = new SvgCssStyle();
        svgCssStyle.Add(expectedFont);
        SvgFont actualFont = svgCssStyle.Fonts.First();
        actualFont.SaveFontToFile(actualFilePath);

        // Assert
        var (isEqual, errorMessage) = FileIO.BinaryFileCompare(expectedFilePath, actualFilePath);
        Assert.True(isEqual, errorMessage);
    }
}