using SkiaSharp;
using OpenSvg.Config;
using OpenSvg.SvgNodes;

namespace OpenSvg.Tests
{
    public class SvgPathTests
    {
        [Fact]
        public void ApproximateToMultiPolygon_BoundingBoxIsUnchanged()
        {
            // Arrange

            SvgFont svgFont = SvgFont.LoadFromFile(TestPaths.GetFontPath(Resources.FontFileNameArial));
            TextConfig textConfig = new TextConfig("HÅÄj", svgFont, 100, new DrawConfig(SKColors.Red, SKColors.Black, 1));
            SvgPath svgPath = new SvgPath(textConfig);
            var expectedBoundingBox = svgPath.BoundingBox;

            // Act
            var multiPolygon = svgPath.ApproximateToMultiPolygon();
            var resultingBoundingBox = multiPolygon.BoundingBox();

            // Assert
            Assert.Equal(expectedBoundingBox, resultingBoundingBox);
        }
    }
}