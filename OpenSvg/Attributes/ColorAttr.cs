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
    /// <param name="isConstant">Whether the attribute is a constant that cannot be changed.</param>
    public ColorAttr(string name, SKColor defaultValue, bool isConstant = false) : base(name, defaultValue, isConstant)
    {
    }

    /// <inheritdoc/>
    protected override SKColor Deserialize(string xmlString) => xmlString.ToColor();

    /// <inheritdoc/>
    protected override string Serialize(SKColor value) => value.ToHexColorString();
}
