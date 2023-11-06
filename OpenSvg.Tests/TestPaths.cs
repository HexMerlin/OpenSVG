namespace OpenSvg.Tests;

public enum FileCategory
{
    Expected,
    Actual
}

public static class TestPaths
{
    private static readonly string BaseDirectory = AppContext.BaseDirectory;

    public static (string ExpectedFilePath, string ActualFilePath) GetReferenceAndActualTestFilePaths(string testName,
        string fileSuffix)
    {
        return (
            GetTestFilePath(testName, fileSuffix, FileCategory.Expected),
            GetTestFilePath(testName, fileSuffix, FileCategory.Actual)
        );
    }

    public static string GetTestFilePath(string testName, string fileSuffix, FileCategory fileCategory)
    {
        return Path.Combine(GetTestDirectory(fileCategory), $"{testName}_{fileCategory}.{fileSuffix}");
    }

    public static string GetTestDirectory(FileCategory fileCategory)
    {
        return fileCategory switch
        {
            FileCategory.Expected => Path.GetFullPath(Path.Combine(BaseDirectory, "..", "..", "..", "TestData")),
            FileCategory.Actual => Path.GetFullPath(Path.Combine(BaseDirectory, "..", "..", "..", "TestOutput")),
            _ => throw new ArgumentOutOfRangeException($"Unsupported {nameof(FileCategory)} {fileCategory}")
        };
    }

    public static string GetFontPath(string fontFileName)
    {
        return Path.Combine(GetTestDirectory(FileCategory.Expected), "Fonts", fontFileName);
    }

    public static (string fileName, string extension) SplitFileName(string fileName)
    {
        return (Path.GetFileNameWithoutExtension(fileName), Path.GetExtension(fileName).TrimStart('.'));
    }
}