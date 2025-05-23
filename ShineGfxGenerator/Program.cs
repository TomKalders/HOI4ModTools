namespace ShineGfxGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Check arguments for application
            if (args.Length != 2)
            {
                ConsoleLogger.WriteErrorLine("Invalid usage");
                ConsoleLogger.WriteErrorLine("Usage: ShineGfxGenerator.exe <source file> <output file>");
                return;
            }

            //Read contents from gfx file
            if (!GfxFileReader.TryToReadGfxFile(args[0], out var gfxFile, out var error))
            {
                ConsoleLogger.WriteErrorLine("Failed to read sprite types from gfx.");
                ConsoleLogger.WriteErrorLine("Error:");
                ConsoleLogger.WriteErrorLine(error);
                return;
            }

            //Convert and write gfx file to shine gfx file
            if (!GfxFileWriter.TryWriteGfxFileAsShineFile(gfxFile, args[1], out error))
            {
                ConsoleLogger.WriteErrorLine("Failed to write gfx shine file.");
                ConsoleLogger.WriteErrorLine("Error:");
                ConsoleLogger.WriteErrorLine(error);
            }
            
            ConsoleLogger.WriteLine($"Successfully converted gfx file '{args[0]}' to shine gfx file '{args[1]}'", ConsoleColor.Green);
        }
    }
}