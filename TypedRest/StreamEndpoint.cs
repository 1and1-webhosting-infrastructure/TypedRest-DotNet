﻿using System;

namespace TypedRest
{
    /// <summary>
    /// REST endpoint that represents a stream of <typeparamref name="TEntity"/>s. Uses the HTTP Range header.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity the endpoint represents.</typeparam>
    public class StreamEndpoint<TEntity> : StreamEndpointBase<TEntity, ElementEndpoint<TEntity>>
    {
        /// <summary>
        /// Creates a new stream endpoint.
        /// </summary>
        /// <param name="parent">The parent endpoint containing this one.</param>
        /// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="parent"/>'s. Missing trailing slash will be appended automatically.</param>
        public StreamEndpoint(IEndpoint parent, Uri relativeUri) : base(parent, relativeUri)
        {
        }

        /// <summary>
        /// Creates a new stream endpoint.
        /// </summary>
        /// <param name="parent">The parent endpoint containing this one.</param>
        /// <param name="relativeUri">The URI of this endpoint relative to the <paramref name="parent"/>'s. Missing trailing slash will be appended automatically.</param>
        public StreamEndpoint(IEndpoint parent, string relativeUri) : base(parent, relativeUri)
        {
        }

        protected override ElementEndpoint<TEntity> GetElement(Uri relativeUri)
        {
            return new ElementEndpoint<TEntity>(this, relativeUri);
        }
    }
}