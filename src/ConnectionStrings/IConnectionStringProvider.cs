using System.Threading.Tasks;
using Gossip.Connection;

namespace Gossip.ConnectionStrings
{
    /// <summary>
    /// Connection String Provider
    /// </summary>
    public interface IConnectionStringProvider
    {
        /// <summary>
        /// Gets a connection string
        /// </summary>
        /// <returns>IConnectionString</returns>
        Task<IConnectionString> GetAsync();
    }
}