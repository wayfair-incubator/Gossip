namespace Gossip.ConnectionStrings
{
    /// <summary>
    /// Connection String Settings
    /// </summary>
    public interface IConnectionStringSettings
    {
        /// <summary>
        /// Name of the application that is connecting to the database
        /// </summary>
        string ApplicationName { get; }

        /// <summary>
        /// Name of the machine that is connecting to the database
        /// </summary>
        string MachineName { get; }

        /// <summary>
        /// The database server name (e.g. localhost)
        /// </summary>
        string Server { get; }

        /// <summary>
        /// The database name
        /// </summary>
        string Database { get; }

        /// <summary>
        /// The max number of database connection objects for the database connection string
        /// </summary>
        int MaxPoolSize { get; }

        /// <summary>
        /// The time in seconds that the connection will wait for a command to execute
        /// </summary>
        int DefaultCommandTimeout { get; }

        /// <summary>
        /// Username of user/application that is connecting to the database
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Database connection password
        /// </summary>
        string Password { get; }

        /// <summary>
        /// Use Windows Authentication to connect to the database server.
        /// </summary>
        bool UseIntegratedSecurity { get; }

        /// <summary>
        /// The number of reconnections attempted after identifying that there was an idle connection failure. This must be an integer between 0 and 255. Default is 1. Set to 0 to disable reconnecting on idle connection failures. An ArgumentException will be thrown if set to a value outside of the allowed range.
        /// </summary>
        int ConnectRetryCount { get; }

        /// <summary>
        /// Amount of time (in seconds) between each reconnection attempt after identifying that there was an idle connection failure. This must be an integer between 1 and 60. The default is 10 seconds. An ArgumentException will be thrown if set to a value outside of the allowed range.
        /// </summary>
        int ConnectRetryInterval { get; }

        /// <summary>
        /// Gets or sets the minimum time, in seconds, for the connection to live in the connection pool before being destroyed.
        /// </summary>
        int LoadBalanceTimeout { get; }

        /// <summary>
        /// If your application is connecting to an AlwaysOn availability group (AG) on different subnets, setting MultiSubnetFailover=true provides faster detection of and connection to the (currently) active server.
        /// </summary>
        bool MultiSubnetFailover { get; }
    }
}
