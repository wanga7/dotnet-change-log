using CSharpFunctionalExtensions;
using DotNetChangelog.Domain;

namespace DotNetChangelog.IO;

public class ConsoleWriter : TextWriter
{
    public ConsoleWriter(string repoDirectory)
        : base(repoDirectory, string.Empty) { }

    public override Result<string> Write(Changelog changelog)
    {
        foreach (string line in GetLines(changelog))
        {
            Console.WriteLine(line);
        }

        return Result.Success("console");
    }

    public override Result<string> Write(ContinuousChangelog continuousChangelog)
    {
        foreach (string line in GetLines(continuousChangelog))
        {
            Console.WriteLine(line);
        }

        return Result.Success("console");
    }
}
