using System;
using System.Threading.Tasks;
using Gossip.Connection;
using Gossip.Connection.Fluent;
using Gossip.Plugins;
using Gossip.TestSupport.Adapters.Sqlite;
using Gossip.TestSupport.Setup;
using Moq;
using NUnit.Framework;

namespace Gossip.UnitTests.Plugins
{
    [TestFixture]
    public class PluginTests
    {
        public class When_a_plugin_is_specified
        {
            [SetUp]
            public async Task SetUp()
            {
                await DatabaseSetup.SetupAsync();
            }

            [Test]
            public async Task It_should_be_called()
            {
                var mockPlugin = new Mock<IDatabasePlugin>();

                var db = Database.Configure<Sqlite>()
                    .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                    .WithPlugin(() => mockPlugin.Object)
                    .Build();

                using (var conn = await db.OpenAsync())
                {
                    await conn.ExecuteAsync("SELECT 1");
                }

                await Task.Delay(TimeSpan.FromMilliseconds(100));

                mockPlugin.Verify(x => x.OnConnectionOpeningAsync(It.IsAny<IConnectionDetails>()), Times.Once());
                mockPlugin.Verify(x => x.OnConnectionOpenAsync(It.IsAny<IConnectionDetails>(), It.IsAny<IExecutionDetails>()), Times.Once());
                mockPlugin.Verify(x => x.OnQueryExecutingAsync(It.IsAny<IConnectionDetails>(), It.IsAny<FunctionMetadata>()), Times.Once());
                mockPlugin.Verify(x => x.OnQueryExecutedAsync(It.IsAny<IConnectionDetails>(), It.IsAny<IExecutionDetails>(), It.IsAny<FunctionMetadata>()), Times.Once());
            }
        }

        [TestFixture]
        public class When_two_plugins_are_specified
        {
            [TestFixture]
            public class When_there_is_one_query_run
            {
                [SetUp]
                public async Task SetUp()
                {
                    await DatabaseSetup.SetupAsync();
                }

                [Test]
                public async Task They_should_both_be_called_once()
                {
                    var mockPlugin = new Mock<IDatabasePlugin>();

                    var db = Database.Configure<Sqlite>()
                        .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                        .WithPlugin(() => mockPlugin.Object)
                        .WithPlugin(() => mockPlugin.Object)
                        .Build();

                    using (var conn = await db.OpenAsync())
                    {
                        await conn.ExecuteAsync("SELECT 1");
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(100));

                    mockPlugin.Verify(x => x.OnConnectionOpeningAsync(It.IsAny<IConnectionDetails>()), Times.Exactly(2));
                    mockPlugin.Verify(x => x.OnConnectionOpenAsync(It.IsAny<IConnectionDetails>(), It.IsAny<IExecutionDetails>()), Times.Exactly(2));
                    mockPlugin.Verify(x => x.OnQueryExecutingAsync(It.IsAny<IConnectionDetails>(), It.IsAny<FunctionMetadata>()), Times.Exactly(2));
                    mockPlugin.Verify(x => x.OnQueryExecutedAsync(It.IsAny<IConnectionDetails>(), It.IsAny<IExecutionDetails>(), It.IsAny<FunctionMetadata>()), Times.Exactly(2));
                }
            }

            public class When_there_are_two_connections_are_opened
            {
                [SetUp]
                public async Task SetUp()
                {
                    await DatabaseSetup.SetupAsync();
                }

                [Test]
                public async Task The_connection_plugins_should_be_called_twice()
                {
                    var mockPlugin = new Mock<IDatabasePlugin>();

                    var db = Database.Configure<Sqlite>()
                        .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                        .WithPlugin(() => mockPlugin.Object)
                        .WithPlugin(() => mockPlugin.Object)
                        .Build();

                    using (var conn = await db.OpenAsync())
                    {
                        await conn.ExecuteAsync("SELECT 1");
                    }

                    using (var conn = await db.OpenAsync())
                    {
                        await conn.ExecuteAsync("SELECT 1");
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(100));

                    mockPlugin.Verify(x => x.OnConnectionOpeningAsync(It.IsAny<IConnectionDetails>()), Times.Exactly(4));
                    mockPlugin.Verify(x => x.OnConnectionOpenAsync(It.IsAny<IConnectionDetails>(), It.IsAny<IExecutionDetails>()), Times.Exactly(4));
                    mockPlugin.Verify(x => x.OnQueryExecutingAsync(It.IsAny<IConnectionDetails>(), It.IsAny<FunctionMetadata>()), Times.Exactly(4));
                    mockPlugin.Verify(x => x.OnQueryExecutedAsync(It.IsAny<IConnectionDetails>(), It.IsAny<IExecutionDetails>(), It.IsAny<FunctionMetadata>()), Times.Exactly(4));
                }
            }

            public class When_there_are_two_queries_in_the_same_connection
            {
                [SetUp]
                public async Task Setup()
                {
                    await DatabaseSetup.SetupAsync();
                }

                [Test]
                public async Task The_connection_plugins_should_be_called_once_and_the_query_plugins_should_be_called_twice()
                {
                    var mockPlugin = new Mock<IDatabasePlugin>();

                    var db = Database.Configure<Sqlite>()
                        .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                        .WithPlugin(() => mockPlugin.Object)
                        .WithPlugin(() => mockPlugin.Object)
                        .Build();

                    using (var conn = await db.OpenAsync())
                    {
                        await conn.ExecuteAsync("SELECT 1");
                        await conn.ExecuteAsync("SELECT 1");
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(100));

                    mockPlugin.Verify(x => x.OnConnectionOpeningAsync(It.IsAny<IConnectionDetails>()), Times.Exactly(2));
                    mockPlugin.Verify(x => x.OnConnectionOpenAsync(It.IsAny<IConnectionDetails>(), It.IsAny<IExecutionDetails>()), Times.Exactly(2));
                    mockPlugin.Verify(x => x.OnQueryExecutingAsync(It.IsAny<IConnectionDetails>(), It.IsAny<FunctionMetadata>()), Times.Exactly(4));
                    mockPlugin.Verify(x => x.OnQueryExecutedAsync(It.IsAny<IConnectionDetails>(), It.IsAny<IExecutionDetails>(), It.IsAny<FunctionMetadata>()), Times.Exactly(4));
                }
            }
        }
    }
}
