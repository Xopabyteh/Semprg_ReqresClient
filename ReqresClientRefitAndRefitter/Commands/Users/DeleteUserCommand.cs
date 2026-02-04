using System.ComponentModel;
using GeneratedCode;
using Refit;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ReqresClientRefitAndRefitter.Commands.Users;

/// <summary>
/// Command to delete a user
/// </summary>
public class DeleteUserCommand : AsyncCommand<DeleteUserCommand.Settings>
{
    private readonly IReqResAPI _api;

    public DeleteUserCommand(IReqResAPI api)
    {
        _api = api;
    }

    public class Settings : BaseCommandSettings
    {
        [CommandArgument(0, "<USER_ID>")]
        [Description("User ID to delete")]
        public int UserId { get; init; }

        [CommandOption("-y|--yes")]
        [Description("Skip confirmation")]
        public bool SkipConfirmation { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        try
        {
            if (!settings.SkipConfirmation)
            {
                var confirm = AnsiConsole.Confirm(
                    $"Are you sure you want to delete user [red]{settings.UserId}[/]?",
                    false
                );

                if (!confirm)
                {
                    AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
                    return 0;
                }
            }

            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync($"Deleting user {settings.UserId}...", async ctx =>
                {
                    await _api.DeleteLegacyUser(settings.UserId);
                    return Task.CompletedTask;
                });

            AnsiConsole.MarkupLine($"[green]? User {settings.UserId} deleted successfully![/]");

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
