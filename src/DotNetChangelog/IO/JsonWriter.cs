using System.Text.Json;
using CSharpFunctionalExtensions;
using DotNetChangelog.Domain;

namespace DotNetChangelog.IO;

public class JsonWriter : TextWriter
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
        string content;
        try
        {
            content = JsonSerializer.Serialize(jsonObject, JsonSerializerOptions);
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ex.ToString());
        }

        return PrependToFile(Path.Combine(_outputDirectory, FileName + ".json"), content);
    }
}
