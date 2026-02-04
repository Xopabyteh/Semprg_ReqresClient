using System.ComponentModel;
using GeneratedCode;
using Refit;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ReqresClientRefitAndRefitter.Commands.Users;

/// <summary>
/// Command to list users with pagination
/// </summary>
public class ListUsersCommand : AsyncCommand<ListUsersCommand.Settings>
{
    private readonly IReqResAPI _api;

    public ListUsersCommand(IReqResAPI api)
    {
        _api = api;
    }

    public class Settings : BaseCommandSettings
    {
        [CommandOption("-p|--page")]
        [Description("Page number")]
        [DefaultValue(1)]
        public int Page { get; init; } = 1;

        [CommandOption("--per-page")]
        [Description("Users per page")]
        [DefaultValue(6)]
        public int PerPage { get; init; } = 6;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        try
        {
            var response = await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync($"Fetching page {settings.Page}...", async ctx =>
                {
                    return await _api.GetLegacyUsers(settings.Page, settings.PerPage);
                });

            var rule = new Rule($"[yellow]Users - Page {response.Page}/{response.TotalPages}[/]")
            {
                Style = Style.Parse("yellow")
            };
            AnsiConsole.Write(rule);
            AnsiConsole.WriteLine();

            var grid = new Grid()
                .AddColumn(new GridColumn().NoWrap().PadRight(2))
                .AddColumn(new GridColumn().NoWrap().PadRight(2))
                .AddColumn();

            grid.AddRow(
                new Markup("[bold yellow]Total:[/]"),
                new Markup($"[green]{response.Total}[/]"),
                new Markup($"[dim]({response.PerPage} per page)[/]")
            );

            AnsiConsole.Write(grid);
            AnsiConsole.WriteLine();

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[yellow]ID[/]").Centered())
                .AddColumn("[yellow]Name[/]")
                .AddColumn("[yellow]Email[/]")
                .AddColumn("[yellow]Avatar[/]");

            foreach (var user in response.Data)
            {
                table.AddRow(
                    $"[cyan]{user.Id}[/]",
                    $"[green]{user.FirstName} {user.LastName}[/]",
                    $"[blue]{user.Email}[/]",
                    $"[dim]{user.Avatar[..Math.Min(40, user.Avatar.Length)]}...[/]"
                );
            }

            AnsiConsole.Write(table);

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
