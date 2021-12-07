using Gossip.ConnectionStrings;
using NUnit.Framework;

namespace Gossip.UnitTests.ConnectionStrings
{
    [TestFixture]
    public class MsSqlConnectionStringBuilderTests
    {
        [Test]
        public void It_should_create_the_proper_connection_string()
        {
            var builder = new MsSqlConnectionStringBuilder();
            var connectionString = builder.Build(new ConnectionStringSettings
            {
                Database = "DatabaseName",
                MaxPoolSize = 3,
                MachineName = "Machine name",
                DefaultCommandTimeout = 100,
                Username = "test_username",
                Password = "test_password",
                Server = "MyServer",
                ApplicationName = "MyApplication",
                LoadBalanceTimeout = 300,
                ConnectRetryCount = 3,
                ConnectRetryInterval = 2,
                MultiSubnetFailover = true
            });

            var expected = "Data Source=MyServer;Initial Catalog=DatabaseName;Integrated Security=False;Persist Security Info=False;User ID=test_username;Password=test_password;Pooling=True;Min Pool Size=0;Max Pool Size=3;MultipleActiveResultSets=True;Replication=False;Connect Timeout=100;Encrypt=False;TrustServerCertificate=False;Load Balance Timeout=300;Type System Version=Latest;Application Name=MyApplication;Workstation ID=\"Machine name\";MultiSubnetFailover=True;ConnectRetryCount=3;ConnectRetryInterval=2";

            Assert.AreEqual(expected, connectionString);
        }

        [Test]
        public void It_should_create_the_proper_connection_string_without_credentials_when_credentials_arent_specified()
        {
            var builder = new MsSqlConnectionStringBuilder();
            var connectionString = builder.Build(new ConnectionStringSettings
            {
                Database = "DatabaseName",
                MaxPoolSize = 3,
                MachineName = "Machine name",
                DefaultCommandTimeout = 100,
                Username = null,
                Password = null,
                Server = "MyServer",
                ApplicationName = "MyApplication",
                LoadBalanceTimeout = 300,
                ConnectRetryCount = 3,
                ConnectRetryInterval = 2,
                MultiSubnetFailover = true
            });

            var expected = "Data Source=MyServer;Initial Catalog=DatabaseName;Integrated Security=False;Persist Security Info=False;Pooling=True;Min Pool Size=0;Max Pool Size=3;MultipleActiveResultSets=True;Replication=False;Connect Timeout=100;Encrypt=False;TrustServerCertificate=False;Load Balance Timeout=300;Type System Version=Latest;Application Name=MyApplication;Workstation ID=\"Machine name\";MultiSubnetFailover=True;ConnectRetryCount=3;ConnectRetryInterval=2";

            Assert.AreEqual(expected, connectionString);
        }
    }
}
