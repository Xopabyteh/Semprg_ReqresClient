using Refit;
using ReqresClientRefit.Models;

namespace ReqresClientRefit.Interfaces;

/// <summary>
/// Refit interface for legacy user endpoints
/// </summary>
public interface IUsersApi
{
    /// <summary>
    /// List users (legacy) - Retrieve a paginated list of demo users
    /// </summary>
    [Get("/api/users")]
    Task<LegacyUserListResponse> GetUsersAsync([Query] int? page = null, [Query, AliasAs("per_page")] int? perPage = null);

    /// <summary>
    /// Get user by id (legacy) - Retrieve a single demo user by id
    /// </summary>
    [Get("/api/users/{id}")]
    Task<LegacyUserResponse> GetUserByIdAsync(int id);

    /// <summary>
    /// Create user (legacy) - Creates a demo user
    /// </summary>
    [Post("/api/users")]
    Task<LegacyMutationResponse> CreateUserAsync([Body] LegacyMutationRequest request);

    /// <summary>
    /// Update user (legacy) - Updates a demo user
    /// </summary>
    [Put("/api/users/{id}")]
    Task<LegacyMutationResponse> UpdateUserAsync(int id, [Body] LegacyMutationRequest request);

    /// <summary>
    /// Patch user (legacy) - Partially updates a demo user
    /// </summary>
    [Patch("/api/users/{id}")]
    Task<LegacyMutationResponse> PatchUserAsync(int id, [Body] LegacyMutationRequest request);

    /// <summary>
    /// Delete user (legacy) - Deletes a demo user and returns no content
    /// </summary>
    [Delete("/api/users/{id}")]
    Task DeleteUserAsync(int id);
}
