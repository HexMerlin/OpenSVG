namespace OpenSvg.Attributes;

public interface IAttr
{
    public string Name { get; }
    public void Set(string xmlString);

    public bool IsConstant { get; }
    public bool HasDefaultValue { get; }
    string ToXmlString();
}