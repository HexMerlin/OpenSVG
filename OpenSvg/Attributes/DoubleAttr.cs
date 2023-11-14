namespace OpenSvg.Attributes;


/// <summary>
/// Represents an attribute for double precision floating-point values.
/// </summary>
public class DoubleAttr : Attr<double>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DoubleAttr"/> class.
    /// </summary>
    /// <param name="name">The name of the attribute.</param>
    public DoubleAttr(string name) : base(name, 0, false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DoubleAttr"/> class.
    /// </summary>
    /// <param name="name">The name of the attribute.</param>
    /// <param name="defaultValue">The default value of the attribute.</param>
    public DoubleAttr(string name, double defaultValue) : base(name, defaultValue.Round(), false)
    {
    }

    /// <inheritdoc/>
    protected override double Deserialize(string xmlString) => xmlString.ToDouble();

    /// <inheritdoc/>
    protected override string Serialize(double value) => value.ToXmlString();
}
