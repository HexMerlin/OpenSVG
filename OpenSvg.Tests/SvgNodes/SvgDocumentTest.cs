
using OpenSvg.SvgNodes;

namespace OpenSvg.Tests.SvgNodes;


public class SvgDocumentTest
{
    [Fact]
    public void Serialization_ConstructorToXDocumentAndBack_ObjectIsRestored()
    {
        // Arrange

        var expected = new SvgDocument();
        expected.Add(new SvgLine());
        expected.Add(new SvgText());

        // Act
        var xDocument = expected.ToXDocument();
        var actual = SvgDocument.FromXDocument(xDocument);

        //Assert
        Assert.Equal(expected, actual);

    }


}