namespace OpenSvg.Attributes;


/// <summary>
/// Defines an interface for elements that can contain element data.
/// </summary>
public interface IHasElementContent
{
    /// <summary>
    /// Gets or sets the content of the element.
    /// </summary>
    public string Content { get; set; }
}
