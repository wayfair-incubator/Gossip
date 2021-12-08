using System;
using System.Threading.Tasks;

namespace Gossip.Strategies
{
    /// <summary>
    /// Execution Strategies
    /// </summary>
    public interface IExecutionStrategy
    {
        /// <summary>
        /// Execution Strategy
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="fn">Execution strategy</param>
        /// <returns>Task with generic type T</returns>
        Task<T> ExecuteAsync<T>(Func<Task<T>> fn);

        /// <summary>
        /// Execution Strategy
        /// </summary>
        /// <param name="fn">Execution Strategy</param>
        /// <returns>Task</returns>
        Task ExecuteAsync(Func<Task> fn);

        /// <summary>
        /// Execution Strategy
        /// </summary>
        /// <typeparam name="T">Generic Type T</typeparam>
        /// <param name="fn">Execution Strategy</param>
        /// <returns>Generic Type T</returns>
        T Execute<T>(Func<T> fn);

        /// <summary>
        /// Execution Strategy
        /// </summary>
        /// <param name="fn">Execution Strategy</param>
        void Execute(Action fn);
    }
}