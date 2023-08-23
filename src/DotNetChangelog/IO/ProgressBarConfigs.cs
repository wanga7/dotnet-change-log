using ShellProgressBar;

namespace DotNetChangelog.IO;

public static class ProgressBarConfigs
{
    public static readonly ProgressBarOptions DefaultOptions =
        new() { DisplayTimeInRealTime = false };
}
