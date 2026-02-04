using System.ComponentModel;
using GeneratedCode;
using Refit;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ReqresClientRefitAndRefitter.Commands.Users;

/// <summary>
/// Command to create a new user
/// </summary>
public class CreateUserCommand : AsyncCommand<CreateUserCommand.Settings>
{
    private readonly IReqResAPI _api;

    public CreateUserCommand(IReqResAPI api)
    {
        _api = api;
    }

    public class Settings : BaseCommandSettings
    {
        [CommandOption("-n|--name")]
        [Description("User name")]
        public string? Name { get; init; }

        [CommandOption("-j|--job")]
        [Description("User job title")]
        public string? Job { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        try
        {
            // Prompt for missing values
            var name = settings.Name ?? AnsiConsole.Ask<string>("Enter [green]name[/]:");
            var job = settings.Job ?? AnsiConsole.Ask<string>("Enter [green]job[/]:");

            var response = await AnsiConsole.Status()
                .Spinner(Spinner.Known.Star)
                .StartAsync("Creating user...", async ctx =>
                {
                    var request = new LegacyMutationRequest();
                    request.AdditionalProperties["name"] = name;
                    request.AdditionalProperties["job"] = job;
                    return await _api.CreateLegacyUser(request);
                });

            var tree = new Tree("[green]? User Created Successfully[/]")
                .Style(Style.Parse("green"));

            var details = tree.AddNode("[yellow]Details[/]");
            details.AddNode($"[cyan]ID:[/] {response.Id}");
            
            if (response.AdditionalProperties.TryGetValue("name", out var nameValue))
                details.AddNode($"[cyan]Name:[/] {nameValue}");
            
            if (response.AdditionalProperties.TryGetValue("job", out var jobValue))
                details.AddNode($"[cyan]Job:[/] {jobValue}");
            
            details.AddNode($"[cyan]Created At:[/] {response.CreatedAt:yyyy-MM-dd HH:mm:ss}");

            AnsiConsole.Write(tree);

            return 0;
        }
        catch (ApiException apiEx)
        {
            AnsiConsole.MarkupLine($"[red]API Error ({apiEx.StatusCode}):[/] {apiEx.Content}");
            return 1;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
            return 1;
        }
    }
}
