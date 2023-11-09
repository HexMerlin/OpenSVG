namespace OpenSvg.Attributes;


public abstract class Attr<T> : IAttr, IEquatable<Attr<T>> where T : notnull, IEquatable<T>
{
    public readonly T DefaultValue;

    private T val;

    protected Attr(string name, T defaultValue, bool isConstant)
    {
        Name = name;
        this.DefaultValue = defaultValue;
        this.val = defaultValue;
        IsConstant = isConstant;
    }

    public string Name { get; }

    public bool IsConstant { get; }

    public void Set(string xmlString) => Set(Deserialize(xmlString));

    public bool HasDefaultValue =>
        !IsConstant && ValueEquals(DefaultValue); //Constant attributes should always be serialized, and therefore this property returns false for them

    public string ToXmlString() => Serialize(Get());

    protected virtual bool DefaultEquals(T value) => this.DefaultValue.Equals(value);

    protected virtual bool ValueEquals(T value) => Get().Equals(value);

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
        if(!ValueEquals(other.Get()))
            return false;
        return true;
    }

    public void Set(T value)
    {
        if (IsConstant) throw new InvalidOperationException("Cannot modify constant attribute");
        this.val = value;
    }

    public T Get() => this.val;

    protected abstract string Serialize(T value);
    protected abstract T Deserialize(string xmlString);

    public override string ToString() => ToXmlString();

    public bool Equals(IAttr? other)
        => Equals(other as Attr<T>);

    public override int GetHashCode() => this.DefaultValue.GetHashCode();

    public static bool operator ==(Attr<T> left, Attr<T> right) => left.Equals(right);

    public static bool operator !=(Attr<T> left, Attr<T> right) => !left.Equals(right);
}