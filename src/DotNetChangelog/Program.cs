using System.Diagnostics;
using CommandLine;
using CSharpFunctionalExtensions;
using DotNetChangelog;
using DotNetChangelog.Domain;
using DotNetChangelog.IO;

Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(RunOptions);
return;

static void RunOptions(CommandLineOptions options)
{
    Console.WriteLine("Changelog Started!");
    Stopwatch stopwatch = Stopwatch.StartNew();

    ChangelogGenerator changelogGenerator = new(options.RepoDirectory, options.ExcludedPattern);

    if (options.ChangelogMode == ChangelogMode.Direct)
    {
        HandleDirectChangelogProcessing(changelogGenerator, options);
    }
    else
    {
        HandleContinuousChangelogProcessing(changelogGenerator, options);
    }

    stopwatch.Stop();
    Console.WriteLine($"Total execution time: {stopwatch.Elapsed.TotalSeconds:N1}s");
}

static void HandleContinuousChangelogProcessing(
    ChangelogGenerator changelogGenerator,
    CommandLineOptions options
)
{
    Result<ContinuousChangelog> result = changelogGenerator.GetContinuousChangelog(
        options.Project,
        options.FromTag,
        options.ToTag,
        options.TagRegex
    );

    if (result.IsSuccess)
    {
        OutputContinuousChangelog(result.Value, options);
    }
    else
    {
        Console.WriteLine($"Failed to generate changelog: {result.Error}"); // TODO: return error code for cli
    }
}

static void HandleDirectChangelogProcessing(
    ChangelogGenerator changelogGenerator,
    CommandLineOptions options
)
{
    Result<Changelog> result = changelogGenerator.GetDirectChangelog(
        options.Project,
        options.FromTag,
        options.ToTag
    );

    if (result.IsSuccess)
    {
        OutputChangelog(result.Value, options);
    }
    else
    {
        Console.WriteLine($"Failed to generate changelog: {result.Error}"); // TODO: return error code for cli
    }
}

static void OutputChangelog(Changelog changelog, CommandLineOptions options)
{
    ChangelogWriter changelogWriter = WriterFactory.Create(
        options.RepoDirectory,
        options.OutputDirectory,
        options.OutputFormat
    );

    Result<string> result = changelogWriter.Write(changelog);
    Console.WriteLine(
        result.IsSuccess
            ? $"Changelog dumped to {result.Value}"
            : $"Failed to dump changelog: {result.Error}"
    );
}

static void OutputContinuousChangelog(
    ContinuousChangelog continuousChangelog,
    CommandLineOptions options
)
{
    ChangelogWriter changelogWriter = WriterFactory.Create(
        options.RepoDirectory,
        options.OutputDirectory,
        options.OutputFormat
    );

    Result<string> result = changelogWriter.Write(continuousChangelog);
    Console.WriteLine(
        result.IsSuccess
            ? $"Changelog dumped to {result.Value}"
            : $"Failed to dump changelog: {result.Error}"
    );
}
