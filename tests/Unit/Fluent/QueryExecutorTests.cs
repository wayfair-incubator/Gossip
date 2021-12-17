using System.Collections.Generic;
using System.Threading.Tasks;
using Gossip.Connection.Fluent;
using Gossip.Plugins;
using Gossip.Strategies;
using Gossip.TestSupport.Adapters.MockDb;
using Moq;
using NUnit.Framework;

namespace Gossip.UnitTests.Fluent
{
    [TestFixture]
    public class QueryExecutorTests
    {
        [Test]
        public void Creates_valid_PartitionedQueryConfigurator_through_QueryExecutor()
        {
            // arrange
            var mockSqlConnection = new MockSqlConnection();
            var queryConfig = new QueryConfiguration
            {
                Query = "select * from test_db.dbo.tblUser WHERE @batchParam = 1",
            };
            var functionMetaData = new FunctionMetadata
            {
                CallerMemberName = "Example 1",
                CallerFilePath = "Example 2"
            };
            var mockPluginManager = new Mock<IPluginManager>();
            var mockExecutionStrategy = new Mock<IExecutionStrategy>();
            var sampleList = new List<int> { 1, 2, 3, 4, 5, 6 };

            // act
            var queryExecutor = new QueryExecutor(mockSqlConnection, queryConfig, functionMetaData, mockPluginManager.Object, mockExecutionStrategy.Object);
            var partitionedQueryConfigurator = queryExecutor.BatchedBy(sampleList);
            
            // assert
            Assert.IsInstanceOf<IPartitionConfigurator<int>>(partitionedQueryConfigurator);
        }
        
        [Test]
        public async Task Verify_BulkInsertConfigurator_calls_BulkInsertAsync_in_QueryExecutor()
        {
            // arrange
            var mockQueryExecutor = new Mock<IBulkQueryExecutor>();
            var sampleList = new List<int> { 1, 2, 3, 4, 5, 6 };

            // act
            var bulkInsertConfigurator = new BulkInsertConfigurator<int>(mockQueryExecutor.Object, sampleList);
            await bulkInsertConfigurator.ExecuteAsync();
            
            // assert
            mockQueryExecutor.Verify(
                queryExecutor =>
                    queryExecutor.InsertInBulkAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<string>(),
                        It.IsAny<int>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }
    }
}