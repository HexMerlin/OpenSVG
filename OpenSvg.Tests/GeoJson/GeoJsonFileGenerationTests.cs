namespace OpenSvg.Tests.GeoJson;
public class GeoJsonFileGenerationTests
{
    [Fact]
    public void Test_GeoJsonFileGeneration_MatchesExpected()
    {
        // Arrange
        (string expectedFilePath, string actualFilePath) = TestPaths.GetReferenceAndActualTestFilePaths(nameof(Test_GeoJsonFileGeneration_MatchesExpected), "geojson");

        // Act
        GeoJsonTestHelper.GenerateTestGeoJsonFile(actualFilePath);

        // Assert
        (bool isEqual, string errorMessage) = FileIO.BinaryFileCompare(expectedFilePath, actualFilePath);
        Assert.True(isEqual, errorMessage);
    }
}
