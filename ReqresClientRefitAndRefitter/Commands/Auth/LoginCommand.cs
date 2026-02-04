using System.ComponentModel;
using GeneratedCode;
using Refit;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ReqresClientRefitAndRefitter.Commands.Auth;

/// <summary>
/// Command to login to the Reqres API
/// </summary>
public class LoginCommand : AsyncCommand<LoginCommand.Settings>
{
    private readonly IReqResAPI _api;

    public LoginCommand(IReqResAPI api)
    {
        _api = api;
    }

    public class Settings : BaseCommandSettings
    {
        [CommandOption("-e|--email")]
        [Description("Email address")]
        [DefaultValue("eve.holt@reqres.in")]
        public string Email { get; init; } = "eve.holt@reqres.in";

        [CommandOption("-p|--password")]
        [Description("Password")]
        [DefaultValue("cityslicka")]
        public string Password { get; init; } = "cityslicka";
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        try
        {
            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[yellow]Property[/]").Centered())
                .AddColumn(new TableColumn("[yellow]Value[/]"));

            table.AddRow("Email", settings.Email);
            table.AddRow("Password", new string('*', settings.Password.Length));
            
            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();

            var response = await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("Logging in...", async ctx =>
                {
                    return await _api.LoginUser(new AuthRequest
                    {
                        Email = settings.Email,
                        Password = settings.Password
                    });
                });

            var resultTable = new Table()
                .Border(TableBorder.Double)
                .AddColumn("[green]Status[/]")
                .AddColumn("[green]Token[/]");

            resultTable.AddRow(
                "[green]? Success[/]",
                $"[dim]{response.Token[..Math.Min(40, response.Token.Length)]}...[/]"
            );

            AnsiConsole.Write(resultTable);
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[green]Full token:[/] {response.Token}");

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
