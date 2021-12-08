using System.Linq;
using System.Threading.Tasks;
using Gossip.Connection;
using Gossip.TestSupport.Adapters.Sqlite;
using Gossip.TestSupport.Setup;
using NUnit.Framework;

namespace Gossip.UnitTests.Queries
{
    [TestFixture]
    public class MultiMappingTests
    {
        [SetUp]
        public async Task Setup()
        {
            await DatabaseSetup.SetupAsync();
        }

        [Test]
        public async Task Multimapping_of_two_tables_should_be_supported()
        {
            var connectionProvider = Database.Configure<Sqlite>()
                .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                .Build();

            using var conn = await connectionProvider.OpenAsync();

            var results = (await conn.Configure()
                .WithQuery("SELECT * FROM tblUser LEFT JOIN tblUserSecurity ON tblUser.Id = tblUserSecurity.UserId")
                .Build()
                .QueryAsync<User, UserSecurity, User>((a, b) =>
                {
                    a.Security = b;
                    return a;
                })).ToList();

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(results.First().Name, "Michael Jordan");
            Assert.AreEqual(results.First().Security.Password, "chicagobulls");
            Assert.AreEqual(results.Last().Name, "LeBron James");
            Assert.AreEqual(results.Last().Security.Password, "lalakers");
        }

        [Test]
        public async Task Multimapping_of_three_tables_should_be_supported()
        {
            var connectionProvider = Database.Configure<Sqlite>()
                .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                .Build();

            using var conn = await connectionProvider.OpenAsync();

            var results = (await conn.Configure()
                .WithQuery(@"
SELECT * 
FROM tblUser 
LEFT JOIN tblUserSecurity ON tblUser.Id = tblUserSecurity.UserId
LEFT JOIN tblUserFavoriteNumber ON tblUser.Id = tblUserFavoriteNumber.UserId")
                .Build()
                .QueryAsync<User, UserSecurity, UserFavoriteNumber, User>((a, b, c) =>
                {
                    a.Security = b;
                    a.FavoriteNumber = c;
                    return a;
                })).ToList();

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(results.First().Name, "Michael Jordan");
            Assert.AreEqual(results.First().Security.Password, "chicagobulls");
            Assert.AreEqual(results.First().FavoriteNumber.Number, 23);
            Assert.AreEqual(results.Last().Name, "LeBron James");
            Assert.AreEqual(results.Last().Security.Password, "lalakers");
            Assert.AreEqual(results.Last().FavoriteNumber.Number, 6);
        }

        [Test]
        public async Task Multimapping_of_four_tables_should_be_supported()
        {
            var connectionProvider = Database.Configure<Sqlite>()
                .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                .Build();

            using var conn = await connectionProvider.OpenAsync();

            var results = (await conn.Configure()
                .WithQuery(@"
SELECT * 
FROM tblUser 
LEFT JOIN tblUserSecurity ON tblUser.Id = tblUserSecurity.UserId
LEFT JOIN tblUserFavoriteNumber ON tblUser.Id = tblUserFavoriteNumber.UserId
LEFT JOIN tblUserAddress ON tblUser.Id = tblUserAddress.UserId
")
                .Build()
                .QueryAsync<User, UserSecurity, UserFavoriteNumber, UserAddress, User>((a, b, c, d) =>
                {
                    a.Security = b;
                    a.FavoriteNumber = c;
                    a.Address = d;
                    return a;
                })).ToList();

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(results.First().Name, "Michael Jordan");
            Assert.AreEqual(results.First().Security.Password, "chicagobulls");
            Assert.AreEqual(results.First().FavoriteNumber.Number, 23);
            Assert.AreEqual(results.First().Address.Address, "1 Chicago Place");
            Assert.AreEqual(results.Last().Name, "LeBron James");
            Assert.AreEqual(results.Last().Security.Password, "lalakers");
            Assert.AreEqual(results.Last().FavoriteNumber.Number, 6);
            Assert.AreEqual(results.Last().Address.Address, "1 LA Way");
        }

        [Test]
        public async Task Multimapping_of_five_tables_should_be_supported()
        {
            var connectionProvider = Database.Configure<Sqlite>()
                .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                .Build();

            using var conn = await connectionProvider.OpenAsync();

            var results = (await conn.Configure()
                .WithQuery(@"
SELECT * 
FROM tblUser 
LEFT JOIN tblUserSecurity ON tblUser.Id = tblUserSecurity.UserId
LEFT JOIN tblUserFavoriteNumber ON tblUser.Id = tblUserFavoriteNumber.UserId
LEFT JOIN tblUserAddress ON tblUser.Id = tblUserAddress.UserId
LEFT JOIN tblUserVacationHouse ON tblUser.Id = tblUserVacationHouse.UserId
")
                .Build()
                .QueryAsync<User, UserSecurity, UserFavoriteNumber, UserAddress, UserVacationHouse, User>((a, b, c, d, e) =>
                {
                    a.Security = b;
                    a.FavoriteNumber = c;
                    a.Address = d;
                    a.VacationHouse = e;
                    return a;
                })).ToList();

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(results.First().Name, "Michael Jordan");
            Assert.AreEqual(results.First().Security.Password, "chicagobulls");
            Assert.AreEqual(results.First().FavoriteNumber.Number, 23);
            Assert.AreEqual(results.First().Address.Address, "1 Chicago Place");
            Assert.AreEqual(results.First().VacationHouse.Color, "Red");
            Assert.AreEqual(results.Last().Name, "LeBron James");
            Assert.AreEqual(results.Last().Security.Password, "lalakers");
            Assert.AreEqual(results.Last().FavoriteNumber.Number, 6);
            Assert.AreEqual(results.Last().Address.Address, "1 LA Way");
            Assert.AreEqual(results.Last().VacationHouse.Color, "Yellow");
        }

        [Test]
        public async Task Multimapping_of_six_tables_should_be_supported()
        {
            var connectionProvider = Database.Configure<Sqlite>()
                .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                .Build();

            using var conn = await connectionProvider.OpenAsync();

            var results = (await conn.Configure()
                .WithQuery(@"
SELECT * 
FROM tblUser 
LEFT JOIN tblUserSecurity ON tblUser.Id = tblUserSecurity.UserId
LEFT JOIN tblUserFavoriteNumber ON tblUser.Id = tblUserFavoriteNumber.UserId
LEFT JOIN tblUserAddress ON tblUser.Id = tblUserAddress.UserId
LEFT JOIN tblUserVacationHouse ON tblUser.Id = tblUserVacationHouse.UserId
LEFT JOIN tblUserCar ON tblUser.Id = tblUserCar.UserId
")
                .Build()
                .QueryAsync<User, UserSecurity, UserFavoriteNumber, UserAddress, UserVacationHouse, UserCar, User>((a, b, c, d, e, f) =>
                {
                    a.Security = b;
                    a.FavoriteNumber = c;
                    a.Address = d;
                    a.VacationHouse = e;
                    a.Car = f;
                    return a;
                })).ToList();

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(results.First().Name, "Michael Jordan");
            Assert.AreEqual(results.First().Security.Password, "chicagobulls");
            Assert.AreEqual(results.First().FavoriteNumber.Number, 23);
            Assert.AreEqual(results.First().Address.Address, "1 Chicago Place");
            Assert.AreEqual(results.First().VacationHouse.Color, "Red");
            Assert.AreEqual(results.First().Car.Make, "Ferrari");
            Assert.AreEqual(results.Last().Name, "LeBron James");
            Assert.AreEqual(results.Last().Security.Password, "lalakers");
            Assert.AreEqual(results.Last().FavoriteNumber.Number, 6);
            Assert.AreEqual(results.Last().Address.Address, "1 LA Way");
            Assert.AreEqual(results.Last().VacationHouse.Color, "Yellow");
            Assert.AreEqual(results.Last().Car.Make, "Tesla");
        }

        [Test]
        public async Task Multimapping_of_seven_tables_should_be_supported()
        {
            var connectionProvider = Database.Configure<Sqlite>()
                .WithConnectionString(() => new ConnectionString { Value = DatabaseSetup.SharedConnectionString })
                .Build();

            using var conn = await connectionProvider.OpenAsync();

            var results = (await conn.Configure()
                .WithQuery(@"
SELECT * 
FROM tblUser 
LEFT JOIN tblUserSecurity ON tblUser.Id = tblUserSecurity.UserId
LEFT JOIN tblUserFavoriteNumber ON tblUser.Id = tblUserFavoriteNumber.UserId
LEFT JOIN tblUserAddress ON tblUser.Id = tblUserAddress.UserId
LEFT JOIN tblUserVacationHouse ON tblUser.Id = tblUserVacationHouse.UserId
LEFT JOIN tblUserCar ON tblUser.Id = tblUserCar.UserId
LEFT JOIN tblUserPet ON tblUser.Id = tblUserPet.UserId
")
                .Build()
                .QueryAsync<User, UserSecurity, UserFavoriteNumber, UserAddress, UserVacationHouse, UserCar, UserPet, User>((a, b, c, d, e, f, g) =>
                {
                    a.Security = b;
                    a.FavoriteNumber = c;
                    a.Address = d;
                    a.VacationHouse = e;
                    a.Car = f;
                    a.Pet = g;
                    return a;
                })).ToList();

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(results.First().Name, "Michael Jordan");
            Assert.AreEqual(results.First().Security.Password, "chicagobulls");
            Assert.AreEqual(results.First().FavoriteNumber.Number, 23);
            Assert.AreEqual(results.First().Address.Address, "1 Chicago Place");
            Assert.AreEqual(results.First().VacationHouse.Color, "Red");
            Assert.AreEqual(results.First().Car.Make, "Ferrari");
            Assert.AreEqual(results.First().Pet.Type, "Dog");
            Assert.AreEqual(results.Last().Name, "LeBron James");
            Assert.AreEqual(results.Last().Security.Password, "lalakers");
            Assert.AreEqual(results.Last().FavoriteNumber.Number, 6);
            Assert.AreEqual(results.Last().Address.Address, "1 LA Way");
            Assert.AreEqual(results.Last().VacationHouse.Color, "Yellow");
            Assert.AreEqual(results.Last().Car.Make, "Tesla");
            Assert.AreEqual(results.Last().Pet.Type, "Cat");
        }

        public class UserPet
        {
            public string Type { get; set; }
        }

        public class UserCar
        {
            public string Make { get; set; }
        }

        public class UserVacationHouse
        {
            public string Color { get; set; }
        }

        public class UserAddress
        {
            public string Address { get; set; }
        }

        private class User
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public UserSecurity Security { get; set; }
            public UserFavoriteNumber FavoriteNumber { get; set; }
            public UserAddress Address { get; set; }
            public UserVacationHouse VacationHouse { get; set; }
            public UserCar Car { get; set; }
            public UserPet Pet { get; set; }
        }

        private class UserSecurity
        {
            public string Id { get; set; }
            public string UserId { get; set; }
            public string Password { get; set; }
        }

        private class UserFavoriteNumber
        {
            public string Id { get; set; }
            public string UserId { get; set; }
            public int Number { get; set; }
        }
    }
}
