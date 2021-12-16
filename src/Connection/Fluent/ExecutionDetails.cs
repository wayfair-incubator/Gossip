using System;

namespace Gossip.Connection.Fluent
{
    /// <inheritdoc cref="IExecutionDetails"/>
    public class ExecutionDetails : IExecutionDetails
    {
        public TimeSpan Duration { get; }

        public ExecutionDetails(TimeSpan duration)
        {
            Duration = duration;
        }
    }
}