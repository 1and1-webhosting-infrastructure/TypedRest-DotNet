using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

namespace TypedRest.Http
{
    /// <summary>
    /// Provides extension methods for <see cref="HttpClient"/>.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Send a HEAD request to the specified URI.
        /// </summary>
        public static Task<HttpResponseMessage> HeadAsync(this HttpClient httpClient, Uri uri, CancellationToken cancellationToken = default)
            => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri), cancellationToken);

        /// <summary>
        /// Send an OPTIONS request to the specified URI.
        /// </summary>
        public static Task<HttpResponseMessage> OptionsAsync(this HttpClient httpClient, Uri uri, CancellationToken cancellationToken = default)
            => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Options, uri), cancellationToken);

        public static Task<HttpResponseMessage> PatchAsync<T>(this HttpClient httpClient, Uri uri, T value, MediaTypeFormatter formatter, CancellationToken cancellationToken = default)
            => httpClient.SendAsync(new HttpRequestMessage(HttpMethods.Patch, uri)
            {
                Content = new ObjectContent<T>(value, formatter)
            }, cancellationToken);
    }
}