using CSharpFunctionalExtensions;
using DotNetChangelog.Domain;
using DotNetChangelog.IO;
using DotNetChangelog.Utilities;
using LibGit2Sharp;
using ShellProgressBar;

namespace DotNetChangelog;

public class GitHistory
{
    private static readonly IComparer<VersionInfo> OfficialVersionComparer =
        new OfficialVersionComparerDesc();

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

    public SortedList<VersionInfo, Tag> GetTagsDesc(string pattern)
    {
        SortedList<VersionInfo, Tag> sortedTags = new(OfficialVersionComparer);

        using (Repository repo = new(_repoPath))
        {
            TagCollection tags = repo.Tags;
            using (
                ProgressBar progressBar =
                    new(tags.Count(), "Analyzing tags", ProgressBarConfigs.DefaultOptions)
            )
            {
                foreach (Tag tag in tags)
                {
                    if (!tag.Matches(pattern))
                    {
                        progressBar.Tick();
                        continue;
                    }

                    Result<VersionInfo> versionInfo = tag.ExtractVersionInfo();
                    if (versionInfo.IsFailure)
                    {
                        Console.WriteLine(
                            $"Failed to extract version info from tag \"{tag.FriendlyName}\": {versionInfo.Error}"
                        );
                        progressBar.Tick();
                        continue;
                    }

                    if (versionInfo.Value.IsPreRelease())
                    {
                        progressBar.Tick();
                        continue;
                    }

                    sortedTags.Add(versionInfo.Value, tag);
                    progressBar.Tick();
                }
            }
        }

        return sortedTags;
    }
}
