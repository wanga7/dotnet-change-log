using CSharpFunctionalExtensions;
using DotNetChangelog.ConventionalCommit;
using DotNetChangelog.Domain;
using DotNetChangelog.Utilities;

namespace DotNetChangelog.IO;

public class MarkdownWriter : ChangelogWriter
{
    public MarkdownWriter(string repoDirectory, string outputDirectory)
        : base(repoDirectory, outputDirectory) { }

    public override Result<string> Write(Changelog changelog) => Write(GetLines(changelog));

    public override Result<string> Write(ContinuousChangelog continuousChangelog) =>
        Write(GetLines(continuousChangelog));

    private Result<string> Write(IReadOnlyList<string> lines)
    {
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

    private static IReadOnlyList<string> GetLines(Changelog changelog)
    {
        List<string> lines = new() { FormatAsH1(changelog.GetTitleForDirectChangelog()) };
        lines.AddRange(FormatCommits(changelog.ConventionalCommits));
        return lines;
    }

    private static IReadOnlyList<string> GetLines(ContinuousChangelog continuousChangelog)
    {
        List<string> lines = new();
        IReadOnlyList<Changelog> changelogs = continuousChangelog.SortedChangelogs;
        foreach (Changelog changelog in changelogs)
        {
            lines.Add(
                changelog.ToTag.IsNormalVersion()
                    ? FormatAsH1(changelog.GetTitleForContinuousChangelog())
                    : FormatAsH2(changelog.GetTitleForContinuousChangelog())
            );
            lines.AddRange(FormatCommits(changelog.ConventionalCommits));
            lines.Add(string.Empty);
        }

        return lines;
    }

    private static IReadOnlyList<string> FormatCommits(ConventionalCommits commits)
    {
        List<string> lines = new();

        if (commits.Features.Length > 0)
        {
            lines.Add(FormatAsH3(ConventionalCommitCategory.Feature));
            lines.AddRange(commits.Features.Select(FormatCommit));
        }

        if (commits.Fixes.Length > 0)
        {
            lines.Add(FormatAsH3(ConventionalCommitCategory.Fixes));
            lines.AddRange(commits.Fixes.Select(FormatCommit));
        }

        if (commits.OtherNotableChanges.Length > 0)
        {
            lines.Add(FormatAsH3(ConventionalCommitCategory.OtherNotableChanges));
            lines.AddRange(commits.OtherNotableChanges.Select(FormatCommit));
        }

        return lines.Count > 0 ? lines : new[] { ConventionalCommitCategory.NoNotableChanges };
    }

    private static string FormatCommit(GitCommit commit)
    {
        return FormatAsBulletPoint($"{commit.ShortMessage} ({commit.Hash})");
    }

    private static string FormatAsH1(string text) => $"# {text}";

    private static string FormatAsH2(string text) => $"## {text}";

    private static string FormatAsH3(string text) => $"### {text}";

    private static string FormatAsBulletPoint(string text) => $"* {text}";

    private static string FormatAsBold(string text) => $"**{text}**";
}
