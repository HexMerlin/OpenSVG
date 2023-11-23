using OpenSvg.SvgNodes;

namespace OpenSvg.Tests;

public class SvgFileGeneratorTests
{
    [Fact]
    public void Test_SvgFileGeneration_MatchesExpected()
    {
        // Arrange
        (string expectedFilePath, string actualFilePath) =
            TestPaths.GetReferenceAndActualTestFilePaths(nameof(Test_SvgFileGeneration_MatchesExpected), "svg");
        var expectedSvgDocument = SvgDocument.Load(expectedFilePath);

        // Act
        Assert.Equal(860, expectedSvgDocument.ViewPortWidth);
        Assert.Equal(3920, expectedSvgDocument.ViewPortHeight);
        var config = new SvgImageConfig();
        SvgDocument actualSvgDocument = config.CreateSvgDocument();
        actualSvgDocument.SetViewPortToActualSize();
        actualSvgDocument.Save(actualFilePath);

        // Assert
        Assert.Equal(860, actualSvgDocument.ViewPortWidth);
        Assert.Equal(3920, actualSvgDocument.ViewPortHeight);
        (bool equal, string diffMessage) = expectedSvgDocument.InformedEquals(actualSvgDocument);
        Assert.True(equal, diffMessage);

        (bool isEqual, string errorMessage) = FileIO.BinaryFileCompare(expectedFilePath, actualFilePath);
        Assert.True(isEqual, errorMessage);
    }
}