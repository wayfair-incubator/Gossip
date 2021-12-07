using System;
using System.Threading.Tasks;
using Gossip.Connection;
using Gossip.TestSupport.Adapters.Sqlite;
using Gossip.TestSupport.Setup;
using Gossip.UnitTests.Fallbacks;
using NUnit.Framework;

namespace Gossip.UnitTests.Queries
{
    [TestFixture]
    public class QuerySingleOrDefaultTests
    {
        public class When_multiple_results_can_be_returned
        {
            [SetUp]
            public async Task Setup()
            {
                await DatabaseSetup.SetupAsync();
            }

            [Test]
            public async Task It_should_throw_an_AggregateException()
            {
                var connectionProvider = Database.Configure<Sqlite>()
                    .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                    .Build();

                using var conn = await connectionProvider.OpenAsync();

                Assert.Throws<AggregateException>(() => conn.QuerySingleOrDefault<TableTest>("SELECT Id, Name FROM tblTest WHERE Grouping='TwoRows'"));
            }
        }

        public class When_one_result_can_be_returned
        {
            [SetUp]
            public async Task Setup()
            {
                await DatabaseSetup.SetupAsync();
            }

            [Test]
            public async Task It_should_return_the_record()
            {
                var connectionProvider = Database.Configure<Sqlite>()
                    .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                    .Build();

                using var conn = await connectionProvider.OpenAsync();

                var result = conn.QuerySingleOrDefault<TableTest>("SELECT Id, Name FROM tblTest WHERE Grouping='OneRow'");
                Assert.AreEqual("SomeID", result.Id);
                Assert.AreEqual("Testing", result.Name);
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

                var result = conn.QuerySingleOrDefault<TableTest>("SELECT Id, Name FROM tblTest WHERE Grouping='NoRows'");
                Assert.IsNull(result);
            }
        }
    }
}
