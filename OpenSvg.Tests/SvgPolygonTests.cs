using OpenSvg.Config;
using OpenSvg.SvgNodes;
using SkiaSharp;
using System.Diagnostics;

namespace OpenSvg.Tests;

public class SvgPolygonTests
{
    [Fact]
    public void CreateSvgPolygon_FromRectangleConfig_BoundingBoxIsExpected()
    {
        // Arrange
        Size expectedSize = new Size(100, 100);
        RectangleConfig rectangleConfig =
            new RectangleConfig(expectedSize, new DrawConfig(SKColors.LightBlue, SKColors.LightBlue, 10));
        BoundingBox expectedBoundingBox = new BoundingBox(new Point(0, 0), new Point(expectedSize.Width, expectedSize.Height));

        // Act
        SvgPolygon svgPolygon = rectangleConfig.ToSvgPolygon();
        BoundingBox actualBoundingBox = svgPolygon.BoundingBox;


        // Assert
        Assert.Equal(expectedBoundingBox, actualBoundingBox);
    }
}