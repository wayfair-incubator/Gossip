using System.Threading.Tasks;
using Gossip.Connection;
using Gossip.TestSupport.Adapters.Sqlite;
using Gossip.TestSupport.Setup;
using Gossip.UnitTests.Fallbacks;
using NUnit.Framework;

namespace Gossip.UnitTests.Queries
{
    [TestFixture]
    public class QueryFirstOrDefaultTests
    {
        public class When_multiple_results_can_be_returned
        {
            [SetUp]
            public async Task Setup()
            {
                await DatabaseSetup.SetupAsync();
            }

            [Test]
            public async Task It_should_return_only_the_first_result()
            {
                var connectionProvider = Database.Configure<Sqlite>()
                    .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                    .Build();

                using var conn = await connectionProvider.OpenAsync();

                var result = conn.Configure()
                    .WithQuery("SELECT Id, Name FROM tblTest WHERE Grouping='TwoRows'")
                    .Build()
                    .QueryFirstOrDefault<TableTest>();
                Assert.AreEqual("SomeID2", result.Id);
                Assert.AreEqual("FirstRecord", result.Name);
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
            public async Task It_should_return_null()
            {
                var connectionProvider = Database.Configure<Sqlite>()
                    .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                    .Build();

                using var conn = await connectionProvider.OpenAsync();

                var result = conn.Configure()
                    .WithQuery("SELECT Id, Name FROM tblTest WHERE Grouping='NoRows'")
                    .Build()
                    .QueryFirstOrDefault<TableTest>();
                Assert.IsNull(result);
            }
        }
    }
}
