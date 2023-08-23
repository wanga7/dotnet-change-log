using CommandLine;
using DotNetChangelog.Domain;

namespace DotNetChangelog;

public class CommandLineOptions
{
    [Option(
        'r',
        "repo-dir",
        Required = true,
        HelpText = "Repo directory (default to current working directory)"
    )]
    public string RepoDirectory { get; set; } = ".";

    [Option('p', "project", Required = true, HelpText = "Project for Changelog (e.g. App.csproj)")]
    public string Project { get; set; }

    [Option('f', "from", Required = true, HelpText = "Starting git tag for Changelog")]
    public string FromTag { get; set; }

    [Option('t', "to", Required = true, HelpText = "Ending git tag for Changelog")]
    public string ToTag { get; set; }

    [Option(
        'c',
        "change-log-mode",
        Required = false,
        HelpText = "Type of changelog to generate, Direct/Continuous (default to Direct)"
    )]
    public ChangelogMode ChangelogMode { get; set; } = ChangelogMode.Direct;

    [Option(
        'e',
        "exclude",
        Required = false,
        HelpText = "Excluded projects pattern (dotnet Regular Expression that will be matched against each Project's Full Path, e.g. \".Tests.\")"
    )]
    public string ExcludedPattern { get; set; } = string.Empty;

    [Option(
        'o',
        "output-dir",
        Required = false,
        HelpText = "Changelog output directory relative to repo root (default to repo root directory)"
    )]
    public string OutputDirectory { get; set; } = ".";

    [Option(
        'm',
        "output-format",
        Required = false,
        HelpText = "Changelog output format (Console/Text/Json/Markdown, default to Console)"
    )]
    public OutputFormat OutputFormat { get; set; } = OutputFormat.Console;
}
