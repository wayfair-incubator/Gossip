using System;
using Gossip.Connection.Fluent;
using Moq;
using NUnit.Framework;

namespace Gossip.UnitTests.Fluent
{
    [TestFixture]
    public class PartitionedQueryConfiguratorTests
    {
        public class When_the_query_does_not_have_batchParam_variable
        {
            [Test]
            public void It_should_throw_an_exception()
            {
                var queryExecutor = new Mock<IUpdatableQueryExecutor>();
                var queryConfig = new QueryConfiguration
                {
                    Query = "select * from test_db.dbo.tblUser",
                };
                var data = new[] { 1, 2, 3 };
                var configurator = new PartitionedQueryConfigurator<int>(queryExecutor.Object, queryConfig, data).WithBatchParamAsJsonArray();
                Assert.Throws<Exception>(() => configurator.Query<int>());
            }
        }

        public class When_no_batchParamCallback_is_specified
        {
            [Test]
            public void It_should_throw_an_exception()
            {
                var queryExecutor = new Mock<IUpdatableQueryExecutor>();
                var queryConfig = new QueryConfiguration
                {
                    Query = "select * from test_db.dbo.tblUser WHERE @batchParam = 1",
                };
                var data = new[] { 1, 2, 3 };
                var configurator = new PartitionedQueryConfigurator<int>(queryExecutor.Object, queryConfig, data);
                Assert.Throws<Exception>(() => configurator.Query<int>());
            }
        }

        public class When_data_is_batched
        {
            [Test]
            public void It_should_call_the_database_once_per_batch()
            {
                var mockQueryExecutor = new Mock<IUpdatableQueryExecutor>();
                var query = "select * from test_db.dbo.tblUser WHERE @batchParam = 1";
                var queryConfig = new QueryConfiguration { Query = query };

                mockQueryExecutor.Setup(x => x.UpdateConfig(It.IsAny<QueryConfiguration>())).Callback<QueryConfiguration>(config => queryConfig = config);
                var data = new[] { 1, 2, 3 };
                var batchSize = 1;
                new PartitionedQueryConfigurator<int>(mockQueryExecutor.Object, queryConfig, data)
                    .WithBatchSize(batchSize)
                    .WithBatchParamAsJsonArray()
                    .Query<int>();

                mockQueryExecutor.Verify(x => x.Query<int>(), Times.Exactly(data.Length / batchSize));
            }
        }
    }
}
