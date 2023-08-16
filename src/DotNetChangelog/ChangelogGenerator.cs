using System.Collections.Concurrent;
using CSharpFunctionalExtensions;
using DotNetChangelog.Domain;
using ShellProgressBar;

namespace DotNetChangelog;

public class ChangelogGenerator
{
    private static readonly ProgressBarOptions ProgressBarOptions =
        new() { DisplayTimeInRealTime = false };

    private readonly GitHistory _gitHistory;
    private readonly DotNetAffectedClient _dotNetAffectedClient;

    public ChangelogGenerator(string repoPath, string excludedPattern)
    {
        _gitHistory = new(repoPath);
        _dotNetAffectedClient = new(repoPath, excludedPattern);
    }

    public Result<Changelog> GetChangelog(string project, string fromTag, string toTag)
    {
        Console.WriteLine($"Analyzing changelog for {project} from {fromTag} to {toTag}...");

        IReadOnlyList<GitCommit> commitsDesc = _gitHistory.GetHistoryDesc(fromTag, toTag);
        Console.WriteLine($"Found {commitsDesc.Count} commits");

        Console.WriteLine($"Determining commits affecting {project}...");
        ConcurrentDictionary<string, bool> commitsStates = new();
        string error = string.Empty;
        
        using (
            ProgressBar progressBar = new(commitsDesc.Count, "Commit analysis", ProgressBarOptions)
        )
        {
            Parallel.ForEach(commitsDesc, (commit, state) =>
                {
                    Result<bool> result = _dotNetAffectedClient.IsProjectAffected(
                        project,
                        commit.ParentHash,
                        commit.Hash
                    );

                    if (result.IsFailure)
                    {
                        error = $"Failed to analyze affected projects from {commit}: {result.Error}";
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
            ? Result.Success(new Changelog(fromTag, toTag,
                commitsDesc.Where(c => commitsStates.TryGetValue(c.Hash, out bool isAffected) && isAffected)
                    .ToArray()))
            : Result.Failure<Changelog>(error);
    }
}
