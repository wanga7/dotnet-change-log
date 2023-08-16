using DotNetChangelog.Domain;
using DotNetChangelog.Utilities;
using LibGit2Sharp;

namespace DotNetChangelog;

public class GitHistory
{
    private readonly string _repoPath;

    public GitHistory(string repoPath)
    {
        _repoPath = repoPath;
    }

    /// <summary>
    ///     Get change commits between two tags, excluding both ends, from newest to oldest
    /// </summary>
    public IReadOnlyList<GitCommit> GetHistoryDesc(string fromTag, string toTag)
    {
        using Repository repo = new(_repoPath);

        if (
            repo.Tags[fromTag].Target is not Commit fromCommit
            || repo.Tags[toTag].Target is not Commit toCommit
        )
        {
            return Array.Empty<GitCommit>();
        }

        CommitFilter filter =
            new() { IncludeReachableFrom = toCommit, ExcludeReachableFrom = fromCommit };

        List<GitCommit> commits = new();

        // Filter out merge commits (where commit.Parents.Count() > 1)
        commits.AddRange(
            repo.Commits.QueryBy(filter).Where(c => c.Parents.Count() == 1).Select(c => c.Convert())
        );

        commits.RemoveAt(0);

        return commits;
    }
}
