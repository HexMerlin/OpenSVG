using SkiaSharp;
using System.Globalization;
using System.Xml.Serialization;
using OpenSvg;
namespace OpenSvg.Attributes;

public class DoubleAttr : Attr<double>
{
    public DoubleAttr(string name) : base(name,0, isConstant: false) { }
    public DoubleAttr(string name, double defaultValue) : base(name, defaultValue, isConstant: false) { }


    public override bool Equals(Attr<double>? other)
    {
        if (other is null) return false;
        return DefaultValue.RobustEquals(other.DefaultValue) && Get().RobustEquals(other.Get());
    }
   
    protected override double Deserialize(string xmlString) => xmlString.ToDouble();

    protected override string Serialize(double value) => value.ToXmlString();

   
}

