using OpenSvg.SvgNodes;

namespace OpenSvg.Tests;

public class SvgFileGeneratorTests
{
    [Fact]
    public void Test_SvgFileGeneration_MatchesExpected()
    {
        // Arrange
        var (expectedFilePath, actualFilePath) =
            TestPaths.GetReferenceAndActualTestFilePaths(nameof(Test_SvgFileGeneration_MatchesExpected), "svg");
        var expectedSvgDocument = SvgDocument.Load(expectedFilePath);

        // Act
        var config = new SvgImageConfig();
        var actualSvgDocument = config.CreateSvgDocument();
        actualSvgDocument.SetViewPortToActualSize();
        actualSvgDocument.Save(actualFilePath);

        // Assert
        var (equal, message) = expectedSvgDocument.CompareSelfAndDescendants(actualSvgDocument);
        Assert.True(equal, message);

        var (isEqual, errorMessage) = FileIO.BinaryFileCompare(expectedFilePath, actualFilePath);
        Assert.True(isEqual, errorMessage);
    }
}