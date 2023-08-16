using CSharpFunctionalExtensions;
using DotNetChangelog.Domain;
using DotNetChangelog.Utilities;

namespace DotNetChangelog.IO;

public class MarkdownWriter : ChangelogWriter
{
    public MarkdownWriter(string repoDirectory, string outputDirectory)
        : base(repoDirectory, outputDirectory) { }

    public override Result<string> Write(Changelog changelog)
    {
        List<string> lines = new() { FormatAsH1(changelog.GetSummary()) };
        lines.AddRange(changelog.Commits.Select(FormatCommit));

        string file = Path.Combine(_outputDirectory, FileName + ".md");

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

    private static string FormatCommit(GitCommit commit)
    {
        int idx = commit.ShortMessage.IndexOf(':');

        if (idx == -1)
        {
            return FormatAsBulletPoint($"{commit.Format()}");
        }

        string typeAndScope = commit.ShortMessage[..(idx + 1)];
        string description = commit.ShortMessage[(idx + 1)..];

        return FormatAsBulletPoint($"{FormatAsBold(typeAndScope)}{description} ({commit.Hash})");
    }

    private static string FormatAsH1(string text) => $"# {text}";

    private static string FormatAsBulletPoint(string text) => $"* {text}";

    private static string FormatAsBold(string text) => $"**{text}**";
}
