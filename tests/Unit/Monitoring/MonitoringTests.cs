using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gossip.Connection;
using Gossip.Connection.Fluent;
using Gossip.Monitoring;
using Gossip.Plugins;
using Gossip.TestSupport.Adapters.Sqlite;
using Gossip.TestSupport.Setup;
using NUnit.Framework;

namespace Gossip.UnitTests.Monitoring
{
    [TestFixture]
    public class MonitoringTests
    {
        public class When_monitoring_is_enabled
        {
            [Test]
            public async Task It_should_have_a_list_of_database_to_connect_to_ordered_by_priority()
            {
                await DatabaseSetup.SetupAsync();
                var plugin = new MonitoringPlugin();

                var db = Database.Configure<Sqlite>()
                    .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString, Server = "primary" })
                    .WithFallbacks(new List<Func<Task<IConnectionString>>>
                    {
                        () => Task.FromResult((IConnectionString)new ConnectionString { Value = "backup1", Server = "backup1" }),
                        () => Task.FromResult((IConnectionString)new ConnectionString { Value = "backup2", Server = "backup2" })
                    })
                    .WithMonitoring(TimeSpan.FromMinutes(1))
                    .WithPlugin(() => plugin)
                    .Build();

                using (var conn = await db.OpenAsync())
                {
                    await conn.ExecuteAsync("SELECT 1");
                }

                var report = plugin.GetReport();
                var connectionDetails = report.LastDatabaseMonitorReport.ConnectionDetails;

                Assert.AreEqual(3, connectionDetails.Count());
                Assert.AreEqual("primary", connectionDetails.First().Server);
                Assert.AreEqual("backup1", connectionDetails.Skip(1).First().Server);
                Assert.AreEqual("backup2", connectionDetails.Skip(2).First().Server);
            }

            [Test]
            public async Task It_should_call_the_monitoring_on_the_specified_interval()
            {
                await DatabaseSetup.SetupAsync();
                var plugin = new MonitoringPlugin();

                var db = Database.Configure<Sqlite>()
                    .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString, Server = "primary" })
                    .WithFallbacks(new List<Func<Task<IConnectionString>>>
                    {
                        () => Task.FromResult((IConnectionString)new ConnectionString { Value = "backup1", Server = "backup1" }),
                        () => Task.FromResult((IConnectionString)new ConnectionString { Value = "backup2", Server = "backup2" })
                    })
                    .WithMonitoring(TimeSpan.FromMilliseconds(10))
                    .WithPlugin(() => plugin)
                    .Build();

                Thread.Sleep(100);

                var report = plugin.GetReport();
                Assert.GreaterOrEqual(report.NumberOfTimesMonitorExecuted, 5);
            }
        }

        public class When_monitoring_is_not_enabled
        {
            [Test]
            public async Task It_should_not_call_the_monitoring_plugins()
            {
                await DatabaseSetup.SetupAsync();
                var plugin = new MonitoringPlugin();

                var db = Database.Configure<Sqlite>()
                    .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString, Server = "primary" })
                    .WithFallbacks(new List<Func<Task<IConnectionString>>>
                    {
                        () => Task.FromResult((IConnectionString)new ConnectionString { Value = "backup1", Server = "backup1" }),
                        () => Task.FromResult((IConnectionString)new ConnectionString { Value = "backup2", Server = "backup2" })
                    })
                    .WithPlugin(() => new MonitoringPlugin())
                    .Build();

                Thread.Sleep(100);

                var report = plugin.GetReport();
                Assert.AreEqual(report.NumberOfTimesMonitorExecuted, 0);
            }
        }
    }

    internal class MonitoringPlugin : IDatabasePlugin
    {
        private int _timesMonitorExecuted;
        private IDatabaseMonitorReport _lastReport;

        public MonitoringPlugin()
        {
            _timesMonitorExecuted = 0;
            _lastReport = null;
        }

        public MonitoringPluginReport GetReport()
        {
            return new MonitoringPluginReport
            {
                LastDatabaseMonitorReport = _lastReport,
                NumberOfTimesMonitorExecuted = _timesMonitorExecuted
            };
        }

        public void OnBuild(UsageDetails usageDetails)
        {
        }

        public Task OnQueryExecutingAsync(IConnectionDetails connectionDetails, FunctionMetadata metadata) => Task.CompletedTask;

        public Task OnQueryExecutedAsync(
            IConnectionDetails connectionDetails,
            IExecutionDetails executionDetails,
            FunctionMetadata metadata) =>
            Task.CompletedTask;

        public Task OnConnectionOpeningAsync(IConnectionDetails connectionDetails) => Task.CompletedTask;

        public Task OnConnectionOpenAsync(IConnectionDetails connectionDetails, IExecutionDetails executionDetails) => Task.CompletedTask;

        public Task OnConnectionExceptionAsync(IConnectionDetails connectionDetails) => Task.CompletedTask;

        public Task OnDatabaseResolutionExceptionAsync(string database) => Task.CompletedTask;

        public Task OnDatabaseMonitorExecutedAsync(IDatabaseMonitorReport databaseMonitorReport)
        {
            _timesMonitorExecuted++;
            _lastReport = databaseMonitorReport;

            return Task.CompletedTask;
        }
    }

    internal class MonitoringPluginReport
    {
        public IDatabaseMonitorReport LastDatabaseMonitorReport { get; set; }

        public int NumberOfTimesMonitorExecuted { get; set; }
    }
}
