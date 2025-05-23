using System.Text.RegularExpressions;

namespace ShineGfxGenerator;

/// <summary>
/// File reader for HOI4 gfx file
/// </summary>
public static partial class GfxFileReader
{
    private static readonly List<string> AllowedFileTypes = new()
    {
        ".gfx"
    };
    
    /// <summary>
    /// Try to read the gfx file contents from a given gfx file path
    /// </summary>
    public static bool TryToReadGfxFile(string gfxFilePath, out GfxFile gfxFile, out string? error)
    {
        gfxFile = default;

        //Check file extension
        var fileExtension = Path.GetExtension(gfxFilePath);
        if (!AllowedFileTypes.Contains(fileExtension.ToLowerInvariant()))
        {
            error = $"Invalid file extension '{fileExtension}'";
            return false;
        }
        
        //Check file path
        var uri = new Uri(gfxFilePath, UriKind.RelativeOrAbsolute);
        if (!uri.IsAbsoluteUri) uri = new Uri(Path.Combine(Environment.CurrentDirectory, gfxFilePath));
        if (!File.Exists(uri.AbsolutePath))
        {
            error = $"Failed to find gfx file at path '{gfxFilePath}'";
            return false;
        }

        gfxFile = new GfxFile
        {
            FileName = Path.GetFileName(gfxFilePath)
        };

        var content = File.ReadAllText(gfxFilePath);
        
        var spriteTypeRegex = SpriteTypeRegex();
        var nameRegex = SpriteTypeNameRegex();
        var textureFileRegex = SpriteTypeTextureFileRegex();

        foreach (Match match in spriteTypeRegex.Matches(content))
        {
            if (!match.Success) continue;
            
            var nameMatch = nameRegex.Match(match.Groups[0].Value);
            if (!nameMatch.Success)
            {
                error = "Failed to parse name from sprite type entry:\n";
                error += match.Value;
                return false;
            }

            var textureFileMatch = textureFileRegex.Match(match.Groups[0].Value);
            if (!textureFileMatch.Success)
            {
                error = "Failed to parse texture file from sprite type entry:\n";
                error += match.Value;
                return false;
            }

            var spriteType = new SpriteType
            {
                Name = nameMatch.Groups[1].Value,
                TextureFile = textureFileMatch.Groups[1].Value,
            };
            
            gfxFile.SpriteTypes.Add(spriteType);
        }

        error = null;
        return true;
    }

    
    // .. REGEX
    
    [GeneratedRegex(@"SpriteType\s*=\s*({[\w\W]+?})")]
    private static partial Regex SpriteTypeRegex();

    [GeneratedRegex(@"name\s*=\s*""(.*)""")]
    private static partial Regex SpriteTypeNameRegex();
    
    [GeneratedRegex(@"texturefile\s*=\s*""(.*)""")]
    private static partial Regex SpriteTypeTextureFileRegex();
}