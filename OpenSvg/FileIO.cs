namespace OpenSvg;

public static class FileIO
{
    /// <summary>
    ///     Compares files for equality using fast generic binary comparison.
    /// </summary>
    /// <param name="filePath1">The path to the first file.</param>
    /// <param name="filePath2">The path to the second file.</param>
    /// <returns>
    ///     A tuple containing a boolean indicating whether the files are equal and a string with an error message if they
    ///     are not.
    /// </returns>
    public static (bool IsEqual, string ErrorMessage) BinaryFileCompare(string filePath1, string filePath2)
    {
        const int bufferSize = 4096;
        byte[] buffer1 = new byte[bufferSize], buffer2 = new byte[bufferSize];

        using var fs1 = new FileStream(filePath1, FileMode.Open, FileAccess.Read);
        using var fs2 = new FileStream(filePath2, FileMode.Open, FileAccess.Read);

        if (fs1.Length != fs2.Length)
            return (false, $"Size mismatch: {filePath1} ({fs1.Length}), {filePath2} ({fs2.Length})");

        int bytesRead;
        while ((bytesRead = fs1.Read(buffer1, 0, bufferSize)) > 0)
        {
            fs2.Read(buffer2, 0, bufferSize);
            for (int i = 0; i < bytesRead; i++)
                if (buffer1[i] != buffer2[i])
                    return (false, $"Byte mismatch at {fs1.Position - bytesRead + i}");
        }

        return (true, string.Empty);
    }
}