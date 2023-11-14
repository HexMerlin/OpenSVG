namespace OpenSvg.Attributes;

/// <summary>
/// Defines an interface for elements that can contain element data.
/// </summary>
public interface IHasElementContent
{
    public string Content { get; set; }
}