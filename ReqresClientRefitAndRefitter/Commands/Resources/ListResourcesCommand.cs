using System.ComponentModel;
using GeneratedCode;
using Refit;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ReqresClientRefitAndRefitter.Commands.Resources;

/// <summary>
/// Command to list unknown resources
/// </summary>
public class ListResourcesCommand : AsyncCommand<ListResourcesCommand.Settings>
{
    private readonly IReqResAPI _api;

    public ListResourcesCommand(IReqResAPI api)
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
        [Description("Resources per page")]
        [DefaultValue(6)]
        public int PerPage { get; init; } = 6;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        try
        {
            var response = await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync($"Fetching resources page {settings.Page}...", async ctx =>
                {
                    return await _api.GetLegacyUnknown(settings.Page, settings.PerPage);
                });

            var rule = new Rule($"[yellow]Resources - Page {response.Page}/{response.TotalPages}[/]")
            {
                Style = Style.Parse("yellow")
            };
            AnsiConsole.Write(rule);
            AnsiConsole.WriteLine();

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[yellow]ID[/]").Centered())
                .AddColumn("[yellow]Name[/]")
                .AddColumn("[yellow]Year[/]")
                .AddColumn("[yellow]Color[/]")
                .AddColumn("[yellow]Pantone[/]");

            foreach (var resource in response.Data)
            {
                // Create colored square for color preview
                var colorSquare = $"[{resource.Color}]???[/]";
                
                table.AddRow(
                    $"[cyan]{resource.Id}[/]",
                    $"[green]{resource.Name}[/]",
                    $"[blue]{resource.Year}[/]",
                    $"{colorSquare} [dim]{resource.Color}[/]",
                    $"[magenta]{resource.PantoneValue}[/]"
                );
            }

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[dim]Total: {response.Total} resources[/]");

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
