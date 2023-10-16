using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Data;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        public UserService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<User>> AddUser(User user)
        {
            await _dataContext.Users.AddAsync(user);
            _dataContext.SaveChanges();
            return _dataContext.Users.ToList();
        }

        public List<User> GetUsers()
        {
            return _dataContext.Users.ToList();
        }

        public IEnumerable<User> GetUsersByFilter(string filter)
        {
            IQueryable<User> query = _dataContext.Users.AsQueryable();
            //Apply filter if provided
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(u =>
                u.FirstName.Contains(filter) ||
                u.LastName.Contains(filter) ||
                u.Email.Contains(filter) ||
                u.Notes.Contains(filter)
            );
            }
            var users = query.ToList();
            return users;

        }

        public List<User> UpdateUser(User user)
        {
            var userResult = _dataContext.Users.FirstOrDefault(u => u.Id == user.Id);
            if (userResult != null)
            {
                userResult.FirstName = user.FirstName;
                userResult.LastName = user.LastName;
                userResult.Email = user.Email;
                userResult.Notes = user.Notes;

                _dataContext.SaveChanges();

                return (_dataContext.Users.ToList());
            }
            else
                return null;
        }

        public ServiceResponse<List<User>> DeleteUser(int userId)
        {
            var serviceResponse = new ServiceResponse<List<User>>();
            var user = _dataContext.Users.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                _dataContext.Users.Remove(user);
                _dataContext.SaveChanges();
                serviceResponse.data = _dataContext.Users.ToList();
                return serviceResponse;
            }
            else
            {
                return ServiceResponse<List<User>>.NotFound("User Not Found");
            }

        }

    }
}
