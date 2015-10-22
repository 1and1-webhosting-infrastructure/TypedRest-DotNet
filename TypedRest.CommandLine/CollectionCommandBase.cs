﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TypedRest.CommandLine
{
    /// <summary>
    /// Base class for building commands operating on an <see cref="ICollectionEndpoint{TEntity,TElement}"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity the <typeparamref name="TEndpoint"/> represents.</typeparam>
    /// <typeparam name="TEndpoint">The specific type of <see cref="ICollectionEndpoint{TEntity,TElement}"/> to operate on.</typeparam>
    /// <typeparam name="TElement">The specific type of <see cref="IElementEndpoint{TEntity}"/>s the <typeparamref name="TEndpoint"/> provides for individual <typeparamref name="TEntity"/>s.</typeparam>
    public abstract class CollectionCommandBase<TEntity, TEndpoint, TElement> : CommandBase<TEndpoint>
        where TEndpoint : ICollectionEndpoint<TEntity, TElement>
        where TElement : class, IElementEndpoint<TEntity>
    {
        /// <summary>
        /// Creates a new REST collection command.
        /// </summary>
        /// <param name="endpoint">The REST endpoint this command operates on.</param>
        protected CollectionCommandBase(TEndpoint endpoint) : base(endpoint)
        {
        }

        public override async Task ExecuteAsync(IReadOnlyList<string> args,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (args.Count == 0)
                OutputEntities(await Endpoint.ReadAllAsync(cancellationToken));
            else if (args[0].ToLowerInvariant() == "create")
            {
                var newEntity = InputEntity(args.Skip(1).ToList());
                var newEndpoint = await Endpoint.CreateAsync(newEntity, cancellationToken);
                Console.WriteLine(newEndpoint.Uri);
            }
            else
                await base.ExecuteAsync(args, cancellationToken);
        }

        protected override ICommand GetSubCommand(string name)
        {
            return GetElementCommand(Endpoint[name]);
        }

        /// <summary>
        /// Creates a sub-<see cref="ICommand"/> for the given <paramref name="element"/>.
        /// </summary>
        protected abstract ICommand GetElementCommand(TElement element);

        /// <summary>
        /// Aquires a <typeparamref name="TEntity"/> from the user, e.g. by parsing the <paramref name="args"/> or via JSON on the command-line.
        /// </summary>
        protected virtual TEntity InputEntity(IReadOnlyList<string> args)
        {
            return JsonConvert.DeserializeObject<TEntity>((args.Count == 0) ? Console.ReadLine() : args[0]);
        }

        /// <summary>
        /// Outputs a collection of <typeparamref name="TEntity"/>s to the user, e.g., via <see cref="object.ToString"/> on the command-line.
        /// </summary>
        protected virtual void OutputEntities(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                Console.WriteLine(entity.ToString());
        }
    }
}