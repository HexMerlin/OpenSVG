
using OpenSvg.SvgNodes;
using SkiaSharp;

namespace OpenSvg.Tests.SvgNodes;


public class SvgLineTest
{
    [Fact]
    public void Constructor_SetsDefaultPropertiesCorrectly()
    {
        var line = new SvgLine();

        SKColor fillColor = line.FillColor;
        SKColor strokeColor = line.StrokeColor;
        float strokeWidth = line.StrokeWidth;

        Assert.Equal(SKColors.Black, fillColor);
        Assert.Equal(SKColors.Transparent, strokeColor);
        Assert.Equal(1, strokeWidth);
        Assert.Equal(0, line.X1);
        Assert.Equal(0, line.Y1);
        Assert.Equal(0, line.X2);
        Assert.Equal(0, line.Y2);
    }

    [Fact]
    public void CompareSelfAndDescendants_ReturnsTrueForEqualObjects()
    {
        var line1 = new SvgLine();
        var line2 = new SvgLine();

        (bool equal, string diffMessage) = line1.InformedEquals(line2);
        Assert.True(equal, diffMessage);

        Assert.Equal(line1, line2);

    }

    [Fact]
    public void CompareSelfAndDescendants_ReturnsFalseForDifferentObjects()
    {
        // Arrange
        var line1 = new SvgLine();
        var line2 = new SvgLine();
        line2.X1= 1000;

        //Act
       
        //Assert
        Assert.NotEqual(line1, line2);

    }

}