using AuthServer.API.Common;
using AuthServer.API.Controllers.Common;
using AuthServer.Core.Dtos;
using AuthServer.Core.Enums;
using AuthServer.Core.Services;
using AuthServer.Data.ContextAccessor;
using AuthServer.Service.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly ISecurityContextAccessor _contextAccessor;

        public OrdersController(IOrderService orderService, ISecurityContextAccessor contextAccessor)
        {
            _orderService = orderService;
            _contextAccessor = contextAccessor;
        }

        [AuthServerAuthorize(RoleType.Individual)]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody]CreateOrderDto createOrderDto)
        {
            var result = await _orderService.CreateOrderAsync(createOrderDto);
            return ActionResult(result);
        }

        [AuthServerAuthorize(RoleType.Individual, Policy = ClaimConstants.IsLoyalUser)]
        [HttpGet("discount")]
        public async Task<IActionResult> GetDiscountCoupon()
        {
            var result = LoyalConstants.DiscountCoupon(_contextAccessor.UserId);
            return Ok(result);
        }

        [AuthServerAuthorize(RoleType.Individual)]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentOrder()
        {
            var result = await _orderService.GetCurrentOrderAsync();
            return ActionResult(result);
        }
    }
}
