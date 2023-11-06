using OpenSvg.Config;
using SkiaSharp;

namespace OpenSvg.Tests;

public class SvgPolygonTests
{
    [Fact]
    public void CreateSvgPolygon_FromRectangleConfig_BoundingBoxIsExpected()
    {
        // Arrange
        var expectedSize = new Size(100, 100);
        var rectangleConfig =
            new RectangleConfig(expectedSize, new DrawConfig(SKColors.LightBlue, SKColors.LightBlue, 10));
        var expectedBoundingBox = new BoundingBox(new Point(0, 0), new Point(expectedSize.Width, expectedSize.Height));

        // Act
        var svgPolygon = rectangleConfig.ToSvgPolygon();
        BoundingBox actualBoundingBox = svgPolygon.BoundingBox;

        // Assert
        Assert.Equal(expectedBoundingBox, actualBoundingBox);
    }
}