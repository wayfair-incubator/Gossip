using System;
using System.Diagnostics.CodeAnalysis;

namespace Gossip.Connection.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class DatabaseResolutionException : Exception
    {
        public DatabaseResolutionException(string message) : base(message)
        {
        }

        public string Database { get; set; }
    }
}