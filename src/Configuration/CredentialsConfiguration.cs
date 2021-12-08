namespace Gossip.Configuration
{
    /// <summary>
    /// Credentials Configuration
    /// </summary>
    public class CredentialsConfiguration
    {
        /// <summary>
        /// Credentials Configuration Constructor
        /// </summary>
        public CredentialsConfiguration()
        {
        }

        /// <summary>
        /// Credentials Configuration Constructor
        /// </summary>
        /// <param name="username">Username for database connection</param>
        /// <param name="password">Password for database connection</param>
        public CredentialsConfiguration(string username, string password)
        {
            Username = username;
            Password = password;
        }

        /// <summary>
        /// Database connection username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Database connection password
        /// </summary>
        public string Password { get; set; }
    }
}