namespace Gossip.Connection.Fluent
{
    /// <summary>
    ///     Provides a method updating an existing <see cref="QueryConfiguration"/> for a <see cref="IQueryExecutor"/>.
    /// </summary>
    public interface IUpdatableQueryExecutor : IQueryExecutor
    {
        /// <summary>
        ///     Update the existing <see cref="QueryConfiguration"/> on the <see cref="IQueryExecutor"/>.
        /// </summary>
        /// <param name="config">The new <see cref="QueryConfiguration"/> to use.</param>
        void UpdateConfig(QueryConfiguration config);
    }
}