using System.Threading.Tasks;
using Gossip.Connection;
using Gossip.TestSupport.Adapters.Sqlite;
using Gossip.TestSupport.Setup;
using NUnit.Framework;

namespace Gossip.UnitTests.Queries
{
    [TestFixture]
    public class ExecuteTests
    {
        public class When_no_records_are_affected
        {
            [SetUp]
            public async Task Setup()
            {
                await DatabaseSetup.SetupAsync();
            }

            [Test]
            public async Task It_should_return_1()
            {
                var connectionProvider = Database.Configure<Sqlite>()
                    .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                    .Build();

                using var conn = await connectionProvider.OpenAsync();

                var rowsAffected = conn.Execute("UPDATE tblTest SET Name='SomethingElse' WHERE Id='SomethingMadeUp'");
                Assert.AreEqual(0, rowsAffected);
            }
        }

        public class When_one_record_is_affected
        {
            [SetUp]
            public async Task Setup()
            {
                await DatabaseSetup.SetupAsync();
            }

            [Test]
            public async Task It_should_return_1()
            {
                var connectionProvider = Database.Configure<Sqlite>()
                    .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                    .Build();

                using var conn = await connectionProvider.OpenAsync();

                var rowsAffected = conn.Execute("UPDATE tblTest SET Name='SomethingElse' WHERE Id='SomeID'");
                Assert.AreEqual(1, rowsAffected);
            }
        }

        public class When_two_records_are_affected
        {
            [SetUp]
            public async Task Setup()
            {
                await DatabaseSetup.SetupAsync();
            }

            [Test]
            public async Task It_should_return_2()
            {
                var connectionProvider = Database.Configure<Sqlite>()
                    .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                    .Build();

                using var conn = await connectionProvider.OpenAsync();

                var rowsAffected = conn.Execute("UPDATE tblTest SET Name='SomethingElse' WHERE Grouping='TwoRows'");
                Assert.AreEqual(2, rowsAffected);
            }
        }
    }
}
