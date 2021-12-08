using System;

namespace Gossip.Connection.Exceptions
{
    public class DatabaseResolutionException : Exception
    {
        public DatabaseResolutionException(string message) : base(message)
        {
        }

        public string Database { get; set; }
    }
}