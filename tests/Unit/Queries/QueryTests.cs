using System.Linq;
using System.Threading.Tasks;
using Gossip.Connection;
using Gossip.TestSupport.Adapters.Sqlite;
using Gossip.TestSupport.Setup;
using Gossip.UnitTests.Fallbacks;
using NUnit.Framework;

namespace Gossip.UnitTests.Queries
{
    [TestFixture]
    public class QueryTests
    {
        public class When_data_is_available_to_be_queried
        {
            [SetUp]
            public async Task Setup()
            {
                await DatabaseSetup.SetupAsync();
            }

            [Test]
            public async Task It_should_return_the_data()
            {
                var connectionProvider = Database.Configure<Sqlite>()
                    .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                    .Build();

                using var conn = await connectionProvider.OpenAsync();

                var results = conn.Query<TableTest>("SELECT Id, Name, Grouping FROM tblTest WHERE Grouping='OneRow'").ToList();
                Assert.AreEqual("SomeID", results.Single().Id);
                Assert.AreEqual("Testing", results.Single().Name);
                Assert.AreEqual("OneRow", results.Single().Grouping);
            }
        }

        public class When_no_data_is_available_to_be_queried
        {
            [SetUp]
            public async Task Setup()
            {
                await DatabaseSetup.SetupAsync();
            }

            [Test]
            public async Task It_should_return_zero_rows()
            {
                var connectionProvider = Database.Configure<Sqlite>()
                    .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                    .Build();

                using var conn = await connectionProvider.OpenAsync();

                var results = conn.Query<TableTest>("SELECT Id, Name FROM tblTest WHERE Grouping='NoRows'").ToList();
                Assert.IsEmpty(results);
            }
        }
    }
}
