using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace TypedRest.Endpoints.Rpc
{
    /// <summary>
    /// An endpoint for a non-RESTful resource that acts like a callable function.
    /// </summary>
    public interface IRpcEndpoint : IEndpoint
    {
        /// <summary>
        /// Queries the server about capabilities of the endpoint without performing any action.
        /// </summary>
        /// <param name="cancellationToken">Used to cancel the request.</param>
        /// <exception cref="InvalidDataException"><see cref="HttpStatusCode.BadRequest"/></exception>
        /// <exception cref="AuthenticationException"><see cref="HttpStatusCode.Unauthorized"/></exception>
        /// <exception cref="UnauthorizedAccessException"><see cref="HttpStatusCode.Forbidden"/></exception>
        /// <exception cref="KeyNotFoundException"><see cref="HttpStatusCode.NotFound"/> or <see cref="HttpStatusCode.Gone"/></exception>
        /// <exception cref="InvalidOperationException"><see cref="HttpStatusCode.Conflict"/></exception>
        /// <exception cref="HttpRequestException">Other non-success status code.</exception>
        Task ProbeAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Shows whether the server has indicated that the invoke method is currently allowed.
        /// </summary>
        /// <remarks>Uses cached data from last response.</remarks>
        /// <returns>An indicator whether the method is allowed. If no request has been sent yet or the server did not specify allowed methods <c>null</c> is returned.</returns>
        bool? InvokeAllowed { get; }
    }
}
