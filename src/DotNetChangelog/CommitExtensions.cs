using LibGit2Sharp;

namespace DotNetChangelog;

public static class CommitExtensions
{
    public static GitCommit Convert(this Commit commit)
    {
        return new(
            commit.Id?.ToString() ?? string.Empty,
            commit.MessageShort,
            commit.Parents.First().Id?.ToString() ?? string.Empty
        );
    }
}
