namespace OpenSvg.Attributes;


/// <summary>
/// Represents an attribute of type T.
/// </summary>
/// <typeparam name="T">The type of the attribute.</typeparam>
public abstract class Attr<T> : IAttr, IEquatable<Attr<T>> where T : notnull, IEquatable<T>
{
    /// <summary>
    /// The default value of the attribute.
    /// </summary>
    public readonly T DefaultValue;

    private T val;

    /// <summary>
    /// Initializes a new instance of the <see cref="Attr{T}"/> class.
    /// </summary>
    /// <param name="name">The name of the attribute.</param>
    /// <param name="defaultValue">The default value of the attribute.</param>
    /// <param name="isConstant">Whether the attribute is constant.</param>
    protected Attr(string name, T defaultValue, bool isConstant)
    {
        Name = name;
        this.DefaultValue = defaultValue;
        this.val = defaultValue;
        IsConstant = isConstant;
    }

    /// <summary>
    /// The name of the attribute.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Whether the attribute is constant.
    /// </summary>
    public bool IsConstant { get; }

    ///<summary>
    /// Gets a value indicating whether the attribute should be serialized.
    /// </summary>
    /// <remarks>
    /// This property determines whether the attribute should be included in the serialization process.
    /// </remarks>
    public bool ShouldBeSerialized => IsConstant || !ValueEquals(DefaultValue);

    /// <summary>
    /// Sets the value of the attribute from an XML string.
    /// </summary>
    /// <param name="xmlString">The XML string.</param>
    public void SetBySerializer(string xmlString) => Set(Deserialize(xmlString));


    /// <summary>
    /// Returns the XML string representation of the attribute.
    /// </summary>
    /// <returns>The XML string representation of the attribute.</returns>
    public string ToXmlString() => Serialize(Get());

    /// <summary>
    /// Whether the default value of the attribute equals the specified value.
    /// </summary>
    /// <param name="value">The value to compare to the default value.</param>
    /// <returns>Whether the default value of the attribute equals the specified value.</returns>
    protected virtual bool DefaultEquals(T value) => this.DefaultValue.Equals(value);

    /// <summary>
    /// Whether the value of the attribute equals the specified value.
    /// </summary>
    /// <param name="value">The value to compare to the value of the attribute.</param>
    /// <returns>Whether the value of the attribute equals the specified value.</returns>
    protected virtual bool ValueEquals(T value) => Get().Equals(value);

    /// <summary>
    /// Whether the attribute equals another attribute.
    /// </summary>
    /// <param name="other">The attribute to compare to.</param>
    /// <returns>Whether the attribute equals another attribute.</returns>
    public bool Equals(Attr<T>? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        if (GetType() != other.GetType())
            return false;
        if (IsConstant != other.IsConstant)
            return false;
        if (Name != other.Name)
            return false;
        if (!DefaultEquals(other.DefaultValue))
            return false;
        if (!ValueEquals(other.Get()))
            return false;
        return true;
    }

    /// <summary>
    /// Sets the value of the attribute.
    /// </summary>
    /// <param name="value">The value to set.</param>
    public void Set(T value)
    {
        if (IsConstant) throw new InvalidOperationException("Cannot modify constant attribute");
        this.val = value;
    }

    /// <summary>
    /// Gets the value of the attribute.
    /// </summary>
    /// <returns>The value of the attribute.</returns>
    public T Get() => this.val;

    /// <summary>
    /// Serializes the value of the attribute to an XML string.
    /// </summary>
    /// <param name="value">The value to serialize.</param>
    /// <returns>The XML string representation of the value.</returns>
    protected abstract string Serialize(T value);

    /// <summary>
    /// Deserializes an XML string to the value of the attribute.
    /// </summary>
    /// <param name="xmlString">The XML string to deserialize.</param>
    /// <returns>The deserialized value.</returns>
    protected abstract T Deserialize(string xmlString);

    /// <summary>
    /// Returns the XML string representation of the attribute.
    /// </summary>
    /// <returns>The XML string representation of the attribute.</returns>
    public override string ToString() => ToXmlString();

    /// <summary>
    /// Whether the attribute equals another attribute.
    /// </summary>
    /// <param name="other">The attribute to compare to.</param>
    /// <returns>Whether the attribute equals another attribute.</returns>
    public bool Equals(IAttr? other)
        => Equals(other as Attr<T>);

    /// <summary>
    /// Returns the hash code of the attribute.
    /// </summary>
    /// <returns>The hash code of the attribute.</returns>
    public override int GetHashCode() => this.DefaultValue.GetHashCode();

    /// <summary>
    /// Whether two attributes are equal.
    /// </summary>
    /// <param name="left">The first attribute.</param>
    /// <param name="right">The second attribute.</param>
    /// <returns>Whether the two attributes are equal.</returns>
    public static bool operator ==(Attr<T> left, Attr<T> right) => left.Equals(right);

    /// <summary>
    /// Whether two attributes are not equal.
    /// </summary>
    /// <param name="left">The first attribute.</param>
    /// <param name="right">The second attribute.</param>
    /// <returns>Whether the two attributes are not equal.</returns>
    public static bool operator !=(Attr<T> left, Attr<T> right) => !left.Equals(right);


    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as Attr<T>);
}
