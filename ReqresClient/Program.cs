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

var app = new ReqresClientApp(client);
await app.RunAsync();

/// <summary>
/// Main application orchestrator for Reqres API operations
/// </summary>
class ReqresClientApp
{
    private readonly ApiClient _client;
    private string? _authToken;

    public ReqresClientApp(ApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task RunAsync()
    {
        Console.WriteLine("=== Reqres API Client ===\n");

        while (true)
        {
            DisplayMenu();
            var choice = Console.ReadLine()?.Trim();

            try
            {
                switch (choice)
                {
                    case "1":
                        await LoginAsync();
                        break;
                    case "2":
                        await RegisterUserAsync();
                        break;
                    case "3":
                        await ListUsersAsync();
                        break;
                    case "4":
                        await GetSingleUserAsync();
                        break;
                    case "5":
                        await CreateUserAsync();
                        break;
                    case "6":
                        await UpdateUserAsync();
                        break;
                    case "7":
                        await DeleteUserAsync();
                        break;
                    case "8":
                        await ListResourcesAsync();
                        break;
                    case "9":
                        await GetSingleResourceAsync();
                        break;
                    case "0":
                        Console.WriteLine("\nExiting application. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("\nInvalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Details: {ex.InnerException.Message}");
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private void DisplayMenu()
    {
        Console.WriteLine("\n╔════════════════════════════════════════╗");
        Console.WriteLine("║         REQRES API OPERATIONS          ║");
        Console.WriteLine("╠════════════════════════════════════════╣");
        Console.WriteLine("║ Authentication                         ║");
        Console.WriteLine("║   1. Login                             ║");
        Console.WriteLine("║   2. Register                          ║");
        Console.WriteLine("║                                        ║");
        Console.WriteLine("║ User Operations                        ║");
        Console.WriteLine("║   3. List Users                        ║");
        Console.WriteLine("║   4. Get Single User                   ║");
        Console.WriteLine("║   5. Create User                       ║");
        Console.WriteLine("║   6. Update User                       ║");
        Console.WriteLine("║   7. Delete User                       ║");
        Console.WriteLine("║                                        ║");
        Console.WriteLine("║ Resource Operations                    ║");
        Console.WriteLine("║   8. List Resources (Unknown)          ║");
        Console.WriteLine("║   9. Get Single Resource               ║");
        Console.WriteLine("║                                        ║");
        Console.WriteLine("║   0. Exit                              ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        
        if (!string.IsNullOrEmpty(_authToken))
        {
            Console.WriteLine($"\n[Authenticated - Token: {_authToken[..Math.Min(20, _authToken.Length)]}...]");
        }
        
        Console.Write("\nSelect an option: ");
    }

    private async Task LoginAsync()
    {
        Console.WriteLine("\n=== LOGIN ===");
        Console.Write("Email (default: eve.holt@reqres.in): ");
        var email = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(email))
            email = "eve.holt@reqres.in";

        Console.Write("Password (default: cityslicka): ");
        var password = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(password))
            password = "cityslicka";

        var loginRequest = new AuthRequest
        {
            Email = email,
            Password = password
        };

        var response = await _client.Api.Login.PostAsync(loginRequest);

        if (response?.Token != null)
        {
            _authToken = response.Token;
            Console.WriteLine($"\n✓ Login successful!");
            Console.WriteLine($"Token: {response.Token}");
        }
        else
        {
            Console.WriteLine("\n✗ Login failed: No token received.");
        }
    }

    private async Task RegisterUserAsync()
    {
        Console.WriteLine("\n=== REGISTER USER ===");
        Console.Write("Email (default: eve.holt@reqres.in): ");
        var email = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(email))
            email = "eve.holt@reqres.in";

        Console.Write("Password (default: pistol): ");
        var password = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(password))
            password = "pistol";

        var registerRequest = new AuthRequest
        {
            Email = email,
            Password = password
        };

        var response = await _client.Api.Register.PostAsync(registerRequest);

        if (response?.Token != null)
        {
            Console.WriteLine($"\n✓ Registration successful!");
            Console.WriteLine($"User ID: {response.Id}");
            Console.WriteLine($"Token: {response.Token}");
        }
        else
        {
            Console.WriteLine("\n✗ Registration failed.");
        }
    }

    private async Task ListUsersAsync()
    {
        Console.WriteLine("\n=== LIST USERS ===");
        Console.Write("Page (default: 1): ");
        var pageInput = Console.ReadLine();
        int page = string.IsNullOrWhiteSpace(pageInput) ? 1 : int.Parse(pageInput);

        Console.Write("Per page (default: 6): ");
        var perPageInput = Console.ReadLine();
        int perPage = string.IsNullOrWhiteSpace(perPageInput) ? 6 : int.Parse(perPageInput);

        var response = await _client.Api.Users.GetAsync(config =>
        {
            config.QueryParameters.Page = page;
            config.QueryParameters.PerPage = perPage;
        });

        if (response?.Data != null)
        {
            Console.WriteLine($"\nTotal users: {response.Total}");
            Console.WriteLine($"Page: {response.Page}/{response.TotalPages}");
            Console.WriteLine($"Per page: {response.PerPage}\n");
            Console.WriteLine(new string('═', 90));

            foreach (var user in response.Data)
            {
                Console.WriteLine($"ID: {user.Id,-5} | {user.FirstName} {user.LastName,-15}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Avatar: {user.Avatar}");
                Console.WriteLine(new string('─', 90));
            }
        }
        else
        {
            Console.WriteLine("\n✗ No users found.");
        }
    }

    private async Task GetSingleUserAsync()
    {
        Console.WriteLine("\n=== GET SINGLE USER ===");
        Console.Write("Enter user ID: ");
        var idInput = Console.ReadLine();
        
        if (int.TryParse(idInput, out int userId))
        {
            var response = await _client.Api.Users[userId].GetAsync();

            if (response?.Data != null)
            {
                var user = response.Data;
                Console.WriteLine($"\n{new string('═', 90)}");
                Console.WriteLine($"ID: {user.Id}");
                Console.WriteLine($"Name: {user.FirstName} {user.LastName}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Avatar: {user.Avatar}");
                Console.WriteLine($"{new string('═', 90)}");
            }
            else
            {
                Console.WriteLine($"\n✗ User with ID {userId} not found.");
            }
        }
        else
        {
            Console.WriteLine("\n✗ Invalid user ID.");
        }
    }

    private async Task CreateUserAsync()
    {
        Console.WriteLine("\n=== CREATE USER ===");
        Console.Write("Name: ");
        var name = Console.ReadLine() ?? "New User";

        Console.Write("Job: ");
        var job = Console.ReadLine() ?? "Developer";

        var createRequest = new LegacyMutationRequest();
        createRequest.AdditionalData["name"] = name;
        createRequest.AdditionalData["job"] = job;

        var response = await _client.Api.Users.PostAsync(createRequest);

        if (response != null)
        {
            Console.WriteLine($"\n✓ User created successfully!");
            Console.WriteLine($"ID: {response.Id}");
            
            if (response.AdditionalData.TryGetValue("name", out var nameObj))
                Console.WriteLine($"Name: {nameObj}");
            
            if (response.AdditionalData.TryGetValue("job", out var jobObj))
                Console.WriteLine($"Job: {jobObj}");
            
            Console.WriteLine($"Created at: {response.CreatedAt}");
        }
    }

    private async Task UpdateUserAsync()
    {
        Console.WriteLine("\n=== UPDATE USER ===");
        Console.Write("Enter user ID to update: ");
        var idInput = Console.ReadLine();

        if (int.TryParse(idInput, out int userId))
        {
            Console.Write("New name: ");
            var name = Console.ReadLine() ?? "Updated User";

            Console.Write("New job: ");
            var job = Console.ReadLine() ?? "Senior Developer";

            var updateRequest = new LegacyMutationRequest();
            updateRequest.AdditionalData["name"] = name;
            updateRequest.AdditionalData["job"] = job;

            var response = await _client.Api.Users[userId].PutAsync(updateRequest);

            if (response != null)
            {
                Console.WriteLine($"\n✓ User updated successfully!");
                
                if (response.AdditionalData.TryGetValue("name", out var nameObj))
                    Console.WriteLine($"Name: {nameObj}");
                
                if (response.AdditionalData.TryGetValue("job", out var jobObj))
                    Console.WriteLine($"Job: {jobObj}");
                
                Console.WriteLine($"Updated at: {response.UpdatedAt}");
            }
        }
        else
        {
            Console.WriteLine("\n✗ Invalid user ID.");
        }
    }

    private async Task DeleteUserAsync()
    {
        Console.WriteLine("\n=== DELETE USER ===");
        Console.Write("Enter user ID to delete: ");
        var idInput = Console.ReadLine();

        if (int.TryParse(idInput, out int userId))
        {
            Console.Write($"Are you sure you want to delete user {userId}? (y/n): ");
            var confirmation = Console.ReadLine()?.ToLower();

            if (confirmation == "y")
            {
                await _client.Api.Users[userId].DeleteAsync();
                Console.WriteLine($"\n✓ User {userId} deleted successfully!");
            }
            else
            {
                Console.WriteLine("\n✗ Deletion cancelled.");
            }
        }
        else
        {
            Console.WriteLine("\n✗ Invalid user ID.");
        }
    }

    private async Task ListResourcesAsync()
    {
        Console.WriteLine("\n=== LIST RESOURCES (UNKNOWN) ===");
        Console.Write("Page (default: 1): ");
        var pageInput = Console.ReadLine();
        int page = string.IsNullOrWhiteSpace(pageInput) ? 1 : int.Parse(pageInput);

        var response = await _client.Api.Unknown.GetAsync(config =>
        {
            config.QueryParameters.Page = page;
        });

        if (response?.Data != null)
        {
            Console.WriteLine($"\nTotal resources: {response.Total}");
            Console.WriteLine($"Page: {response.Page}/{response.TotalPages}\n");
            Console.WriteLine(new string('═', 90));

            foreach (var resource in response.Data)
            {
                Console.WriteLine($"ID: {resource.Id,-5} | {resource.Name}");
                Console.WriteLine($"Year: {resource.Year}");
                Console.WriteLine($"Color: {resource.Color} | Pantone: {resource.PantoneValue}");
                Console.WriteLine(new string('─', 90));
            }
        }
        else
        {
            Console.WriteLine("\n✗ No resources found.");
        }
    }

    private async Task GetSingleResourceAsync()
    {
        Console.WriteLine("\n=== GET SINGLE RESOURCE ===");
        Console.Write("Enter resource ID: ");
        var idInput = Console.ReadLine();

        if (int.TryParse(idInput, out int resourceId))
        {
            var response = await _client.Api.Unknown[resourceId].GetAsync();

            if (response?.Data != null)
            {
                var resource = response.Data;
                Console.WriteLine($"\n{new string('═', 90)}");
                Console.WriteLine($"ID: {resource.Id}");
                Console.WriteLine($"Name: {resource.Name}");
                Console.WriteLine($"Year: {resource.Year}");
                Console.WriteLine($"Color: {resource.Color}");
                Console.WriteLine($"Pantone Value: {resource.PantoneValue}");
                Console.WriteLine($"{new string('═', 90)}");
            }
            else
            {
                Console.WriteLine($"\n✗ Resource with ID {resourceId} not found.");
            }
        }
        else
        {
            Console.WriteLine("\n✗ Invalid resource ID.");
        }
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
