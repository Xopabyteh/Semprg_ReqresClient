using Spectre.Console.Cli;

namespace ReqresClientRefitAndRefitter.Commands;

/// <summary>
/// Base class for all commands with shared settings
/// </summary>
public abstract class BaseCommandSettings : CommandSettings
{
    [CommandOption("--api-key")]
    public string? ApiKey { get; init; }
}
