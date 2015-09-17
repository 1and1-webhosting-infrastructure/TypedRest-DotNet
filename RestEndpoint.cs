﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TypedRest
{
    /// <summary>
    /// REST endpoint, i.e. a remote HTTP resource.
    /// </summary>
    public abstract class RestEndpoint : IRestEndpoint
    {
        public HttpClient HttpClient { get; }

        public Uri Uri { get; }

        /// <summary>
        /// Creates a new REST endpoint with an absolute URI.
        /// </summary>
        /// <param name="httpClient">The HTTP client used to communicate with the remote element.</param>
        /// <param name="uri">The HTTP URI of the remote element.</param>
        protected RestEndpoint(HttpClient httpClient, Uri uri)
        {
            HttpClient = httpClient;
            Uri = uri;
        }

        /// <summary>
        /// Creates a new REST endpoint with a relative URI.
        /// </summary>
        /// <param name="parent">The parent endpoint containing this one.</param>
        /// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="parent"/>'s.</param>
        protected RestEndpoint(IRestEndpoint parent, Uri relativeUri)
            : this(parent.HttpClient, new Uri(parent.Uri.EnsureTrailingSlash(), relativeUri))
        {
        }

        /// <summary>
        /// Wraps HTTP status codes in appropriate <see cref="Exception"/> types.
        /// </summary>
        /// <exception cref="InvalidDataException"><see cref="HttpStatusCode.BadRequest"/></exception>
        /// <exception cref="UnauthorizedAccessException"><see cref="HttpStatusCode.Unauthorized"/> or <see cref="HttpStatusCode.Forbidden"/></exception>
        /// <exception cref="KeyNotFoundException"><see cref="HttpStatusCode.NotFound"/> or <see cref="HttpStatusCode.Gone"/></exception>
        /// <exception cref="InvalidOperationException"><see cref="HttpStatusCode.Conflict"/></exception>
        /// <exception cref="HttpRequestException">Other non-success status code.</exception>
        protected static async Task HandleErrors(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return;

            var message = (response.Content.Headers.ContentType.MediaType == "application/json")
                ? JsonConvert.DeserializeAnonymousType(await response.Content.ReadAsStringAsync(), new {Message = ""}).Message
                : response.ReasonPhrase;

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new InvalidDataException(message);
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    throw new UnauthorizedAccessException(message);
                case HttpStatusCode.NotFound:
                case HttpStatusCode.Gone:
                    throw new KeyNotFoundException(message);
                case HttpStatusCode.Conflict:
                    throw new InvalidOperationException(message);
                default:
                    throw new HttpRequestException(message);
            }
        }
    }
}