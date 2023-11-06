namespace OpenSvg;

public readonly struct AbsoluteOrRatio : IEquatable<AbsoluteOrRatio>
{
    public readonly double Value;

    public readonly bool IsAbsolute;

    public AbsoluteOrRatio(double value, bool isAbsolute)
    {
        Value = value;
        IsAbsolute = isAbsolute;
    }

    public static AbsoluteOrRatio Absolute(double value)
    {
        return new AbsoluteOrRatio(value, true);
    }

    public static AbsoluteOrRatio Ratio(double value)
    {
        return new AbsoluteOrRatio(value, false);
    }

    public double Resolve(Func<double> GetReferenceValue)
    {
        return IsAbsolute ? Value : Value * GetReferenceValue();
    }


    public static implicit operator AbsoluteOrRatio(double value)
    {
        return Absolute(value);
    }


    public bool Equals(AbsoluteOrRatio other)
    {
        return Value.RobustEquals(other.Value) && IsAbsolute == other.IsAbsolute;
    }

    public override bool Equals(object? obj)
    {
        return obj is AbsoluteOrRatio other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, IsAbsolute);
    }

    public static bool operator ==(AbsoluteOrRatio left, AbsoluteOrRatio right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(AbsoluteOrRatio left, AbsoluteOrRatio right)
    {
        return !left.Equals(right);
    }
}