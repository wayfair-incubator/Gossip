using System.Threading;
using System.Threading.Tasks;

namespace Gossip.Connection
{
    /// <summary>
    /// Opens a connection to a database
    /// </summary>
    public interface IDatabaseConnectionProvider
    {
        /// <summary>
        /// Opens an async connection to a database.
        /// </summary>
        /// <returns>IDatabaseConnection</returns>
        Task<IDatabaseConnection> OpenAsync();

        /// <summary>
        /// Opens an async connection to a database.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>IDatabaseConnection</returns>
        Task<IDatabaseConnection> OpenAsync(CancellationToken cancellationToken);
    }
}