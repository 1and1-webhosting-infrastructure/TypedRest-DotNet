﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TypedRest
{
    /// <summary>
    /// REST endpoint that represents an RPC-like function which returns <typeparamref name="TResult"/> as output.
    /// </summary>
    /// <typeparam name="TResult">The type of entity the endpoint returns as output.</typeparam>
    public class ProducerEndpoint<TResult> : RpcEndpointBase, IProducerEndpoint<TResult>
    {
        /// <summary>
        /// Creates a new function endpoint with a relative URI.
        /// </summary>
        /// <param name="referrer">The endpoint used to navigate to this one.</param>
        /// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="referrer"/>'s.</param>
        public ProducerEndpoint(IEndpoint referrer, Uri relativeUri)
            : base(referrer, relativeUri)
        {}

        /// <summary>
        /// Creates a new function endpoint with a relative URI.
        /// </summary>
        /// <param name="referrer">The endpoint used to navigate to this one.</param>
        /// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="referrer"/>'s. Prefix <c>./</c> to append a trailing slash to the <paramref name="referrer"/> URI if missing.</param>
        public ProducerEndpoint(IEndpoint referrer, string relativeUri)
            : base(referrer, relativeUri)
        {}

        public async Task<TResult> InvokeAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var response =
                await
                    HandleResponseAsync(HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, Uri),
                        cancellationToken));

            return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted
                ? await response.Content.ReadAsAsync<TResult>(new[] {Serializer}, cancellationToken)
                : default;
        }
    }
}
