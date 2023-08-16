using CSharpFunctionalExtensions;
using DotNetChangelog.Domain;

namespace DotNetChangelog.IO;

public abstract class ChangelogWriter
{
    protected const string FileName = "CHANGELOG";

    protected readonly string _outputDirectory;

    protected ChangelogWriter(string repoDirectory, string outputDirectory)
    {
        _outputDirectory = Path.Combine(repoDirectory, outputDirectory);
    }

    public abstract Result<string> Write(Changelog changelog);
}
