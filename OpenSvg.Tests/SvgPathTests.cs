using OpenSvg.Config;
using OpenSvg.SvgNodes;
using SkiaSharp;

namespace OpenSvg.Tests;

public class SvgPathTests
{
    [Fact]
    public void ApproximateToMultiPolygon_BoundingBoxIsUnchanged()
    {
        // Arrange

        var svgFont = SvgFont.LoadFromFile(TestPaths.GetFontPath(Resources.FontFileNameArial));
        var textConfig = new TextConfig("HÅÄj", svgFont, 100, new DrawConfig(SKColors.Red, SKColors.Black, 1));
        var svgPath = new SvgPath(textConfig);
        BoundingBox expectedBoundingBox = svgPath.BoundingBox;

        // Act
        MultiPolygon multiPolygon = svgPath.ApproximateToMultiPolygon();
        BoundingBox resultingBoundingBox = multiPolygon.BoundingBox();

        // Assert
        Assert.Equal(expectedBoundingBox, resultingBoundingBox);
    }
}