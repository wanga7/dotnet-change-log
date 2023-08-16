using DotNetChangelog.Domain;
using LibGit2Sharp;

namespace DotNetChangelog.Utilities;

public static class CommitExtensions
{
    public static GitCommit Convert(this Commit commit)
    {
        return new()
        {
            Hash = commit.Id?.ToString() ?? string.Empty,
            ShortMessage = commit.MessageShort,
            ParentHash = commit.Parents.First().Id?.ToString() ?? string.Empty
        };
    }

    public static string Format(this GitCommit commit) => $"{commit.Hash} {commit.ShortMessage}";
}
