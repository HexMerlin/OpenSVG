// File FontTests.cs

using OpenSvg.SvgNodes;

namespace OpenSvg.Tests;

public class FontTests
{
    [Fact]
    public void LoadAndSaveFont_SameFile_BinaryFilesMatch()
    {
        // Arrange
        var expectedFilePath = TestPaths.GetFontPath(Resources.FontFileNameDsDigital);
        var expectedFont = SvgFont.LoadFromFile(expectedFilePath);

        // Act
        var actualFilePath = TestPaths.GetTestFilePath(nameof(LoadAndSaveFont_SameFile_BinaryFilesMatch), "output",
            FileCategory.Actual);
        expectedFont.Font.SaveToFile(actualFilePath);

        // Assert
        var (isEqual, errorMessage) = FileIO.BinaryFileCompare(expectedFilePath, actualFilePath);
        Assert.True(isEqual, errorMessage);
    }

    [Fact]
    public void CreateSvgCssStyle_FromFont_SavedFontMatchesOriginal()
    {
        // Arrange
        var expectedFilePath = TestPaths.GetFontPath(Resources.FontFileNameSourceSans);

        var expectedFont = SvgFont.LoadFromFile(expectedFilePath);

        // Act
        var actualFilePath = TestPaths.GetTestFilePath(nameof(CreateSvgCssStyle_FromFont_SavedFontMatchesOriginal),
            "output", FileCategory.Actual);
        var svgCssStyle = new SvgCssStyle();
        svgCssStyle.Add(expectedFont);
        var actualFont = svgCssStyle.Fonts.First();
        actualFont.SaveFontToFile(actualFilePath);

        // Assert
        var (isEqual, errorMessage) = FileIO.BinaryFileCompare(expectedFilePath, actualFilePath);
        Assert.True(isEqual, errorMessage);
    }
}