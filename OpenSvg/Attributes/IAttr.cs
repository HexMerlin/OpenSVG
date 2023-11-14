namespace OpenSvg.Attributes;

/// <summary>
/// Defines a common interface for SVG attributes.
/// </summary>
public interface IAttr : IEquatable<IAttr>
{
    public string Name { get; }

    public bool IsConstant { get; }
    public bool HasDefaultValue { get; }
    public void Set(string xmlString);
    string ToXmlString();

}