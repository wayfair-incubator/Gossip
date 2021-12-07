using System;

namespace Gossip.Containers
{
    /// <summary>
    /// Context
    /// </summary>
    public interface IContext : IDisposable
    {
        /// <summary>
        /// Save Changes
        /// </summary>
        /// <returns>integer result</returns>
        int SaveChanges();

        /// <summary>
        /// Database Connection String
        /// </summary>
        string ConnectionString { get; }
    }
}
