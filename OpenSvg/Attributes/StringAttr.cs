namespace OpenSvg.Attributes;


public class StringAttr : Attr<string>
{
    public StringAttr(string name, string defaultValue, bool isConstant) : base(name, defaultValue, isConstant) { }
    protected override string Deserialize(string xmlString) => xmlString;
    
    protected override string Serialize(string value) => value;
}

