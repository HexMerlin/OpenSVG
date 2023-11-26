namespace OpenSvg;

public enum FileFormat
{
    /// <summary>
    /// Uncompressed SVG in XML format
    /// </summary>
    Svg,

    /// <summary>
    /// Compressed SVG
    /// </summary>
    Svgz,

    /// <summary>
    /// Auto detects the file format based on the file extension
    /// <list type="bullet">
    /// <item>.svg => <see cref="Svg"/> (uncompressed)</item>
    /// <item>.svgz => <see cref="Svgz"/> (compressed)</item>
    /// </list>
    /// </summary>
    Auto
}
