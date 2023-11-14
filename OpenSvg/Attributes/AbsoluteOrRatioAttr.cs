namespace OpenSvg.Attributes;

/// <summary>
/// Represents an attribute that can hold either an absolute or a ratio value.
/// </summary>
public class AbsoluteOrRatioAttr : Attr<AbsoluteOrRatio>
{
    public AbsoluteOrRatioAttr(string name) : base(name, AbsoluteOrRatio.Ratio(1), false)
    {
    }

    protected override string Serialize(AbsoluteOrRatio value) => value.IsAbsolute ? value.Value.ToXmlString() : $"{value.Value * 100d}%";

    protected override AbsoluteOrRatio Deserialize(string xmlString)
    {
        xmlString = xmlString.Trim();
        return xmlString.EndsWith("%")
            ? AbsoluteOrRatio.Ratio(xmlString[..^1].ToDouble() / 100d)
            : AbsoluteOrRatio.Absolute(xmlString.ToDouble());
    }

}