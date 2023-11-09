namespace OpenSvg.Attributes;

public class DoubleAttr : Attr<double>
{
    public DoubleAttr(string name) : base(name, 0, false)
    {
    }

    public DoubleAttr(string name, double defaultValue) : base(name, defaultValue.Round(), false)
    {
    }

    protected override double Deserialize(string xmlString) => xmlString.ToDouble();

    protected override string Serialize(double value) => value.ToXmlString();
}