using AuthServer.API.Common;
using AuthServer.API.Controllers.Common;
using AuthServer.Core.Dtos;
using AuthServer.Core.Entities;
using AuthServer.Core.Enums;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : BaseController
    {
        private readonly IAddressService _addressService;   
        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        /// <summary>
        /// Kullanıcı detay sayfası için adres bilgisi eklemeye yarar
        /// </summary>
        /// <param name="addressDto"></param>
        /// <returns></returns>
        [AuthServerAuthorize(RoleType.Individual, RoleType.Corporate)]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressDto addressDto)
        {
            var result = await _addressService.CreateAddressAsync(addressDto);
            return ActionResult(result);
        }
    }
}
