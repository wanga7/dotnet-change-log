using System.Text.Json.Serialization;

namespace DotNetChangelog.Domain;

public record GitCommit
{
    public string Hash { get; init; }
    public string ShortMessage { get; init; }

    [JsonIgnore]
    public string ParentHash { get; init; }

    public override string ToString()
    {
        return $"{ShortMessage} ({Hash})";
    }
}
