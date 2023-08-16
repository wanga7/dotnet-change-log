using CommandLine;
using DotNetChangelog;

Parser.Default
    .ParseArguments<CommandLineOptions>(args)
    .WithParsed(RunOptions)
    .WithNotParsed(HandleParseError);
return;

static void RunOptions(CommandLineOptions commandLineOptions)
{
    Console.WriteLine("Changelog Started!");

    ChangeLogGenerator changeLogGenerator =
        new(commandLineOptions.RepoPath, commandLineOptions.ExcludedPattern);

    changeLogGenerator.GetChangeLog(
        commandLineOptions.Project,
        commandLineOptions.FromTag,
        commandLineOptions.ToTag
    );
}

static void HandleParseError(IEnumerable<Error> errors)
{
    Console.Error.WriteLine($"Invalid cmd args: {string.Join(Environment.NewLine, errors)}");
}
