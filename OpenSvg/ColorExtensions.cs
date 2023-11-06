using System.Reflection;
using SkiaSharp;

namespace OpenSvg;

/// <summary>
///     Provides extension methods for SKColor to support hexadecimal color codes.
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    ///     Gets a <see cref="SKColor" /> that represents the transparent color.
    /// </summary>
    public static SKColor Transparent => SKColors.Transparent;

    /// <summary>
    ///     Returns whether the specified <see cref="SKColor" /> is transparent.
    /// </summary>
    /// <param name="sKColor">The <see cref="SKColor" /> instance to check.</param>
    /// <returns><c>true</c> if the color is transparent; otherwise, <c>false</c>.</returns>
    public static bool IsTransparent(this SKColor sKColor)
    {
        return sKColor.Alpha == 0;
    }

    /// <summary>
    ///     Parses a hexadecimal color string and returns the corresponding <see cref="SKColor" />.
    /// </summary>
    /// <param name="colorString">The hexadecimal color string to parse.</param>
    /// <returns>A <see cref="SKColor" /> instance that corresponds to the given hexadecimal color string.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when the specified string is null, empty, or has an invalid format.
    /// </exception>
    /// <remarks>
    ///     The method accepts color strings in the following formats:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>
    ///                 Hexadecimal RGB: Starts with a '#' followed by 6 hexadecimal characters (e.g., "#FF0000" for
    ///                 red).
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>Transparent: The string "none" to indicate a transparent color.</description>
    ///         </item>
    ///         <item>
    ///             <description>Named colors: Colors defined in SkiaSharp (e.g., "Red", "LightSkyBlue") are also supported.</description>
    ///         </item>
    ///     </list>
    ///     The method treats input case insensitive.
    /// </remarks>
    public static SKColor ToColor(this string colorString)
    {
        ArgumentException.ThrowIfNullOrEmpty(colorString, nameof(colorString));
        if (colorString.Equals(SvgNames.Transparent, StringComparison.OrdinalIgnoreCase))
            return Transparent;

        if (colorString[0] == '#' && SKColor.TryParse(colorString, out var color))
            return color;

        if (TryGetNamedColor(colorString, out var namedColor))
            return namedColor;

        throw new ArgumentException("Malformed color string or unknown color.", nameof(colorString));
    }

    /// <summary>
    ///     Returns a hexadecimal representation of the given <see cref="SKColor" />.
    /// </summary>
    /// <param name="color">The <see cref="SKColor" /> instance to be converted.</param>
    /// <returns>A hexadecimal string that represents the given color.</returns>
    public static string ToHexColorString(this SKColor color)
    {
        if (color == SKColors.Transparent)
            return SvgNames.Transparent;

        return $"#{color.Red:X2}{color.Green:X2}{color.Blue:X2}";
    }

    public static string ToString(this SKColor color)
    {
        return color.ToHexColorString();
    }

    /// <summary>
    ///     Tries to get an SKColor by its named color.
    /// </summary>
    /// <param name="colorString">The name of the color (case-insensitive).</param>
    /// <param name="color">The resulting SKColor if found.</param>
    /// <returns>True if the named color is found, otherwise false.</returns>
    private static bool TryGetNamedColor(string colorString, out SKColor color)
    {
        // Get all public static fields of the SKColors struct
        var fields = typeof(SKColors).GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (var field in fields)
            // Compare the name of the field to the input string, ignoring case
            if (string.Equals(field.Name, colorString, StringComparison.OrdinalIgnoreCase))
            {
                // If a match is found, set the out parameter and return true
                color = (SKColor)field.GetValue(null)!;
                return true;
            }

        // If no match is found, set the out parameter to SKColor.Empty and return false
        color = SKColors.Empty;
        return false;
    }
}