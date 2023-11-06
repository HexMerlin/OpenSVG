using JetBrains.Annotations;
using OpenSvg.SvgNodes;
using SkiaSharp;

namespace OpenSvg.Tests.SvgNodes;

[TestSubject(typeof(SvgLine))]
public class SvgLineTest
{
    [Fact]
    public void Constructor_SetsDefaultPropertiesCorrectly()
    {
        var line = new SvgLine();

        var fillColor = line.FillColor.Get();
        var strokeColor = line.StrokeColor.Get();
        var strokeWidth = line.StrokeWidth.Get();

        Assert.Equal(SKColors.Black, fillColor);
        Assert.Equal(SKColors.Transparent, strokeColor);
        Assert.Equal(1, strokeWidth);
        Assert.Equal(0, line.X1.Get());
        Assert.Equal(0, line.Y1.Get());
        Assert.Equal(0, line.X2.Get());
        Assert.Equal(0, line.Y2.Get());
    }

    [Fact]
    public void CompareSelfAndDescendants_ReturnsTrueForEqualObjects()
    {
        var line1 = new SvgLine();
        var line2 = new SvgLine();

        var (equal, message) = line1.CompareSelfAndDescendants(line2);
        Assert.True(equal, message);
    }

    [Fact]
    public void CompareSelfAndDescendants_ReturnsFalseForDifferentObjects()
    {
        // Arrange
        var line1 = new SvgLine();
        var line2 = new SvgLine();
        line2.X1.Set(1000);

        //Act
        var (equal, message) = line1.CompareSelfAndDescendants(line2);

        //Assert
        Assert.Equal("X1: 0 != 1000", message);
        Assert.False(equal, message);
    }


    //[Fact]
    //public void SerializeToXml_XmlContainsEmptyValuesForDefaultProperties()
    //{
    //    SvgLine line = new SvgLine();

    //}   

    //[Fact]
    //public void SerializeToXml_XmlContainsEmptyValuesForDefaultProperties()
    //{
    //    SvgLine line = new SvgLine();
    //    line.FillColor.Set(SKColors.Red);

    //    XmlFormat xmlFormat = XmlFormat.Serialize(line);
    //    XElement xElement = xmlFormat.XElement;
    //    var elements = xElement.Elements();

    //    XElement? fillElementChild = xElement.Element(SvgNames.Fill);
    //    Assert.NotNull(fillElementChild);
    //    Assert.Equal("black", fillElementChild!.Value);

    //    Console.WriteLine(xElement);
    //    //string xml = xmlFormat.ToString();
    //    //Assert.Contains("X1=\"0\"", xml);
    //    //Assert.Contains("X2=\"0\"", xml);
    //    //Assert.Contains("Y1=\"0\"", xml);
    //    //Assert.Contains("Y2=\"0\"", xml);
    //    //Assert.Contains("Stroke=\"none\"", xml);
    //    //Assert.Contains("StrokeWidth=\"1\"", xml);
    //    //Assert.Contains("Fill=\"black\"", xml);
    //}
}