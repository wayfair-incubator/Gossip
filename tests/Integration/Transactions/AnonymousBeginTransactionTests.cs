using System.Linq;
using System.Threading.Tasks;
using Gossip.Connection;
using Gossip.Connection.Factories;
using Gossip.TestSupport.Setup;
using NUnit.Framework;

namespace Gossip.IntegrationTests.Transactions
{
    [TestFixture]
    public class AnonymousBeginTransactionTests
    {
        public class When_using_a_using_statement
        {
            private const string CreateTable = @"
        CREATE TABLE tblTest(
	        Id INTEGER PRIMARY KEY Identity(1,1),
	        Name [nvarchar](15) NOT NULL
        )";

            [TestFixture]
            public class When_the_transasction_is_committed
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

                    using var conn = await db.OpenAsync();

                    using (var tran = conn.BeginTransaction())
                    {
                        await conn.ExecuteAsync("INSERT INTO tblTest(Name) VALUES ('ABC')");
                        tran.Commit();
                    }

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
            public class When_the_transasction_is_neither_committed_nor_rolled_back
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
                public async Task It_should_roll_back_the_data()
                {
                    var db = Database
                        .Configure<MsSql>()
                        .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.LocalMsSqlConnectionString })
                        .Build();

                    using var conn = await db.OpenAsync();

                    using (conn.BeginTransaction())
                    {
                        await conn.ExecuteAsync("INSERT INTO tblTest(Name) VALUES ('ABC')");
                    }

                    var results = await conn.QueryAsync<Record>("SELECT Id, Name FROM tblTest");
                    Assert.AreEqual(0, results.ToList().Count);
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
            public class When_the_transasction_is_rolledback
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
                public async Task It_should_rollback_the_data()
                {
                    var db = Database
                        .Configure<MsSql>()
                        .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.LocalMsSqlConnectionString })
                        .Build();

                    using var conn = await db.OpenAsync();

                    using (var tran = conn.BeginTransaction())
                    {
                        await conn.ExecuteAsync("INSERT INTO tblTest(Name) VALUES ('ABC')");
                        tran.Rollback();
                    }

                    var results = await conn.QueryAsync<Record>("SELECT Id, Name FROM tblTest");
                    Assert.AreEqual(0, results.ToList().Count);
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
}
