using System.Diagnostics;
using CommandLine;
using CSharpFunctionalExtensions;
using DotNetChangelog;
using DotNetChangelog.Domain;
using DotNetChangelog.IO;

Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(RunOptions);
return;

static void RunOptions(CommandLineOptions commandLineOptions)
{
    Console.WriteLine("Changelog Started!");
    Stopwatch stopwatch = Stopwatch.StartNew();

    ChangelogGenerator changelogGenerator =
        new(commandLineOptions.RepoDirectory, commandLineOptions.ExcludedPattern);

    Result<Changelog> result = changelogGenerator.GetChangelog(
        commandLineOptions.Project,
        commandLineOptions.FromTag,
        commandLineOptions.ToTag
    );

    if (result.IsSuccess)
    {
        OutputChangelog(result.Value, commandLineOptions);
    }
    else
    {
        Console.WriteLine($"Failed to generate changelog: {result.Error}"); // TODO: return error code for cli
    }

    stopwatch.Stop();
    Console.WriteLine($"Total execution time: {stopwatch.Elapsed.TotalSeconds:N1}s");
}

static void OutputChangelog(Changelog changelog, CommandLineOptions commandLineOptions)
{
    ChangelogWriter changelogWriter = WriterFactory.Create(
        commandLineOptions.RepoDirectory,
        commandLineOptions.OutputDirectory,
        commandLineOptions.OutputFormat
    );

    Result<string> result = changelogWriter.Write(changelog);
    if (result.IsSuccess)
    {
        Console.WriteLine($"Changelog dumped to {result.Value}");
    }
    else
    {
        Console.WriteLine($"Failed to dump changelog: {result.Error}");
    }
}
