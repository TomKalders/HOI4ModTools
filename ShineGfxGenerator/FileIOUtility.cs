namespace ShineGfxGenerator;

/// <summary>
/// Utility for file input and output
/// </summary>
public static class FileIoUtility
{
    /// <summary>
    /// Get the absolute file path for the given file path (doesn't change path if it's already absolute)
    /// </summary>
    public static string GetAbsolutePath(string filePath)
    {
        var uri = new Uri(filePath, UriKind.RelativeOrAbsolute);
        if (!uri.IsAbsoluteUri) uri = new Uri(Path.Combine(Environment.CurrentDirectory, filePath));
        return Uri.UnescapeDataString(uri.AbsolutePath);
    }
}