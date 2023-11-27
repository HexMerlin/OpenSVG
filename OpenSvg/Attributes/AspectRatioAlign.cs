namespace OpenSvg.Attributes;

/// <summary>
/// Specifies the alignment part of the 'preserveAspectRatio' attribute in SVG.
/// Determines how the SVG content is aligned within its viewport.
/// </summary>
public enum AspectRatioAlign
{
    /// <summary>
    /// Do not force uniform scaling. Scales the SVG content to fill the viewport.
    /// </summary>
    None,

    /// <summary>
    /// Align the minimum X and Y values of the viewBox with the smallest X and Y values of the viewport.
    /// </summary>
    XMinYMin,

    /// <summary>
    /// Align the midpoint X value of the viewBox with the midpoint X value of the viewport;
    /// Align the minimum Y value of the viewBox with the smallest Y value of the viewport.
    /// </summary>
    XMidYMin,

    /// <summary>
    /// Align the maximum X value of the viewBox with the largest X value of the viewport;
    /// Align the minimum Y value of the viewBox with the smallest Y value of the viewport.
    /// </summary>
    XMaxYMin,

    /// <summary>
    /// Align the minimum X value of the viewBox with the smallest X value of the viewport;
    /// Align the midpoint Y value of the viewBox with the midpoint Y value of the viewport.
    /// </summary>
    XMinYMid,

    /// <summary>
    /// Align the midpoint X value of the viewBox with the midpoint X value of the viewport;
    /// Align the midpoint Y value of the viewBox with the midpoint Y value of the viewport.
    /// </summary>
    XMidYMid,

    /// <summary>
    /// Align the maximum X value of the viewBox with the largest X value of the viewport;
    /// Align the midpoint Y value of the viewBox with the midpoint Y value of the viewport.
    /// </summary>
    XMaxYMid,

    /// <summary>
    /// Align the minimum X value of the viewBox with the smallest X value of the viewport;
    /// Align the maximum Y value of the viewBox with the largest Y value of the viewport.
    /// </summary>
    XMinYMax,

    /// <summary>
    /// Align the midpoint X value of the viewBox with the midpoint X value of the viewport;
    /// Align the maximum Y value of the viewBox with the largest Y value of the viewport.
    /// </summary>
    XMidYMax,

    /// <summary>
    /// Align the maximum X value of the viewBox with the largest X value of the viewport;
    /// Align the maximum Y value of the viewBox with the largest Y value of the viewport.
    /// </summary>
    XMaxYMax
}
