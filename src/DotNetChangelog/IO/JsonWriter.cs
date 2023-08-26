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

    public override Result<string> Write(Changelog changelog) => WriteAsJson(changelog);

    public override Result<string> Write(ContinuousChangelog continuousChangelog) =>
        WriteAsJson(continuousChangelog);

    private Result<string> WriteAsJson<T>(T jsonObject)
    {
        string file = Path.Combine(_outputDirectory, FileName + ".json");

        try
        {
            string content = JsonSerializer.Serialize(jsonObject, JsonSerializerOptions);
            File.WriteAllText(file, content);
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ex.ToString());
        }

        return Result.Success(Path.GetFullPath(file));
    }
}
