using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Gossip.Connection;
using Gossip.Connection.Factories;
using Gossip.TestSupport.Setup;
using NUnit.Framework;

namespace Gossip.IntegrationTests.Transactions
{
    [TestFixture]
    public class TransactionScopeTests
    {
        public class When_a_transaction_enabled
        {
            private const string CreateTable = @"
        CREATE TABLE tblTest(
	        Id INTEGER PRIMARY KEY Identity(1,1),
	        Name [nvarchar](15) NOT NULL
        )";

            [TestFixture]
            public class When_no_rollback_is_needed
            {
                [SetUp]
                public async Task Setup()
                {
                    var db = Database
                        .Configure<MsSql>()
                        .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.LocalMsSqlConnectionString })
                        .Build();

                    using var conn = await db.OpenAsync();

                    await conn.ExecuteAsync(CreateTable);
                }

                [Test]
                public async Task It_should_commit_the_data()
                {
                    var db = Database
                        .Configure<MsSql>()
                        .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.LocalMsSqlConnectionString })
                        .Build();

                    using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                    using var conn = await db.OpenAsync();

                    await conn.ExecuteAsync("INSERT INTO tblTest(Name) VALUES ('ABC')");
                    scope.Complete();
                    var results = await conn.QueryAsync<Record>("SELECT Id, Name FROM tblTest");
                    Assert.AreEqual(1, results.ToList().Count);
                }

                [TearDown]
                public async Task TearDown()
                {
                    var db = Database
                        .Configure<MsSql>()
                        .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.LocalMsSqlConnectionString })
                        .Build();

                    using var conn = await db.OpenAsync();

                    await conn.ExecuteAsync("DROP TABLE tblTest");
                }
            }

            [TestFixture]
            public class When_an_connection_is_opened_after_TransactionScope_and_rollback_is_called
            {
                [SetUp]
                public async Task Setup()
                {
                    var db = Database
                        .Configure<MsSql>()
                        .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.LocalMsSqlConnectionString })
                        .Build();

                    using var conn = await db.OpenAsync();

                    await conn.ExecuteAsync(CreateTable);
                }

                [Test]
                public async Task It_should_not_commit_the_data()
                {
                    var db = Database
                        .Configure<MsSql>()
                        .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.LocalMsSqlConnectionString })
                        .Build();

                    var conn = await db.OpenAsync();

                    try
                    {
                        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                        conn.EnlistTransaction(Transaction.Current);
                        await conn.ExecuteAsync("INSERT INTO tblTest(Name) VALUES ('ABC')");
                        throw new Exception("Something bad happened");
                    }
                    catch (Exception)
                    {
                    }

                    var results = await conn.QueryAsync<Record>("SELECT Id, Name FROM tblTest");
                    Assert.AreEqual(0, results.ToList().Count);
                    conn.Dispose();
                }

                [TearDown]
                public async Task TearDown()
                {
                    var db = Database
                        .Configure<MsSql>()
                        .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.LocalMsSqlConnectionString })
                        .Build();

                    using var conn = await db.OpenAsync();

                    await conn.ExecuteAsync("DROP TABLE tblTest");
                }
            }

            [TestFixture]
            public class When_an_connection_is_opened_within_TransactionScope_and_rollback_is_called
            {
                [SetUp]
                public async Task Setup()
                {
                    var db = Database
                        .Configure<MsSql>()
                        .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.LocalMsSqlConnectionString })
                        .Build();

                    using var conn = await db.OpenAsync();

                    await conn.ExecuteAsync(CreateTable);
                }

                [Test]
                public async Task It_should_not_commit_the_data()
                {
                    var db = Database
                        .Configure<MsSql>()
                        .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.LocalMsSqlConnectionString })
                        .Build();

                    try
                    {
                        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                        using var conn = await db.OpenAsync();

                        var c = new SqlConnection();
                        await conn.ExecuteAsync("INSERT INTO tblTest(Name) VALUES ('ABC')");
                        throw new Exception("Something bad happened");
                    }
                    catch (Exception)
                    {
                    }

                    using (var conn = await db.OpenAsync())
                    {
                        var results = await conn.QueryAsync<Record>("SELECT Id, Name FROM tblTest");
                        Assert.AreEqual(0, results.ToList().Count);
                    }
                }

                [TearDown]
                public async Task TearDown()
                {
                    var db = Database
                        .Configure<MsSql>()
                        .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.LocalMsSqlConnectionString })
                        .Build();

                    using var conn = await db.OpenAsync();

                    await conn.ExecuteAsync("DROP TABLE tblTest");
                }
            }
        }
    }

    public class Record
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
