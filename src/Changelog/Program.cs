using Changelog;
using CommandLine;

Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(RunOptions)
    .WithNotParsed(HandleParseError);

static void RunOptions(CommandLineOptions commandLineOptions)
{
    Console.WriteLine("Changelog Started!");

    ChangLogGenerator changLogGenerator =
        new(commandLineOptions.RepoPath, commandLineOptions.ExcludedPattern);

    changLogGenerator.GetChangeLog(commandLineOptions.Project, commandLineOptions.FromTag,
        commandLineOptions.ToTag);
}

static void HandleParseError(IEnumerable<Error> errors)
{
    Console.Error.WriteLine($"Invalid cmd args: {string.Join(Environment.NewLine, errors)}");
}