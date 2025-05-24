using System.Text;

namespace ShineGfxGenerator;

/// <summary>
/// File writer for HOI4 gfx files
/// </summary>
public static class GfxFileWriter
{
    private static readonly List<string> AllowedFileTypes = new()
    {
        ".gfx"
    };
    
    // .. CONTENT GENERATION
    
    /// <summary>
    /// Generate a shine file from a given gfx file
    /// </summary>
    public static bool TryWriteGfxFileAsShineFile(GfxFile gfxFile, string outputFilePath, out string? error)
    {
        //Check for valid extension
        var extensions = Path.GetExtension(outputFilePath);
        if (!AllowedFileTypes.Contains(extensions))
        {
            error = $"file format is not supported {Path.GetExtension(extensions)}.";
            return false;
        }
        
        //Create absolute path for output directory
        outputFilePath = FileIoUtility.GetAbsolutePath(outputFilePath);
        
        //Check if it's a valid directory
        var fileDirectory = Path.GetDirectoryName(outputFilePath);
        if (fileDirectory == null)
        {
            error = $"Invalid directory for file output '{fileDirectory}'";
            return false;
        }
        
        //Create directory if it doesn't exist yet
        var directory = Directory.CreateDirectory(fileDirectory);
        if (!directory.Exists)
        {
            error = $"Failed to create directory for path '{fileDirectory}'";
            return false;
        }
        
        //Generate file content
        var shineFileContent = GenerateShineFileContentFromGfxFile(gfxFile);

        //Try to write content to file
        try
        {
            File.WriteAllText(outputFilePath, shineFileContent);
        }
        catch (Exception e)
        {
            error = $"Failed to write file to file path '{outputFilePath}'\n";
            error += $"Exception Occured:\n{e}";
            return false;
        }
        
        error = null;
        return true;
    }

    /// <summary>
    /// Generate the content for a shine gfx file from a gfx file
    /// </summary>
    private static string GenerateShineFileContentFromGfxFile(GfxFile gfxFile)
    {
        var currentTabIndex = 0;
        var sb = new StringBuilder();
        
        //Add root sprite types object
        AddObjectOpening(sb, "spriteTypes", ref currentTabIndex);
        foreach (var spriteType in gfxFile.SpriteTypes)
        {
            AddSpace(sb);
            
            //Add new sprite type object and properties
            AddObjectOpening(sb, "SpriteType", ref currentTabIndex);
            AddProperty(sb, "name", $"{spriteType.Name}_shine".WithQuotes(), ref currentTabIndex);
            AddProperty(sb, "texturefile", spriteType.TextureFile.WithQuotes(), ref currentTabIndex);
            AddProperty(sb, "effectFile", "gfx/FX/buttonstate.lua".WithQuotes(), ref currentTabIndex);
            
            //Add animation object and properties
            AddAnimationSection(sb, spriteType, ref currentTabIndex, "-90.0");
            AddSpace(sb);
            AddAnimationSection(sb, spriteType, ref currentTabIndex, "90.0");
            
            AddProperty(sb, "legacy_lazy_load", "no", ref currentTabIndex);
            
            AddObjectClosing(sb, ref currentTabIndex);
        }
        AddObjectClosing(sb, ref currentTabIndex);
        
        return sb.ToString();
    }
    
    /// <summary>
    /// Generate content for the animation section of the shine gfx file
    /// </summary>
    private static void AddAnimationSection(StringBuilder sb, SpriteType spriteType, ref int currentTabIndex, string animationRotation)
    {
        AddObjectOpening(sb, "animation", ref currentTabIndex);
        AddProperty(sb, "animationmaskfile", spriteType.TextureFile.WithQuotes(), ref currentTabIndex);
        AddProperty(sb, "animationtexturefile", "gfx/interface/goals/shine_overlay.dds".WithQuotes(), ref currentTabIndex);
        AddProperty(sb, "animationrotation", animationRotation, ref currentTabIndex);
        AddProperty(sb, "animationlooping", "no", ref currentTabIndex);
        AddProperty(sb, "animationtime", "0.75", ref currentTabIndex);
        AddProperty(sb, "animationdelay", "0", ref currentTabIndex);
        AddProperty(sb, "animationblendmode", "add".WithQuotes(), ref currentTabIndex);
        AddProperty(sb, "animationtype", "scrolling".WithQuotes(), ref currentTabIndex);
        AddProperty(sb, "animationrotationoffset", "{ x = 0.0 y = 0.0 }", ref currentTabIndex);
        AddProperty(sb, "animationtexturescale", "{ x = 1.0 y = 1.0 }", ref currentTabIndex);
        AddObjectClosing(sb, ref currentTabIndex);
    }

    
    // .. OBJECT/PROPERTY GENERATION 
    
    private static string WithQuotes(this string value)
    {
        return $"\"{value}\"";
    }
    
    private static void AddSpace(StringBuilder currentContent)
    {
        currentContent.AppendLine();
    }
    
    private static void AddObjectOpening(StringBuilder currentContent, string newObjectName, ref int tabIndex)
    {
        currentContent.Append(GetTabIndexString(tabIndex));
        currentContent.Append(newObjectName);
        currentContent.AppendLine(" = {");
        tabIndex++;
    }
    
    private static void AddObjectClosing(StringBuilder currentContent, ref int tabIndex)
    {
        tabIndex--;
        currentContent.Append(GetTabIndexString(tabIndex));
        currentContent.AppendLine("}");
    }

    private static void AddProperty(StringBuilder currentContent, string propertyName, string propertyValue, ref int tabIndex)
    {
        currentContent.Append(GetTabIndexString(tabIndex));
        currentContent.Append(propertyName);
        currentContent.Append(" = ");
        currentContent.AppendLine(propertyValue);
    }
    
    private static string GetTabIndexString(int tabIndex, char tabChar = '\t')
    {
        var tabString = "";
        for (var i = 0; i < tabIndex; i++)
        {
            tabString += tabChar;
        }
        
        return tabString;
    }
}