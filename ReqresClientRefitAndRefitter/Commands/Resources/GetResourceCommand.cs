using System.ComponentModel;
using GeneratedCode;
using Refit;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ReqresClientRefitAndRefitter.Commands.Resources;

/// <summary>
/// Command to get a single resource by ID
/// </summary>
public class GetResourceCommand : AsyncCommand<GetResourceCommand.Settings>
{
    private readonly IReqResAPI _api;

    public GetResourceCommand(IReqResAPI api)
    {
        _api = api;
    }

    public class Settings : BaseCommandSettings
    {
        [CommandArgument(0, "<RESOURCE_ID>")]
        [Description("Resource ID to fetch")]
        public int ResourceId { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        try
        {
            var response = await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync($"Fetching resource {settings.ResourceId}...", async ctx =>
                {
                    return await _api.GetLegacyUnknownById(settings.ResourceId);
                });

            var resource = response.Data;

            var grid = new Grid()
                .AddColumn()
                .AddColumn();

            grid.AddRow(
                new Markup("[yellow]ID:[/]"),
                new Markup($"[cyan]{resource.Id}[/]")
            );
            grid.AddRow(
                new Markup("[yellow]Name:[/]"),
                new Markup($"[green]{resource.Name}[/]")
            );
            grid.AddRow(
                new Markup("[yellow]Year:[/]"),
                new Markup($"[blue]{resource.Year}[/]")
            );
            grid.AddRow(
                new Markup("[yellow]Color:[/]"),
                new Markup($"[{resource.Color}]???[/] [dim]{resource.Color}[/]")
            );
            grid.AddRow(
                new Markup("[yellow]Pantone Value:[/]"),
                new Markup($"[magenta]{resource.PantoneValue}[/]")
            );

            var panel = new Panel(grid)
                .Header($"[green]Resource Details - ID {resource.Id}[/]")
                .BorderColor(Color.Green)
                .Padding(2, 1);

            AnsiConsole.Write(panel);

            return 0;
        }
        catch (ApiException apiEx)
        {
            AnsiConsole.Write(
                new Panel($"[red]Status:[/] {apiEx.StatusCode}\n[red]Message:[/] {apiEx.Content}")
                    .Header($"[red]Resource {settings.ResourceId} Not Found[/]")
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
