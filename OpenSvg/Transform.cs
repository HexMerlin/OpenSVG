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

    private Transform(Matrix3x2 matrix) => this.Matrix = matrix;


    /// <summary>
    ///     Gets the translation components of the transform.
    /// </summary>
    public (double dx, double dy) Translation => (this.Matrix.Translation.X, this.Matrix.Translation.Y);

    /// <summary>
    ///     Gets the scale components of the transform.
    /// </summary>
    public (double scaleX, double scaleY) Scale => (this.Matrix.M11, this.Matrix.M22);

    /// <summary>
    ///     Gets the skew components of the transform in radians.
    /// </summary>
    public (double skewXDegrees, double skewYDegrees) Skew =>
        (Math.Atan2(this.Matrix.M21, this.Matrix.M11), Math.Atan2(this.Matrix.M12, this.Matrix.M22));

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

    private static float ToRad(double degrees) => (float)(Math.PI * degrees / 180.0);


    public string ToXmlString() => $"matrix({this.Matrix.M11.ToXmlString()} {this.Matrix.M12.ToXmlString()} {this.Matrix.M21.ToXmlString()} {this.Matrix.M22.ToXmlString()} {this.Matrix.Translation.X.ToXmlString()} {this.Matrix.Translation.Y.ToXmlString()})";

    public override string ToString() => ToXmlString();

    public bool Equals(Transform other) => this.Matrix.M11.RobustEquals(other.Matrix.M11) &&
               this.Matrix.M12.RobustEquals(other.Matrix.M12) &&
               this.Matrix.M21.RobustEquals(other.Matrix.M21) &&
               this.Matrix.M22.RobustEquals(other.Matrix.M22) &&
               this.Matrix.M31.RobustEquals(other.Matrix.M31) &&
               this.Matrix.M32.RobustEquals(other.Matrix.M32);

    public override bool Equals(object? obj) => obj is Transform transform && Equals(transform);

    public override int GetHashCode() => this.Matrix.GetHashCode();

    public static bool operator ==(Transform left, Transform right) => left.Equals(right);

    public static bool operator !=(Transform left, Transform right) => !(left == right);
}