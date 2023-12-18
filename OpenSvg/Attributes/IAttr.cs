namespace OpenSvg.Attributes;


/// <summary>
/// Defines a common interface for SVG attributes.
/// </summary>
public interface IAttr : IEquatable<IAttr>
{
    /// <summary>
    /// Gets the name of the attribute.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets a value indicating whether the attribute is constant.
    /// </summary>
    public bool IsConstant { get; }

    
    ///<summary>
    /// Gets a value indicating whether the attribute should be serialized.
    /// </summary>
    /// <remarks>
    /// This property determines whether the attribute should be included in the serialization process.
    /// </remarks>
    public bool ShouldBeSerialized { get; }


    /// <summary>
    /// Used by the serializer to set the value of the attribute from an XML string.
    /// </summary>
    /// <param name="xmlString">The XML string to set the value from.</param>
     void SetBySerializer(string xmlString);

    /// <summary>
    /// Converts the attribute to an XML string.
    /// </summary>
    /// <returns>The attribute as an XML string.</returns>
    public string ToXmlString();
}
