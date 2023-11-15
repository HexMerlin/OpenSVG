namespace OpenSvg.Attributes;

/// <summary>
/// Represents an attribute for string values.
/// </summary>
public class StringAttr : Attr<string>
{
    /// <summary>
    /// Constructs a new instance of <see cref="StringAttr"/>.
    /// </summary>
    /// <param name="name">The name of this attribute.</param> 
    /// <param name="defaultValue">The default value of this attribute.</param>
    /// <param name="isConstant">Set whether this attribute is constant or if the value can be changed.</param>

    public StringAttr(string name, string defaultValue, bool isConstant) : base(name, defaultValue, isConstant)
    {
    }

    ///<inheritdoc/>
    protected override string Deserialize(string xmlString) => xmlString;
    
    ///<inheritdoc/>
    protected override string Serialize(string value) => value;
}