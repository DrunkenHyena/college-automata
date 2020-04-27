using System.IO;

/// <summary>
/// This class manages the file that is used for user seeds.
/// Manages the saving, reading and overwriting of the file
/// </summary>
internal class FileWorker
{
    private static string SeedFile = "seedfile.txt";
    internal static string GetSeed()
    {
        string seed = "";
        if (File.Exists(SeedFile))
        {
            using (StreamReader sr = new StreamReader(SeedFile))
            {
                seed = sr.ReadLine();
            }
        }
        return seed;
    }

    internal static void WriteSeed(string text)
    {
        using (StreamWriter sw = new StreamWriter(SeedFile))
        {
            sw.WriteLine(text);
        }
    }

    internal static void RemoveSeed()
    {
        if (File.Exists(SeedFile))
            File.Delete(SeedFile);
    }

}