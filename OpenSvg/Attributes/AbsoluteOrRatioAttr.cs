﻿namespace OpenSvg.Attributes;

public class AbsoluteOrRatioAttr : Attr<AbsoluteOrRatio>
{
    public AbsoluteOrRatioAttr(string name) : base(name, AbsoluteOrRatio.Ratio(1), false)
    {
    }

    protected override string Serialize(AbsoluteOrRatio value)
    {
        return value.IsAbsolute ? value.Value.ToXmlString() : $"{value.Value * 100d}%";
    }

    protected override AbsoluteOrRatio Deserialize(string xmlString)
    {
        xmlString = xmlString.Trim();
        return xmlString.EndsWith("%")
            ? AbsoluteOrRatio.Ratio(xmlString[..^1].ToDouble() / 100d)
            : AbsoluteOrRatio.Absolute(xmlString.ToDouble());
    }
}