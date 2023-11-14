using SkiaSharp;

namespace OpenSvg.Attributes;


/// <summary>
/// Represents an attribute for color values.
/// </summary>
public class ColorAttr : Attr<SKColor>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ColorAttr"/> class.
    /// </summary>
    /// <param name="name">The name of the attribute.</param>
    /// <param name="defaultValue">The default value of the attribute.</param>
    /// <param name="isInherited">Whether the attribute is inherited.</param>
    public ColorAttr(string name, SKColor defaultValue, bool isInherited = false) : base(name, defaultValue, isInherited)
    {
    }

    /// <inheritdoc/>
    protected override SKColor Deserialize(string xmlString) => xmlString.ToColor();

    /// <inheritdoc/>
    protected override string Serialize(SKColor value) => value.ToHexColorString();
}
