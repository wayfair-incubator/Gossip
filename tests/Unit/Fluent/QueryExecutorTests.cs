using System.Collections.Generic;
using System.Threading.Tasks;
using Gossip.Connection.Fluent;
using Gossip.Plugins;
using Gossip.Strategies;
using Gossip.TestSupport.Adapters.MockDb;
using Gossip.TestSupport.Setup;
using Moq;
using NUnit.Framework;

namespace Gossip.UnitTests.Fluent
{
    [TestFixture]
    public class QueryExecutorTests
    {
        [SetUp]
        public async Task Setup()
        {
            await DatabaseSetup.SetupAsync();
        }
        
        [Test]
        public void Creates_valid_PartitionedQueryConfigurator_through_QueryExecutor()
        {
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

            var queryExecutor = new QueryExecutor(mockSqlConnection, queryConfig, functionMetaData, mockPluginManager.Object, mockExecutionStrategy.Object);
            var sampleList = new List<int> { 1, 2, 3, 4, 5, 6 };
            var partitionedQueryConfigurator = queryExecutor.BatchedBy(sampleList);
            
            Assert.IsInstanceOf<IPartitionConfigurator<int>>(partitionedQueryConfigurator);
        }
    }
}