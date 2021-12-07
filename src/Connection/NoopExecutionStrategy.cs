using System;
using System.Threading.Tasks;
using Gossip.Strategies;

namespace Gossip.Connection
{
    /// <inheritdoc cref="IExecutionStrategy"/>

    /// <summary>
    /// Noop Execution Strategy
    /// </summary>
    public class NoopExecutionStrategy : IExecutionStrategy
    {
        /// <inheritdoc />
        public Task<T> ExecuteAsync<T>(Func<Task<T>> fn)
        {
            return fn();
        }

        /// <inheritdoc />
        public Task ExecuteAsync(Func<Task> fn)
        {
            return fn();
        }

        /// <inheritdoc />
        public T Execute<T>(Func<T> fn)
        {
            return fn();
        }

        /// <inheritdoc />
        public void Execute(Action fn)
        {
            fn();
        }
    }
}