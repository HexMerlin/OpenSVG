
namespace OpenSvg.Attributes;


/// <summary>
/// Represents an attribute for setting 'preserveAspectRatio' attribute values for an SVG document.
/// </summary>
public class PreserveAspectRatioAttr : Attr<AspectRatio>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PreserveAspectRatioAttr"/> class.
    /// </summary>
    public PreserveAspectRatioAttr() : base(SvgNames.PreserveAspectRatio, new AspectRatio(), false)
    {
    }

    /// <inheritdoc/>
    protected override string Serialize(AspectRatio value) => value.ToXmlString();

    /// <inheritdoc/>
    protected override AspectRatio Deserialize(string xmlString) => AspectRatio.FromXmlString(xmlString);
}
