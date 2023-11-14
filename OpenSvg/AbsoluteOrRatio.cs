namespace OpenSvg;


/// <summary>
/// Represents a value that can be either an absolute value or a ratio.
/// </summary>
public readonly struct AbsoluteOrRatio : IEquatable<AbsoluteOrRatio>
{
    /// <summary>
    /// The value of the AbsoluteOrRatio.
    /// </summary>
    public readonly double Value;

    /// <summary>
    /// Indicates whether the value is absolute or a ratio.
    /// </summary>
    public readonly bool IsAbsolute;

    /// <summary>
    /// Initializes a new instance of the AbsoluteOrRatio struct.
    /// </summary>
    /// <param name="value">The value of the AbsoluteOrRatio.</param>
    /// <param name="isAbsolute">Indicates whether the value is absolute or a ratio.</param>
    public AbsoluteOrRatio(double value, bool isAbsolute)
    {
        this.Value = value.Round();
        this.IsAbsolute = isAbsolute;
    }

    /// <summary>
    /// Creates a new AbsoluteOrRatio with an absolute value.
    /// </summary>
    /// <param name="value">The absolute value.</param>
    /// <returns>A new AbsoluteOrRatio with an absolute value.</returns>
    public static AbsoluteOrRatio Absolute(double value) => new(value, true);

    /// <summary>
    /// Creates a new AbsoluteOrRatio with a ratio value.
    /// </summary>
    /// <param name="value">The ratio value.</param>
    /// <returns>A new AbsoluteOrRatio with a ratio value.</returns>
    public static AbsoluteOrRatio Ratio(double value) => new(value, false);

    /// <summary>
    /// Resolves the value of the AbsoluteOrRatio.
    /// </summary>
    /// <param name="GetReferenceValue">A function that returns the reference value for resolving the abolute value.</param>
    /// <returns>The resolved value of the AbsoluteOrRatio.</returns>
    public double Resolve(Func<double> GetReferenceValue) => this.IsAbsolute ? this.Value : this.Value * GetReferenceValue();

    /// <summary>
    /// Implicitly converts a double to an AbsoluteOrRatio with an absolute value.
    /// </summary>
    /// <param name="value">The absolute value.</param>
    /// <returns>An AbsoluteOrRatio with an absolute value.</returns>
    public static implicit operator AbsoluteOrRatio(double value) => Absolute(value);

    /// <summary>
    /// Determines whether the specified AbsoluteOrRatio is equal to the current AbsoluteOrRatio.
    /// </summary>
    /// <param name="other">The AbsoluteOrRatio to compare with the current AbsoluteOrRatio.</param>
    /// <returns>true if the specified AbsoluteOrRatio is equal to the current AbsoluteOrRatio; otherwise, false.</returns>
    public bool Equals(AbsoluteOrRatio other) => this.Value.Equals(other.Value) && this.IsAbsolute == other.IsAbsolute;

    /// <summary>
    /// Determines whether the specified object is equal to the current AbsoluteOrRatio.
    /// </summary>
    /// <param name="obj">The object to compare with the current AbsoluteOrRatio.</param>
    /// <returns>true if the specified object is equal to the current AbsoluteOrRatio; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is AbsoluteOrRatio other && Equals(other);

    /// <summary>
    /// Returns a hash code for the current AbsoluteOrRatio.
    /// </summary>
    /// <returns>A hash code for the current AbsoluteOrRatio.</returns>
    public override int GetHashCode() => HashCode.Combine(this.Value, this.IsAbsolute);

    /// <summary>
    /// Determines whether two AbsoluteOrRatio objects are equal.
    /// </summary>
    /// <param name="left">The first AbsoluteOrRatio to compare.</param>
    /// <param name="right">The second AbsoluteOrRatio to compare.</param>
    /// <returns>true if the two AbsoluteOrRatio objects are equal; otherwise, false.</returns>
    public static bool operator ==(AbsoluteOrRatio left, AbsoluteOrRatio right) => left.Equals(right);

    /// <summary>
    /// Determines whether two AbsoluteOrRatio objects are not equal.
    /// </summary>
    /// <param name="left">The first AbsoluteOrRatio to compare.</param>
    /// <param name="right">The second AbsoluteOrRatio to compare.</param>
    /// <returns>true if the two AbsoluteOrRatio objects are not equal; otherwise, false.</returns>
    public static bool operator !=(AbsoluteOrRatio left, AbsoluteOrRatio right) => !left.Equals(right);
}
