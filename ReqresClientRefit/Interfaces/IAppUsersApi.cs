using Refit;
using ReqresClientRefit.Models;

namespace ReqresClientRefit.Interfaces;

/// <summary>
/// Refit interface for app users endpoints
/// </summary>
public interface IAppUsersApi
{
    /// <summary>
    /// List app users - List app users for the current project
    /// </summary>
    [Get("/api/app-users")]
    Task<AppUserListResponse> GetAppUsersAsync([Header("X-Reqres-Env")] string? environment = null, [Query] int? limit = null);

    /// <summary>
    /// Create app user - Create an app user in the project
    /// </summary>
    [Post("/api/app-users")]
    Task<AppUserResponse> CreateAppUserAsync([Body] object request);

    /// <summary>
    /// Get app user - Fetch an app user by id
    /// </summary>
    [Get("/api/app-users/{id}")]
    Task<AppUserResponse> GetAppUserAsync(string id);

    /// <summary>
    /// Update app user - Update an app user by id
    /// </summary>
    [Put("/api/app-users/{id}")]
    Task<AppUserResponse> UpdateAppUserAsync(string id, [Body] object request);

    /// <summary>
    /// Delete app user - Delete an app user by id
    /// </summary>
    [Delete("/api/app-users/{id}")]
    Task<Dictionary<string, object>> DeleteAppUserAsync(string id);

    /// <summary>
    /// App user login - Create or log in an app user and send a magic link token
    /// </summary>
    [Post("/api/app-users/login")]
    Task<Dictionary<string, object>> LoginAppUserAsync([Body] object request);

    /// <summary>
    /// Verify app user token - Verify a magic link token and return an app-session token
    /// </summary>
    [Post("/api/app-users/verify")]
    Task<Dictionary<string, object>> VerifyAppUserTokenAsync([Body] object request);

    /// <summary>
    /// Current app user - Return the current app user based on session token
    /// </summary>
    [Get("/api/app-users/me")]
    Task<Dictionary<string, object>> GetCurrentAppUserAsync();
}
