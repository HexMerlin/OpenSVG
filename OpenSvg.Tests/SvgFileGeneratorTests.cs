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
        var config = new SvgImageConfig();
        SvgDocument actualSvgDocument = config.CreateSvgDocument();
        actualSvgDocument.SetViewPortToActualSize();
        actualSvgDocument.Save(actualFilePath);

        // Assert
        (bool equal, string message) = expectedSvgDocument.CompareSelfAndDescendants(actualSvgDocument);
        Assert.True(equal, message);

        (bool isEqual, string errorMessage) = FileIO.BinaryFileCompare(expectedFilePath, actualFilePath);
        Assert.True(isEqual, errorMessage);
    }
}