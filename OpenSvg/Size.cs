namespace OpenSvg;

/// <summary>
///     A rectangular size in a pixel coordinate system.
/// </summary>
/// <param name="Width">The width of this size.</param>
/// <param name="Height">The height of this size.</param>
public readonly record struct Size(double Width, double Height)
{
    /// <summary>
    ///     Combines two sizes and returns the union size
    /// </summary>
    /// <param name="size1">The first size.</param>
    /// <param name="size2">The second size.</param>
    /// <returns>The union size</returns>
    public static Size Union(Size size1, Size size2)
    {
        return new Size(Math.Max(size1.Width, size2.Width), Math.Max(size1.Height, size2.Height));
    }


    public readonly override string ToString()
    {
        return $"Width: {Width.ToXmlString()}, Height: {Height.ToXmlString()}";
    }
}