using System.Threading.Tasks;
using Gossip.Connection;
using Gossip.Connection.Fluent;
using Gossip.Monitoring;

namespace Gossip.Plugins
{
    /// <summary>
    /// Interface for database plugins.
    /// </summary>
    public interface IDatabasePlugin
    {
        /// <summary>
        /// Executes when DatabaseConnectionProvider is built.
        /// </summary>
        /// <param name="usageDetails">Library usage details</param>
        void OnBuild(UsageDetails usageDetails);

        /// <summary>
        /// Executes while SQL query is in progress
        /// </summary>
        /// <param name="connectionDetails">Database connection details</param>
        /// <param name="metadata">Metadata</param>
        Task OnQueryExecutingAsync(IConnectionDetails connectionDetails, FunctionMetadata metadata);

        /// <summary>
        /// Executes when SQL query is complete
        /// </summary>
        /// <param name="connectionDetails">Database connection details</param>
        /// <param name="executionDetails">Execution details providing information about the query</param>
        /// <param name="metadata">Metadata</param>
        Task OnQueryExecutedAsync(IConnectionDetails connectionDetails, IExecutionDetails executionDetails, FunctionMetadata metadata);

        /// <summary>
        /// Executes when database connection is opening
        /// </summary>
        /// <param name="connectionDetails">Database connection details</param>
        Task OnConnectionOpeningAsync(IConnectionDetails connectionDetails);

        /// <summary>
        /// Executes when database connection has finished opening
        /// </summary>
        /// <param name="connectionDetails">Database connection details</param>
        /// <param name="executionDetails">Execution details providing information about the query</param>
        Task OnConnectionOpenAsync(IConnectionDetails connectionDetails, IExecutionDetails executionDetails);

        /// <summary>
        /// Executes when database connection handles an exception during connection
        /// </summary>
        /// <param name="connectionDetails">Database connection details</param>
        Task OnConnectionExceptionAsync(IConnectionDetails connectionDetails);

        /// <summary>
        /// Executes when database connection exception is resolved/handled
        /// </summary>
        /// <param name="database">Database name</param>
        Task OnDatabaseResolutionExceptionAsync(string database);
        /// <summary>
        /// Executes when database monitor is executed.
        /// </summary>
        /// <param name="databaseMonitorReport"><see cref="IDatabaseMonitorReport"/></param>
        Task OnDatabaseMonitorExecutedAsync(IDatabaseMonitorReport databaseMonitorReport);
    }
}