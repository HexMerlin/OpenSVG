namespace OpenSvg.Attributes;


/// <summary>
/// Represents an attribute that can hold either an absolute or a ratio value.
/// </summary>
public class AbsoluteOrRatioAttr : Attr<AbsoluteOrRatio>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AbsoluteOrRatioAttr"/> class.
    /// </summary>
    /// <param name="name">The name of the attribute.</param>
    public AbsoluteOrRatioAttr(string name) : base(name, AbsoluteOrRatio.Ratio(1), false)
    {
    }

    /// <inheritdoc/>
    protected override string Serialize(AbsoluteOrRatio value) => value.IsAbsolute ? value.Value.ToXmlString() : $"{value.Value * 100d}%";

    /// <inheritdoc/>
    protected override AbsoluteOrRatio Deserialize(string xmlString)
        => AbsoluteOrRatio.FromXmlString(xmlString);

}
