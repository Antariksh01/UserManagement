using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;
using UserManagementAPI.Services.UserService;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private ServiceResponse<List<User>> users;
        public UserController(IUserService userService)
        {
                _userService = userService;
        }

        [HttpGet("GetUsers")]
        public ActionResult<List<User>> GetAllUsers() {

           
            return Ok(_userService.GetUsers());
        }

        [HttpGet("GetUsersByFilter")]
        public ActionResult<IEnumerable<User>> GetUsersByFilter([FromQuery] string filter) { 
        
            var userData = _userService.GetUsersByFilter(filter);
            return Ok(userData);
        }

        [HttpPost("AddUser")]
        public async Task<ActionResult<List<User>>> AddUser(User userData) {

            try
            {
                //Unique Email Attribute already created to check whether email is unique or not
                var users = await _userService.AddUser(userData);
                return Ok(users);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");               
            }
        }

        [HttpPut("UpdateUser")]
        public ActionResult<User> UpdateUser(User userData) { 
        
            var userList = _userService.UpdateUser(userData);
            if(userList == null)
            {
                return NotFound("User Not Found");
            }
            return Ok(userList);
        }

        [HttpDelete("DeleteUser/{id}")]
        public ActionResult<ServiceResponse<List<User>>> DeleteUser(int id) { 
        
            users = _userService.DeleteUser(id);
            return Ok(users);
        }

    }
}
