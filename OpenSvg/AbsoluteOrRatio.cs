namespace OpenSvg;

public readonly struct AbsoluteOrRatio : IEquatable<AbsoluteOrRatio>
{
    public readonly double Value;

    public readonly bool IsAbsolute;

    public AbsoluteOrRatio(double value, bool isAbsolute)
    {
        this.Value = value;
        this.IsAbsolute = isAbsolute;
    }

    public static AbsoluteOrRatio Absolute(double value) => new(value, true);

    public static AbsoluteOrRatio Ratio(double value) => new(value, false);

    public double Resolve(Func<double> GetReferenceValue) => this.IsAbsolute ? this.Value : this.Value * GetReferenceValue();


    public static implicit operator AbsoluteOrRatio(double value) => Absolute(value);


    public bool Equals(AbsoluteOrRatio other) => this.Value.RobustEquals(other.Value) && this.IsAbsolute == other.IsAbsolute;

    public override bool Equals(object? obj) => obj is AbsoluteOrRatio other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(this.Value, this.IsAbsolute);

    public static bool operator ==(AbsoluteOrRatio left, AbsoluteOrRatio right) => left.Equals(right);

    public static bool operator !=(AbsoluteOrRatio left, AbsoluteOrRatio right) => !left.Equals(right);
}