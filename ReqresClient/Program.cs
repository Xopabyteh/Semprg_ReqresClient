using ApiSdk;
using ApiSdk.Models;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

var apiKey = Environment.GetEnvironmentVariable("quick_temporary_api_key", EnvironmentVariableTarget.User);
if (string.IsNullOrEmpty(apiKey))
{
    Console.WriteLine("API key not found in environment variable 'quick_temporary_api_key'. Please set it and try again.");
    return;
}

var authProvider = new ApiKeyAuthenticationProvider(apiKey);
var adapter = new HttpClientRequestAdapter(authProvider);
var client = new ApiClient(adapter);

try
{
    // Step 1: Login
    Console.WriteLine("Logging in...");
    var loginRequest = new AuthRequest
    {
        Email = "eve.holt@reqres.in",
        Password = "cityslicka"
    };

    var loginResponse = await client.Api.Login.PostAsync(loginRequest);
    
    if (loginResponse?.Token != null)
    {
        Console.WriteLine($"Login successful! Token: {loginResponse.Token}");
    }
    else
    {
        Console.WriteLine("Login failed: No token received.");
        return;
    }

    // Step 2: List users using legacy API
    Console.WriteLine("\nFetching users...");
    var usersResponse = await client.Api.Users.GetAsync(config =>
    {
        config.QueryParameters.Page = 1;
        config.QueryParameters.PerPage = 6;
    });

    if (usersResponse?.Data != null)
    {
        Console.WriteLine($"\nTotal users: {usersResponse.Total}");
        Console.WriteLine($"Page: {usersResponse.Page}/{usersResponse.TotalPages}");
        Console.WriteLine($"Per page: {usersResponse.PerPage}");
        Console.WriteLine("\nUsers:");
        Console.WriteLine(new string('-', 80));

        foreach (var user in usersResponse.Data)
        {
            Console.WriteLine($"ID: {user.Id}");
            Console.WriteLine($"Name: {user.FirstName} {user.LastName}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Avatar: {user.Avatar}");
            Console.WriteLine(new string('-', 80));
        }
    }
    else
    {
        Console.WriteLine("No users found.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
    }
}

/// <summary>
/// Custom authentication provider to add x-api-key header from environment variable
/// </summary>
class ApiKeyAuthenticationProvider : IAuthenticationProvider
{
    private readonly string _apiKey;

    public ApiKeyAuthenticationProvider(string apiKey)
    {
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
    }

    public Task AuthenticateRequestAsync(RequestInformation request, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
    {
        request.Headers.Add("x-api-key", _apiKey);
        return Task.CompletedTask;
    }
}
