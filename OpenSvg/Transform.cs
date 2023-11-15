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
        static float R(float value) => (float)Math.Round(value, Constants.DoubleDecimalPrecision);
        Matrix = new Matrix3x2(R(matrix.M11), R(matrix.M12), R(matrix.M21), R(matrix.M22), R(matrix.M31), R(matrix.M32));
    }

    /// <summary>
    ///     Gets the translation components of the transform.
    /// </summary>
    public (double dx, double dy) Translation => (this.Matrix.M31, this.Matrix.M32);

    /// <summary>
    ///     Gets the scale components of the transform.
    /// </summary>
    public (double scaleX, double scaleY) Scale => (this.Matrix.M11, this.Matrix.M22);

    /// <summary>
    ///     Gets the skew components of the transform in degrees.
    /// </summary>
    public (double skewXDegrees, double skewYDegrees) Skew =>
        (ToDeg(Math.Atan2(this.Matrix.M21, this.Matrix.M11)), ToDeg(Math.Atan2(this.Matrix.M12, this.Matrix.M22)));

    /// <summary>
    ///     Creates a translation transform that moves an element along the x and y-axis.
    /// </summary>
    /// <param name="dx">The distance to move along the x-axis.</param>
    /// <param name="dy">The distance to move along the y-axis.</param>
    /// <returns>A translation transform.</returns>
    public static Transform CreateTranslation(double dx = 0, double dy = 0) => new(Matrix3x2.CreateTranslation((float)dx, (float)dy));

    /// <summary>
    ///     Creates a scale transform that resizes an element.
    /// </summary>
    /// <param name="scaleX">The scaling factor along the x-axis.</param>
    /// <param name="scaleY">The scaling factor along the y-axis.</param>
    /// <returns>A scaling transform.</returns>
    public static Transform CreateScale(double scaleX = 1, double scaleY = 1) => new(Matrix3x2.CreateScale((float)scaleX, (float)scaleY));

    /// <summary>
    ///     Creates a rotation transform that rotates an element around a given point.
    /// </summary>
    /// <param name="angleDegrees">The angle of rotation in degrees.</param>
    /// <param name="pivotX">The x-coordinate of the pivot point.</param>
    /// <param name="pivotY">The y-coordinate of the pivot point.</param>
    /// <returns>A rotation transform.</returns>
    public static Transform CreateRotation(double angleDegrees = 0, double pivotX = 0, double pivotY = 0) => new(Matrix3x2.CreateRotation(ToRad(angleDegrees), new Vector2((float)pivotX, (float)pivotY)));

    /// <summary>
    ///     Creates a skew transform that slants the x and y coordinates of an element.
    /// </summary>
    /// <param name="skewXDegrees">The skew angle for the x-axis in degrees.</param>
    /// <param name="skewYDegrees">The skew angle for the y-axis in degrees.</param>
    /// <returns>A skew transform.</returns>
    public static Transform CreateSkew(double skewXDegrees = 0, double skewYDegrees = 0) => new(new Matrix3x2(1, ToRad(skewYDegrees), ToRad(skewXDegrees), 1, 0, 0));

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
    public static Transform CreateMatrix(double m11 = 1, double m12 = 0, double m21 = 0, double m22 = 1, double dx = 0,
        double dy = 0) => new(new Matrix3x2((float)m11, (float)m12, (float)m21, (float)m22, (float)dx, (float)dy));

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
    private static float ToRad(double degrees) => (float)(Math.PI * degrees / 180.0);

    /// <summary>
    /// Converts radians to degrees.
    /// </summary>
    /// <param name="radians">Radians to convert</param>
    private static double ToDeg(double radians) => radians * (180.0 / Math.PI);


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
    public bool Equals(Transform other) => this.Matrix.Equals(other.Matrix);

    /// <summary>
    ///     Gets the XML string representation of this <see cref="Transform"/>.
    /// </summary>
    /// <returns>The XML string representation of this <see cref="Transform"/>.</returns>
    public string ToXmlString() => $"matrix({this.Matrix.M11.ToXmlString()} {this.Matrix.M12.ToXmlString()} {this.Matrix.M21.ToXmlString()} {this.Matrix.M22.ToXmlString()} {this.Matrix.M31.ToXmlString()} {this.Matrix.M32.ToXmlString()})";

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
