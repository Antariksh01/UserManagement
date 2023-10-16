using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services.UserService
{
    public interface IUserService
    {
        public List<User> GetUsers();
        IEnumerable<User> GetUsersByFilter(string filter);
        public Task<List<User>> AddUser(User user);
        public List<User> UpdateUser(User user);
        public ServiceResponse<List<User>> DeleteUser(int userId);
    }
}
