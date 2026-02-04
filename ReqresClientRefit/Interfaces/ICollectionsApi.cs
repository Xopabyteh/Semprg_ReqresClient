using Refit;
using ReqresClientRefit.Models;

namespace ReqresClientRefit.Interfaces;

/// <summary>
/// Refit interface for collections endpoints
/// </summary>
public interface ICollectionsApi
{
    /// <summary>
    /// List collections - List collections for the current project/environment
    /// </summary>
    [Get("/api/collections")]
    Task<CollectionListResponse> GetCollectionsAsync([Header("X-Reqres-Env")] string? environment = null);

    /// <summary>
    /// Create collection - Create a new collection
    /// </summary>
    [Post("/api/collections")]
    Task<CollectionResponse> CreateCollectionAsync([Body] object request, [Header("X-Reqres-Env")] string? environment = null);

    /// <summary>
    /// Get collection - Fetch a collection by slug
    /// </summary>
    [Get("/api/collections/{slug}")]
    Task<CollectionResponse> GetCollectionAsync(string slug, [Header("X-Reqres-Env")] string? environment = null);

    /// <summary>
    /// Update collection - Update a collection by slug
    /// </summary>
    [Put("/api/collections/{slug}")]
    Task<CollectionResponse> UpdateCollectionAsync(string slug, [Body] object request, [Header("X-Reqres-Env")] string? environment = null);

    /// <summary>
    /// Delete collection - Delete a collection by slug
    /// </summary>
    [Delete("/api/collections/{slug}")]
    Task DeleteCollectionAsync(string slug, [Header("X-Reqres-Env")] string? environment = null);
}
