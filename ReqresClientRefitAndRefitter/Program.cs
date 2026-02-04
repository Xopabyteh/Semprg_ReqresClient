using GeneratedCode;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using ReqresClientRefitAndRefitter.Commands.Auth;
using ReqresClientRefitAndRefitter.Commands.Resources;
using ReqresClientRefitAndRefitter.Commands.Users;
using ReqresClientRefitAndRefitter.Handlers;
using ReqresClientRefitAndRefitter.Infrastructure;
using Spectre.Console;
using Spectre.Console.Cli;

// Display welcome banner
AnsiConsole.Write(
    new FigletText("Reqres CLI")
        .Centered()
        .Color(Color.Cyan1)
);

AnsiConsole.Write(
    new Rule("[yellow]Powered by Refit + Refitter + Spectre.Console[/]")
        .RuleStyle("grey")
        .LeftJustified()
);
AnsiConsole.WriteLine();

// Get API key from environment
var apiKey = Environment.GetEnvironmentVariable("quick_temporary_api_key", EnvironmentVariableTarget.User);
if (string.IsNullOrEmpty(apiKey))
{
    AnsiConsole.MarkupLine("[red]API key not found in environment variable 'quick_temporary_api_key'. Please set it and try again.[/]", EnvironmentVariableTarget.User);
    return 1;
}

// Setup dependency injection
var services = new ServiceCollection();

// Configure HttpClient with API key authentication
services.AddTransient(_ =>
{
    var authHandler = new ApiKeyAuthenticationHandler(apiKey)
    {
        InnerHandler = new HttpClientHandler()
    };

    var httpClient = new HttpClient(authHandler)
    {
        BaseAddress = new Uri("https://reqres.in")
    };

    return RestService.For<IReqResAPI>(httpClient);
});

// Configure Spectre.Console.Cli
var registrar = new TypeRegistrar(services);
var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.SetApplicationName("reqres-cli");

    // Add example usage instructions
    config.SetApplicationVersion("1.0.0");

    // Authentication commands
    config.AddBranch("auth", auth =>
    {
        auth.SetDescription("Authentication commands");
        auth.AddCommand<LoginCommand>("login")
            .WithDescription("Login to the Reqres API")
            .WithExample(["auth", "login"])
            .WithExample(["auth", "login", "--email", "eve.holt@reqres.in", "--password", "cityslicka"]);

        auth.AddCommand<RegisterCommand>("register")
            .WithDescription("Register a new user")
            .WithExample(["auth", "register"])
            .WithExample(["auth", "register", "-e", "test@example.com", "-p", "password123"]);
    });

    // User management commands
    config.AddBranch("users", users =>
    {
        users.SetDescription("User management commands");
        users.AddCommand<ListUsersCommand>("list")
            .WithDescription("List users with pagination")
            .WithExample(["users", "list"])
            .WithExample(["users", "list", "--page", "2", "--per-page", "10"]);

        users.AddCommand<GetUserCommand>("get")
            .WithDescription("Get a single user by ID")
            .WithExample(["users", "get", "2"]);

        users.AddCommand<CreateUserCommand>("create")
            .WithDescription("Create a new user")
            .WithExample(["users", "create"])
            .WithExample(["users", "create", "--name", "John Doe", "--job", "Developer"]);

        users.AddCommand<DeleteUserCommand>("delete")
            .WithDescription("Delete a user by ID")
            .WithExample(["users", "delete", "2"])
            .WithExample(["users", "delete", "2", "--yes"]);
    });

    // Resource management commands
    config.AddBranch("resources", resources =>
    {
        resources.SetDescription("Resource management commands (unknown resources)");
        resources.AddCommand<ListResourcesCommand>("list")
            .WithDescription("List resources with pagination")
            .WithExample(["resources", "list"])
            .WithExample(["resources", "list", "--page", "1"]);

        resources.AddCommand<GetResourceCommand>("get")
            .WithDescription("Get a single resource by ID")
            .WithExample(["resources", "get", "2"]);
    });

    // Validation error message
    config.ValidateExamples();
});

// Run the application
return await app.RunAsync(args);