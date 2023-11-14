using System.Globalization;

namespace OpenSvg;


/// <summary>
/// Contains common type extension methods.
/// </summary>
public static class CommonTypeExtensions
{

    /// <summary>
    /// Rounds the given double value to the nearest integer.
    /// </summary>
    /// <param name="value">The value to round.</param>
    /// <returns>The rounded value.</returns>
    public static double Round(this double value) => Math.Round(value, Constants.DoubleDecimalPrecision);

    /// <summary>
    /// Converts the given string value to a double.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <returns>The converted double value.</returns>
    public static double ToDouble(this string value) => double.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture).Round();

    /// <summary>
    /// Converts the given double value to a string representation for XML serialization.
    /// </summary>
    /// <param name="value">The double value to convert.</param>
    /// <returns>The string representation of the double value.</returns>
    public static string ToXmlString(this double value) => value.Round().ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Converts the given float value to a string representation for XML serialization.
    /// </summary>
    /// <param name="value">The float value to convert.</param>
    /// <returns>The string representation of the float value.</returns>
    public static string ToXmlString(this float value) => value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Converts the given string value to an integer.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <returns>The converted integer value.</returns>
    public static int ToInt(this string value) => int.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture);

    /// <summary>
    /// Converts the given integer value to a string representation for XML serialization.
    /// </summary>
    /// <param name="value">The integer value to convert.</param>
    /// <returns>The string representation of the integer value.</returns>
    public static string ToXmlString(this int value) => value.ToString(CultureInfo.InvariantCulture);
}
