using System.Globalization;

namespace OpenSvg;

public static class CommonTypeExtensions
{
    /// <summary>
    ///     Compare two <see cref="double" /> values are equal, within the given precision<paramref name="precision" />.
    /// </summary>
    /// <param name="number1">The first number</param>
    /// <param name="number2">The second number</param>
    /// <param name="precision">The precision of the comparison. Default value is <c>0.000001</c></param>
    /// <seealso cref="Constants.DoublePrecision" />
    public static bool RobustEquals(this double number1, double number2, double precision = Constants.DoublePrecision) => Math.Abs(number1 - number2) < precision;

    public static bool RobustEquals(this float number1, float number2, double precision = Constants.DoublePrecision) => Math.Abs(number1 - number2) < precision;

    public static double ToDouble(this string value) => double.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture);

    public static string ToXmlString(this double value) => value.ToString(CultureInfo.InvariantCulture);

    public static float ToFloat(this string value) => float.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture);

    public static string ToXmlString(this float value) => value.ToString(CultureInfo.InvariantCulture);


    public static int ToInt(this string value) => int.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture);

    public static string ToXmlString(this int value) => value.ToString(CultureInfo.InvariantCulture);
}