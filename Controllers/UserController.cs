using Microsoft.AspNetCore.Mvc;
using PhoneVault.Models;
using PhoneVault.Services;

namespace PhoneVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult> AddUser(UserDTO userDto)
        {
            if(userDto == null)
            {
                return BadRequest();
            }
            await _userService.AddUser(userDto);
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult> UpdateUser(User user) 
        {
            if(user == null)
            {
                return BadRequest();
            }
            await _userService.UpdateUser(user);
            return Ok();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteUser(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            await _userService.DeleteUser(id);
            return Ok();
        }
    }
}




//[HttpDelete]
//public async Task<ActionResult> DeleteShoppingCartItem(int id)
//{
//    if (id == 0)
//    {
//        return BadRequest();
//    }
//    await _shoppingCartItemService.DeleteShoppingCartItem(id);
//    return Ok();
//}

