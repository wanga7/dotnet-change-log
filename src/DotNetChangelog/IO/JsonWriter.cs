using System.Text.Json;
using CSharpFunctionalExtensions;
using DotNetChangelog.Domain;

namespace DotNetChangelog.IO;

public class JsonWriter : ChangelogWriter
{
    private static readonly JsonSerializerOptions JsonSerializerOptions =
        new() { WriteIndented = true };

    public JsonWriter(string repoDirectory, string outputDirectory)
        : base(repoDirectory, outputDirectory) { }

    public override Result<string> Write(Changelog changelog)
    {
        string file = Path.Combine(_outputDirectory, FileName + ".json");

        try
        {
            string content = JsonSerializer.Serialize(changelog, JsonSerializerOptions);
            File.WriteAllText(file, content);
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ex.ToString());
        }

        return Result.Success(Path.GetFullPath(file));
    }
}
