using System.Diagnostics;
using CommandLine;
using DotNetChangelog;

Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(RunOptions);
return;

static void RunOptions(CommandLineOptions commandLineOptions)
{
    Console.WriteLine("Changelog Started!");

    Stopwatch stopwatch = Stopwatch.StartNew();

    ChangeLogGenerator changeLogGenerator =
        new(commandLineOptions.RepoPath, commandLineOptions.ExcludedPattern);

    IReadOnlyList<GitCommit> affectedCommits = changeLogGenerator.GetChangeLog(
        commandLineOptions.Project,
        commandLineOptions.FromTag,
        commandLineOptions.ToTag
    );

    stopwatch.Stop();
    Console.WriteLine(
        $"Found {affectedCommits.Count} commits affecting {commandLineOptions.Project} in {stopwatch.Elapsed.TotalSeconds:N1}s"
    );
}
