using AuthServer.API.Controllers.Common;
using AuthServer.Core.Dtos;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Giriş yapan kullanıcı için token almaya yarar
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [HttpPost("token")]
        public async Task<IActionResult> GetToken([FromBody]LoginDto loginDto)
        {
            var result = await _authenticationService.CreateTokenAsync(loginDto);

            return ActionResult(result);
        }

        /// <summary>
        /// İlgili client için token almaya yarar
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [HttpPost("tokenByClient")]
        public async Task<IActionResult> GetTokenByClient([FromBody] ClientLoginDto loginDto)
        {
            var result = await _authenticationService.CreateTokenByClientAsync(loginDto);

            return ActionResult(result);
        }

        /// <summary>
        /// Logout durumlarında refresh token'ı silmeye yarar
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var result = await _authenticationService.RevokeRefreshTokenAsync(refreshTokenDto.Token);

            return ActionResult(result);
        }

        /// <summary>
        /// Token süremizin bittiği durumlarda refresh token üzerinden yeni token yaratmaya yarar
        /// </summary>
        /// <param name="refreshTokenDto"></param>
        /// <returns></returns>
        [HttpPost("tokenByRefresh")]
        public async Task<IActionResult> CreateTokenByRefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var result = await _authenticationService.CreateTokenByRefreshTokenAsync(refreshTokenDto.Token);

            return ActionResult(result);
        }
    }
}
