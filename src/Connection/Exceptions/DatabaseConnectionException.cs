using System;
using System.Diagnostics.CodeAnalysis;

namespace Gossip.Connection.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class DatabaseConnectionException : Exception
    {
        public DatabaseConnectionException(string message) : base(message)
        {
        }

        public DatabaseConnectionException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}