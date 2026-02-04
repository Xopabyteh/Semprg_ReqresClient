using System.ComponentModel;
using GeneratedCode;
using Refit;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ReqresClientRefitAndRefitter.Commands.Auth;

/// <summary>
/// Command to register a new user with the Reqres API
/// </summary>
public class RegisterCommand : AsyncCommand<RegisterCommand.Settings>
{
    private readonly IReqResAPI _api;

    public RegisterCommand(IReqResAPI api)
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
        [DefaultValue("pistol")]
        public string Password { get; init; } = "pistol";
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        try
        {
            var panel = new Panel(
                Align.Left(
                    new Markup($"[yellow]Email:[/] {settings.Email}\n[yellow]Password:[/] {new string('*', settings.Password.Length)}")
                ))
                .Header("[blue]Registration Details[/]")
                .BorderColor(Color.Blue);

            AnsiConsole.Write(panel);
            AnsiConsole.WriteLine();

            var response = await AnsiConsole.Status()
                .Spinner(Spinner.Known.Star)
                .StartAsync("Registering user...", async ctx =>
                {
                    return await _api.RegisterUser(new AuthRequest
                    {
                        Email = settings.Email,
                        Password = settings.Password
                    });
                });

            var successPanel = new Panel(
                Align.Left(
                    new Markup($"[green]? Registration successful![/]\n\n[yellow]User ID:[/] {response.Id}\n[yellow]Token:[/] {response.Token}")
                ))
                .Header("[green]Success[/]")
                .BorderColor(Color.Green);

            AnsiConsole.Write(successPanel);

            return 0;
        }
        catch (ApiException apiEx)
        {
            AnsiConsole.Write(
                new Panel($"[red]Status Code:[/] {apiEx.StatusCode}\n[red]Content:[/] {apiEx.Content}")
                    .Header("[red]API Error[/]")
                    .BorderColor(Color.Red)
            );
            return 1;
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(
                new Panel($"[red]{ex.Message}[/]")
                    .Header("[red]Error[/]")
                    .BorderColor(Color.Red)
            );
            return 1;
        }
    }
}
