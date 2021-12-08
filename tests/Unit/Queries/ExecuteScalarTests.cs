using System.Threading.Tasks;
using Gossip.Connection;
using Gossip.TestSupport.Adapters.Sqlite;
using Gossip.TestSupport.Setup;
using NUnit.Framework;

namespace Gossip.UnitTests.Queries
{
    [TestFixture]
    public class ExecuteScalarTests
    {
        public class When_using_synchronous_functionality
        {
            [SetUp]
            public async Task Setup()
            {
                await DatabaseSetup.SetupAsync();
            }

            [Test]
            public async Task It_should_return_the_data_from_the_query()
            {
                var connectionProvider = Database.Configure<Sqlite>()
                    .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                    .Build();

                using var conn = await connectionProvider.OpenAsync();

                var insertedName = conn.Configure()
                    .WithQuery("INSERT INTO tblTest(Id, Name, Grouping) VALUES ('abc', 'myName', 'someGrouping');" +
                               "SELECT Name FROM tblTest WHERE Id = 'abc'")
                    .Build()
                    .ExecuteScalar<string>();

                Assert.AreEqual("myName", insertedName);
            }
        }

        public class When_using_asynchronous_functionality
        {
            [SetUp]
            public async Task Setup()
            {
                await DatabaseSetup.SetupAsync();
            }

            [Test]
            public async Task It_should_return_the_data_from_the_query()
            {
                var connectionProvider = Database.Configure<Sqlite>()
                    .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                    .Build();

                using var conn = await connectionProvider.OpenAsync();

                var insertedName = await conn.Configure()
                    .WithQuery("INSERT INTO tblTest(Id, Name, Grouping) VALUES ('abc', 'myName', 'someGrouping');" +
                               "SELECT Name FROM tblTest WHERE Id = 'abc'")
                    .Build()
                    .ExecuteScalarAsync<string>();

                Assert.AreEqual("myName", insertedName);
            }
        }
    }
}
