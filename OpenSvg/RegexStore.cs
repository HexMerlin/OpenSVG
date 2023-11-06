
using System.Text.RegularExpressions;

namespace OpenSvg;

internal static partial class RegexStore
{
    [GeneratedRegex($@"\b(\w+)\(([^)]+)\)")]
    internal static partial Regex ValidTransformString();

    [GeneratedRegex($@"\b{SvgNames.FontName}:\s*'([^']+)';")]
    internal static partial Regex GetFontNameFromXText();
    
    [GeneratedRegex($"{SvgNames.Scale}\\(([^,]+),([^)]+)\\)")]
    internal static partial Regex ValidScaleString();

    [GeneratedRegex($"{SvgNames.Rotate}\\(([^,]+),([^,]+),([^)]+)\\)")]
    internal static partial Regex ValidRotateString();

    [GeneratedRegex($"{SvgNames.SkewX}\\(([^)]+)\\)")]
    internal static partial Regex ValidSkewXString();

    [GeneratedRegex($"{SvgNames.SkewY}\\(([^)]+)\\)")]
    internal static partial Regex ValidSkewYString();

    [GeneratedRegex($"{SvgNames.Matrix}\\(([^,]+),([^,]+),([^,]+),([^,]+),([^,]+),([^)]+)\\)")]
    internal static partial Regex ValidMatrixString();
}