using CSharpFunctionalExtensions;
using DotNetChangelog.Domain;
using DotNetChangelog.Utilities;

namespace DotNetChangelog.IO;

public class TextWriter : ChangelogWriter
{
    public TextWriter(string repoDirectory, string outputDirectory)
        : base(repoDirectory, outputDirectory) { }

    public override Result<string> Write(Changelog changelog)
    {
        List<string> lines = new() { changelog.GetSummary() };
        lines.AddRange(changelog.Commits.Select(c => c.Format()));

        string file = Path.Combine(_outputDirectory, FileName + ".txt");

        try
        {
            File.WriteAllText(file, string.Join(Environment.NewLine, lines));
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ex.ToString());
        }

        return Result.Success(Path.GetFullPath(file));
    }
}
