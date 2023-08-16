﻿using System.Collections.Concurrent;
using CSharpFunctionalExtensions;

namespace DotNetChangelog;

public class ChangeLogGenerator
{
    private readonly GitHistory _gitHistory;
    private readonly DotNetAffectedClient _dotNetAffectedClient;

    public ChangeLogGenerator(string repoPath, string excludedPattern)
    {
        _gitHistory = new(repoPath);
        _dotNetAffectedClient = new(repoPath, excludedPattern);
    }

    public IReadOnlyList<GitCommit> GetChangeLog(string project, string fromTag, string toTag)
    {
        Console.WriteLine($"Analyzing changelog for {project} from {fromTag} to {toTag}...");

        IReadOnlyList<GitCommit> commitsDesc = _gitHistory.GetHistoryDesc(fromTag, toTag);
        Console.WriteLine($"Found {commitsDesc.Count} commits");

        Console.WriteLine($"Determining commits affecting {project}...");
        ConcurrentDictionary<string, bool> commitsStates = new();
        Parallel.ForEach(
            commitsDesc,
            commit =>
            {
                Result<bool> result = _dotNetAffectedClient.IsProjectAffected(
                    project,
                    commit.ParentHash,
                    commit.Hash
                );

                if (result.IsFailure)
                {
                    Console.WriteLine(
                        $"Failed to analyze affected projects from {commit}: {result.Error}"
                    );
                }
                else
                {
                    commitsStates[commit.Hash] = result.Value;
                }
            }
        );

        return commitsDesc
            .Where(c => commitsStates.TryGetValue(c.Hash, out bool isAffected) && isAffected)
            .ToArray();
    }
}
