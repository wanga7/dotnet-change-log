using ChangelogPlayground;
using CommandLine;

Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(RunOptions).WithNotParsed(HandleParseError);

static void RunOptions(CommandLineOptions commandLineOptions)
{
    Console.WriteLine("Playground Started!");
}

static void HandleParseError(IEnumerable<Error> errors)
{
    Console.Error.WriteLine($"Invalid cmd args: {string.Join(Environment.NewLine, errors)}");
}