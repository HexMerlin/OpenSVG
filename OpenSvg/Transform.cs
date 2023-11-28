using System.Numerics;

namespace OpenSvg;


/// <summary>
///     Represents a 2D transformation matrix applied to graphical elements.
/// </summary>
public readonly struct Transform : IEquatable<Transform>
{
    public readonly Matrix3x2 Matrix;

    /// <summary>
    ///     Represents the identity transformation matrix that, when applied, does not change the element.
    /// </summary>
    public static Transform Identity => new(Matrix3x2.Identity);

    /// <summary>
    ///     Initializes a new instance of the <see cref="Transform"/> struct.
    /// </summary>
    /// <param name="matrix">The matrix.</param>
    private Transform(Matrix3x2 matrix)
    { 
        Matrix = matrix;
    }

    /// <summary>
    ///     Gets the translation components of the transform.
    /// </summary>
    public (float dx, float dy) Translation => (this.Matrix.M31, this.Matrix.M32);

    /// <summary>
    ///     Gets the scale components of the transform.
    /// </summary>
    public (float scaleX, float scaleY) Scale => (this.Matrix.M11, this.Matrix.M22);

    /// <summary>
    ///     Gets the skew components of the transform in degrees.
    /// </summary>
    public (float skewXDegrees, float skewYDegrees) Skew =>
        (ToDeg(MathF.Atan2(this.Matrix.M21, this.Matrix.M11)), ToDeg(MathF.Atan2(this.Matrix.M12, this.Matrix.M22)));

    /// <summary>
    ///     Creates a translation transform that moves an element along the x and y-axis.
    /// </summary>
    /// <param name="dx">The distance to move along the x-axis.</param>
    /// <param name="dy">The distance to move along the y-axis.</param>
    /// <returns>A translation transform.</returns>
    public static Transform CreateTranslation(float dx = 0, float dy = 0) => new(Matrix3x2.CreateTranslation(dx, dy));

    /// <summary>
    ///     Creates a scale transform that resizes an element.
    /// </summary>
    /// <param name="scaleX">The scaling factor along the x-axis.</param>
    /// <param name="scaleY">The scaling factor along the y-axis.</param>
    /// <returns>A scaling transform.</returns>
    public static Transform CreateScale(float scaleX = 1, float scaleY = 1) => new(Matrix3x2.CreateScale(scaleX, scaleY));

    /// <summary>
    ///     Creates a rotation transform that rotates an element around a given point.
    /// </summary>
    /// <param name="angleDegrees">The angle of rotation in degrees.</param>
    /// <param name="pivotX">The x-coordinate of the pivot point.</param>
    /// <param name="pivotY">The y-coordinate of the pivot point.</param>
    /// <returns>A rotation transform.</returns>
    public static Transform CreateRotation(float angleDegrees = 0, float pivotX = 0, float pivotY = 0) => new(Matrix3x2.CreateRotation(ToRad(angleDegrees), new Vector2((float)pivotX, (float)pivotY)));

    /// <summary>
    ///     Creates a skew transform that slants the x and y coordinates of an element.
    /// </summary>
    /// <param name="skewXDegrees">The skew angle for the x-axis in degrees.</param>
    /// <param name="skewYDegrees">The skew angle for the y-axis in degrees.</param>
    /// <returns>A skew transform.</returns>
    public static Transform CreateSkew(float skewXDegrees = 0, float skewYDegrees = 0) => new(new Matrix3x2(1, ToRad(skewYDegrees), ToRad(skewXDegrees), 1, 0, 0));

    /// <summary>
    ///     Creates a general 2D transformation matrix.
    /// </summary>
    /// <param name="m11">Value at row 1, column 1 in the matrix.</param>
    /// <param name="m12">Value at row 1, column 2 in the matrix.</param>
    /// <param name="m21">Value at row 2, column 1 in the matrix.</param>
    /// <param name="m22">Value at row 2, column 2 in the matrix.</param>
    /// <param name="dx">The distance to translate along the x-axis.</param>
    /// <param name="dy">The distance to translate along the y-axis.</param>
    /// <returns>A general 2D transformation matrix.</returns>
    public static Transform CreateMatrix(float m11 = 1, float m12 = 0, float m21 = 0, float m22 = 1, float dx = 0, float dy = 0) 
        => new(new Matrix3x2(m11, m12, m21, m22, dx, dy));

    /// <summary>
    ///     Composes this transform with another, resulting in a transform that is the equivalent of applying each in sequence.
    /// </summary>
    /// <param name="other">The other transform to compose with.</param>
    /// <returns>A new transform that applies both transforms in sequence.</returns>
    public Transform ComposeWith(Transform other) => new(Matrix3x2.Multiply(this.Matrix, other.Matrix));

    /// <summary>
    /// Converts degrees to radians.
    /// </summary>
    /// <param name="degrees">Degrees to convert</param>
    private static float ToRad(float degrees) => (MathF.PI * degrees / 180.0f);

    /// <summary>
    /// Converts radians to degrees.
    /// </summary>
    /// <param name="radians">Radians to convert</param>
    private static float ToDeg(float radians) => radians * (180.0f / MathF.PI);


    /// <summary>
    ///     Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => ToXmlString();

    /// <summary>
    ///     Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is Transform transform && Equals(transform);

    /// <summary>
    ///     Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode() => this.Matrix.GetHashCode();

    /// <summary>
    ///     Determines whether the specified <see cref="Transform"/> is equal to the current <see cref="Transform"/>.
    /// </summary>
    /// <param name="other">The <see cref="Transform"/> to compare with the current <see cref="Transform"/>.</param>
    /// <returns>true if the specified <see cref="Transform"/> is equal to the current <see cref="Transform"/>; otherwise, false.</returns>
    public bool Equals(Transform other)
    {
        static bool AreEqual(float a, float b) => MathF.Abs(a - b) <= 1E-5f;
        Matrix3x2 m1 = this.Matrix;
        Matrix3x2 m2 = other.Matrix;
        return AreEqual(m1.M11, m2.M11) && AreEqual(m1.M12, m2.M12) && AreEqual(m1.M21, m2.M21) && AreEqual(m1.M22, m2.M22) && AreEqual(m1.M31, m2.M31) && AreEqual(m1.M32, m2.M32);

    }

    /// <summary>
    ///     Gets the XML string representation of this <see cref="Transform"/>.
    /// </summary>
    /// <returns>The XML string representation of this <see cref="Transform"/>.</returns>
    public string ToXmlString() => $"matrix({this.Matrix.M11.Round().ToXmlString()} {this.Matrix.M12.Round().ToXmlString()} {this.Matrix.M21.Round().ToXmlString()} {this.Matrix.M22.Round().ToXmlString()} {this.Matrix.M31.Round().ToXmlString()} {this.Matrix.M32.Round().ToXmlString()})";
     
    /// <summary>
    ///     Determines whether two specified instances of <see cref="Transform"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Transform"/> to compare.</param>
    /// <param name="right">The second <see cref="Transform"/> to compare.</param>
    /// <returns>true if <paramref name="left"/> and <paramref name="right"/> represent the same <see cref="Transform"/>; otherwise, false.</returns>
    public static bool operator ==(Transform left, Transform right) => left.Equals(right);

    /// <summary>
    ///     Determines whether two specified instances of <see cref="Transform"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Transform"/> to compare.</param>
    /// <param name="right">The second <see cref="Transform"/> to compare.</param>
    /// <returns>true if <paramref name="left"/> and <paramref name="right"/> do not represent the same <see cref="Transform"/>; otherwise, false.</returns>
    public static bool operator !=(Transform left, Transform right) => !(left == right);
}
