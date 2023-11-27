namespace OpenSvg.Attributes;



/// <summary>
/// Represents the 'preserveAspectRatio' attribute value in SVG.
/// Encapsulates the alignment and meet-or-slice settings.
/// </summary>
/// <param name="Align">Specifies the alignment setting for the SVG content within its viewport.</param>
/// <param name="MeetOrSlice">Specifies whether the SVG content should meet or slice the boundaries of its viewport.</param>
public readonly record struct AspectRatio(AspectRatioAlign Align = AspectRatioAlign.None, AspectRatioMeetOrSlice MeetOrSlice = AspectRatioMeetOrSlice.Meet)
{
    /// <summary>
    /// Converts the <see cref="AspectRatio"/> to its equivalent SVG attribute string value.
    /// </summary>
    /// <returns>A string representing the 'preserveAspectRatio' attribute value in the format required by SVG.</returns>
    public string ToXmlString() => $"{EnumValueToString(Align)} {EnumValueToString(MeetOrSlice)}";


    /// <summary>
    /// Creates an <see cref="AspectRatio"/> object from an SVG attribute string.
    /// </summary>
    /// <param name="xmlString">The SVG attribute string to parse.</param>
    /// <returns>An <see cref="AspectRatio"/> object represented by the XML string.</returns>
    /// <exception cref="ArgumentException">Thrown if the XML string is not in a valid format.</exception>
    public static AspectRatio FromXmlString(string xmlString)
    {
        if (string.IsNullOrWhiteSpace(xmlString))
            return new AspectRatio();

        string[] parts = xmlString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
            throw new ArgumentException("Invalid XML string format for AspectRatio.", nameof(xmlString));
        
        AspectRatioAlign align = ParseEnum<AspectRatioAlign>(parts[0]);
        AspectRatioMeetOrSlice meetOrSlice = ParseEnum<AspectRatioMeetOrSlice>(parts[1]);

        return new AspectRatio(align, meetOrSlice);
    }

    private static T ParseEnum<T>(string value) where T : struct, Enum
    {
        if (!Enum.TryParse(value, true, out T result))
            throw new ArgumentException($"Invalid value '{value}' for enum type {typeof(T).Name}.");
        
        return result;
    }

    /// <inheritdoc/>
    public override string ToString() => ToXmlString();
    

    /// <summary>
    /// Converts an enum value to a string with the first letter in lowercase.
    /// This is used to match the case-sensitive format required by SVG attributes.
    /// </summary>
    /// <param name="enumValue">The enum value to convert.</param>
    /// <returns>The converted string with the first letter in lowercase.</returns>
    private static string EnumValueToString(Enum enumValue)
    {
        string enumString = enumValue.ToString();
        return char.ToLowerInvariant(enumString[0]) + enumString.Substring(1);
    }
}
