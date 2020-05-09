using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TypedRest.Errors;
using TypedRest.Links;
using TypedRest.Serializers;

namespace TypedRest.Endpoints
{
    /// <summary>
    /// Represent the top-level URI of an API. Derive from this class and add your own set of child-<see cref="IEndpoint"/>s as properties.
    /// </summary>
    public class EntryEndpoint : EndpointBase
    {
        /// <summary>
        /// Creates a new entry endpoint.
        /// </summary>
        /// <param name="httpClient">The HTTP client used to communicate with the REST API.</param>
        /// <param name="uri">The base URI of the REST API. Missing trailing slash will be appended automatically. <see cref="HttpClient.BaseAddress"/> is used if this is unset.</param>
        /// <param name="serializer">Controls the serialization of entities sent to and received from the server. Defaults to a JSON serializer if unset.</param>
        /// <param name="errorHandler">Handles errors in HTTP responses. Leave unset for default implementation.</param>
        /// <param name="linkHandler">Detects links in HTTP responses. Leave unset for default implementation.</param>
        public EntryEndpoint(HttpClient httpClient, Uri? uri = null, MediaTypeFormatter? serializer = null, IErrorHandler? errorHandler = null, ILinkHandler? linkHandler = null)
            : base(
                (uri ?? httpClient.BaseAddress ?? throw new ArgumentException("uri or httpClient.BaseAddress must be set.", nameof(uri))).EnsureTrailingSlash(),
                httpClient,
                serializer ?? new DefaultJsonSerializer(),
                errorHandler ?? new DefaultErrorHandler(),
                linkHandler ?? new DefaultLinkHandler())
        {
            foreach (var mediaType in Serializer.SupportedMediaTypes)
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType.MediaType));
        }

        /// <summary>
        /// Creates a new entry endpoint.
        /// </summary>
        /// <param name="uri">The base URI of the REST API.</param>
        /// <param name="credentials">Optional HTTP Basic Auth credentials used to authenticate against the REST API.</param>
        /// <param name="serializer">Controls the serialization of entities sent to and received from the server. Defaults to a JSON serializer if unset.</param>
        /// <param name="errorHandler">Handles errors in HTTP responses. Leave unset for default implementation.</param>
        /// <param name="linkHandler">Detects links in HTTP responses. Leave unset for default implementation.</param>
        public EntryEndpoint(Uri uri, ICredentials? credentials = null, MediaTypeFormatter? serializer = null, IErrorHandler? errorHandler = null, ILinkHandler? linkHandler = null)
            : this(new HttpClient(), uri, serializer, errorHandler, linkHandler)
        {
            var basicAuthCredentials = credentials?.GetCredential(Uri, authType: "Basic");
            string? userInfo = (basicAuthCredentials != null)
                ? basicAuthCredentials.UserName + ":" + basicAuthCredentials.Password
                : uri.UserInfo;

            if (!string.IsNullOrEmpty(userInfo))
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.GetEncoding("iso-8859-1").GetBytes(userInfo)));
            }
        }

        /// <summary>
        /// Fetches meta data such as links from the server.
        /// </summary>
        /// <exception cref="AuthenticationException"><see cref="HttpStatusCode.Unauthorized"/></exception>
        /// <exception cref="UnauthorizedAccessException"><see cref="HttpStatusCode.Forbidden"/></exception>
        /// <exception cref="KeyNotFoundException"><see cref="HttpStatusCode.NotFound"/> or <see cref="HttpStatusCode.Gone"/></exception>
        /// <exception cref="HttpRequestException">Other non-success status code.</exception>
        public Task ReadMetaAsync(CancellationToken cancellationToken = default)
            => HandleResponseAsync(HttpClient.GetAsync(Uri, cancellationToken));
    }
}
