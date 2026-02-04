namespace ReqresClientRefit.Handlers;

/// <summary>
/// HTTP message handler that adds API key authentication header to requests
/// </summary>
public class ApiKeyAuthenticationHandler : DelegatingHandler
{
    private readonly string _apiKey;

    public ApiKeyAuthenticationHandler(string apiKey)
    {
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Add x-api-key header for API key authentication
        request.Headers.Add("x-api-key", _apiKey);
        
        return base.SendAsync(request, cancellationToken);
    }
}
