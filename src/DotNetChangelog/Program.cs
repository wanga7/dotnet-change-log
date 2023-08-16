using CommandLine;
using DotNetChangelog;

Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(RunOptions);
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
