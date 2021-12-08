using System.Data.SqlClient;

namespace Gossip.ConnectionStrings
{
    /// <summary>
    /// Builds a MsSql connection string
    /// </summary>
    /// <inheritdoc cref="IConnectionStringBuilder"/>
    public class MsSqlConnectionStringBuilder : IConnectionStringBuilder
    {
        /// <inheritdoc />
        public string Build(IConnectionStringSettings settings)
        {
            var builder = new SqlConnectionStringBuilder
            {
                ["Server"] = settings.Server,
                ["Database"] = settings.Database,
                MinPoolSize = 0,
                MaxPoolSize = settings.MaxPoolSize,
                MultipleActiveResultSets = true,
                PersistSecurityInfo = false,
                Pooling = true,
                Replication = false,
                InitialCatalog = settings.Database,
                TrustServerCertificate = false,
                TypeSystemVersion = "Latest",
                WorkstationID = settings.MachineName,
                ApplicationName = settings.ApplicationName,
                ConnectRetryCount = settings.ConnectRetryCount,
                Encrypt = false,
                LoadBalanceTimeout = settings.LoadBalanceTimeout,
                ConnectTimeout = settings.DefaultCommandTimeout,
                IntegratedSecurity = settings.UseIntegratedSecurity,
                MultiSubnetFailover = settings.MultiSubnetFailover
            };

            if (settings.ConnectRetryCount > 0)
            {
                builder.ConnectRetryInterval = settings.ConnectRetryInterval;
            }

            if (!settings.UseIntegratedSecurity && !string.IsNullOrEmpty(settings.Username))
            {
                builder.UserID = settings.Username;
            }

            if (!settings.UseIntegratedSecurity && !string.IsNullOrEmpty(settings.Password))
            {
                builder.Password = settings.Password;
            }
            
            return builder.ConnectionString;
        }
    }
}