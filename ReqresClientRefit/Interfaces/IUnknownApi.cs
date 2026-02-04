using Refit;
using ReqresClientRefit.Models;

namespace ReqresClientRefit.Interfaces;

/// <summary>
/// Refit interface for legacy unknown resource endpoints
/// </summary>
public interface IUnknownApi
{
    /// <summary>
    /// List resources (legacy) - Retrieve a paginated list of the demo "unknown" resource
    /// </summary>
    [Get("/api/unknown")]
    Task<LegacyUnknownListResponse> GetResourcesAsync([Query] int? page = null, [Query, AliasAs("per_page")] int? perPage = null);

    /// <summary>
    /// Get resource by id (legacy) - Retrieve a single demo resource by id
    /// </summary>
    [Get("/api/unknown/{id}")]
    Task<LegacyUnknownResponse> GetResourceByIdAsync(int id);
}
