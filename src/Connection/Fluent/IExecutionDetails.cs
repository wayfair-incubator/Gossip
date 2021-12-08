using System;

namespace Gossip.Connection.Fluent
{
    /// <summary>
    ///     Provides execution details about the query.
    /// </summary>
    public interface IExecutionDetails
    {
        /// <summary>
        ///     The duration of the query.
        /// </summary>
        TimeSpan Duration { get; }
    }
}