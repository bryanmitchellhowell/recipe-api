using Common.Shared.Utilities;
using Microsoft.AspNetCore.Mvc;
using RecipeApi.Models;
using RecipeApi.Services;

namespace RecipeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Success");
        }

        [HttpGet("grabPwd/{pwd}")]
        public IActionResult GrabPwd(string pwd)
        {
            return Ok(_userService.GetPwd(pwd));            
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {            
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            
            if (!response.User.ErrorMessage.IsNullOrEmpty())
                return BadRequest(new { message = "Password is incorrect" });

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}
