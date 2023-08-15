using CommandLine;

namespace DotNetChangelog;

public class CommandLineOptions
{
    [Option('r', "repo-path", Required = true, HelpText = "Path to repository")]
    public string RepoPath { get; set; }

    [Option('p', "project", Required = true, HelpText = "Project for Changelog (e.g. App.csproj)")]
    public string Project { get; set; }

    [Option('f', "from", Required = true, HelpText = "Starting git tag for Changelog")]
    public string FromTag { get; set; }

    [Option('t', "to", Required = true, HelpText = "Ending git tag for Changelog")]
    public string ToTag { get; set; }

    [Option('e', "exclude", Required = false,
        HelpText =
            "Excluded projects pattern (dotnet Regular Expression that will be matched against each Project's Full Path, e.g. \".Tests.\")")]
    public string ExcludedPattern { get; set; }
}