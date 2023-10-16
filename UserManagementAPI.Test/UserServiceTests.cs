using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.Models;
using UserManagementAPI.Services.UserService;

namespace UserManagementAPI.Test
{
    [TestFixture]
    public class UserServiceTests
    {
        private DbContextOptions<DataContext> _options;

        [SetUp]
        public void SetUp()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new DataContext(_options))
            {
                context.Database.EnsureCreated();
                // Additional setup code if needed
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var context = new DataContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }

        [Test]
        public void GetUsers_ShouldReturnListOfUsers_WhenUsersExist()
        {

            using (var context = new DataContext(_options))
            {
                context.Users.Add(new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Notes = "Notes1", CreationTime = DateTime.Now });
                context.Users.Add(new User { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", Notes = "Notes2", CreationTime = DateTime.Now });
                context.SaveChanges();
            }

            using (var context = new DataContext(_options))
            {
                var userService = new UserService(context);

                // Act
                var users = userService.GetUsers();

                // Assert
                Assert.NotNull(users);
                Assert.AreEqual(2, users.Count);
                Assert.AreEqual("John", users[0].FirstName);
                Assert.AreEqual("Jane", users[1].FirstName);
            }
        }

        [Test]
        public async Task AddUser_ShouldReturnUpdatedUserList_WhenUserIsAdded()
        {

            using (var context = new DataContext(_options))
            {
                var userService = new UserService(context);
                var userToAdd = new User { Id = 1, FirstName = "Antariksh", LastName = "Sharma", Email = "user.sharma@example.com", Notes = "My Notes", CreationTime = DateTime.Now };

                // Act
                var updatedUserList = await userService.AddUser(userToAdd);

                // Assert
                Assert.NotNull(updatedUserList);
                Assert.AreEqual(1, updatedUserList.Count);
                Assert.AreEqual("Antariksh", updatedUserList[0].FirstName);
            }
        }

        [Test]
        public void UpdateUser_ShouldReturnUpdatedUserList_WhenUserExists()
        {

            using (var context = new DataContext(_options))
            {
                // Add a user to the database
                var initialUser = new User { Id = 1, FirstName = "Roger", LastName = "Federer", Email = "r.federer@example.com", Notes = "Grass Lover", CreationTime = DateTime.Now };
                context.Users.Add(initialUser);
                context.SaveChanges();

                var userService = new UserService(context);
                var updatedUserData = new User { Id = 1, FirstName = "Rafael", LastName = "Nadal", Email = "r.nadal@example.com", Notes = "Clay King", CreationTime = DateTime.Now };

                // Act
                var updatedUserList = userService.UpdateUser(updatedUserData);

                // Assert
                Assert.NotNull(updatedUserList);
                Assert.AreEqual(1, updatedUserList.Count);
                Assert.AreEqual("Rafael", updatedUserList[0].FirstName);
                Assert.AreEqual("Nadal", updatedUserList[0].LastName);
                Assert.AreEqual("r.nadal@example.com", updatedUserList[0].Email);
            }
        }

        [Test]
        public void UpdateUser_ShouldReturnNull_WhenUserDoesNotExist()
        {

            using (var context = new DataContext(_options))
            {
                var userService = new UserService(context);
                var nonExistingUserData = new User { Id = 1, FirstName = "Roger", LastName = "Federer", Email = "r.federer@example.com", Notes = "Classic Tour", CreationTime = DateTime.Now };

                // Act
                var updatedUserList = userService.UpdateUser(nonExistingUserData);

                // Assert
                Assert.Null(updatedUserList);
            }
        }

        [Test]
        public void DeleteUser_ShouldReturnUpdatedUserList_WhenUserExists()
        {

            using (var context = new DataContext(_options))
            {
                // Add a user to the database
                var initialUser = new User { Id = 1, FirstName = "Stephen", LastName = "Curry", Email = "stephen.curry@example.com", Notes = "Curry Notes", CreationTime = DateTime.Now };
                context.Users.Add(initialUser);
                context.SaveChanges();

                var userService = new UserService(context);

                // Act
                var serviceResponse = userService.DeleteUser(1);

                // Assert
                Assert.NotNull(serviceResponse);
                Assert.NotNull(serviceResponse.data);
                Assert.AreEqual(0, serviceResponse.data.Count);
                Assert.AreEqual(string.Empty, serviceResponse.Message);
            }
        }

        [Test]
        public void DeleteUser_ShouldReturnNotFoundResponse_WhenUserDoesNotExist()
        {

            using (var context = new DataContext(_options))
            {
                var userService = new UserService(context);

                // Act
                var serviceResponse = userService.DeleteUser(1);

                // Assert
                Assert.NotNull(serviceResponse);
                Assert.IsNull(serviceResponse.data);
                Assert.AreEqual("User Not Found", serviceResponse.Message);
            }
        }


        [Test]
        public void GetUsersByFilter_ShouldReturnFilteredUsers_WhenFilterIsProvided()
        {

            using (var context = new DataContext(_options))
            {
                // Add users to the database
                var user1 = new User { Id = 1, FirstName = "Johny", LastName = "Walker", Email = "Johny.Walker@example.com", Notes = "Note1", CreationTime = DateTime.Now };
                var user2 = new User { Id = 2, FirstName = "White", LastName = "Walker", Email = "W.Walker@example.com", Notes = "Note2", CreationTime = DateTime.Now };
                var user3 = new User { Id = 3, FirstName = "Jack", LastName = "Daniels", Email = "Jack.Daniels@example.com", Notes = "Note3", CreationTime = DateTime.Now };

                context.Users.AddRange(user1, user2, user3);
                context.SaveChanges();

                var userService = new UserService(context);

                // Act
                var filteredUsers = userService.GetUsersByFilter("Walker");

                // Assert
                Assert.NotNull(filteredUsers);
                Assert.AreEqual(2, filteredUsers.Count());
                Assert.AreEqual("Johny", filteredUsers.ElementAt(0).FirstName);
                Assert.AreEqual("White", filteredUsers.ElementAt(1).FirstName);
            }
        }

        [Test]
        public void GetUsersByFilter_ShouldReturnAllUsers_WhenNoFilterIsProvided()
        {

            using (var context = new DataContext(_options))
            {
                // Add users to the database
                var user1 = new User { Id = 1, FirstName = "Johny", LastName = "Walker", Email = "Johny.Walker@example.com", Notes = "Note1", CreationTime = DateTime.Now };
                var user2 = new User { Id = 2, FirstName = "White", LastName = "Walker", Email = "W.Walker@example.com", Notes = "Note2", CreationTime = DateTime.Now };
                var user3 = new User { Id = 3, FirstName = "Jack", LastName = "Daniels", Email = "Jack.Daniels@example.com", Notes = "Note3", CreationTime = DateTime.Now };

                context.Users.AddRange(user1, user2, user3);
                context.SaveChanges();

                var userService = new UserService(context);

                // Act
                var allUsers = userService.GetUsersByFilter(string.Empty);

                // Assert
                Assert.NotNull(allUsers);
                Assert.AreEqual(3, allUsers.Count());
            }
        }
    }
}
