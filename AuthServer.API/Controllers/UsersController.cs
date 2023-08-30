using AuthServer.API.Controllers.Common;
using AuthServer.Core.Dtos;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Yeni bir kullanıcı yaratmaya yarar
        /// </summary>
        /// <param name="createUserDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody]CreateUserDto createUserDto)
        {
            var result = await _userService.RegisterUserAsync(createUserDto);
            return ActionResult(result);
        }

        /// <summary>
        /// Token alıp authentice olmuş olan user'ın bilgilerini almaya yarar
        /// </summary>
        /// <returns></returns>
        [Authorize] // Bu endpoint mutlaka token ile birlikte kullanılabilsin diyoruz
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var result = await _userService.GetUserByNameAsync();
            return ActionResult(result);
        }
    }
}
