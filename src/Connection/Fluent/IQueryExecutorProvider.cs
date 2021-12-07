namespace Gossip.Connection.Fluent
{
    /// <summary>
    ///     Provides various query executors.
    /// </summary>
    public interface IQueryExecutorProvider
    {
        /// <summary>
        ///     Get an <see cref="IQueryExecutor"/> from the given connection, query configuration, and metadata.
        /// </summary>
        /// <param name="conn">The SQL connection.</param>
        /// <param name="config">The query configuration.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>An <see cref="IQueryExecutor"/>.</returns>
        IQueryExecutor GetQueryExecutor(ISqlConnection conn, QueryConfiguration config, FunctionMetadata metadata);

        /// <summary>
        ///     Get an <see cref="IBulkQueryExecutor"/> from the given connection, query configuration, and metadata.
        /// </summary>
        /// <param name="conn">The SQL connection.</param>
        /// <param name="config">The query configuration.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>An <see cref="IBulkQueryExecutor"/>.</returns>
        IBulkQueryExecutor GetBulkQueryExecutor(ISqlConnection conn, QueryConfiguration config, FunctionMetadata metadata);
    }
}
