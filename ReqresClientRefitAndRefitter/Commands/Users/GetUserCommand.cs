using System.ComponentModel;
using GeneratedCode;
using Refit;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ReqresClientRefitAndRefitter.Commands.Users;

/// <summary>
/// Command to get a single user by ID
/// </summary>
public class GetUserCommand : AsyncCommand<GetUserCommand.Settings>
{
    private readonly IReqResAPI _api;

    public GetUserCommand(IReqResAPI api)
    {
        _api = api;
    }

    public class Settings : BaseCommandSettings
    {
        [CommandArgument(0, "<USER_ID>")]
        [Description("User ID to fetch")]
        public int UserId { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        try
        {
            var response = await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync($"Fetching user {settings.UserId}...", async ctx =>
                {
                    return await _api.GetLegacyUserById(settings.UserId);
                });

            var user = response.Data;

            var panel = new Panel(
                Align.Left(new Rows(
                    new Markup($"[yellow]ID:[/] [cyan]{user.Id}[/]"),
                    new Markup($"[yellow]First Name:[/] [green]{user.FirstName}[/]"),
                    new Markup($"[yellow]Last Name:[/] [green]{user.LastName}[/]"),
                    new Markup($"[yellow]Email:[/] [blue]{user.Email}[/]"),
                    new Markup($"[yellow]Avatar:[/] [dim]{user.Avatar}[/]")
                ))
            )
            .Header($"[green]User Details - ID {user.Id}[/]")
            .BorderColor(Color.Green)
            .Padding(2, 1);

            AnsiConsole.Write(panel);

            return 0;
        }
        catch (ApiException apiEx)
        {
            AnsiConsole.Write(
                new Panel($"[red]Status:[/] {apiEx.StatusCode}\n[red]Message:[/] {apiEx.Content}")
                    .Header($"[red]User {settings.UserId} Not Found[/]")
                    .BorderColor(Color.Red)
            );
            return 1;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
            return 1;
        }
    }
}
