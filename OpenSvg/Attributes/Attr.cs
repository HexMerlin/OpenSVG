namespace OpenSvg.Attributes;

public abstract class Attr<T> : IAttr, IEquatable<Attr<T>> where T : notnull
{
    public string Name { get; }

    public readonly T DefaultValue;

    private T val;

    public bool IsConstant { get; }

    protected Attr(string name, T defaultValue, bool isConstant)
    {
        Name = name;
        DefaultValue = defaultValue;
        val = defaultValue;
        IsConstant = isConstant;
    }

    public void Set(T value)
    {
        if (IsConstant) throw new InvalidOperationException("Cannot modify constant attribute");
        val = value;
    } 
    public void Set(string xmlString) => Set(Deserialize(xmlString));
    public bool HasDefaultValue => !IsConstant && Serialize(DefaultValue) == Serialize(val); //Constant attributes should always be serialized, and therefore this property returns false for them

    public T Get() => val;

    protected abstract string Serialize(T value);
    protected abstract T Deserialize(string xmlString);

    public string ToXmlString() => Serialize(val);

    public override string ToString() => ToXmlString();

    public virtual bool Equals(Attr<T>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        if (IsConstant != other.IsConstant) return false;
        if (Serialize(DefaultValue) != other.Serialize(other.DefaultValue)) return false;
        return Serialize(val) == other.Serialize(other.val);
    }

    public override bool Equals(object? obj) => Equals(obj as Attr<T>);

    public override int GetHashCode() => DefaultValue.GetHashCode();

    public static bool operator ==(Attr<T>? left, Attr<T>? right) => Equals(left, right);

    public static bool operator !=(Attr<T>? left, Attr<T>? right) => !Equals(left, right);

}
