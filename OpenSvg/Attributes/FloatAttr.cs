namespace OpenSvg.Attributes;


/// <summary>
/// Represents an attribute for floating-point values.
/// </summary>
public class FloatAttr : Attr<float>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FloatAttr"/> class.
    /// </summary>
    /// <param name="name">The name of the attribute.</param>
    public FloatAttr(string name) : base(name, 0, false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FloatAttr"/> class.
    /// </summary>
    /// <param name="name">The name of the attribute.</param>
    /// <param name="defaultValue">The default value of the attribute.</param>
    public FloatAttr(string name, float defaultValue) : base(name, defaultValue.Round(), false)
    {
    }

    /// <inheritdoc/>
    protected override float Deserialize(string xmlString) => xmlString.ToFloat();

    /// <inheritdoc/>
    protected override string Serialize(float value) => value.ToXmlString();
}
