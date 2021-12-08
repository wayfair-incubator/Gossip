using System;
using System.Threading.Tasks;
using Gossip.Connection;
using Gossip.ConnectionStrings;
using Gossip.TestSupport.Adapters.Sqlite;

namespace Gossip.TestSupport.Setup
{
    public class DatabaseSetup
    {
        [ThreadStatic] public static string SharedConnectionString;

        public static string GetDatabaseServerAddress()
        {
            var databaseServerName = Environment.GetEnvironmentVariable("DB_SERVER_NAME") ?? "localhost";
            var databaseServerPort = Environment.GetEnvironmentVariable("DB_SERVER_PORT") ?? "11433";

            return $"{databaseServerName},{databaseServerPort}";
        }

        public static string LocalMsSqlConnectionString
        {
            get
            {
                var mssqlConnectionStringBuilder = new MsSqlConnectionStringBuilder();
                var databaseServerAddress = GetDatabaseServerAddress();
                return mssqlConnectionStringBuilder.Build(new ConnectionStringSettings
                {
                    ApplicationName = "Test",
                    Database = "master",
                    MachineName = "Test",
                    Server = databaseServerAddress,
                    Password = "55Data!Access!Password!",
                    Username = "sa",
                    MaxPoolSize = 50,
                    DefaultCommandTimeout = 5
                });
            }
        }

        private const string CreateTestTable = @"
    CREATE TABLE if not exists tblTest(
	    Id [nvarchar](8) NOT NULL,	    
	    Name [nvarchar](50) NOT NULL,
        Grouping [nvarchar](50) NOT NULL
    )";

        private const string CreateUserTable = @"
    CREATE TABLE if not exists tblUser(
	    Id [nvarchar](8) NOT NULL,	    
	    Name [nvarchar](50) NOT NULL        
    )";

        private const string CreateUserSecurityTable = @"
    CREATE TABLE if not exists tblUserSecurity(
	    Id [nvarchar](8) NOT NULL,	    
        UserId [nvarchar](8) NOT NULL,
	    Password [nvarchar](50) NOT NULL        
    )";

        private const string CreateUserFavoriteNumberTable = @"
    CREATE TABLE if not exists tblUserFavoriteNumber(
	    Id [nvarchar](8) NOT NULL,	    
        UserId [nvarchar](8) NOT NULL,
	    Number [int] NOT NULL        
    )";

        private const string CreateUserAddressTable = @"
    CREATE TABLE if not exists tblUserAddress(
	    Id [nvarchar](8) NOT NULL,	    
        UserId [nvarchar](8) NOT NULL,
	    Address [nvarchar](255) NOT NULL        
    )";

        private const string CreateUserVacationHouseTable = @"
    CREATE TABLE if not exists tblUserVacationHouse(
	    Id [nvarchar](8) NOT NULL,	    
        UserId [nvarchar](8) NOT NULL,
	    Color [nvarchar](255) NOT NULL        
    )";

        private const string CreateUserCarTable = @"
    CREATE TABLE if not exists tblUserCar(
	    Id [nvarchar](8) NOT NULL,	    
        UserId [nvarchar](8) NOT NULL,
	    Make [nvarchar](255) NOT NULL
    )";

        private const string CreateUserPetTable = @"
    CREATE TABLE if not exists tblUserPet(
	    Id [nvarchar](8) NOT NULL,	    
        UserId [nvarchar](8) NOT NULL,
	    Type [nvarchar](255) NOT NULL
    )";

        private const string InsertData = @"
INSERT INTO tblTest(Id, Name, Grouping) VALUES ('SomeID', 'Testing', 'OneRow');
INSERT INTO tblTest(Id, Name, Grouping) VALUES ('SomeID2', 'FirstRecord', 'TwoRows');
INSERT INTO tblTest(Id, Name, Grouping) VALUES ('SomeID3', 'SecondRecord', 'TwoRows');
INSERT INTO tblTest(Id, Name, Grouping) VALUES ('TenRows1', 'FirstRecord', 'TenRows');
INSERT INTO tblTest(Id, Name, Grouping) VALUES ('TenRows2', 'SecondRecord', 'TenRows');
INSERT INTO tblTest(Id, Name, Grouping) VALUES ('TenRows3', 'ThirdRecord', 'TenRows');
INSERT INTO tblTest(Id, Name, Grouping) VALUES ('TenRows4', 'FourthRecord', 'TenRows');
INSERT INTO tblTest(Id, Name, Grouping) VALUES ('TenRows5', 'FifthRecord', 'TenRows');
INSERT INTO tblTest(Id, Name, Grouping) VALUES ('TenRows6', 'SixthRecord', 'TenRows');
INSERT INTO tblTest(Id, Name, Grouping) VALUES ('TenRows7', 'SeventhRecord', 'TenRows');
INSERT INTO tblTest(Id, Name, Grouping) VALUES ('TenRows8', 'EighthRecord', 'TenRows');
INSERT INTO tblTest(Id, Name, Grouping) VALUES ('TenRows9', 'NinthRecord', 'TenRows');
INSERT INTO tblTest(Id, Name, Grouping) VALUES ('TenRows10', 'TenthRecord', 'TenRows');
INSERT INTO tblUser(Id, Name) VALUES ('123', 'Michael Jordan');
INSERT INTO tblUser(Id, Name) VALUES ('234', 'LeBron James');
INSERT INTO tblUserSecurity(Id, UserId, Password) VALUES ('abc', '123', 'chicagobulls');
INSERT INTO tblUserSecurity(Id, UserId, Password) VALUES ('def', '234', 'lalakers');
INSERT INTO tblUserFavoriteNumber(Id, UserId, Number) VALUES ('aaa', '123', 23);
INSERT INTO tblUserFavoriteNumber(Id, UserId, Number) VALUES ('bbb', '234', 6);
INSERT INTO tblUserAddress(Id, UserId, Address) VALUES ('aaa', '123', '1 Chicago Place');
INSERT INTO tblUserAddress(Id, UserId, Address) VALUES ('bbb', '234', '1 LA Way');
INSERT INTO tblUserVacationHouse(Id, UserId, Color) VALUES ('aaa', '123', 'Red');
INSERT INTO tblUserVacationHouse(Id, UserId, Color) VALUES ('aaa', '234', 'Yellow');
INSERT INTO tblUserCar(Id, UserId, Make) VALUES ('aaa', '123', 'Ferrari');
INSERT INTO tblUserCar(Id, UserId, Make) VALUES ('bbb', '234', 'Tesla');
INSERT INTO tblUserPet(Id, UserId, Type) VALUES ('bbb', '123', 'Dog');
INSERT INTO tblUserPet(Id, UserId, Type) VALUES ('bbb', '234', 'Cat');
";

        public static async Task SetupAsync()
        {
            SharedConnectionString = $"DataSource={Guid.NewGuid()};mode=memory;cache=shared";

            var setupDb = Database
                .Configure<Sqlite>()
                .WithConnectionString(() => new ConnectionString { Value = SharedConnectionString })
                .Build();

            using var conn = await setupDb.OpenAsync();

            await conn.ExecuteAsync(CreateTestTable);
            await conn.ExecuteAsync(CreateUserTable);
            await conn.ExecuteAsync(CreateUserSecurityTable);
            await conn.ExecuteAsync(CreateUserFavoriteNumberTable);
            await conn.ExecuteAsync(CreateUserAddressTable);
            await conn.ExecuteAsync(CreateUserVacationHouseTable);
            await conn.ExecuteAsync(CreateUserCarTable);
            await conn.ExecuteAsync(CreateUserPetTable);
            await conn.ExecuteAsync(InsertData);
        }
    }
}
