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

    /// <summary>
    /// Gets a value indicating whether the attribute has a default value.
    /// </summary>
    public bool HasDefaultValue { get; }

    /// <summary>
    /// Sets the value of the attribute from an XML string.
    /// </summary>
    /// <param name="xmlString">The XML string to set the value from.</param>
    public void Set(string xmlString);

    /// <summary>
    /// Converts the attribute to an XML string.
    /// </summary>
    /// <returns>The attribute as an XML string.</returns>
    public string ToXmlString();
}
