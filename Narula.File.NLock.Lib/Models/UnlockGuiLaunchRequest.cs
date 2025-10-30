namespace Narula.File.NLock.Models;

public sealed class UnlockGuiLaunchRequest
{
    public IReadOnlyList<string> Files { get; init; } = Array.Empty<string>();
    public IReadOnlyList<string> Directories { get; init; } = Array.Empty<string>();
    public string? OutputFolder { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public string CliVersion { get; init; } = "";
}


