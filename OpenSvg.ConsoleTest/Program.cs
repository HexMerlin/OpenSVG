namespace OpenSvg.ConsoleTest;

internal class Program
{

   
    static async Task Main(string[] args)
    {
        try
        {
            
            await Docfx.Docset.Build(@"C:\Dev\HexMerlin\OpenSvg\docfx.json");
            Console.WriteLine("Documentation site built successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
    
}