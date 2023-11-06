using JetBrains.Annotations;
using OpenSvg.SvgNodes;

namespace OpenSvg.Tests.SvgNodes;

[TestSubject(typeof(SvgDocument))]
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
        (bool equal, string message) = expected.CompareSelfAndDescendants(actual);
        Assert.True(equal, message);
    }

    [Fact]
    public void Serialization_ConstructorToSvgAndBackToObject_ObjectIsRestored()
    {
        //// Arrange
        //SvgLine svgLine = new SvgLine();
        //SvgDocument doc = new SvgDocument();
        //doc.Add(svgLine);
        //XmlFormat xmlFormat = doc.SerializeToXmlFormat();

        //SvgFormat expected = new SvgFormat(xmlFormat);

        ////Act
        //XmlFormat xmlFormat2 = new XmlFormat(expected);

        //SvgDocument doc2 = xmlFormat2.Deserialize();

        //SvgDocument actualDoc = xmlFormat.Deserialize();

        ////Assert

        //Assert.Equal(xmlFormat, xmlFormat2);
    }
}