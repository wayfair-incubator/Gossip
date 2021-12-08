using System;
using System.Threading;
using System.Threading.Tasks;
using Gossip.Connection;
using Gossip.Connection.Exceptions;
using Gossip.Connection.Fluent;
using Gossip.Monitoring;
using Gossip.Plugins;
using Gossip.TestSupport.Adapters.Sqlite;
using Gossip.TestSupport.Setup;
using NUnit.Framework;

namespace Gossip.UnitTests.Fallbacks
{
    [TestFixture]
    public class FallbackTests
    {
        public class When_the_primary_connection_string_does_not_work
        {
            [SetUp]
            public async Task Setup()
            {
                await DatabaseSetup.SetupAsync();
            }

            [Test]
            public async Task It_should_fall_back_to_the_next_database()
            {
                var plugin = new FallbackPlugin();

                var connectionProvider = Database.Configure<Sqlite>()
                    .WithConnectionString(() => new ConnectionString { Value = "Something fake" })
                    .WithFallbacks(new Func<Task<IConnectionString>>[]
                    {
                        () => Task.FromResult((IConnectionString)new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                    })
                    .WithPlugin(() => plugin)
                    .Build();

                using var conn = await connectionProvider.OpenAsync();

                var results = await conn.QueryAsync<TableTest>("SELECT * FROM tblTest");
                Thread.Sleep(10); // Wait for plugins to get hit
                var connectionDetails = plugin.GetDatabaseConnectionDetails();
                Assert.AreEqual(DatabaseSetup.SharedConnectionString, connectionDetails.Server);
            }
        }

        public class When_the_secondary_connection_string_does_not_work
        {
            [SetUp]
            public async Task Setup()
            {
                await DatabaseSetup.SetupAsync();
            }

            [Test]
            public async Task It_should_fall_back_to_the_next_database()
            {
                var plugin = new FallbackPlugin();

                var connectionProvider = Database.Configure<Sqlite>()
                    .WithConnectionString(() => new ConnectionString { Value = "Something fake" })
                    .WithFallbacks(new Func<Task<IConnectionString>>[]
                    {
                        () => Task.FromResult((IConnectionString)new ConnectionString { Value = "Another fake one" }),
                        () => Task.FromResult((IConnectionString)new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                    })
                    .WithPlugin(() => plugin)
                    .Build();

                using var conn = await connectionProvider.OpenAsync();

                var results = await conn.QueryAsync<TableTest>("SELECT * FROM tblTest");
                Thread.Sleep(10); // Wait for plugins to get hit
                var connectionDetails = plugin.GetDatabaseConnectionDetails();
                Assert.AreEqual(DatabaseSetup.SharedConnectionString, connectionDetails.Server);
            }
        }

        public class When_none_of_the_connection_strings_work
        {
            private DatabaseConnectionException _exceptionThrown;

            [SetUp]
            public async Task Setup()
            {
                var plugin = new FallbackPlugin();
                var connectionProvider = Database.Configure<Sqlite>()
                    .WithConnectionString(() => new ConnectionString { Value = "Something fake" })
                    .WithFallbacks(new Func<Task<IConnectionString>>[]
                    {
                        () => Task.FromResult((IConnectionString)new ConnectionString { Value = "Another fake one" }),
                        () => Task.FromResult((IConnectionString)new ConnectionString
                            { Value = "Yet another fake one" }),
                    })
                    .WithPlugin(() => plugin)
                    .Build();

                try
                {
                    await connectionProvider.OpenAsync();
                }
                catch (DatabaseConnectionException ex)
                {
                    _exceptionThrown = ex;
                }
            }

            [Test]
            public void It_should_throw_a_DatabaseConnectionException()
            {
                Assert.IsNotNull(_exceptionThrown);
            }

            [Test]
            public void It_should_have_an_inner_exception()
            {
                Assert.IsNotNull(_exceptionThrown.InnerException);
            }
        }
    }

    public class TableTest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Grouping { get; set; }
    }

    public class FallbackPlugin : IDatabasePlugin
    {
        private IConnectionDetails _connectionDetails;

        public IConnectionDetails GetDatabaseConnectionDetails()
        {
            return _connectionDetails;
        }

        public void OnBuild(UsageDetails usageDetails) { }

        public Task OnQueryExecutingAsync(IConnectionDetails connectionDetails, FunctionMetadata metadata)
        {
            _connectionDetails = connectionDetails;

            return Task.CompletedTask;
        }

        public Task OnQueryExecutedAsync(
            IConnectionDetails connectionDetails,
            IExecutionDetails executionDetails,
            FunctionMetadata metadata) =>
            Task.CompletedTask;

        public Task OnConnectionOpeningAsync(IConnectionDetails connectionDetails) => Task.CompletedTask;

        public Task OnConnectionOpenAsync(IConnectionDetails connectionDetails, IExecutionDetails executionDetails) => Task.CompletedTask;

        public Task OnConnectionExceptionAsync(IConnectionDetails connectionDetails) => Task.CompletedTask;

        public Task OnDatabaseResolutionExceptionAsync(string database) => Task.CompletedTask;

        public Task OnDatabaseMonitorExecutedAsync(IDatabaseMonitorReport databaseMonitorReport) => Task.CompletedTask;
    }
}
