using System;
using System.Diagnostics.CodeAnalysis;

namespace Gossip.Connection.Fluent
{
    /// <inheritdoc cref="IExecutionDetails"/>
    [ExcludeFromCodeCoverage]
    public class ExecutionDetails : IExecutionDetails
    {
        public TimeSpan Duration { get; }

        public ExecutionDetails(TimeSpan duration)
        {
            Duration = duration;
        }
    }
}