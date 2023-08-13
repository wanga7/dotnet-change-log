using LibGit2Sharp;

namespace Changelog;

public class GitHistory
{
    private readonly string _repoPath;

    public GitHistory(string repoPath)
    {
        _repoPath = repoPath;
    }

    public IReadOnlyList<Commit> GetHistory(string fromTag, string toTag)
    {
        using Repository repo = new(_repoPath);
        List<Commit> commits = new();

        var fromCommit = repo.Tags[fromTag].Target as Commit;
        var toCommit = repo.Tags[toTag].Target as Commit;

        if (fromCommit == null || toCommit == null) return commits;

        CommitFilter filter =
            new() { IncludeReachableFrom = toCommit, ExcludeReachableFrom = fromCommit };

        commits.AddRange(repo.Commits.QueryBy(filter));

        return commits;
    }
}