namespace OpenSvg.Attributes;

/// <summary>
/// Specifies the 'meet or slice' part of the 'preserveAspectRatio' attribute in SVG.
/// Determines how the SVG content is scaled within its viewport.
/// </summary>
public enum AspectRatioMeetOrSlice
{
    /// <summary>
    /// Scale the graphic content to ensure that the entire viewBox is visible within the viewport.
    /// The entire viewBox is contained within the viewport, maintaining the aspect ratio.
    /// </summary>
    Meet,

    /// <summary>
    /// Scale the graphic content to cover the entire viewport, maintaining the aspect ratio.
    /// Parts of the viewBox might be sliced off to ensure that the viewport is fully covered.
    /// </summary>
    Slice
}
