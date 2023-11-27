using OpenSvg;
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
        Assert.Equal(860, expectedSvgDocument.BoundingBox.Size.Width);
        Assert.Equal(3920, expectedSvgDocument.BoundingBox.Size.Height);
        Assert.Equal(expectedSvgDocument.BoundingBox, expectedSvgDocument.ViewBox);
        Assert.Equal(Constants.DefaultContainerWidth, expectedSvgDocument.ViewPortWidth);
        Assert.Equal(Constants.DefaultContainerHeight, expectedSvgDocument.ViewPortHeight);
        
        var config = new SvgImageConfig();
        SvgDocument actualSvgDocument = config.CreateSvgDocument();
        actualSvgDocument.SetViewBoxToActualSizeAndDefaultViewPort();
        actualSvgDocument.Save(actualFilePath);

        // Assert
        Assert.Equal(860, actualSvgDocument.BoundingBox.Size.Width);
        Assert.Equal(3920, actualSvgDocument.BoundingBox.Size.Height);
        Assert.Equal(actualSvgDocument.BoundingBox, actualSvgDocument.ViewBox);
        Assert.Equal(Constants.DefaultContainerWidth, actualSvgDocument.ViewPortWidth);
        Assert.Equal(Constants.DefaultContainerHeight, actualSvgDocument.ViewPortHeight);
        (bool equal, string diffMessage) = expectedSvgDocument.InformedEquals(actualSvgDocument);
        Assert.True(equal, diffMessage);

        (bool isEqual, string errorMessage) = FileIO.BinaryFileCompare(expectedFilePath, actualFilePath);
        Assert.True(isEqual, errorMessage);
    }
}