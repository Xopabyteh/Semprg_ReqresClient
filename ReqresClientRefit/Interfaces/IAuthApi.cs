using Refit;
using ReqresClientRefit.Models;

namespace ReqresClientRefit.Interfaces;

/// <summary>
/// Refit interface for authentication endpoints
/// </summary>
public interface IAuthApi
{
    /// <summary>
    /// Register user - Register a demo user account
    /// </summary>
    [Post("/api/register")]
    Task<RegisterResponse> RegisterAsync([Body] AuthRequest request);

    /// <summary>
    /// Login user - Authenticate a demo user and return a token
    /// </summary>
    [Post("/api/login")]
    Task<LoginResponse> LoginAsync([Body] AuthRequest request);

    /// <summary>
    /// Logout user - Legacy logout endpoint (returns empty object)
    /// </summary>
    [Post("/api/logout")]
    Task<Dictionary<string, object>> LogoutAsync();
}
