using CommandLine;

namespace ChangelogPlayground;

public class CommandLineOptions
{
    [Option('p', "repo-path", Required = true, HelpText = "Path to repository")]
    public string RepoPath { get; set; }

    [Option('f', "from", Required = true, HelpText = "Starting git tag for Changelog")]
    public string FromTag { get; set; }

    [Option('t', "to", Required = true, HelpText = "Ending git tag for Changelog")]
    public string ToTag { get; set; }
}