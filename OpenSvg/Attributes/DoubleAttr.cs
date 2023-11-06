namespace OpenSvg.Attributes;

public class DoubleAttr : Attr<double>
{
    public DoubleAttr(string name) : base(name, 0, false)
    {
    }

    public DoubleAttr(string name, double defaultValue) : base(name, defaultValue, false)
    {
    }


    public override bool Equals(Attr<double>? other)
    {
        if (other is null) return false;
        return DefaultValue.RobustEquals(other.DefaultValue) && Get().RobustEquals(other.Get());
    }

    protected override double Deserialize(string xmlString)
    {
        return xmlString.ToDouble();
    }

    protected override string Serialize(double value)
    {
        return value.ToXmlString();
    }
}