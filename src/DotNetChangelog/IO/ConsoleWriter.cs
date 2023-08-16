using CSharpFunctionalExtensions;
using DotNetChangelog.Domain;
using DotNetChangelog.Utilities;

namespace DotNetChangelog.IO;

public class ConsoleWriter : ChangelogWriter
{
    public ConsoleWriter(string repoDirectory)
        : base(repoDirectory, string.Empty) { }

    public override Result<string> Write(Changelog changelog)
    {
        Console.WriteLine(changelog.GetSummary());

        foreach (GitCommit commit in changelog.Commits)
        {
            Console.WriteLine(commit.Format());
        }

        return Result.Success("console");
    }
}
