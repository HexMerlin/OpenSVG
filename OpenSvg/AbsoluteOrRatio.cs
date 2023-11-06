
using OpenSvg.Attributes;

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
    public static AbsoluteOrRatio Absolute(double value) => new(value, true);

    public static AbsoluteOrRatio Ratio(double value) => new(value, false);

    public double Resolve(Func<double> GetReferenceValue) => IsAbsolute ? Value : Value * GetReferenceValue();


    public static implicit operator AbsoluteOrRatio(double value) => Absolute(value);


    public bool Equals(AbsoluteOrRatio other) => Value.RobustEquals(other.Value) && IsAbsolute == other.IsAbsolute;

    public override bool Equals(object? obj) => obj is AbsoluteOrRatio other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Value, IsAbsolute);

    public static bool operator ==(AbsoluteOrRatio left, AbsoluteOrRatio right) => left.Equals(right);

    public static bool operator !=(AbsoluteOrRatio left, AbsoluteOrRatio right) => !left.Equals(right);
}

