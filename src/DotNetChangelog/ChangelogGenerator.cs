using System.Collections.Concurrent;
using CSharpFunctionalExtensions;
using DotNetChangelog.ConventionalCommit;
using DotNetChangelog.Domain;
using DotNetChangelog.IO;
using ShellProgressBar;

namespace DotNetChangelog;

public class ChangelogGenerator
{
    private readonly GitHistory _gitHistory;
    private readonly DotNetAffectedClient _dotNetAffectedClient;

    public ChangelogGenerator(string repoPath, string excludedPattern)
    {
        _gitHistory = new(repoPath);
        _dotNetAffectedClient = new(repoPath, excludedPattern);
    }

    public Result<Changelog> GetDirectChangelog(string project, string fromTag, string toTag)
    {
        Console.WriteLine($"Analyzing changelog for {project} from {fromTag} to {toTag}...");

        Result<VersionTag> fromVersionTag = _gitHistory.GetVersionTag(fromTag);
        if (fromVersionTag.IsFailure)
        {
            return Result.Failure<Changelog>($"invalid tag \"{fromTag}\": {fromVersionTag.Error}");
        }

        Result<VersionTag> toVersionTag = _gitHistory.GetVersionTag(toTag);
        if (toVersionTag.IsFailure)
        {
            return Result.Failure<Changelog>($"invalid tag \"{toTag}\": {toVersionTag.Error}");
        }

        IReadOnlyList<GitCommit> commitsDesc = _gitHistory.GetHistoryDesc(fromTag, toTag);
        Console.WriteLine($"Found {commitsDesc.Count} commits");

        Console.WriteLine($"Determining commits affecting {project}...");
        ConcurrentDictionary<string, bool> commitsStates = new();
        string error = string.Empty;

        using (
            ProgressBar progressBar =
                new(
                    commitsDesc.Count,
                    $"Analyzing commits {fromTag}...{toTag}",
                    ProgressBarConfigs.DefaultOptions
                )
        )
        {
            Parallel.ForEach(
                commitsDesc,
                (commit, state) =>
                {
                    Result<bool> result = _dotNetAffectedClient.IsProjectAffected(
                        project,
                        commit.ParentHash,
                        commit.Hash
                    );

                    if (result.IsFailure)
                    {
                        error =
                            $"Failed to analyze affected projects from {commit}: {result.Error}";
                        state.Stop();
                    }
                    else
                    {
                        commitsStates[commit.Hash] = result.Value;
                    }

                    progressBar.Tick();
                }
            );
        }

        return string.IsNullOrEmpty(error)
            ? Result.Success(
                new Changelog(
                    fromVersionTag.Value,
                    toVersionTag.Value,
                    ConventionalCommitClassifier.Classify(
                        commitsDesc
                            .Where(
                                c =>
                                    commitsStates.TryGetValue(c.Hash, out bool isAffected)
                                    && isAffected
                            )
                            .ToArray()
                    )
                )
            )
            : Result.Failure<Changelog>(error);
    }

    public Result<ContinuousChangelog> GetContinuousChangelog(
        string project,
        string fromTag,
        string toTag,
        string tagPattern
    )
    {
        IReadOnlyList<VersionTag> tagsDesc = _gitHistory.GetVersionTagsDesc(
            tagPattern,
            fromTag,
            toTag
        );
        if (tagsDesc.Count == 0)
        {
            return Result.Failure<ContinuousChangelog>("no tag match");
        }

        List<Changelog> changelogs = new();
        for (int i = 0; i < tagsDesc.Count - 1; i++)
        {
            VersionTag toVersionTag = tagsDesc[i];

            // minor version is compared against minor version, patch version is compared against patch version
            VersionTag fromVersionTag = toVersionTag.IsPatchVersion()
                ? tagsDesc[i + 1]
                : GetPreviousMinorVersion(i);

            Result<Changelog> directChangelogResult = GetDirectChangelog(
                project,
                fromVersionTag.TagName,
                toVersionTag.TagName
            );

            if (directChangelogResult.IsFailure)
            {
                return Result.Failure<ContinuousChangelog>(
                    $"failed to generate direct changelog from \"{fromVersionTag.TagName}\" to \"{toVersionTag.TagName}\": {directChangelogResult.Error}"
                );
            }

            changelogs.Add(directChangelogResult.Value);
        }

        return Result.Success(new ContinuousChangelog(changelogs));

        VersionTag GetPreviousMinorVersion(int toVersionIndex)
        {
            for (int j = toVersionIndex + 1; j < tagsDesc.Count; j++)
            {
                if (!tagsDesc[j].IsPatchVersion())
                {
                    return tagsDesc[j];
                }
            }

            return tagsDesc[^1];
        }
    }
}
