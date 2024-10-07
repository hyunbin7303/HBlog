using Blazored.LocalStorage;
using HBlog.Contract.Common;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HBlog.WebClient.Handlers;

/// <summary>
/// Handler to ensure token is automatically sent over with each request.
/// </summary>
public class TokenHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorageService;

    public TokenHandler(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }
    /// <summary>
    /// Main method to override for the handler.
    /// </summary>
    /// <param name="request">The original request.</param>
    /// <param name="cancellationToken">The token to handle cancellations.</param>
    /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _localStorageService.GetItemAsync<string>(Constants.AccessToken);
        if (token != null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
