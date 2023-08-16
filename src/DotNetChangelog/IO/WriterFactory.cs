using DotNetChangelog.Domain;

namespace DotNetChangelog.IO;

public static class WriterFactory
{
    public static ChangelogWriter Create(
        string repoDirectory,
        string outputDirectory,
        OutputFormat outputFormat
    )
    {
        return outputFormat switch
        {
            OutputFormat.Console => new ConsoleWriter(repoDirectory),
            OutputFormat.Text => new TextWriter(repoDirectory, outputDirectory),
            OutputFormat.Json => new JsonWriter(repoDirectory, outputDirectory),
            OutputFormat.Markdown => new MarkdownWriter(repoDirectory, outputDirectory),
            _ => new ConsoleWriter(repoDirectory)
        };
    }
}
