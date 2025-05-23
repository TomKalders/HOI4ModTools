namespace ShineGfxGenerator;

/// <summary>
/// Model class for gfx file
/// </summary>
public class GfxFile
{
    public string FileName = "";
    public readonly List<SpriteType> SpriteTypes = new();
}

/// <summary>
/// Model for sprite type object from gfx file
/// </summary>
public class SpriteType
{
    public required string Name;
    public required string TextureFile;
}