using System.Globalization;

namespace OpenSvg.Gtfs;
public static class CommonParsing
{
        public static T ParseNumber<T>(this string input) where T : struct, IConvertible
    {
        if (input.Length == 0) return default;

        try
        {
            Type targetType = typeof(T);
            return (T)Convert.ChangeType(input, targetType, CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
            // Rethrow exceptions not handled properly by the method
            throw;
        }
    }
}
