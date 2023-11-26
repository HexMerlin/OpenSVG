using SkiaSharp;

namespace OpenSvg;

/// <summary>
///     Extension methods for SkiaSharp types
/// </summary>
public static class SkiaSharpExtensions
{


    /// <summary>
    ///     Loads a <see cref="SKTypeface" /> font from the specified file path.
    /// </summary>
    /// <param name="fontFilePath">The file path of the font.</param>
    /// <returns>The loaded <see cref="SKTypeface" /> font.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="fontFilePath" /> is null.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the font file is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the font fails to load from the file path.</exception>
    public static SKTypeface LoadFromFile(this string fontFilePath)
    {
        ArgumentNullException.ThrowIfNull(fontFilePath, nameof(fontFilePath));
        if (!File.Exists(fontFilePath))
            throw new FileNotFoundException($"Font file not found: {fontFilePath}");

        using var fs = new FileStream(fontFilePath, FileMode.Open, FileAccess.Read);
        return SKTypeface.FromStream(fs) ??
               throw new InvalidOperationException($"Failed to load font from {fontFilePath}");
    }

    /// <summary>
    ///     Saves the specified <see cref="SKTypeface" /> font to a file
    /// </summary>
    /// <param name="font">The <see cref="SKTypeface" /> font.</param>
    /// <param name="fontFilePath">The file path of the font.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="fontFilePath" /> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if not all the data font from the font can be read.</exception>
    public static void SaveToFile(this SKTypeface font, string fontFilePath)
    {
        ArgumentNullException.ThrowIfNull(fontFilePath, nameof(fontFilePath));

        using SKStreamAsset stream = font.OpenStream(out _);
        byte[] fontData = new byte[stream.Length];
        int bytesRead = stream.Read(fontData, fontData.Length);
        if (bytesRead != fontData.Length)
            throw new InvalidOperationException("Failed to read the entire font data stream.");

        File.WriteAllBytes(fontFilePath, fontData);
    }
}