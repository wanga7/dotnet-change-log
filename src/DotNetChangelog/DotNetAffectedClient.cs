using System.Diagnostics;
using CSharpFunctionalExtensions;

namespace DotNetChangelog;

public class DotNetAffectedClient
{
    private readonly string _workingDirectory;
    private readonly string _excludedPattern;

    public DotNetAffectedClient(string workingDirectory, string excludedPattern = "")
    {
        _workingDirectory = workingDirectory;
        _excludedPattern = excludedPattern;
    }

    public Result<bool> IsProjectAffected(string projectPath, string fromTarget, string toTarget)
    {
        Result<IReadOnlyList<string>> result = GetAffectedProjects(fromTarget, toTarget);
        if (result.IsFailure)
            return Result.Failure<bool>(result.Error);

        return Result.Success(result.Value.Any(p => p.Contains(projectPath)));
    }

    private Result<IReadOnlyList<string>> GetAffectedProjects(string fromTarget, string toTarget)
    {
        Process process = new();

        string arguments = $"affected --from {fromTarget} --to {toTarget} --format text --dry-run";
        if (!string.IsNullOrWhiteSpace(_excludedPattern))
            arguments += $" -e {_excludedPattern}";

        ProcessStartInfo startInfo =
            new()
            {
                FileName = "dotnet",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = _workingDirectory
            };

        process.StartInfo = startInfo;

        List<string> affectedProjects = new();
        List<string> errors = new();
        process.OutputDataReceived += (_, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
                affectedProjects.Add(e.Data);
        };
        process.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
                errors.Add(e.Data);
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.WaitForExit();

        if (errors.Count > 0)
            return Result.Failure<IReadOnlyList<string>>(string.Join(' ', errors));

        return Result.Success<IReadOnlyList<string>>(affectedProjects.ToArray());
    }
}
