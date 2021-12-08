using System.Data.SqlClient;
using System.Threading.Tasks;
using Gossip.Connection;
using Gossip.Connection.Factories;
using Gossip.ConnectionStrings;
using Gossip.TestSupport.Setup;
using NUnit.Framework;

namespace Gossip.IntegrationTests.Connections
{
    [TestFixture]
    public class CommandTimeoutTests
    {
        [Test]
        public async Task When_command_timeout_is_lower_than_the_execution_time_it_should_throw_SqlException()
        {
            var mssql = new MsSql();

            var db = Database
                .Configure(mssql)
                .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.LocalMsSqlConnectionString })
                .WithCommandTimeout(1)
                .Build();

            using var conn = await db.OpenAsync();

            Assert.ThrowsAsync<SqlException>(() => conn.QueryAsync<string>("WAITFOR DELAY '000:00:02' SELECT '2 Second Delay'"));
        }

        [Test]
        public async Task When_command_timeout_is_lower_than_the_execution_time_it_should_not_throw_SqlException()
        {
            var mssqlConnectionStringBuilder = new MsSqlConnectionStringBuilder();
            var databaseServerAddress = DatabaseSetup.GetDatabaseServerAddress();
            var connectionString = mssqlConnectionStringBuilder.Build(new ConnectionStringSettings
            {
                ApplicationName = "Test",
                Database = "master",
                MachineName = "Test",
                Server = databaseServerAddress,
                Password = "55Data!Access!Password!",
                Username = "sa",
                MaxPoolSize = 50,
                DefaultCommandTimeout = 5
            });

            var mssql = new MsSql();

            var db = Database
                .Configure(mssql)
                .WithConnectionString(() => new ConnectionString { Value = connectionString })
                .WithCommandTimeout(2)
                .Build();

            using var conn = await db.OpenAsync();

            Assert.DoesNotThrowAsync(() => conn.QueryAsync<string>("WAITFOR DELAY '000:00:01' SELECT '1 Second Delay'"));
        }
    }
}
